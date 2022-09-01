Imports System.Threading

Public Class UsersManager
    Private Shared instance As UsersManager
    Private pingingTask As Task = Task.CompletedTask
    Private stoppingTask As Task = Task.CompletedTask
    Private users As New List(Of UserWrapper)
    Private selectedUser As UserWrapper

    Public OnComponentInit As Func(Of UserWrapper, ComponentInstance, CancellationToken, Task)
    Public OnComponentUnInit As Func(Of UserWrapper, ComponentInstance, Task)
    Public OnNewUser As Action(Of UserWrapper)
    Public OnReceiveCallback As Func(Of UserWrapper, ComponentInstance, StructurePacketHeader, TokenSecurity, Task)
    Public OnComponentDisconnect As Action(Of UserWrapper, ComponentInstance)
    Public OnUserDisconnect As Action(Of UserWrapper)
    Public OnSelectedUserDisconnect As Action
    Public OnSelectedUserStoppedProcessing As Action

    Public Sub New()

    End Sub


    Public Shared Function GetInstance() As UsersManager
        If instance Is Nothing Then
            instance = New UsersManager()
        End If
        Return instance
    End Function

    Public Function AddNewComponent(component As ComponentInstance) As UserWrapper
        For Each user In users
            If user.Matches(component) Then
                user.AddNewComponent(component)
                Return user
            End If
        Next

        '' If no existing users was found then create a new one
        Dim newUser As New UserWrapper()
        newUser.OnComponentInit = OnComponentInit
        newUser.OnComponentUnInit = OnComponentUnInit
        newUser.OnComponentDisconnect = OnComponentDisconnect
        newUser.OnUserDisconnect = AddressOf EmptyWrapper
        newUser.OnReceiveCallback = OnReceiveCallback

        users.Add(newUser)
        newUser.AddNewComponent(component)

        OnNewUser(newUser)

        Return newUser
    End Function

    ''' <summary>
    ''' Switches current user the one given as argument.
    ''' Does not switch the user if the last one did not completely stopped.
    ''' Returns true if switch does happen
    ''' </summary>
    Public Async Function SwitchUser(systemId As String) As Task(Of Boolean)
        If Not stoppingTask.IsCompleted Then
            '' Only switch user if the last one finished
            Return False
        End If

        If selectedUser IsNot Nothing Then
            stoppingTask = selectedUser.StopProcessing()
            Await stoppingTask

            '' Deselect the current user
            selectedUser = Nothing
            OnSelectedUserStoppedProcessing()
        End If

        If Not String.IsNullOrEmpty(systemId) Then
            For Each user In users
                If systemId = user.SystemId Then
                    '' Wait for a pinging session to finish
                    Await pingingTask

                    selectedUser = user
                    Await user.StartProcessing()
                    Exit For
                End If
            Next
        End If

        Return True
    End Function


    Public Async Function PingUsers() As Task
        If Not pingingTask.IsCompleted Then
            Return
        End If

        pingingTask = InternalPingUsers()
        Await pingingTask
    End Function
    Private Function InternalPingUsers() As Task
        Dim pingTasks As New List(Of Task)
        For i As Integer = users.Count - 1 To 0 Step -1
            Dim user As UserWrapper = users(i)

            If user Is selectedUser Then
                Continue For
            End If

            '' I know for a fact that this command below will invoke sometimes the EmptyWrapper event immediatelly (from tests)
            '' But because I iterate with index the list is safe
            pingTasks.Add(user.PingComponents())
        Next

        Return Task.WhenAll(pingTasks)
    End Function

    Public Function GetCurrentUser() As UserWrapper
        Return selectedUser
    End Function

    ''' <summary>
    ''' Returns specified component of current user or Nothing if not set
    ''' </summary>
    ''' <returns></returns>
    Public Function GetComponent(componentType As ComponentTypes) As ComponentInstance
        If selectedUser Is Nothing Then
            Return Nothing
        End If
        Return selectedUser.GetComponent(componentType)
    End Function

    Public Sub EmptyWrapper(user As UserWrapper)
        users.Remove(user)
        If user Is selectedUser Then
            OnSelectedUserDisconnect()
            selectedUser = Nothing
        End If

        OnUserDisconnect(user)
    End Sub
End Class
