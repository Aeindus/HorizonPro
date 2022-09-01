Imports System.IO
Imports System.Threading
Imports ServerCore

Public Class FtpController
    Inherits ComponentController

    Enum FtpShortcut As Integer
        Desktop
        AppData
        Temp
        Windows
        Startup
        StartupAll
        Home
        RecycleBin
        Pinned1
        Pinned2
        Pinned3
    End Enum
    Enum FileDataTag As Integer
        Folder
        File
        ErrorCarrier
    End Enum
    Structure FileData
        Dim TypeTag As FileDataTag
        Dim CompletePath As String
    End Structure

    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Public Const FtpHome As String = "\"
    Public ftpPinnedShortcuts As New Dictionary(Of FtpShortcut, String)
    Private ftpCurrentPath As String = FtpHome

    Private WithEvents ftpUrl As TextBox
    Private WithEvents ftpView As ListView
    Private WithEvents ftpPinPathButton As ToolStripMenuItem
    Private WithEvents ftpBtnBack As Control
    Private WithEvents ftpBtnNewFolder As ToolStripMenuItem
    Private WithEvents ftpBtnNewFile As ToolStripMenuItem
    Private WithEvents btnFtpRun As ToolStripMenuItem
    Private WithEvents btnFtpRunAs As ToolStripMenuItem
    Private WithEvents ftpBtnDelete As ToolStripMenuItem
    Private WithEvents ftpBackgroundMenu As ContextMenuStrip
    Private WithEvents ftpFileMenu As ContextMenuStrip
    Private WithEvents ftpBtnRefresh As ToolStripMenuItem
    Private WithEvents ftpBtnRename As ToolStripMenuItem

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.ftp))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()
        viewer.btnShortcutPin1.Text = "______________"
        viewer.btnShortcutPin2.Text = "______________"
        viewer.btnShortcutPin3.Text = "______________"

        '' Resize ftp columns to sync with the column-labels above
        viewer.ftpView.Columns(0).Width = viewer.lblColumnFtp2.Left - viewer.lblColumnFtp1.Left
        viewer.ftpView.Columns(1).Width = viewer.lblColumnFtp3.Left - viewer.lblColumnFtp2.Left
        viewer.ftpView.Columns(2).Width = viewer.ftpView.Width - viewer.lblColumnFtp3.Left - 30
    End Sub

    Public Overrides Async Function Init() As Task
        AddHandlers()

        viewer.TextBox5.Text = "on"
        viewer.TextBox5.ForeColor = Color.LightGreen

        ftpView.Items.Clear()
        ClearFtpShortcuts()
        PopulateFtpShortcuts()
        ftpCurrentPath = FtpHome

        Await component.SendHeader(CreateHeader(DataTypes.enum_drives, 0, ""), CreateDefaultToken(DataTypes.enum_drives))
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox5.Text = "off"
        viewer.TextBox5.ForeColor = Color.FromArgb(214, 69, 38)

        ftpView.Items.Clear()
        ftpUrl.Text = FtpHome
        ClearFtpShortcuts()

        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        ftpUrl = viewer.ftpUrl
        ftpView = viewer.ftpView
        ftpPinPathButton = viewer.ftpPinPathButton
        ftpBtnBack = viewer.ftpBtnBack
        ftpBtnNewFolder = viewer.ftpBtnNewFolder
        ftpBtnNewFile = viewer.ftpBtnNewFile
        btnFtpRun = viewer.btnFtpRun
        btnFtpRunAs = viewer.btnFtpRunAs
        ftpBtnDelete = viewer.ftpBtnDelete
        ftpBackgroundMenu = viewer.ftpBackgroundMenu
        ftpFileMenu = viewer.ftpFileMenu
        ftpBtnRefresh = viewer.ftpBtnRefresh
        ftpBtnRename = viewer.ftpBtnRename

        AddHandler viewer.btnShortcutDesktop.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutAppData.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutTemp.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutWindows.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutStartup.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutStartupAll.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutPin1.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutPin2.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutPin3.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutHome.Click, AddressOf btnShortcut_Click
        AddHandler viewer.btnShortcutRecycleBin.Click, AddressOf btnShortcut_Click

    End Sub
    Protected Overrides Sub RemoveHandlers()
        ftpUrl = Nothing
        ftpView = Nothing
        ftpPinPathButton = Nothing
        ftpBtnBack = Nothing
        ftpBtnNewFolder = Nothing
        ftpBtnNewFile = Nothing
        btnFtpRun = Nothing
        btnFtpRunAs = Nothing
        ftpBtnDelete = Nothing
        ftpBackgroundMenu = Nothing
        ftpFileMenu = Nothing
        ftpBtnRefresh = Nothing
        ftpBtnRename = Nothing

        RemoveHandler viewer.btnShortcutDesktop.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutAppData.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutTemp.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutWindows.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutStartup.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutStartupAll.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutPin1.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutPin2.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutPin3.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutHome.Click, AddressOf btnShortcut_Click
        RemoveHandler viewer.btnShortcutRecycleBin.Click, AddressOf btnShortcut_Click
    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.enum_drives, DataTypes.enum_folders_and_files
                Dim win32FindData As New StructureWIN32FindData
                Dim noOfEntries As UInt64
                Dim buffer(win32FindData.GetSize() + 8) As Byte

                If Not Await component.RecvData(buffer, 8) Then
                    Return
                End If

                noOfEntries = BitConverter.ToUInt64(buffer, 0)

                '' Clear control
                ftpView.Items.Clear()
                '' Optimise control
                ftpView.BeginUpdate()

                If noOfEntries <> 0 Then
                    If Not Await component.RecvFragmented(buffer, win32FindData.GetSize(),
                                                      Sub()
                                                          win32FindData.Deserialize(buffer, 0)
                                                          Dim smallIconIndex As Integer = 1

                                                          If win32FindData.FileAttributes And 16 Then smallIconIndex = 0

                                                          Dim newItem As ListViewItem = New ListViewItem(win32FindData.FileName, smallIconIndex)
                                                          newItem.SubItems.Add(FormatFileSize(win32FindData.FileSize))
                                                          newItem.SubItems.Add(win32FindData.LastWriteTime.ToString("g"))

                                                          Dim filePath As String = ftpCurrentPath + win32FindData.FileName
                                                          If win32FindData.FileAttributes And 16 Then
                                                              newItem.Tag = New FileData With {.TypeTag = FileDataTag.Folder, .CompletePath = filePath}
                                                          Else
                                                              newItem.Tag = New FileData With {.TypeTag = FileDataTag.File, .CompletePath = filePath}
                                                          End If

                                                          ftpView.Items.Add(newItem)
                                                      End Sub) Then
                        ftpView.EndUpdate()
                        Return
                    End If

                Else
                    Dim newItem As ListViewItem = New ListViewItem(packet.arguments, 0)
                    newItem.SubItems.Add("0")
                    newItem.SubItems.Add("")
                    newItem.Tag = New FileData With {.TypeTag = FileDataTag.ErrorCarrier, .CompletePath = ""}

                    ftpView.Items.Add(newItem)
                End If

                ftpView.EndUpdate()
        End Select
    End Function
#End Region



#Region "Model"
    '' Functions which modify the model/data
    Private Function FtpNavigateToShortcut(shortcut As FtpShortcut) As Task
        Dim component As ComponentInstance = user.GetComponent(ComponentTypes.ftp)
        Dim expandedPath As String = ""

        If component Is Nothing Then
            Return Task.CompletedTask
        End If

        Select Case shortcut
            Case FtpShortcut.Desktop
                expandedPath = "C:\Users\" + user.Descriptor.userName + "\Desktop"
            Case FtpShortcut.AppData
                expandedPath = "C:\Users\" + user.Descriptor.userName + "\AppData"
            Case FtpShortcut.Startup
                expandedPath = "C:\Users\" + user.Descriptor.userName + "\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Startup"
            Case FtpShortcut.Temp
                expandedPath = "C:\Users\" + user.Descriptor.userName + "\AppData\Local\Temp"
            Case FtpShortcut.StartupAll
                expandedPath = "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\StartUp"
            Case FtpShortcut.Windows
                expandedPath = "C:\Windows"
            Case FtpShortcut.Home
                expandedPath = FtpHome
            Case FtpShortcut.RecycleBin
                expandedPath = "C:\$Recycle.Bin"
            Case Else
                If ftpPinnedShortcuts.ContainsKey(shortcut) Then
                    expandedPath = ftpPinnedShortcuts.Item(shortcut)
                End If
        End Select

        If Not expandedPath = "" Then
            Return FtpNavigateTo(expandedPath)
        End If
        Return Task.CompletedTask
    End Function
    Private Function FtpNavigateTo(newUrl As String) As Task
        If component Is Nothing Then
            Return Task.CompletedTask
        End If

        '' Update current path to new path
        UpdateFtpPath(newUrl)

        If ftpCurrentPath = FtpHome Then
            Return component.SendHeader(CreateHeader(DataTypes.enum_drives, 0, ftpCurrentPath), CreateDefaultToken(DataTypes.enum_drives))
        Else
            Return component.SendHeader(CreateHeader(DataTypes.enum_folders_and_files, 0, ftpCurrentPath), CreateDefaultToken(DataTypes.enum_folders_and_files))
        End If
    End Function
#End Region


#Region "View"
    '' Functions which modify the view

    ''' <summary>
    ''' Updates the current path and url field of the window.
    ''' Handles the format of the path and normalizes it.
    ''' </summary>
    ''' <param name="newUrl"></param>
    Private Sub UpdateFtpPath(newUrl As String)
        '' Filter out non-ascii
        newUrl = NormalizeText(newUrl)

        If String.IsNullOrEmpty(newUrl.Replace(" ", "")) Then
            newUrl = FtpHome
        End If
        newUrl = NormalizePath(newUrl)

        '' Move the cursor at the end of the textbox
        ftpUrl.Text = newUrl
        ftpUrl.SelectionStart = ftpUrl.Text.Length
        ftpUrl.SelectionLength = 0

        '' Update 
        ftpCurrentPath = newUrl
    End Sub

    Private Sub PopulateFtpShortcuts()
        viewer.btnShortcutDesktop.Tag = FtpShortcut.Desktop
        viewer.btnShortcutAppData.Tag = FtpShortcut.AppData
        viewer.btnShortcutTemp.Tag = FtpShortcut.Temp
        viewer.btnShortcutWindows.Tag = FtpShortcut.Windows
        viewer.btnShortcutStartup.Tag = FtpShortcut.Startup
        viewer.btnShortcutStartupAll.Tag = FtpShortcut.StartupAll
        viewer.btnShortcutHome.Tag = FtpShortcut.Home
        viewer.btnShortcutRecycleBin.Tag = FtpShortcut.RecycleBin
        viewer.btnShortcutPin1.Tag = FtpShortcut.Pinned1
        viewer.btnShortcutPin2.Tag = FtpShortcut.Pinned2
        viewer.btnShortcutPin3.Tag = FtpShortcut.Pinned3
        If ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned1) Then
            viewer.btnShortcutPin1.Text = Path.GetFileName(ftpPinnedShortcuts.Item(FtpShortcut.Pinned1))
        End If
        If ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned2) Then
            viewer.btnShortcutPin2.Text = Path.GetFileName(ftpPinnedShortcuts.Item(FtpShortcut.Pinned2))
        End If
        If ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned3) Then
            viewer.btnShortcutPin3.Text = Path.GetFileName(ftpPinnedShortcuts.Item(FtpShortcut.Pinned3))
        End If
    End Sub
    Private Sub ClearFtpShortcuts()
        ftpPinnedShortcuts.Clear()
        viewer.btnShortcutPin1.Text = "______________"
        viewer.btnShortcutPin2.Text = "______________"
        viewer.btnShortcutPin3.Text = "______________"
    End Sub

#End Region


#Region "Events"
    Private Sub ftpView_MouseUp(sender As Object, e As MouseEventArgs) Handles ftpView.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Right Then
            Dim lvi As ListViewItem = ftpView.HitTest(e.Location).Item

            If lvi Is Nothing Then
                ftpBackgroundMenu.Show(ftpView, e.Location)
            Else
                Dim fileData As FileData = lvi.Tag
                If fileData.TypeTag = FileDataTag.File Or fileData.TypeTag = FileDataTag.Folder Then
                    ftpFileMenu.Show(ftpView, e.Location)
                End If
            End If
        End If
    End Sub

    Private Async Sub ftpView_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles ftpView.MouseDoubleClick
        Dim selectedItem As ListViewItem = ftpView.HitTest(e.Location).Item
        If selectedItem Is Nothing Then Return

        Dim fileName As String = selectedItem.SubItems(0).Text
        Dim fileData As FileData = selectedItem.Tag

        If fileData.TypeTag = FileDataTag.File Then
            '' If I pressed a button representing a file then I have to do some processing first
        ElseIf fileData.TypeTag = FileDataTag.Folder Then
            If ftpCurrentPath = FtpHome Then
                '' This means we clicked a drive letter
                ftpCurrentPath = fileName
            Else
                ftpCurrentPath += fileName + "\"
            End If

            '' Send the command to the client
            Await FtpNavigateTo(ftpCurrentPath)
        End If
    End Sub
    Private Async Sub ftpUrl_KeyDown(sender As Object, e As KeyEventArgs) Handles ftpUrl.KeyUp
        If e.KeyCode = Keys.Enter Then
            '' Remove controls characters
            ftpUrl.Text = NormalizeText(ftpUrl.Text)

            '' Send the command to the client
            Await FtpNavigateTo(ftpUrl.Text)
        End If
    End Sub
    Private Async Sub ftpBtnBack_Click(sender As Object, e As EventArgs) Handles ftpBtnBack.Click
        Dim slashCount As Integer = ftpCurrentPath.Count(Function(x) x = "\")
        Dim newPath As String = ftpCurrentPath

        Using sync = New ControlResource(syncUse)
            If slashCount >= 2 Then
                '' Remove the last slash
                newPath = newPath.Substring(0, newPath.LastIndexOf("\"))

                '' Remove the last folder
                newPath = newPath.Substring(0, newPath.LastIndexOf("\"))

                Await FtpNavigateTo(newPath)
            End If

            If slashCount = 1 Then
                '' This means we only have the drive letter followd by a slash
                newPath = FtpHome
                Await FtpNavigateTo(newPath)
            End If
        End Using
    End Sub


    '' SHORTCUT EVENTS
    Private Async Sub btnShortcut_Click(sender As Object, e As EventArgs)
        Await FtpNavigateToShortcut(DirectCast(sender, Button).Tag)
    End Sub

    Private Sub ftpPinPathButton_Click(sender As Object, e As EventArgs) Handles ftpPinPathButton.Click
        Static index As Integer = 0
        Dim emptyPinSlot As FtpShortcut

        If Not ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned1) Then
            emptyPinSlot = FtpShortcut.Pinned1
        ElseIf Not ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned2) Then
            emptyPinSlot = FtpShortcut.Pinned2
        ElseIf Not ftpPinnedShortcuts.ContainsKey(FtpShortcut.Pinned3) Then
            emptyPinSlot = FtpShortcut.Pinned3
        Else
            emptyPinSlot = index + FtpShortcut.Pinned1
            index += 1
            If emptyPinSlot = FtpShortcut.Pinned3 Then
                index = 0
            End If
        End If

        '' Add the current path without the slash because
        '' Path.GetFileName() returns empty 
        ftpPinnedShortcuts(emptyPinSlot) = ftpCurrentPath.TrimEnd("\")
        PopulateFtpShortcuts()
    End Sub

    Private Async Sub ftpBtnNewFolder_Click(sender As Object, e As EventArgs) Handles ftpBtnNewFolder.Click
        Dim folderName As String = InputBox("Create folder ", "New", "default")
        Dim folderPath As String = ftpCurrentPath

        Using sync = New ControlResource(syncUse)
            If folderName = "default" Or folderName = "" Then Exit Sub
            folderPath += folderName

            Await component.SendHeader(CreateHeader(DataTypes.create_folder, 0, StructurePacketHeader.CreatePacketParam("wantedFile", folderPath)), Nothing)

            '' Trigger a refresh
            Await FtpNavigateTo(ftpCurrentPath)
        End Using
    End Sub
    Private Async Sub ftpBtnNewFile_Click(sender As Object, e As EventArgs) Handles ftpBtnNewFile.Click
        Dim fileName As String = InputBox("Create file ", "New", "default")
        Dim filePath As String = ftpCurrentPath

        Using sync = New ControlResource(syncUse)
            If fileName = "default" Or fileName = "" Then Exit Sub
            filePath += fileName

            Await component.SendHeader(CreateHeader(DataTypes.create_file, 0, StructurePacketHeader.CreatePacketParam("wantedFile", filePath)), Nothing)

            '' Trigger a refresh
            Await FtpNavigateTo(ftpCurrentPath)
        End Using
    End Sub

    Private Async Sub btnFtpRun_Click(sender As Object, e As EventArgs) Handles btnFtpRun.Click
        Using sync = New ControlResource(syncUse)
            For Each lvItem As ListViewItem In ftpView.SelectedItems
                Dim fileData As FileData = lvItem.Tag

                If fileData.TypeTag = FileDataTag.File Then
                    Dim filePath As String = ftpCurrentPath + lvItem.Text

                    Await component.SendHeader(CreateHeader(DataTypes.run, 0, StructurePacketHeader.CreatePacketParam("wantedFile", filePath)), Nothing)
                End If
            Next
        End Using
    End Sub
    Private Async Sub btnFtpRunAs_Click(sender As Object, e As EventArgs) Handles btnFtpRunAs.Click
        Using sync = New ControlResource(syncUse)
            For Each lvItem As ListViewItem In ftpView.SelectedItems
                Dim fileData As FileData = lvItem.Tag

                If fileData.TypeTag = FileDataTag.File Then
                    Dim filePath As String = ftpCurrentPath + lvItem.Text
                    Dim params As String

                    params = StructurePacketHeader.CreatePacketParam("wantedFile", filePath)
                    params += StructurePacketHeader.CreatePacketParam("invokeAdmin", "true")

                    Await component.SendHeader(CreateHeader(DataTypes.run, 0, params), Nothing)
                End If
            Next
        End Using
    End Sub

    Private Async Sub ftpBtnDelete_Click(sender As Object, e As EventArgs) Handles ftpBtnDelete.Click
        Using sync = New ControlResource(syncUse)
            For Each lvItem As ListViewItem In ftpView.SelectedItems
                Dim fileData As FileData = lvItem.Tag

                If fileData.TypeTag = FileDataTag.File Then
                    Dim filePath As String = ftpCurrentPath + lvItem.Text
                    Dim params As String = StructurePacketHeader.CreatePacketParam("wantedFile", filePath)

                    Await component.SendHeader(CreateHeader(DataTypes.delete_file, 0, params), Nothing)
                End If
                If fileData.TypeTag = FileDataTag.Folder Then
                    Dim filePath As String = ftpCurrentPath + lvItem.Text
                    Dim params As String = StructurePacketHeader.CreatePacketParam("wantedFile", filePath)

                    Await component.SendHeader(CreateHeader(DataTypes.delete_folder, 0, params), Nothing)
                End If
            Next

            '' Trigger a refresh
            Await FtpNavigateTo(ftpCurrentPath)
        End Using
    End Sub
    Private Async Sub ftpBtnRefresh_Click(sender As Object, e As EventArgs) Handles ftpBtnRefresh.Click
        Await FtpNavigateTo(ftpCurrentPath)
    End Sub
    Private Async Sub ftpBtnRename_Click(sender As Object, e As EventArgs) Handles ftpBtnRename.Click
        Dim newFileName As String
        Dim selectedItem As Object = ftpView.SelectedItems(0)
        Dim newFilePath As String
        Dim fileData As FileData = selectedItem.Tag
        Dim warning As String = ""

        Using sync = New ControlResource(syncUse)
            If ftpView.SelectedItems.Count > 1 Then
                warning = "WARNING! Cannot rename multiple files" + vbNewLine
            End If

            newFileName = InputBox(warning + "New file name ", "New", selectedItem.Text)
            newFilePath = ftpCurrentPath + newFileName

            If fileData.TypeTag = FileDataTag.File Or fileData.TypeTag = FileDataTag.Folder Then
                Dim oldFilePath As String = ftpCurrentPath + selectedItem.Text
                Dim params As String

                params = StructurePacketHeader.CreatePacketParam("wantedFile", oldFilePath)
                params += StructurePacketHeader.CreatePacketParam("newName", newFilePath)

                Await component.SendHeader(CreateHeader(DataTypes.rename, 0, params), Nothing)
            End If

            '' Trigger a refresh
            Await FtpNavigateTo(ftpCurrentPath)
        End Using
    End Sub
#End Region


End Class
