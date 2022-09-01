Imports System.IO
Imports System.Threading

Public Class UserWrapper
    Private stoppingTask As Task = Task.CompletedTask
    Private pingingTask As Task = Task.CompletedTask
    Private registeredComponents As New Dictionary(Of ComponentInstance, Task)
    Private processing As Boolean = False
    Private cancelProcessingSource As CancellationTokenSource
    Private userDescriptor As StructureUserDescriptor
    Private report As Reporting = Reporting.GetInstance()

    '' The callback can close the client (indirectly) or throw an exception if something goes wrong
    Public OnComponentInit As Func(Of UserWrapper, ComponentInstance, CancellationToken, Task)
    Public OnComponentUnInit As Func(Of UserWrapper, ComponentInstance, Task)
    Public OnComponentDisconnect As Action(Of UserWrapper, ComponentInstance)
    Public OnUserDisconnect As Action(Of UserWrapper)
    Public OnReceiveCallback As Func(Of UserWrapper, ComponentInstance, StructurePacketHeader, TokenSecurity, Task)


    Public Property Ip As String
    Public ReadOnly Property SystemId As String
        Get
            Return userDescriptor.uniqueId
        End Get
    End Property
    Public ReadOnly Property Descriptor As StructureUserDescriptor
        Get
            Return userDescriptor.Clone()
        End Get
    End Property
    ''' <summary>
    ''' Returns true if the component can be pinged
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property CanPing As Boolean
        Get
            Return (Not processing And stoppingTask.IsCompleted)
        End Get
    End Property
    Public ReadOnly Property IsProcessing As Boolean
        Get
            Return processing
        End Get
    End Property
    ''' <summary>
    ''' Gets the path of this user on local server. Creates the folder if it doesn't exist.
    ''' Path is terminated with a slash
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property GetUserPath As String
        Get
            Dim dateDependency As String = Date.Now.ToString("dd_MM_yyyy")
            Dim userPath As String = Path.Combine(Directory.GetCurrentDirectory(), $"Resources\{SystemId}_{Descriptor.userName}\{dateDependency}\")

            If Not Directory.Exists(userPath) Then Directory.CreateDirectory(userPath)

            Return userPath
        End Get
    End Property



    Public Sub New()

    End Sub


    ''' <summary>
    ''' Sends to the user the updated user descriptor.
    ''' Picks the first component as a gateway to the user.
    ''' Updates the internal descriptor if send is succesful.
    ''' Returns true if succeded in sending the user descriptor
    ''' </summary>
    Public Async Function UpdateDescriptor(updatedDescriptor As StructureUserDescriptor) As Task(Of Boolean)
        Dim component As ComponentInstance = GetComponent(ComponentTypes.any_load)
        If Not Await component.SendHeader(CreateHeader(DataTypes.authentificate, updatedDescriptor.GetSize()), Nothing) Then
            Return False
        End If

        If Not Await component.SendData(updatedDescriptor.Serialize(), userDescriptor.GetSize()) Then
            Return False
        End If

        userDescriptor.customName = updatedDescriptor.customName
        userDescriptor.extraData = updatedDescriptor.extraData
        userDescriptor.pcDescription = updatedDescriptor.pcDescription

        Return True
    End Function

    ''' <summary>
    ''' Returns true if the given component belongs to this user
    ''' </summary>
    Public Function Matches(component As ComponentInstance) As Boolean
        Return SystemId = component.Descriptor.uniqueId
    End Function
    ''' <summary>
    ''' Checks if the user already contains this component type.
    ''' </summary>
    Public Function HasComponent(component As ComponentInstance) As Boolean
        For Each instance In registeredComponents
            If instance.Key.ComponentType = component.ComponentType Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Sub CloseUser()
        For i As Integer = registeredComponents.Count - 1 To 0 Step -1
            Dim component As ComponentInstance = registeredComponents.Keys(i)
            component.CloseClient()
        Next
    End Sub


    ''' <summary>
    ''' Tries to add the instance to this wrapper
    ''' </summary>
    Public Sub AddNewComponent(component As ComponentInstance)
        If userDescriptor Is Nothing Then
            '' This is the first component added to this wrapper
            userDescriptor = component.Descriptor.Clone()
            Ip = component.Ip
        End If
        If HasComponent(component) Then
            component.CloseClient()
            Throw New Exception(String.Format("User {0} already has a component of type {1}", Descriptor.userName, component.ComponentType.ToString()))
        End If

        component.OnReceiveCallback = AddressOf ComponentReceive
        component.OnComponentInit = AddressOf ComponentInit
        component.OnComponentUnInit = AddressOf ComponentUnInit
        component.OnComponentDisconnect = AddressOf ComponentDisconnect
        component.OnComponentFinished = AddressOf ComponentFinished

        If processing Then
            registeredComponents.Add(component, component.ProcessComponent(cancelProcessingSource.Token))
        Else
            registeredComponents.Add(component, Task.CompletedTask)
        End If
    End Sub

    ''' <summary>
    ''' This function must be called from UI otherwise some callbacks may execute on another thread
    ''' </summary>
    Public Async Function StartProcessing() As Task
        If processing Then
            Return
        End If

        Await pingingTask

        cancelProcessingSource = New CancellationTokenSource()
        processing = True

        For i As Integer = registeredComponents.Count - 1 To 0 Step -1
            Dim instance As ComponentInstance = registeredComponents.Keys(i)
            registeredComponents.Item(instance) = instance.ProcessComponent(cancelProcessingSource.Token)
        Next
    End Function

    ''' <summary>
    ''' This function must be called from UI otherwise some callbacks may execute on another thread
    ''' </summary>
    Public Async Function StopProcessing() As Task
        If Not processing Then
            Return
        End If

        cancelProcessingSource.Cancel()
        stoppingTask = Task.WhenAll(registeredComponents.Values)

        '' Do not allow new components to be added or started
        '' We need to restrict new components from being started
        processing = False

        Try
            '' Wait for all components to finish
            Await stoppingTask
        Catch ex As Exception

        End Try

        '' All the logic is handled from within the callbacks - removing from list and calling OnEmptyUser()
    End Function



    ''' <summary>
    ''' Ping all registered components if this wrapper is not processing
    ''' </summary>
    Public Async Function PingComponents() As Task
        If Not CanPing Then
            Return
        End If

        pingingTask = InternalPingComponents()
        Await pingingTask
    End Function
    Private Async Function InternalPingComponents() As Task
        For i As Integer = registeredComponents.Count - 1 To 0 Step -1
            Dim instance As ComponentInstance = registeredComponents.Keys(i)
            Dim pingPacket = CreateHeader(DataTypes.ping_send, 0)
            Dim pingResultTask As Task(Of Boolean) = instance.SendHeader(pingPacket, Nothing)

            Try
                If Await pingResultTask Then
                    Continue For
                End If
            Catch ex As Exception
                report.Low(instance.ComponentType.ToString(), "InternalPingComponents", ex.Message, True)
            End Try

            ComponentDisconnect(instance)
            ComponentFinished(instance)
        Next
    End Function


    ''' <summary>
    ''' Get component of currently selected user.
    ''' If none selected or no component found this returns Nothing.
    ''' </summary>
    Public Function GetComponent(componentType As ComponentTypes) As ComponentInstance
        If componentType = ComponentTypes.any_load Then
            Return registeredComponents.Keys(0)
        End If

        For i As Integer = registeredComponents.Count - 1 To 0 Step -1
            Dim instance As ComponentInstance = registeredComponents.Keys(i)
            If instance.ComponentType = componentType Then
                Return instance
            End If
        Next

        Return Nothing
    End Function
    Public Async Function ForceLoadComponent(componentType As ComponentTypes) As Task
        Dim instance As ComponentInstance = GetComponent(ComponentTypes.any_load)
        If instance Is Nothing Then
            Return
        End If

        Dim type As Integer = Convert.ToInt32(componentType)

        Dim pingPacket = CreateHeader(DataTypes.load_component, 4)
        Await instance.SendHeader(pingPacket, Nothing)
        Await instance.SendData(BitConverter.GetBytes(type), 4)
    End Function


    Private Function ComponentReceive(component As ComponentInstance, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Return OnReceiveCallback(Me, component, packet, token)
    End Function
    Private Function ComponentInit(component As ComponentInstance, cancellationToken As CancellationToken) As Task
        Return OnComponentInit(Me, component, cancellationToken)
    End Function
    Private Function ComponentUnInit(component As ComponentInstance) As Task
        Return OnComponentUnInit(Me, component)
    End Function
    Private Sub ComponentDisconnect(component As ComponentInstance)
        registeredComponents.Remove(component)
        OnComponentDisconnect(Me, component)
    End Sub
    Private Sub ComponentFinished(component As ComponentInstance)
        If registeredComponents.Count = 0 Then
            OnUserDisconnect(Me)
        End If
    End Sub

End Class
