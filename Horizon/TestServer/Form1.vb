Imports System.Net
Imports System.Net.Sockets
Imports System.Threading
Imports ServerCore

Public Class Form1
    Dim TcpListener As TcpListener
    Dim mangement As UsersManager
    Dim testController As TestController

    Dim testVar As Object
    Dim testList As New List(Of Object)

    Private Sub debug(title As String, message As String)
        Console.WriteLine(title + ":" + vbTab + message)
    End Sub


    Function OnComponentInit(client As UserWrapper, component As ComponentInstance) As Task
        debug("client", "OnComponentInit")
        debug("OnComponentInit thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "Component initialized" + vbCrLf

        Return Task.CompletedTask
    End Function

    Function OnComponentUnInit(client As UserWrapper, component As ComponentInstance) As Task
        debug("client", "OnComponentUnInit")
        debug("OnComponentUnInit thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "Component Uninitialized" + vbCrLf

        Return Task.CompletedTask
    End Function

    Function OnComponentDisconnect(client As UserWrapper, component As ComponentInstance) As Task
        debug("client", "OnComponentDisconnect")
        debug("OnComponentDisconnect thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "Component disconnected" + vbCrLf

        Return Task.CompletedTask
    End Function

    Sub OnEmptyWrapper(client As UserWrapper)
        debug("client", "OnEmptyWrapper")
        debug("OnEmptyWrapper thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "Empty wrapper" + vbCrLf
    End Sub
    Sub OnNewUser(client As UserWrapper)
        debug("client", "OnNewUser")
        debug("OnNewUser thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "New wrapper " + client.SystemId + vbCrLf
    End Sub

    Function OnReceive(client As UserWrapper, component As ComponentInstance, packet As StructurePacketHeader, cancellationToken As CancellationToken) As Task
        debug("client", "OnReceive")
        debug("OnReceive thread", Thread.CurrentThread.ManagedThreadId)
        TextBox1.Text += "Received packet" + vbCrLf

        Return Task.CompletedTask
    End Function



    Private Sub testParamByRef(ByRef list As Byte())
        Dim temp() As Byte = {70, 100}
        list(0) = 78
        list = Nothing
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'mangement = UsersManager.GetInstance()
        'mangement.SetCallbacks(AddressOf OnComponentInit,
        '                            AddressOf OnComponentUnInit,
        '                            AddressOf OnComponentDisconnect,
        '                            AddressOf OnEmptyWrapper,
        '                            AddressOf OnNewUser,
        '                            AddressOf OnReceive)

        'TcpListener = New TcpListener(IPAddress.Any, 1717)
        'TcpListener.Start()
        'debug("Server started on thread", Thread.CurrentThread.ManagedThreadId)

        'testController = New TestController(Me)
        Dim temp() As Byte = {0, 1}
        testParamByRef(temp)
        Console.WriteLine(temp)

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Button1.Enabled = False

        Try
            Dim acceptedClient As TcpClient = Await TcpListener.AcceptTcpClientAsync()
            Dim client As New ComponentInstance(acceptedClient)

            If Await client.Authentificate() Then
                Dim user As UserWrapper = mangement.AddNewComponent(client)
                debug("Server new client", "Added new component of type " + client.ComponentType.ToString())
                debug("Server new client", "Authentificated " + user.SystemId)

            End If

        Catch ex As Exception
            debug("Server exception", ex.Message)
        End Try

        Button1.Enabled = True
    End Sub

    Private Async Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Button2.Enabled = False
        Try
            Await mangement.SwitchUser(TextBox2.Text)
            debug("Event", "Switched to user " + TextBox2.Text)
        Catch ex As Exception
            debug("Button2 Exception", ex.Message)
        End Try
        Button2.Enabled = True
    End Sub

    Private Async Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Button3.Enabled = False
        Try
            Await mangement.SwitchUser("")
            debug("Event", "Ended processing")
        Catch ex As Exception
            debug("Button3 Exception", ex.Message)
        End Try

        Button3.Enabled = True
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        testController.StopEvents()
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        testController.RestartEvents()
    End Sub
End Class
