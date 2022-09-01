Imports System.IO
Imports System.Threading
Imports MetroFramework.Controls
Imports ServerCore

Public Class FtpUploadController
    Inherits ComponentController

    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Private realProgressBytes As UInt64
    Private Const progressBarMaxValue As UInt64 = 10000
    Private report As Reporting

    Private WithEvents ftpBtnUpload As ToolStripMenuItem
    Private WithEvents ftpUrl As TextBox
    Private WithEvents ftpUploadProgressBar As MetroProgressBar
    Private WithEvents lblUploadName As Label
    Private WithEvents btnFtpUploadInterrupt As Button
    Private openFileDialog As OpenFileDialog

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.upload_manager))
        Me.user = user
        Me.cancellationToken = cancellationToken

        report = Reporting.GetInstance()
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()

        viewer.TextBox4.Text = "on"
        viewer.TextBox4.ForeColor = Color.LightGreen

        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox4.Text = "off"
        viewer.TextBox4.ForeColor = Color.FromArgb(214, 69, 38)

        ClearUploadControls()
        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        ftpBtnUpload = viewer.ftpBtnUpload
        ftpUrl = viewer.ftpUrl
        ftpUploadProgressBar = viewer.ftpUploadProgressBar
        lblUploadName = viewer.lblUploadName
        btnFtpUploadInterrupt = viewer.btnFtpUploadInterrupt
        openFileDialog = viewer.openFileDialog

    End Sub
    Protected Overrides Sub RemoveHandlers()
        ftpBtnUpload = Nothing
        ftpUrl = Nothing
        ftpUploadProgressBar = Nothing
        lblUploadName = Nothing
        btnFtpUploadInterrupt = Nothing
        openFileDialog = Nothing

    End Sub


#Region "PacketProcessor"
    Public Overrides Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Throw New Exception("Ftp upload controller should not receive any packet but it did")
        Return Task.CompletedTask
    End Function
#End Region



#Region "Model"
    Private Async Function StartFilesUpload(argumentList As List(Of String)) As Task
        Using sync = New ControlResource(syncUse)
            If Not ControllerState = CONTROLLER_STATE.PAUSED Then
                Return
            End If
            ControllerState = CONTROLLER_STATE.PROCESSING

            Await SendFiles(argumentList)

            ClearUploadControls()
            EndTransaction()
        End Using
    End Function

    Private Async Function SendFiles(argumentList As List(Of String)) As Task
        For argumentItterator = 0 To argumentList.Count() - 1 Step 2
            '' Get the file parameters/paths
            Dim pathOnMyMachine As String = argumentList(argumentItterator)
            Dim pathOnClientMachine As String = argumentList(argumentItterator + 1)

            '' Get the filename from the path string
            Dim fileName As String = Path.GetFileName(pathOnMyMachine)

            '' Create an argument to be sent to the client
            Dim paramArguments As String = pathOnClientMachine

            If ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION Or cancellationToken.IsCancellationRequested Then
                Return
            End If

            Try
                Using reader As FileStream = New FileStream(pathOnMyMachine, FileMode.Open)
                    Dim thunkWindowSize As Long = 65536
                    Dim buffer(thunkWindowSize) As Byte
                    Dim fileLength As UInt64 = reader.Length
                    Dim remainingLength As UInt64 = fileLength

                    If Not Await component.SendHeader(CreateHeader(DataTypes.upload_file, fileLength, paramArguments), Nothing) Then
                        Return
                    End If

                    '' Update controls
                    UpdateControlsBeforeUpload(fileLength, pathOnMyMachine)

                    '' Send this entire file in a fragmented manner.
                    While remainingLength > 0
                        Dim bufferSize As Long
                        Await Task.Run(Sub()
                                           '' Decide how big the chunk size should be .
                                           bufferSize = Math.Min(thunkWindowSize, remainingLength)

                                           '' Read the chunk into the file.
                                           reader.Read(buffer, 0, bufferSize)
                                       End Sub)

                        '' Send the chunk to the client.
                        If Not Await component.SendData(buffer, bufferSize) Then Exit While

                        UpdateProgress(bufferSize, fileLength)

                        '' Update remaining bytes count
                        remainingLength -= bufferSize
                    End While

                End Using
            Catch ex As Exception
                report.Low("Failed to upload file """ + fileName + """ because " + ex.Message, False)
            End Try
        Next
    End Function


    Private Sub StopFilesUpload()
        If Not ControllerState = CONTROLLER_STATE.PROCESSING Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION
    End Sub

    Private Sub EndTransaction()
        ControllerState = CONTROLLER_STATE.PAUSED
    End Sub
#End Region

#Region "View"
    Private Sub UpdateControlsBeforeUpload(fileLength As UInt64, pathOnMyMachine As String)
        '' Initializing the progress bar
        ftpUploadProgressBar.Minimum = 0
        ftpUploadProgressBar.Maximum = progressBarMaxValue
        ftpUploadProgressBar.Value = 1 * progressBarMaxValue / 100

        '' Update the label with the name of the file uploading
        lblUploadName.Text = pathOnMyMachine

        realProgressBytes = 0
    End Sub

    Private Sub UpdateProgress(bytesCount As UInt64, packetSize As UInt64)
        realProgressBytes += bytesCount

        '' Use a non zero minimum value in order to signal to the user the fact that download is active
        ftpUploadProgressBar.Value = Math.Max(1 * progressBarMaxValue / 100, realProgressBytes * progressBarMaxValue / packetSize)
    End Sub

    Private Sub ClearUploadControls()
        ftpUploadProgressBar.Value = 0
        lblUploadName.Text = ""
    End Sub
#End Region


#Region "Events"
    Private Async Sub ftpBtnUpload_Click(sender As Object, e As EventArgs) Handles ftpBtnUpload.Click
        Dim result As DialogResult = openFileDialog.ShowDialog()

        If ControllerState = CONTROLLER_STATE.PROCESSING Then
            Return
        End If

        If result = Windows.Forms.DialogResult.OK Then
            Dim basePath As String = GetCurrentPath()
            Dim argumentList As New List(Of String)

            If basePath(0) = FtpController.FtpHome Then Exit Sub

            For Each selectedFileToUpload As String In openFileDialog.FileNames
                argumentList.Add(selectedFileToUpload)
                argumentList.Add(basePath + IO.Path.GetFileName(selectedFileToUpload))
            Next

            Await StartFilesUpload(argumentList)
        End If
    End Sub

    Private Sub btnFtpUploadInterrupt_Click(sender As Object, e As EventArgs) Handles btnFtpUploadInterrupt.Click
        StopFilesUpload()
    End Sub
#End Region

    Private Function GetCurrentPath() As String
        Dim path As String = ftpUrl.Text

        path = NormalizeText(path)

        If String.IsNullOrEmpty(path.Replace(" ", "")) Then
            path = FtpController.FtpHome
        End If

        Return NormalizePath(path)
    End Function
End Class
