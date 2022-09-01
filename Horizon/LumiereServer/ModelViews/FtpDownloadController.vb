Imports System
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ServerCore

Public Class FtpDownloadController
    Inherits ComponentController

    Private Structure RequestMetadata
        Public singleFile As Boolean
        Public path As String
    End Structure


    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Private realProgressBytes As UInt64
    Private Const progressBarMaxValue As UInt64 = 10000
    Private isFolderOperation As Boolean = False
    Private report As Reporting = Reporting.GetInstance()

    Private WithEvents ftpView As ListView
    Private WithEvents ftpBtnDownloadFile As ToolStripMenuItem
    Private WithEvents ftpDownloadProgressBar As ProgressBar
    Private WithEvents lblDownloadName As Label
    Private WithEvents ftpOperationHistory As ListBox
    Private WithEvents btnFtpDownloadInterrupt As Button

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.download_manager))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()

        viewer.TextBox3.Text = "on"
        viewer.TextBox3.ForeColor = Color.LightGreen

        cancellationToken.Register(Async Sub()
                                       Await StopFolderDownload()
                                   End Sub)
        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox3.Text = "off"
        viewer.TextBox3.ForeColor = Color.FromArgb(214, 69, 38)

        ClearDownloadControls()
        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        ftpView = viewer.ftpView
        ftpBtnDownloadFile = viewer.ftpBtnDownloadFile
        ftpDownloadProgressBar = viewer.ftpDownloadProgressBar
        lblDownloadName = viewer.lblDownloadName
        ftpOperationHistory = viewer.ftpOperationHistory
        btnFtpDownloadInterrupt = viewer.btnFtpDownloadInterrupt

    End Sub
    Protected Overrides Sub RemoveHandlers()
        ftpView = Nothing
        ftpBtnDownloadFile = Nothing
        ftpDownloadProgressBar = Nothing
        lblDownloadName = Nothing
        ftpOperationHistory = Nothing
        btnFtpDownloadInterrupt = Nothing

    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.download_file
                Dim buffer(65536) As Byte
                Dim clientFilePath As String = packet.arguments
                Dim serverFilePath As String = ConstructLocalPathFromRemote(user.GetUserPath(), clientFilePath)
                Dim serverFolderPath As String = Path.GetDirectoryName(serverFilePath)
                Dim metadata As RequestMetadata = token.metadata

                If Not IsPathValid(clientFilePath, metadata.path) Then
                    Throw New Exception("Received file has tampered path - different from metadata")
                End If

                '' If I want to build dirs then build them
                If Not Directory.Exists(serverFolderPath) Then Directory.CreateDirectory(serverFolderPath)

                '' Save the file locally
                Await RecvFileStreamAndSave(packet, serverFilePath, clientFilePath)

                '' Only end the transaction if this was only one file request
                If metadata.singleFile Then
                    ClearDownloadControls()
                    EndTransaction()
                End If

            Case DataTypes.download_folder_end
                isFolderOperation = False

                ClearDownloadControls()
                EndTransaction()
        End Select
    End Function
#End Region



#Region "Model"
    '' MUST NOT be used in events but only in exception-"managed" functions like HandlePacket
    '' Because it throws a cancellation error
    Private Sub EndTransaction()
        component.GetTokenManager().DisableAllTokens()

        ControllerState = CONTROLLER_STATE.PAUSED

        ''Uninitialize component if it's requested
        cancellationToken.ThrowIfCancellationRequested()
    End Sub


    Private Async Function StartFileDownload(fileData As FtpController.FileData) As Task
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.PROCESSING

        Await SendStartFileDownloadPacket(fileData)
    End Function
    Private Async Function SendStartFileDownloadPacket(fileData As FtpController.FileData) As Task
        Dim filePath As String = fileData.CompletePath

        Dim downloadFileToken As TokenSecurity = CreateDefaultToken(DataTypes.download_file, 1, 1 * 60 * 60 * 1000)
        downloadFileToken.metadata = New RequestMetadata With {.singleFile = True, .path = filePath}

        Await component.SendHeader(CreateHeader(DataTypes.download_file, 0, filePath), downloadFileToken)
    End Function


    Private Async Function StartFolderDownload(fileData As FtpController.FileData) As Task
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.PROCESSING

        '' Signal that we will download a folder
        isFolderOperation = True

        Await SendStartFolderDownloadPacket(fileData)
    End Function
    Private Async Function SendStartFolderDownloadPacket(rootFolder As FtpController.FileData) As Task
        Dim filePath As String = rootFolder.CompletePath + "\"

        '' Prepare finished operation token
        Dim endToken As TokenSecurity = CreateLooseToken(DataTypes.download_folder_end).EnforceUses(1)
        component.GetTokenManager().RegisterPacket(DataTypes.download_folder, endToken)

        '' Add metadata to the request
        Dim downloadFileToken As TokenSecurity = CreateLooseToken(DataTypes.download_file)
        downloadFileToken.metadata = New RequestMetadata With {.singleFile = False, .path = filePath}

        '' Send start process packet
        Await component.SendHeader(CreateHeader(DataTypes.download_folder, endToken.id.Length, filePath), downloadFileToken)
        Await component.SendData(endToken.id, endToken.id.Length)
    End Function

    Private Async Function StopFolderDownload() As Task
        If Not ControllerState = CONTROLLER_STATE.PROCESSING Or Not isFolderOperation Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION

        Await SendStopFolderDownloadPacket()
    End Function
    Private Async Function SendStopFolderDownloadPacket() As Task
        Await component.SendHeader(CreateHeader(DataTypes.flags_cancel_operation, 0), CreateDefaultToken(DataTypes.flags_cancelled_operation))
    End Function

    Private Async Function RecvFileStreamAndSave(packet As StructurePacketHeader, serverFilePath As String, clientFilePath As String) As Task
        Dim buffer(65536) As Byte
        Dim filePath As String = packet.arguments

        '' Update controls before the downloading starts
        UpdateControlsBeforeDownload(clientFilePath)

        ''Trying to receive the entire file stream.
        Using writer As FileStream = New FileStream(serverFilePath, FileMode.Create)
            If Not Await component.RecvFragmented(buffer, buffer.Length,
                       Sub(bytesCount As UInt64)
                           '' Write the received bytes to the file stream
                           writer.Write(buffer, 0, bytesCount)

                           FileDownloadProgressCallback(bytesCount, packet.packetSize)
                       End Sub,
                       True) Then

                writer.Close()
                report.Low("File is incomplete: " + serverFilePath, True)
            End If
        End Using

        '' Update controls after the download
        UpdateControlsAfterDownload(clientFilePath)
    End Function


    ''' <summary>
    ''' Parses the remote path and constructs a local one. 
    ''' Normalizes the special characters and checks with a proper function if the path is within its bounds.
    ''' Throws exception if the resulting path tries to traverse the root folder.
    ''' </summary>
    ''' <returns></returns>
    Private Function ConstructLocalPathFromRemote(rootFolder As String, remotePath As String) As String
        Dim localPath As String
        Dim filterUnicode As Regex = New Regex("[^\x00-\x7F]+", RegexOptions.ECMAScript)

        Dim convertSlashes As Regex = New Regex("\/", RegexOptions.ECMAScript)
        Dim filterTooManySlashes As New Regex("\\+", RegexOptions.ECMAScript)

        Dim filterRootDrive As Regex = New Regex("[a-zA-Z]\s*:\s*[\\]*", RegexOptions.ECMAScript)
        Dim filterStartingSlash As Regex = New Regex("^\s*\\+", RegexOptions.ECMAScript)

        Dim filterNonWindows As Regex = New Regex("[^\w .\\]", RegexOptions.ECMAScript)

        ''Replace with a visible character to show it's unicode
        remotePath = filterUnicode.Replace(remotePath, "_")

        remotePath = convertSlashes.Replace(remotePath, "\")
        remotePath = filterTooManySlashes.Replace(remotePath, "\")

        remotePath = filterRootDrive.Replace(remotePath, "")
        remotePath = filterStartingSlash.Replace(remotePath, "")

        remotePath = filterNonWindows.Replace(remotePath, "")

        localPath = Path.Combine(New String() {rootFolder, remotePath})

        If Path.GetFullPath(localPath).IndexOf(rootFolder) = 0 Then
            Return localPath
        End If

        Throw New Exception("Remote path is corrupted or attempted path traversal: " + remotePath + ", processed into: " + localPath)
    End Function

    ''' <summary>
    ''' Checks whether the reported path resolves to the same path the request started with
    ''' </summary>
    ''' <returns></returns>
    Private Function IsPathValid(reportedPath As String, remoteMetadataPath As String)
        Return reportedPath.Contains(remoteMetadataPath)
    End Function
#End Region


#Region "View"
    Private Sub UpdateControlsBeforeDownload(clientLocalPath As String)
        ''Initialize the progress bar
        ftpDownloadProgressBar.Minimum = 0
        ftpDownloadProgressBar.Maximum = progressBarMaxValue
        ftpDownloadProgressBar.Value = 1 * progressBarMaxValue / 100
        lblDownloadName.Text = clientLocalPath

        realProgressBytes = 0
    End Sub
    Private Sub UpdateControlsAfterDownload(clientLocalPath As String)
        Dim followBottomOfList As Boolean = (ftpOperationHistory.TopIndex > ftpOperationHistory.Items.Count - 50)

        '' Adding the file path to the list with operation activity holder
        ftpOperationHistory.Items.Add(clientLocalPath)

        '' This enables the user to scroll up without being dragged back down again
        If followBottomOfList Then
            ftpOperationHistory.TopIndex = ftpOperationHistory.Items.Count - 1
        End If
    End Sub
    Private Sub FileDownloadProgressCallback(bytesCount As UInt64, packetSize As UInt64)
        realProgressBytes += bytesCount

        '' Use a non zero minimum value in order to signal to the user the fact that download is active
        ftpDownloadProgressBar.Value = Math.Max(1 * progressBarMaxValue / 100, realProgressBytes * progressBarMaxValue / packetSize)
    End Sub

    ''' <summary>
    ''' Clears the controls used for download.
    ''' </summary>
    Private Sub ClearDownloadControls()
        '' Clear the label which stored the name of the file being downloaded
        lblDownloadName.Text = ""
        '' Reset the progress bar's level
        ftpDownloadProgressBar.Value = 0
    End Sub
#End Region


#Region "Events"
    Private Async Sub ftpBtnDownloadFile_Click(sender As Object, e As EventArgs) Handles ftpBtnDownloadFile.Click
        For Each fileItem As ListViewItem In ftpView.SelectedItems
            Dim fileData As FtpController.FileData = fileItem.Tag

            If fileData.TypeTag = FtpController.FileDataTag.File Then
                Await StartFileDownload(fileData)
            End If
            If fileData.TypeTag = FtpController.FileDataTag.Folder Then
                Await StartFolderDownload(fileData)
            End If
        Next
    End Sub

    Private Async Sub btnFtpDownloadInterrupt_Click(sender As Object, e As EventArgs) Handles btnFtpDownloadInterrupt.Click
        Await StopFolderDownload()
    End Sub
#End Region


End Class
