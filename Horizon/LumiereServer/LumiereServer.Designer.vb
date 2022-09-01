<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LumiereServer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(LumiereServer))
        Me.mainMenu = New System.Windows.Forms.MenuStrip()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EeererToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnViewClientLogs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HelpToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HorizonDovaLumiereSombreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.btnSwitchOut = New System.Windows.Forms.Button()
        Me.btnDestroy = New System.Windows.Forms.Button()
        Me.btnCam = New System.Windows.Forms.Button()
        Me.btnBan = New System.Windows.Forms.Button()
        Me.btnLocalDir = New System.Windows.Forms.Button()
        Me.btnLive = New System.Windows.Forms.Button()
        Me.Panel5 = New System.Windows.Forms.Panel()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.rtfCmdOuput = New System.Windows.Forms.RichTextBox()
        Me.shellMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.shellBtnCancel = New System.Windows.Forms.ToolStripMenuItem()
        Me.panelDebugView = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.debugLowLevelView = New System.Windows.Forms.RichTextBox()
        Me.debugView = New System.Windows.Forms.RichTextBox()
        Me.Panel13 = New System.Windows.Forms.Panel()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.iconList = New System.Windows.Forms.ImageList(Me.components)
        Me.lblColumnFtp3 = New System.Windows.Forms.Label()
        Me.lblColumnFtp2 = New System.Windows.Forms.Label()
        Me.lblColumnFtp1 = New System.Windows.Forms.Label()
        Me.ftpUrl = New System.Windows.Forms.TextBox()
        Me.btnFtpDownloadInterrupt = New System.Windows.Forms.Button()
        Me.ftpFileMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.btnFtpOpen = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ftpBtnDownloadFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpBtnRename = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ftpBtnDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.btnFtpRun = New System.Windows.Forms.ToolStripMenuItem()
        Me.btnFtpRunAs = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpBackgroundMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ftpBtnRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpNewToolstrip = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpBtnNewFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpBtnNewFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpBtnUpload = New System.Windows.Forms.ToolStripMenuItem()
        Me.ftpPinPathButton = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel12 = New System.Windows.Forms.Panel()
        Me.btnFtpUploadInterrupt = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnShortcutPin3 = New System.Windows.Forms.Button()
        Me.btnShortcutPin2 = New System.Windows.Forms.Button()
        Me.btnShortcutPin1 = New System.Windows.Forms.Button()
        Me.btnShortcutHome = New System.Windows.Forms.Button()
        Me.btnShortcutRecycleBin = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.btnShortcutDesktop = New System.Windows.Forms.Button()
        Me.btnShortcutWindows = New System.Windows.Forms.Button()
        Me.btnShortcutTemp = New System.Windows.Forms.Button()
        Me.btnShortcutAppData = New System.Windows.Forms.Button()
        Me.btnShortcutStartup = New System.Windows.Forms.Button()
        Me.btnShortcutStartupAll = New System.Windows.Forms.Button()
        Me.Panel14 = New System.Windows.Forms.Panel()
        Me.ftpBtnBack = New System.Windows.Forms.Button()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.lblUploadName = New System.Windows.Forms.Label()
        Me.lblDownloadName = New System.Windows.Forms.Label()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.ftpUploadProgressBar = New MetroFramework.Controls.MetroProgressBar()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.ftpDownloadProgressBar = New MetroFramework.Controls.MetroProgressBar()
        Me.openFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.panelComponentsList = New MetroFramework.Controls.MetroTabControl()
        Me.MetroTabPage2 = New MetroFramework.Controls.MetroTabPage()
        Me.ftpView = New System.Windows.Forms.ListView()
        Me.clmnName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmnSize = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.clmnLastWriteTime = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ftpOperationHistory = New System.Windows.Forms.ListBox()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.lblActivity = New System.Windows.Forms.Label()
        Me.MetroTabPage1 = New MetroFramework.Controls.MetroTabPage()
        Me.txtCmdInput = New System.Windows.Forms.TextBox()
        Me.cmdHistoryList = New System.Windows.Forms.ListBox()
        Me.MetroTabPage3 = New MetroFramework.Controls.MetroTabPage()
        Me.Panel10 = New System.Windows.Forms.Panel()
        Me.Label17 = New System.Windows.Forms.Label()
        Me.Label19 = New System.Windows.Forms.Label()
        Me.Label21 = New System.Windows.Forms.Label()
        Me.Label22 = New System.Windows.Forms.Label()
        Me.regValueHolder = New System.Windows.Forms.ListView()
        Me.rgClmnName = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.rgClmnType = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.rgClmnData = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.regKeyHolder = New System.Windows.Forms.TreeView()
        Me.txtEditStructureUserDescriptor = New System.Windows.Forms.TextBox()
        Me.panelEditStructureUserDescriptor = New System.Windows.Forms.Panel()
        Me.regKeyMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.regBtnKeyRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.regBtnKeyDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.regBtnKeyNewSubkey = New System.Windows.Forms.ToolStripMenuItem()
        Me.regValueMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.regBtnValueRefresh = New System.Windows.Forms.ToolStripMenuItem()
        Me.regBtnValueDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.regBtnValueNewValue = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.clientsControlBoard = New MetroFramework.Controls.MetroTabControl()
        Me.clientInfoTab = New MetroFramework.Controls.MetroTabPage()
        Me.btnForce7 = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TextBox7 = New System.Windows.Forms.TextBox()
        Me.btnForce6 = New System.Windows.Forms.Button()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.btnForce5 = New System.Windows.Forms.Button()
        Me.Label24 = New System.Windows.Forms.Label()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.btnForce4 = New System.Windows.Forms.Button()
        Me.btnForce3 = New System.Windows.Forms.Button()
        Me.btnForce2 = New System.Windows.Forms.Button()
        Me.btnForce1 = New System.Windows.Forms.Button()
        Me.Label25 = New System.Windows.Forms.Label()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label26 = New System.Windows.Forms.Label()
        Me.TextBox4 = New System.Windows.Forms.TextBox()
        Me.Label27 = New System.Windows.Forms.Label()
        Me.Label28 = New System.Windows.Forms.Label()
        Me.TextBox5 = New System.Windows.Forms.TextBox()
        Me.TextBox6 = New System.Windows.Forms.TextBox()
        Me.txtUserInfoOs = New System.Windows.Forms.TextBox()
        Me.txtUserInfoVersion = New System.Windows.Forms.TextBox()
        Me.txtUserInfoPc = New System.Windows.Forms.TextBox()
        Me.txtUserInfoExtraData = New System.Windows.Forms.TextBox()
        Me.txtUserInfoAlias = New System.Windows.Forms.TextBox()
        Me.txtUserInfoName = New System.Windows.Forms.TextBox()
        Me.txtUserInfoId = New System.Windows.Forms.TextBox()
        Me.Label29 = New System.Windows.Forms.Label()
        Me.Label30 = New System.Windows.Forms.Label()
        Me.Label31 = New System.Windows.Forms.Label()
        Me.Label32 = New System.Windows.Forms.Label()
        Me.Label33 = New System.Windows.Forms.Label()
        Me.Label34 = New System.Windows.Forms.Label()
        Me.Label35 = New System.Windows.Forms.Label()
        Me.clientListTab = New MetroFramework.Controls.MetroTabPage()
        Me.clientList = New System.Windows.Forms.ListBox()
        Me.PingingTimer = New System.Windows.Forms.Timer(Me.components)
        Me.mainMenu.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel5.SuspendLayout()
        Me.shellMenu.SuspendLayout()
        Me.panelDebugView.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel13.SuspendLayout()
        Me.ftpFileMenu.SuspendLayout()
        Me.ftpBackgroundMenu.SuspendLayout()
        Me.Panel12.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.Panel14.SuspendLayout()
        Me.Panel6.SuspendLayout()
        Me.Panel8.SuspendLayout()
        Me.Panel7.SuspendLayout()
        Me.panelComponentsList.SuspendLayout()
        Me.MetroTabPage2.SuspendLayout()
        Me.Panel4.SuspendLayout()
        Me.MetroTabPage1.SuspendLayout()
        Me.MetroTabPage3.SuspendLayout()
        Me.Panel10.SuspendLayout()
        Me.panelEditStructureUserDescriptor.SuspendLayout()
        Me.regKeyMenu.SuspendLayout()
        Me.regValueMenu.SuspendLayout()
        Me.clientsControlBoard.SuspendLayout()
        Me.clientInfoTab.SuspendLayout()
        Me.clientListTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu
        '
        Me.mainMenu.BackColor = System.Drawing.Color.DimGray
        Me.mainMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.mainMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mainMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TestToolStripMenuItem})
        Me.mainMenu.Location = New System.Drawing.Point(0, 0)
        Me.mainMenu.Name = "mainMenu"
        Me.mainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.mainMenu.Size = New System.Drawing.Size(1803, 24)
        Me.mainMenu.TabIndex = 6
        Me.mainMenu.Text = "MenuStrip1"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EeererToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(46, 20)
        Me.TestToolStripMenuItem.Text = "Tools"
        '
        'EeererToolStripMenuItem
        '
        Me.EeererToolStripMenuItem.Name = "EeererToolStripMenuItem"
        Me.EeererToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.EeererToolStripMenuItem.Text = "Show Blacklist"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.BackColor = System.Drawing.Color.Black
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnViewClientLogs, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 22)
        Me.FileToolStripMenuItem.Text = "&File"
        '
        'btnViewClientLogs
        '
        Me.btnViewClientLogs.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.btnViewClientLogs.ForeColor = System.Drawing.Color.White
        Me.btnViewClientLogs.Name = "btnViewClientLogs"
        Me.btnViewClientLogs.Size = New System.Drawing.Size(129, 22)
        Me.btnViewClientLogs.Text = "&View users"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ExitToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(129, 22)
        Me.ExitToolStripMenuItem.Text = "&Exit"
        '
        'HelpToolStripMenuItem
        '
        Me.HelpToolStripMenuItem.ForeColor = System.Drawing.Color.White
        Me.HelpToolStripMenuItem.Name = "HelpToolStripMenuItem"
        Me.HelpToolStripMenuItem.Size = New System.Drawing.Size(44, 22)
        Me.HelpToolStripMenuItem.Text = "&Help"
        '
        'HorizonDovaLumiereSombreToolStripMenuItem
        '
        Me.HorizonDovaLumiereSombreToolStripMenuItem.Font = New System.Drawing.Font("Corbel", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.HorizonDovaLumiereSombreToolStripMenuItem.ForeColor = System.Drawing.Color.Red
        Me.HorizonDovaLumiereSombreToolStripMenuItem.Name = "HorizonDovaLumiereSombreToolStripMenuItem"
        Me.HorizonDovaLumiereSombreToolStripMenuItem.Size = New System.Drawing.Size(300, 22)
        Me.HorizonDovaLumiereSombreToolStripMenuItem.Text = "Horizon Dova ┌∩┐(◣_◢)┌∩┐ LumiereSombre"
        '
        'Panel2
        '
        Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Panel2.Controls.Add(Me.btnSwitchOut)
        Me.Panel2.Controls.Add(Me.btnDestroy)
        Me.Panel2.Controls.Add(Me.btnCam)
        Me.Panel2.Controls.Add(Me.btnBan)
        Me.Panel2.Controls.Add(Me.btnLocalDir)
        Me.Panel2.Controls.Add(Me.btnLive)
        Me.Panel2.Controls.Add(Me.Panel5)
        Me.Panel2.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.Panel2.Location = New System.Drawing.Point(12, 421)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(284, 170)
        Me.Panel2.TabIndex = 9
        '
        'btnSwitchOut
        '
        Me.btnSwitchOut.BackColor = System.Drawing.Color.Firebrick
        Me.btnSwitchOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnSwitchOut.FlatAppearance.BorderSize = 0
        Me.btnSwitchOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSwitchOut.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnSwitchOut.ForeColor = System.Drawing.Color.White
        Me.btnSwitchOut.Location = New System.Drawing.Point(158, 90)
        Me.btnSwitchOut.Name = "btnSwitchOut"
        Me.btnSwitchOut.Size = New System.Drawing.Size(92, 22)
        Me.btnSwitchOut.TabIndex = 30
        Me.btnSwitchOut.Text = "Switch Out"
        Me.btnSwitchOut.UseVisualStyleBackColor = False
        '
        'btnDestroy
        '
        Me.btnDestroy.BackColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.btnDestroy.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnDestroy.FlatAppearance.BorderSize = 0
        Me.btnDestroy.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnDestroy.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnDestroy.ForeColor = System.Drawing.Color.White
        Me.btnDestroy.Location = New System.Drawing.Point(158, 118)
        Me.btnDestroy.Name = "btnDestroy"
        Me.btnDestroy.Size = New System.Drawing.Size(92, 22)
        Me.btnDestroy.TabIndex = 29
        Me.btnDestroy.Tag = ""
        Me.btnDestroy.Text = "Destroy"
        Me.btnDestroy.UseVisualStyleBackColor = False
        '
        'btnCam
        '
        Me.btnCam.BackColor = System.Drawing.Color.Firebrick
        Me.btnCam.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnCam.FlatAppearance.BorderSize = 0
        Me.btnCam.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCam.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnCam.ForeColor = System.Drawing.Color.White
        Me.btnCam.Location = New System.Drawing.Point(16, 88)
        Me.btnCam.Name = "btnCam"
        Me.btnCam.Size = New System.Drawing.Size(135, 22)
        Me.btnCam.TabIndex = 28
        Me.btnCam.Text = "Camera"
        Me.btnCam.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCam.UseVisualStyleBackColor = False
        '
        'btnBan
        '
        Me.btnBan.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.btnBan.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnBan.FlatAppearance.BorderSize = 0
        Me.btnBan.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnBan.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnBan.ForeColor = System.Drawing.Color.White
        Me.btnBan.Location = New System.Drawing.Point(158, 62)
        Me.btnBan.Name = "btnBan"
        Me.btnBan.Size = New System.Drawing.Size(92, 22)
        Me.btnBan.TabIndex = 27
        Me.btnBan.Tag = ""
        Me.btnBan.Text = "Ban"
        Me.btnBan.UseVisualStyleBackColor = False
        '
        'btnLocalDir
        '
        Me.btnLocalDir.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.btnLocalDir.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnLocalDir.FlatAppearance.BorderSize = 0
        Me.btnLocalDir.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLocalDir.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnLocalDir.ForeColor = System.Drawing.Color.White
        Me.btnLocalDir.Location = New System.Drawing.Point(16, 118)
        Me.btnLocalDir.Name = "btnLocalDir"
        Me.btnLocalDir.Size = New System.Drawing.Size(136, 22)
        Me.btnLocalDir.TabIndex = 26
        Me.btnLocalDir.Text = "View Local Files"
        Me.btnLocalDir.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLocalDir.UseVisualStyleBackColor = False
        '
        'btnLive
        '
        Me.btnLive.BackColor = System.Drawing.Color.Firebrick
        Me.btnLive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnLive.FlatAppearance.BorderSize = 0
        Me.btnLive.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLive.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.btnLive.ForeColor = System.Drawing.Color.White
        Me.btnLive.Location = New System.Drawing.Point(16, 62)
        Me.btnLive.Name = "btnLive"
        Me.btnLive.Size = New System.Drawing.Size(135, 22)
        Me.btnLive.TabIndex = 24
        Me.btnLive.Text = "Live Stream"
        Me.btnLive.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnLive.UseVisualStyleBackColor = False
        '
        'Panel5
        '
        Me.Panel5.BackColor = System.Drawing.Color.Black
        Me.Panel5.Controls.Add(Me.Label10)
        Me.Panel5.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel5.Location = New System.Drawing.Point(0, 0)
        Me.Panel5.Name = "Panel5"
        Me.Panel5.Size = New System.Drawing.Size(284, 37)
        Me.Panel5.TabIndex = 22
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.Label10.ForeColor = System.Drawing.Color.Silver
        Me.Label10.Location = New System.Drawing.Point(13, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(99, 16)
        Me.Label10.TabIndex = 0
        Me.Label10.Text = "&Active payloads"
        '
        'rtfCmdOuput
        '
        Me.rtfCmdOuput.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.rtfCmdOuput.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtfCmdOuput.CausesValidation = False
        Me.rtfCmdOuput.ContextMenuStrip = Me.shellMenu
        Me.rtfCmdOuput.DetectUrls = False
        Me.rtfCmdOuput.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtfCmdOuput.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.rtfCmdOuput.HideSelection = False
        Me.rtfCmdOuput.Location = New System.Drawing.Point(5, 7)
        Me.rtfCmdOuput.Name = "rtfCmdOuput"
        Me.rtfCmdOuput.ReadOnly = True
        Me.rtfCmdOuput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.rtfCmdOuput.Size = New System.Drawing.Size(1223, 445)
        Me.rtfCmdOuput.TabIndex = 23
        Me.rtfCmdOuput.Text = "                                    Horizon Dova (◣_◢) LumiereSombre" & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'shellMenu
        '
        Me.shellMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.shellMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.shellMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.shellBtnCancel})
        Me.shellMenu.Name = "ftpMenuEmptyClick"
        Me.shellMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.shellMenu.Size = New System.Drawing.Size(111, 26)
        '
        'shellBtnCancel
        '
        Me.shellBtnCancel.ForeColor = System.Drawing.Color.White
        Me.shellBtnCancel.Name = "shellBtnCancel"
        Me.shellBtnCancel.Size = New System.Drawing.Size(110, 22)
        Me.shellBtnCancel.Text = "&Cancel"
        '
        'panelDebugView
        '
        Me.panelDebugView.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.panelDebugView.Controls.Add(Me.TableLayoutPanel1)
        Me.panelDebugView.Controls.Add(Me.Panel13)
        Me.panelDebugView.Location = New System.Drawing.Point(12, 597)
        Me.panelDebugView.Name = "panelDebugView"
        Me.panelDebugView.Size = New System.Drawing.Size(1771, 179)
        Me.panelDebugView.TabIndex = 13
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.debugLowLevelView, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.debugView, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 31)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1771, 148)
        Me.TableLayoutPanel1.TabIndex = 24
        '
        'debugLowLevelView
        '
        Me.debugLowLevelView.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.debugLowLevelView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.debugLowLevelView.CausesValidation = False
        Me.debugLowLevelView.DetectUrls = False
        Me.debugLowLevelView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.debugLowLevelView.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.debugLowLevelView.ForeColor = System.Drawing.Color.White
        Me.debugLowLevelView.Location = New System.Drawing.Point(888, 3)
        Me.debugLowLevelView.Name = "debugLowLevelView"
        Me.debugLowLevelView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.debugLowLevelView.Size = New System.Drawing.Size(880, 142)
        Me.debugLowLevelView.TabIndex = 24
        Me.debugLowLevelView.Text = ""
        '
        'debugView
        '
        Me.debugView.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.debugView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.debugView.CausesValidation = False
        Me.debugView.DetectUrls = False
        Me.debugView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.debugView.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.debugView.ForeColor = System.Drawing.Color.White
        Me.debugView.Location = New System.Drawing.Point(3, 3)
        Me.debugView.Name = "debugView"
        Me.debugView.ReadOnly = True
        Me.debugView.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical
        Me.debugView.Size = New System.Drawing.Size(879, 142)
        Me.debugView.TabIndex = 23
        Me.debugView.Text = ""
        '
        'Panel13
        '
        Me.Panel13.BackColor = System.Drawing.Color.Black
        Me.Panel13.Controls.Add(Me.Label14)
        Me.Panel13.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel13.Location = New System.Drawing.Point(0, 0)
        Me.Panel13.Name = "Panel13"
        Me.Panel13.Size = New System.Drawing.Size(1771, 31)
        Me.Panel13.TabIndex = 22
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.Label14.ForeColor = System.Drawing.Color.Silver
        Me.Label14.Location = New System.Drawing.Point(13, 10)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(113, 16)
        Me.Label14.TabIndex = 0
        Me.Label14.Text = "&Internal debugger"
        '
        'iconList
        '
        Me.iconList.ImageStream = CType(resources.GetObject("iconList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.iconList.TransparentColor = System.Drawing.Color.Transparent
        Me.iconList.Images.SetKeyName(0, "folder.png")
        Me.iconList.Images.SetKeyName(1, "file.png")
        Me.iconList.Images.SetKeyName(2, "ico205.ico")
        '
        'lblColumnFtp3
        '
        Me.lblColumnFtp3.AutoSize = True
        Me.lblColumnFtp3.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.lblColumnFtp3.ForeColor = System.Drawing.Color.Gray
        Me.lblColumnFtp3.Location = New System.Drawing.Point(679, 26)
        Me.lblColumnFtp3.Name = "lblColumnFtp3"
        Me.lblColumnFtp3.Size = New System.Drawing.Size(112, 15)
        Me.lblColumnFtp3.TabIndex = 7
        Me.lblColumnFtp3.Text = "Last Write Time"
        '
        'lblColumnFtp2
        '
        Me.lblColumnFtp2.AutoSize = True
        Me.lblColumnFtp2.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.lblColumnFtp2.ForeColor = System.Drawing.Color.Gray
        Me.lblColumnFtp2.Location = New System.Drawing.Point(543, 26)
        Me.lblColumnFtp2.Name = "lblColumnFtp2"
        Me.lblColumnFtp2.Size = New System.Drawing.Size(35, 15)
        Me.lblColumnFtp2.TabIndex = 6
        Me.lblColumnFtp2.Text = "Size"
        '
        'lblColumnFtp1
        '
        Me.lblColumnFtp1.AutoSize = True
        Me.lblColumnFtp1.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.lblColumnFtp1.ForeColor = System.Drawing.Color.Gray
        Me.lblColumnFtp1.Location = New System.Drawing.Point(6, 26)
        Me.lblColumnFtp1.Name = "lblColumnFtp1"
        Me.lblColumnFtp1.Size = New System.Drawing.Size(77, 15)
        Me.lblColumnFtp1.TabIndex = 5
        Me.lblColumnFtp1.Text = "File Names"
        '
        'ftpUrl
        '
        Me.ftpUrl.BackColor = System.Drawing.Color.Black
        Me.ftpUrl.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ftpUrl.Font = New System.Drawing.Font("Verdana", 8.25!)
        Me.ftpUrl.ForeColor = System.Drawing.Color.White
        Me.ftpUrl.Location = New System.Drawing.Point(10, 5)
        Me.ftpUrl.Name = "ftpUrl"
        Me.ftpUrl.ShortcutsEnabled = False
        Me.ftpUrl.Size = New System.Drawing.Size(867, 14)
        Me.ftpUrl.TabIndex = 2
        Me.ftpUrl.Text = "\"
        Me.ftpUrl.WordWrap = False
        '
        'btnFtpDownloadInterrupt
        '
        Me.btnFtpDownloadInterrupt.BackColor = System.Drawing.Color.Black
        Me.btnFtpDownloadInterrupt.FlatAppearance.BorderSize = 0
        Me.btnFtpDownloadInterrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFtpDownloadInterrupt.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.btnFtpDownloadInterrupt.ForeColor = System.Drawing.Color.Silver
        Me.btnFtpDownloadInterrupt.Location = New System.Drawing.Point(6, 473)
        Me.btnFtpDownloadInterrupt.Name = "btnFtpDownloadInterrupt"
        Me.btnFtpDownloadInterrupt.Size = New System.Drawing.Size(121, 22)
        Me.btnFtpDownloadInterrupt.TabIndex = 24
        Me.btnFtpDownloadInterrupt.Text = "&Cancel download"
        Me.btnFtpDownloadInterrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFtpDownloadInterrupt.UseVisualStyleBackColor = False
        '
        'ftpFileMenu
        '
        Me.ftpFileMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpFileMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ftpFileMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.btnFtpOpen, Me.ToolStripSeparator2, Me.ftpBtnDownloadFile, Me.ftpBtnRename, Me.ToolStripSeparator1, Me.ftpBtnDelete, Me.ToolStripSeparator3, Me.btnFtpRun, Me.btnFtpRunAs})
        Me.ftpFileMenu.Name = "ftpMenu"
        Me.ftpFileMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ftpFileMenu.Size = New System.Drawing.Size(151, 154)
        '
        'btnFtpOpen
        '
        Me.btnFtpOpen.ForeColor = System.Drawing.Color.White
        Me.btnFtpOpen.Name = "btnFtpOpen"
        Me.btnFtpOpen.Size = New System.Drawing.Size(150, 22)
        Me.btnFtpOpen.Text = "&Open"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(147, 6)
        '
        'ftpBtnDownloadFile
        '
        Me.ftpBtnDownloadFile.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpBtnDownloadFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ftpBtnDownloadFile.ForeColor = System.Drawing.Color.White
        Me.ftpBtnDownloadFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ftpBtnDownloadFile.Name = "ftpBtnDownloadFile"
        Me.ftpBtnDownloadFile.Size = New System.Drawing.Size(150, 22)
        Me.ftpBtnDownloadFile.Text = "Download"
        Me.ftpBtnDownloadFile.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage
        '
        'ftpBtnRename
        '
        Me.ftpBtnRename.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpBtnRename.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ftpBtnRename.ForeColor = System.Drawing.Color.White
        Me.ftpBtnRename.Name = "ftpBtnRename"
        Me.ftpBtnRename.Size = New System.Drawing.Size(150, 22)
        Me.ftpBtnRename.Text = "Rename"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(147, 6)
        '
        'ftpBtnDelete
        '
        Me.ftpBtnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpBtnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ftpBtnDelete.ForeColor = System.Drawing.Color.White
        Me.ftpBtnDelete.Name = "ftpBtnDelete"
        Me.ftpBtnDelete.Size = New System.Drawing.Size(150, 22)
        Me.ftpBtnDelete.Text = "Delete"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(147, 6)
        '
        'btnFtpRun
        '
        Me.btnFtpRun.ForeColor = System.Drawing.Color.White
        Me.btnFtpRun.Name = "btnFtpRun"
        Me.btnFtpRun.Size = New System.Drawing.Size(150, 22)
        Me.btnFtpRun.Text = "&Run"
        '
        'btnFtpRunAs
        '
        Me.btnFtpRunAs.ForeColor = System.Drawing.Color.White
        Me.btnFtpRunAs.Name = "btnFtpRunAs"
        Me.btnFtpRunAs.Size = New System.Drawing.Size(150, 22)
        Me.btnFtpRunAs.Text = "&Run As Admin"
        '
        'ftpBackgroundMenu
        '
        Me.ftpBackgroundMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpBackgroundMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ftpBackgroundMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ftpBtnRefresh, Me.ftpNewToolstrip, Me.ftpBtnUpload, Me.ftpPinPathButton})
        Me.ftpBackgroundMenu.Name = "ftpMenuEmptyClick"
        Me.ftpBackgroundMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ftpBackgroundMenu.Size = New System.Drawing.Size(119, 92)
        '
        'ftpBtnRefresh
        '
        Me.ftpBtnRefresh.ForeColor = System.Drawing.Color.White
        Me.ftpBtnRefresh.Name = "ftpBtnRefresh"
        Me.ftpBtnRefresh.Size = New System.Drawing.Size(118, 22)
        Me.ftpBtnRefresh.Text = "&Refresh"
        '
        'ftpNewToolstrip
        '
        Me.ftpNewToolstrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpNewToolstrip.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ftpNewToolstrip.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ftpBtnNewFolder, Me.ftpBtnNewFile})
        Me.ftpNewToolstrip.ForeColor = System.Drawing.Color.White
        Me.ftpNewToolstrip.Name = "ftpNewToolstrip"
        Me.ftpNewToolstrip.Size = New System.Drawing.Size(118, 22)
        Me.ftpNewToolstrip.Text = "&New"
        '
        'ftpBtnNewFolder
        '
        Me.ftpBtnNewFolder.Name = "ftpBtnNewFolder"
        Me.ftpBtnNewFolder.Size = New System.Drawing.Size(107, 22)
        Me.ftpBtnNewFolder.Text = "Folder"
        '
        'ftpBtnNewFile
        '
        Me.ftpBtnNewFile.Name = "ftpBtnNewFile"
        Me.ftpBtnNewFile.Size = New System.Drawing.Size(107, 22)
        Me.ftpBtnNewFile.Text = "File"
        '
        'ftpBtnUpload
        '
        Me.ftpBtnUpload.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ftpBtnUpload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ftpBtnUpload.ForeColor = System.Drawing.Color.White
        Me.ftpBtnUpload.Name = "ftpBtnUpload"
        Me.ftpBtnUpload.Size = New System.Drawing.Size(118, 22)
        Me.ftpBtnUpload.Text = "&Upload"
        '
        'ftpPinPathButton
        '
        Me.ftpPinPathButton.ForeColor = System.Drawing.Color.White
        Me.ftpPinPathButton.Name = "ftpPinPathButton"
        Me.ftpPinPathButton.Size = New System.Drawing.Size(118, 22)
        Me.ftpPinPathButton.Text = "Pin path"
        '
        'Panel12
        '
        Me.Panel12.BackColor = System.Drawing.Color.Black
        Me.Panel12.Controls.Add(Me.btnFtpUploadInterrupt)
        Me.Panel12.Controls.Add(Me.btnFtpDownloadInterrupt)
        Me.Panel12.Controls.Add(Me.Panel1)
        Me.Panel12.Controls.Add(Me.Label16)
        Me.Panel12.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel12.Location = New System.Drawing.Point(0, 0)
        Me.Panel12.Margin = New System.Windows.Forms.Padding(0)
        Me.Panel12.Name = "Panel12"
        Me.Panel12.Size = New System.Drawing.Size(147, 515)
        Me.Panel12.TabIndex = 19
        '
        'btnFtpUploadInterrupt
        '
        Me.btnFtpUploadInterrupt.BackColor = System.Drawing.Color.Black
        Me.btnFtpUploadInterrupt.FlatAppearance.BorderSize = 0
        Me.btnFtpUploadInterrupt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnFtpUploadInterrupt.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.btnFtpUploadInterrupt.ForeColor = System.Drawing.Color.Silver
        Me.btnFtpUploadInterrupt.Location = New System.Drawing.Point(6, 491)
        Me.btnFtpUploadInterrupt.Name = "btnFtpUploadInterrupt"
        Me.btnFtpUploadInterrupt.Size = New System.Drawing.Size(112, 22)
        Me.btnFtpUploadInterrupt.TabIndex = 24
        Me.btnFtpUploadInterrupt.Text = "&Cancel upload"
        Me.btnFtpUploadInterrupt.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnFtpUploadInterrupt.UseVisualStyleBackColor = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Controls.Add(Me.btnShortcutPin3)
        Me.Panel1.Controls.Add(Me.btnShortcutPin2)
        Me.Panel1.Controls.Add(Me.btnShortcutPin1)
        Me.Panel1.Controls.Add(Me.btnShortcutHome)
        Me.Panel1.Controls.Add(Me.btnShortcutRecycleBin)
        Me.Panel1.Controls.Add(Me.Label11)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.btnShortcutDesktop)
        Me.Panel1.Controls.Add(Me.btnShortcutWindows)
        Me.Panel1.Controls.Add(Me.btnShortcutTemp)
        Me.Panel1.Controls.Add(Me.btnShortcutAppData)
        Me.Panel1.Controls.Add(Me.btnShortcutStartup)
        Me.Panel1.Controls.Add(Me.btnShortcutStartupAll)
        Me.Panel1.Controls.Add(Me.Panel14)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(147, 439)
        Me.Panel1.TabIndex = 33
        '
        'btnShortcutPin3
        '
        Me.btnShortcutPin3.BackColor = System.Drawing.Color.Black
        Me.btnShortcutPin3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutPin3.FlatAppearance.BorderSize = 0
        Me.btnShortcutPin3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutPin3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutPin3.Location = New System.Drawing.Point(18, 247)
        Me.btnShortcutPin3.Name = "btnShortcutPin3"
        Me.btnShortcutPin3.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutPin3.TabIndex = 39
        Me.btnShortcutPin3.Text = "______________"
        Me.btnShortcutPin3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutPin3.UseVisualStyleBackColor = False
        '
        'btnShortcutPin2
        '
        Me.btnShortcutPin2.BackColor = System.Drawing.Color.Black
        Me.btnShortcutPin2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutPin2.FlatAppearance.BorderSize = 0
        Me.btnShortcutPin2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutPin2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutPin2.Location = New System.Drawing.Point(18, 224)
        Me.btnShortcutPin2.Name = "btnShortcutPin2"
        Me.btnShortcutPin2.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutPin2.TabIndex = 38
        Me.btnShortcutPin2.Text = "______________"
        Me.btnShortcutPin2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutPin2.UseVisualStyleBackColor = False
        '
        'btnShortcutPin1
        '
        Me.btnShortcutPin1.BackColor = System.Drawing.Color.Black
        Me.btnShortcutPin1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutPin1.FlatAppearance.BorderSize = 0
        Me.btnShortcutPin1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutPin1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutPin1.Location = New System.Drawing.Point(18, 197)
        Me.btnShortcutPin1.Name = "btnShortcutPin1"
        Me.btnShortcutPin1.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutPin1.TabIndex = 37
        Me.btnShortcutPin1.Text = "______________"
        Me.btnShortcutPin1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutPin1.UseVisualStyleBackColor = False
        '
        'btnShortcutHome
        '
        Me.btnShortcutHome.BackColor = System.Drawing.Color.Black
        Me.btnShortcutHome.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutHome.FlatAppearance.BorderSize = 0
        Me.btnShortcutHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutHome.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutHome.Location = New System.Drawing.Point(18, 323)
        Me.btnShortcutHome.Name = "btnShortcutHome"
        Me.btnShortcutHome.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutHome.TabIndex = 35
        Me.btnShortcutHome.Text = "&My Computer"
        Me.btnShortcutHome.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutHome.UseVisualStyleBackColor = False
        '
        'btnShortcutRecycleBin
        '
        Me.btnShortcutRecycleBin.BackColor = System.Drawing.Color.Black
        Me.btnShortcutRecycleBin.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutRecycleBin.FlatAppearance.BorderSize = 0
        Me.btnShortcutRecycleBin.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutRecycleBin.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutRecycleBin.Location = New System.Drawing.Point(18, 346)
        Me.btnShortcutRecycleBin.Name = "btnShortcutRecycleBin"
        Me.btnShortcutRecycleBin.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutRecycleBin.TabIndex = 36
        Me.btnShortcutRecycleBin.Text = "&Recycle Bin"
        Me.btnShortcutRecycleBin.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutRecycleBin.UseVisualStyleBackColor = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.ForeColor = System.Drawing.Color.Silver
        Me.Label11.Location = New System.Drawing.Point(10, 304)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(61, 16)
        Me.Label11.TabIndex = 34
        Me.Label11.Text = "Standard"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.Silver
        Me.Label2.Location = New System.Drawing.Point(10, 40)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(62, 16)
        Me.Label2.TabIndex = 33
        Me.Label2.Text = "Shortcuts"
        '
        'btnShortcutDesktop
        '
        Me.btnShortcutDesktop.BackColor = System.Drawing.Color.Black
        Me.btnShortcutDesktop.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutDesktop.FlatAppearance.BorderSize = 0
        Me.btnShortcutDesktop.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutDesktop.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutDesktop.Location = New System.Drawing.Point(18, 59)
        Me.btnShortcutDesktop.Name = "btnShortcutDesktop"
        Me.btnShortcutDesktop.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutDesktop.TabIndex = 27
        Me.btnShortcutDesktop.Text = "&Desktop"
        Me.btnShortcutDesktop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutDesktop.UseVisualStyleBackColor = False
        '
        'btnShortcutWindows
        '
        Me.btnShortcutWindows.BackColor = System.Drawing.Color.Black
        Me.btnShortcutWindows.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutWindows.FlatAppearance.BorderSize = 0
        Me.btnShortcutWindows.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutWindows.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutWindows.Location = New System.Drawing.Point(18, 127)
        Me.btnShortcutWindows.Name = "btnShortcutWindows"
        Me.btnShortcutWindows.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutWindows.TabIndex = 30
        Me.btnShortcutWindows.Text = "&Windows"
        Me.btnShortcutWindows.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutWindows.UseVisualStyleBackColor = False
        '
        'btnShortcutTemp
        '
        Me.btnShortcutTemp.BackColor = System.Drawing.Color.Black
        Me.btnShortcutTemp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutTemp.FlatAppearance.BorderSize = 0
        Me.btnShortcutTemp.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutTemp.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutTemp.Location = New System.Drawing.Point(18, 104)
        Me.btnShortcutTemp.Name = "btnShortcutTemp"
        Me.btnShortcutTemp.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutTemp.TabIndex = 29
        Me.btnShortcutTemp.Text = "&Temp"
        Me.btnShortcutTemp.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutTemp.UseVisualStyleBackColor = False
        '
        'btnShortcutAppData
        '
        Me.btnShortcutAppData.BackColor = System.Drawing.Color.Black
        Me.btnShortcutAppData.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutAppData.FlatAppearance.BorderSize = 0
        Me.btnShortcutAppData.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutAppData.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutAppData.Location = New System.Drawing.Point(18, 82)
        Me.btnShortcutAppData.Name = "btnShortcutAppData"
        Me.btnShortcutAppData.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutAppData.TabIndex = 28
        Me.btnShortcutAppData.Text = "&AppData"
        Me.btnShortcutAppData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutAppData.UseVisualStyleBackColor = False
        '
        'btnShortcutStartup
        '
        Me.btnShortcutStartup.BackColor = System.Drawing.Color.Black
        Me.btnShortcutStartup.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutStartup.FlatAppearance.BorderSize = 0
        Me.btnShortcutStartup.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutStartup.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutStartup.Location = New System.Drawing.Point(18, 150)
        Me.btnShortcutStartup.Name = "btnShortcutStartup"
        Me.btnShortcutStartup.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutStartup.TabIndex = 31
        Me.btnShortcutStartup.Text = "&Startup"
        Me.btnShortcutStartup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutStartup.UseVisualStyleBackColor = False
        '
        'btnShortcutStartupAll
        '
        Me.btnShortcutStartupAll.BackColor = System.Drawing.Color.Black
        Me.btnShortcutStartupAll.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnShortcutStartupAll.FlatAppearance.BorderSize = 0
        Me.btnShortcutStartupAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnShortcutStartupAll.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnShortcutStartupAll.Location = New System.Drawing.Point(18, 172)
        Me.btnShortcutStartupAll.Name = "btnShortcutStartupAll"
        Me.btnShortcutStartupAll.Size = New System.Drawing.Size(114, 22)
        Me.btnShortcutStartupAll.TabIndex = 32
        Me.btnShortcutStartupAll.Text = "&StartupAll"
        Me.btnShortcutStartupAll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnShortcutStartupAll.UseVisualStyleBackColor = False
        '
        'Panel14
        '
        Me.Panel14.BackColor = System.Drawing.Color.Black
        Me.Panel14.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel14.Controls.Add(Me.ftpBtnBack)
        Me.Panel14.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel14.Location = New System.Drawing.Point(0, 0)
        Me.Panel14.Name = "Panel14"
        Me.Panel14.Size = New System.Drawing.Size(147, 30)
        Me.Panel14.TabIndex = 26
        '
        'ftpBtnBack
        '
        Me.ftpBtnBack.BackColor = System.Drawing.Color.Black
        Me.ftpBtnBack.FlatAppearance.BorderColor = System.Drawing.Color.White
        Me.ftpBtnBack.FlatAppearance.BorderSize = 0
        Me.ftpBtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.ftpBtnBack.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.ftpBtnBack.ForeColor = System.Drawing.Color.Silver
        Me.ftpBtnBack.Location = New System.Drawing.Point(8, 3)
        Me.ftpBtnBack.Name = "ftpBtnBack"
        Me.ftpBtnBack.Size = New System.Drawing.Size(133, 22)
        Me.ftpBtnBack.TabIndex = 25
        Me.ftpBtnBack.Text = "&Go back"
        Me.ftpBtnBack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ftpBtnBack.UseVisualStyleBackColor = False
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.Label16.ForeColor = System.Drawing.Color.White
        Me.Label16.Location = New System.Drawing.Point(10, 46)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(0, 15)
        Me.Label16.TabIndex = 2
        Me.Label16.Tag = ""
        '
        'Panel6
        '
        Me.Panel6.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Panel6.Controls.Add(Me.lblUploadName)
        Me.Panel6.Controls.Add(Me.lblDownloadName)
        Me.Panel6.Controls.Add(Me.Panel8)
        Me.Panel6.Controls.Add(Me.Panel7)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel6.Location = New System.Drawing.Point(147, 477)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(1329, 38)
        Me.Panel6.TabIndex = 34
        '
        'lblUploadName
        '
        Me.lblUploadName.AutoSize = True
        Me.lblUploadName.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblUploadName.ForeColor = System.Drawing.Color.Silver
        Me.lblUploadName.Location = New System.Drawing.Point(298, 20)
        Me.lblUploadName.Name = "lblUploadName"
        Me.lblUploadName.Size = New System.Drawing.Size(0, 16)
        Me.lblUploadName.TabIndex = 37
        '
        'lblDownloadName
        '
        Me.lblDownloadName.AutoSize = True
        Me.lblDownloadName.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDownloadName.ForeColor = System.Drawing.Color.Silver
        Me.lblDownloadName.Location = New System.Drawing.Point(298, 3)
        Me.lblDownloadName.Name = "lblDownloadName"
        Me.lblDownloadName.Size = New System.Drawing.Size(0, 16)
        Me.lblDownloadName.TabIndex = 36
        '
        'Panel8
        '
        Me.Panel8.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Panel8.Controls.Add(Me.ftpUploadProgressBar)
        Me.Panel8.Location = New System.Drawing.Point(12, 20)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(274, 13)
        Me.Panel8.TabIndex = 30
        '
        'ftpUploadProgressBar
        '
        Me.ftpUploadProgressBar.Location = New System.Drawing.Point(-3, -2)
        Me.ftpUploadProgressBar.Name = "ftpUploadProgressBar"
        Me.ftpUploadProgressBar.Size = New System.Drawing.Size(281, 16)
        Me.ftpUploadProgressBar.Style = MetroFramework.MetroColorStyle.Red
        Me.ftpUploadProgressBar.TabIndex = 39
        Me.ftpUploadProgressBar.Theme = MetroFramework.MetroThemeStyle.Dark
        Me.ftpUploadProgressBar.UseCustomBackColor = True
        '
        'Panel7
        '
        Me.Panel7.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Panel7.Controls.Add(Me.ftpDownloadProgressBar)
        Me.Panel7.Location = New System.Drawing.Point(12, 4)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(274, 13)
        Me.Panel7.TabIndex = 29
        '
        'ftpDownloadProgressBar
        '
        Me.ftpDownloadProgressBar.Location = New System.Drawing.Point(-1, -2)
        Me.ftpDownloadProgressBar.Name = "ftpDownloadProgressBar"
        Me.ftpDownloadProgressBar.Size = New System.Drawing.Size(281, 16)
        Me.ftpDownloadProgressBar.Style = MetroFramework.MetroColorStyle.Red
        Me.ftpDownloadProgressBar.TabIndex = 38
        Me.ftpDownloadProgressBar.Theme = MetroFramework.MetroThemeStyle.Dark
        Me.ftpDownloadProgressBar.UseCustomBackColor = True
        '
        'openFileDialog
        '
        Me.openFileDialog.Multiselect = True
        Me.openFileDialog.Title = "Upload File"
        '
        'panelComponentsList
        '
        Me.panelComponentsList.Appearance = System.Windows.Forms.TabAppearance.FlatButtons
        Me.panelComponentsList.Controls.Add(Me.MetroTabPage2)
        Me.panelComponentsList.Controls.Add(Me.MetroTabPage1)
        Me.panelComponentsList.Controls.Add(Me.MetroTabPage3)
        Me.panelComponentsList.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.panelComponentsList.FontWeight = MetroFramework.MetroTabControlWeight.Regular
        Me.panelComponentsList.Location = New System.Drawing.Point(299, 31)
        Me.panelComponentsList.Margin = New System.Windows.Forms.Padding(0)
        Me.panelComponentsList.Name = "panelComponentsList"
        Me.panelComponentsList.SelectedIndex = 1
        Me.panelComponentsList.Size = New System.Drawing.Size(1484, 560)
        Me.panelComponentsList.Style = MetroFramework.MetroColorStyle.Red
        Me.panelComponentsList.TabIndex = 13
        Me.panelComponentsList.Theme = MetroFramework.MetroThemeStyle.Dark
        Me.panelComponentsList.UseSelectable = True
        '
        'MetroTabPage2
        '
        Me.MetroTabPage2.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.MetroTabPage2.Controls.Add(Me.ftpView)
        Me.MetroTabPage2.Controls.Add(Me.ftpOperationHistory)
        Me.MetroTabPage2.Controls.Add(Me.Panel6)
        Me.MetroTabPage2.Controls.Add(Me.Panel4)
        Me.MetroTabPage2.Controls.Add(Me.Panel12)
        Me.MetroTabPage2.HorizontalScrollbarBarColor = True
        Me.MetroTabPage2.HorizontalScrollbarHighlightOnWheel = False
        Me.MetroTabPage2.HorizontalScrollbarSize = 10
        Me.MetroTabPage2.Location = New System.Drawing.Point(4, 41)
        Me.MetroTabPage2.Margin = New System.Windows.Forms.Padding(0)
        Me.MetroTabPage2.Name = "MetroTabPage2"
        Me.MetroTabPage2.Size = New System.Drawing.Size(1476, 515)
        Me.MetroTabPage2.TabIndex = 1
        Me.MetroTabPage2.Text = "File Transfer Protocol"
        Me.MetroTabPage2.Theme = MetroFramework.MetroThemeStyle.Dark
        Me.MetroTabPage2.UseCustomBackColor = True
        Me.MetroTabPage2.VerticalScrollbarBarColor = True
        Me.MetroTabPage2.VerticalScrollbarHighlightOnWheel = False
        Me.MetroTabPage2.VerticalScrollbarSize = 10
        '
        'ftpView
        '
        Me.ftpView.AllowDrop = True
        Me.ftpView.AutoArrange = False
        Me.ftpView.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.ftpView.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ftpView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.clmnName, Me.clmnSize, Me.clmnLastWriteTime})
        Me.ftpView.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ftpView.ForeColor = System.Drawing.Color.White
        Me.ftpView.FullRowSelect = True
        Me.ftpView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.ftpView.HideSelection = False
        Me.ftpView.LabelWrap = False
        Me.ftpView.Location = New System.Drawing.Point(147, 51)
        Me.ftpView.Name = "ftpView"
        Me.ftpView.Size = New System.Drawing.Size(906, 423)
        Me.ftpView.SmallImageList = Me.iconList
        Me.ftpView.TabIndex = 28
        Me.ftpView.TileSize = New System.Drawing.Size(148, 150)
        Me.ftpView.UseCompatibleStateImageBehavior = False
        Me.ftpView.View = System.Windows.Forms.View.Details
        '
        'clmnName
        '
        Me.clmnName.Text = "Name"
        Me.clmnName.Width = 300
        '
        'clmnSize
        '
        Me.clmnSize.Text = "Size"
        '
        'clmnLastWriteTime
        '
        Me.clmnLastWriteTime.Text = "Last Write Time"
        Me.clmnLastWriteTime.Width = 150
        '
        'ftpOperationHistory
        '
        Me.ftpOperationHistory.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.ftpOperationHistory.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.ftpOperationHistory.Dock = System.Windows.Forms.DockStyle.Right
        Me.ftpOperationHistory.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.ftpOperationHistory.ForeColor = System.Drawing.Color.Gray
        Me.ftpOperationHistory.FormattingEnabled = True
        Me.ftpOperationHistory.ItemHeight = 14
        Me.ftpOperationHistory.Location = New System.Drawing.Point(1078, 47)
        Me.ftpOperationHistory.Margin = New System.Windows.Forms.Padding(10)
        Me.ftpOperationHistory.Name = "ftpOperationHistory"
        Me.ftpOperationHistory.Size = New System.Drawing.Size(398, 430)
        Me.ftpOperationHistory.TabIndex = 37
        '
        'Panel4
        '
        Me.Panel4.BackColor = System.Drawing.Color.Black
        Me.Panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel4.Controls.Add(Me.lblActivity)
        Me.Panel4.Controls.Add(Me.lblColumnFtp3)
        Me.Panel4.Controls.Add(Me.ftpUrl)
        Me.Panel4.Controls.Add(Me.lblColumnFtp2)
        Me.Panel4.Controls.Add(Me.lblColumnFtp1)
        Me.Panel4.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel4.Location = New System.Drawing.Point(147, 0)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1329, 47)
        Me.Panel4.TabIndex = 27
        '
        'lblActivity
        '
        Me.lblActivity.AutoSize = True
        Me.lblActivity.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.lblActivity.ForeColor = System.Drawing.Color.Gray
        Me.lblActivity.Location = New System.Drawing.Point(926, 26)
        Me.lblActivity.Name = "lblActivity"
        Me.lblActivity.Size = New System.Drawing.Size(63, 15)
        Me.lblActivity.TabIndex = 8
        Me.lblActivity.Text = "Activity"
        '
        'MetroTabPage1
        '
        Me.MetroTabPage1.BackColor = System.Drawing.Color.Black
        Me.MetroTabPage1.Controls.Add(Me.txtCmdInput)
        Me.MetroTabPage1.Controls.Add(Me.cmdHistoryList)
        Me.MetroTabPage1.Controls.Add(Me.rtfCmdOuput)
        Me.MetroTabPage1.HorizontalScrollbarBarColor = True
        Me.MetroTabPage1.HorizontalScrollbarHighlightOnWheel = False
        Me.MetroTabPage1.HorizontalScrollbarSize = 10
        Me.MetroTabPage1.Location = New System.Drawing.Point(4, 41)
        Me.MetroTabPage1.Name = "MetroTabPage1"
        Me.MetroTabPage1.Size = New System.Drawing.Size(1476, 515)
        Me.MetroTabPage1.TabIndex = 2
        Me.MetroTabPage1.Text = "Terminal"
        Me.MetroTabPage1.UseCustomBackColor = True
        Me.MetroTabPage1.VerticalScrollbarBarColor = True
        Me.MetroTabPage1.VerticalScrollbarHighlightOnWheel = False
        Me.MetroTabPage1.VerticalScrollbarSize = 10
        '
        'txtCmdInput
        '
        Me.txtCmdInput.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtCmdInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCmdInput.Font = New System.Drawing.Font("Consolas", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCmdInput.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.txtCmdInput.HideSelection = False
        Me.txtCmdInput.Location = New System.Drawing.Point(5, 458)
        Me.txtCmdInput.Multiline = True
        Me.txtCmdInput.Name = "txtCmdInput"
        Me.txtCmdInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.txtCmdInput.Size = New System.Drawing.Size(1223, 54)
        Me.txtCmdInput.TabIndex = 37
        '
        'cmdHistoryList
        '
        Me.cmdHistoryList.BackColor = System.Drawing.Color.Black
        Me.cmdHistoryList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.cmdHistoryList.Dock = System.Windows.Forms.DockStyle.Right
        Me.cmdHistoryList.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.cmdHistoryList.ForeColor = System.Drawing.Color.Gray
        Me.cmdHistoryList.FormattingEnabled = True
        Me.cmdHistoryList.ItemHeight = 14
        Me.cmdHistoryList.Items.AddRange(New Object() {"   dir", "   cd", "   rm", "   shutdown -s -t 10", "   ipconfig", "   net users", "   net stat -an", "   systeminfo", "   tasklist", "   taskkill /f /im", "   cd ../../../../Windows/System32"})
        Me.cmdHistoryList.Location = New System.Drawing.Point(1234, 0)
        Me.cmdHistoryList.Margin = New System.Windows.Forms.Padding(3, 50, 3, 3)
        Me.cmdHistoryList.Name = "cmdHistoryList"
        Me.cmdHistoryList.Size = New System.Drawing.Size(242, 515)
        Me.cmdHistoryList.TabIndex = 36
        '
        'MetroTabPage3
        '
        Me.MetroTabPage3.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.MetroTabPage3.Controls.Add(Me.Panel10)
        Me.MetroTabPage3.Controls.Add(Me.regValueHolder)
        Me.MetroTabPage3.Controls.Add(Me.regKeyHolder)
        Me.MetroTabPage3.HorizontalScrollbarBarColor = True
        Me.MetroTabPage3.HorizontalScrollbarHighlightOnWheel = False
        Me.MetroTabPage3.HorizontalScrollbarSize = 10
        Me.MetroTabPage3.Location = New System.Drawing.Point(4, 41)
        Me.MetroTabPage3.Name = "MetroTabPage3"
        Me.MetroTabPage3.Size = New System.Drawing.Size(1476, 515)
        Me.MetroTabPage3.TabIndex = 3
        Me.MetroTabPage3.Text = "Registry Editor"
        Me.MetroTabPage3.UseCustomBackColor = True
        Me.MetroTabPage3.UseCustomForeColor = True
        Me.MetroTabPage3.UseStyleColors = True
        Me.MetroTabPage3.VerticalScrollbarBarColor = True
        Me.MetroTabPage3.VerticalScrollbarHighlightOnWheel = False
        Me.MetroTabPage3.VerticalScrollbarSize = 10
        '
        'Panel10
        '
        Me.Panel10.BackColor = System.Drawing.Color.Black
        Me.Panel10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.Panel10.Controls.Add(Me.Label17)
        Me.Panel10.Controls.Add(Me.Label19)
        Me.Panel10.Controls.Add(Me.Label21)
        Me.Panel10.Controls.Add(Me.Label22)
        Me.Panel10.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel10.Location = New System.Drawing.Point(0, 0)
        Me.Panel10.Name = "Panel10"
        Me.Panel10.Size = New System.Drawing.Size(1476, 24)
        Me.Panel10.TabIndex = 28
        '
        'Label17
        '
        Me.Label17.AutoSize = True
        Me.Label17.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.Label17.ForeColor = System.Drawing.Color.Gray
        Me.Label17.Location = New System.Drawing.Point(25, 4)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(147, 15)
        Me.Label17.TabIndex = 8
        Me.Label17.Text = "Regedit Entry Viewer"
        '
        'Label19
        '
        Me.Label19.AutoSize = True
        Me.Label19.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.Label19.ForeColor = System.Drawing.Color.Gray
        Me.Label19.Location = New System.Drawing.Point(1037, 7)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(35, 15)
        Me.Label19.TabIndex = 7
        Me.Label19.Text = "Data"
        '
        'Label21
        '
        Me.Label21.AutoSize = True
        Me.Label21.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.Label21.ForeColor = System.Drawing.Color.Gray
        Me.Label21.Location = New System.Drawing.Point(854, 4)
        Me.Label21.Name = "Label21"
        Me.Label21.Size = New System.Drawing.Size(35, 15)
        Me.Label21.TabIndex = 6
        Me.Label21.Text = "Type"
        '
        'Label22
        '
        Me.Label22.AutoSize = True
        Me.Label22.Font = New System.Drawing.Font("Consolas", 9.75!)
        Me.Label22.ForeColor = System.Drawing.Color.Gray
        Me.Label22.Location = New System.Drawing.Point(547, 4)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(35, 15)
        Me.Label22.TabIndex = 5
        Me.Label22.Text = "Name"
        '
        'regValueHolder
        '
        Me.regValueHolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.regValueHolder.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.regValueHolder.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.rgClmnName, Me.rgClmnType, Me.rgClmnData})
        Me.regValueHolder.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.regValueHolder.ForeColor = System.Drawing.Color.White
        Me.regValueHolder.FullRowSelect = True
        Me.regValueHolder.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.regValueHolder.HideSelection = False
        Me.regValueHolder.Location = New System.Drawing.Point(544, 30)
        Me.regValueHolder.Name = "regValueHolder"
        Me.regValueHolder.Size = New System.Drawing.Size(903, 485)
        Me.regValueHolder.SmallImageList = Me.iconList
        Me.regValueHolder.TabIndex = 3
        Me.regValueHolder.UseCompatibleStateImageBehavior = False
        Me.regValueHolder.View = System.Windows.Forms.View.Details
        '
        'rgClmnName
        '
        Me.rgClmnName.Text = "Name"
        Me.rgClmnName.Width = 289
        '
        'rgClmnType
        '
        Me.rgClmnType.Text = "Type"
        Me.rgClmnType.Width = 175
        '
        'rgClmnData
        '
        Me.rgClmnData.Text = "Data"
        Me.rgClmnData.Width = 358
        '
        'regKeyHolder
        '
        Me.regKeyHolder.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.regKeyHolder.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.regKeyHolder.Font = New System.Drawing.Font("Verdana", 9.0!)
        Me.regKeyHolder.ForeColor = System.Drawing.Color.White
        Me.regKeyHolder.FullRowSelect = True
        Me.regKeyHolder.ImageIndex = 0
        Me.regKeyHolder.ImageList = Me.iconList
        Me.regKeyHolder.Location = New System.Drawing.Point(10, 46)
        Me.regKeyHolder.Name = "regKeyHolder"
        Me.regKeyHolder.SelectedImageIndex = 0
        Me.regKeyHolder.Size = New System.Drawing.Size(527, 469)
        Me.regKeyHolder.TabIndex = 2
        '
        'txtEditStructureUserDescriptor
        '
        Me.txtEditStructureUserDescriptor.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtEditStructureUserDescriptor.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtEditStructureUserDescriptor.Dock = System.Windows.Forms.DockStyle.Fill
        Me.txtEditStructureUserDescriptor.ForeColor = System.Drawing.Color.White
        Me.txtEditStructureUserDescriptor.Location = New System.Drawing.Point(10, 10)
        Me.txtEditStructureUserDescriptor.Multiline = True
        Me.txtEditStructureUserDescriptor.Name = "txtEditStructureUserDescriptor"
        Me.txtEditStructureUserDescriptor.Size = New System.Drawing.Size(194, 206)
        Me.txtEditStructureUserDescriptor.TabIndex = 16
        '
        'panelEditStructureUserDescriptor
        '
        Me.panelEditStructureUserDescriptor.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.panelEditStructureUserDescriptor.Controls.Add(Me.txtEditStructureUserDescriptor)
        Me.panelEditStructureUserDescriptor.Location = New System.Drawing.Point(385, 829)
        Me.panelEditStructureUserDescriptor.Name = "panelEditStructureUserDescriptor"
        Me.panelEditStructureUserDescriptor.Padding = New System.Windows.Forms.Padding(10)
        Me.panelEditStructureUserDescriptor.Size = New System.Drawing.Size(214, 226)
        Me.panelEditStructureUserDescriptor.TabIndex = 17
        Me.panelEditStructureUserDescriptor.Visible = False
        '
        'regKeyMenu
        '
        Me.regKeyMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.regKeyMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.regKeyMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.regBtnKeyRefresh, Me.ToolStripSeparator5, Me.regBtnKeyDelete, Me.ToolStripSeparator6, Me.regBtnKeyNewSubkey})
        Me.regKeyMenu.Name = "ftpMenu"
        Me.regKeyMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.regKeyMenu.Size = New System.Drawing.Size(139, 82)
        '
        'regBtnKeyRefresh
        '
        Me.regBtnKeyRefresh.ForeColor = System.Drawing.Color.White
        Me.regBtnKeyRefresh.Name = "regBtnKeyRefresh"
        Me.regBtnKeyRefresh.Size = New System.Drawing.Size(138, 22)
        Me.regBtnKeyRefresh.Text = "&Refresh"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(135, 6)
        '
        'regBtnKeyDelete
        '
        Me.regBtnKeyDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.regBtnKeyDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.regBtnKeyDelete.ForeColor = System.Drawing.Color.White
        Me.regBtnKeyDelete.Name = "regBtnKeyDelete"
        Me.regBtnKeyDelete.Size = New System.Drawing.Size(138, 22)
        Me.regBtnKeyDelete.Text = "Delete"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(135, 6)
        '
        'regBtnKeyNewSubkey
        '
        Me.regBtnKeyNewSubkey.ForeColor = System.Drawing.Color.White
        Me.regBtnKeyNewSubkey.Name = "regBtnKeyNewSubkey"
        Me.regBtnKeyNewSubkey.Size = New System.Drawing.Size(138, 22)
        Me.regBtnKeyNewSubkey.Text = "New subkey"
        '
        'regValueMenu
        '
        Me.regValueMenu.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.regValueMenu.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.regValueMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.regBtnValueRefresh, Me.regBtnValueDelete, Me.regBtnValueNewValue})
        Me.regValueMenu.Name = "ftpMenu"
        Me.regValueMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.regValueMenu.Size = New System.Drawing.Size(130, 70)
        '
        'regBtnValueRefresh
        '
        Me.regBtnValueRefresh.ForeColor = System.Drawing.Color.White
        Me.regBtnValueRefresh.Name = "regBtnValueRefresh"
        Me.regBtnValueRefresh.Size = New System.Drawing.Size(129, 22)
        Me.regBtnValueRefresh.Text = "&Refresh"
        '
        'regBtnValueDelete
        '
        Me.regBtnValueDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.regBtnValueDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.regBtnValueDelete.ForeColor = System.Drawing.Color.White
        Me.regBtnValueDelete.Name = "regBtnValueDelete"
        Me.regBtnValueDelete.Size = New System.Drawing.Size(129, 22)
        Me.regBtnValueDelete.Text = "Delete"
        '
        'regBtnValueNewValue
        '
        Me.regBtnValueNewValue.ForeColor = System.Drawing.Color.White
        Me.regBtnValueNewValue.Name = "regBtnValueNewValue"
        Me.regBtnValueNewValue.Size = New System.Drawing.Size(129, 22)
        Me.regBtnValueNewValue.Text = "New value"
        '
        'ReportingTimer
        '
        Me.ReportingTimer.Enabled = True
        Me.ReportingTimer.Interval = 500
        '
        'clientsControlBoard
        '
        Me.clientsControlBoard.Appearance = System.Windows.Forms.TabAppearance.Buttons
        Me.clientsControlBoard.Controls.Add(Me.clientInfoTab)
        Me.clientsControlBoard.Controls.Add(Me.clientListTab)
        Me.clientsControlBoard.FontWeight = MetroFramework.MetroTabControlWeight.Regular
        Me.clientsControlBoard.Location = New System.Drawing.Point(12, 31)
        Me.clientsControlBoard.Name = "clientsControlBoard"
        Me.clientsControlBoard.SelectedIndex = 0
        Me.clientsControlBoard.Size = New System.Drawing.Size(284, 384)
        Me.clientsControlBoard.Style = MetroFramework.MetroColorStyle.Blue
        Me.clientsControlBoard.TabIndex = 18
        Me.clientsControlBoard.Theme = MetroFramework.MetroThemeStyle.Dark
        Me.clientsControlBoard.UseSelectable = True
        '
        'clientInfoTab
        '
        Me.clientInfoTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.clientInfoTab.Controls.Add(Me.btnForce7)
        Me.clientInfoTab.Controls.Add(Me.Label1)
        Me.clientInfoTab.Controls.Add(Me.TextBox7)
        Me.clientInfoTab.Controls.Add(Me.btnForce6)
        Me.clientInfoTab.Controls.Add(Me.Label13)
        Me.clientInfoTab.Controls.Add(Me.TextBox1)
        Me.clientInfoTab.Controls.Add(Me.btnForce5)
        Me.clientInfoTab.Controls.Add(Me.Label24)
        Me.clientInfoTab.Controls.Add(Me.TextBox2)
        Me.clientInfoTab.Controls.Add(Me.btnForce4)
        Me.clientInfoTab.Controls.Add(Me.btnForce3)
        Me.clientInfoTab.Controls.Add(Me.btnForce2)
        Me.clientInfoTab.Controls.Add(Me.btnForce1)
        Me.clientInfoTab.Controls.Add(Me.Label25)
        Me.clientInfoTab.Controls.Add(Me.TextBox3)
        Me.clientInfoTab.Controls.Add(Me.Label26)
        Me.clientInfoTab.Controls.Add(Me.TextBox4)
        Me.clientInfoTab.Controls.Add(Me.Label27)
        Me.clientInfoTab.Controls.Add(Me.Label28)
        Me.clientInfoTab.Controls.Add(Me.TextBox5)
        Me.clientInfoTab.Controls.Add(Me.TextBox6)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoOs)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoVersion)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoPc)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoExtraData)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoAlias)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoName)
        Me.clientInfoTab.Controls.Add(Me.txtUserInfoId)
        Me.clientInfoTab.Controls.Add(Me.Label29)
        Me.clientInfoTab.Controls.Add(Me.Label30)
        Me.clientInfoTab.Controls.Add(Me.Label31)
        Me.clientInfoTab.Controls.Add(Me.Label32)
        Me.clientInfoTab.Controls.Add(Me.Label33)
        Me.clientInfoTab.Controls.Add(Me.Label34)
        Me.clientInfoTab.Controls.Add(Me.Label35)
        Me.clientInfoTab.ForeColor = System.Drawing.Color.LightGray
        Me.clientInfoTab.HorizontalScrollbarBarColor = True
        Me.clientInfoTab.HorizontalScrollbarHighlightOnWheel = False
        Me.clientInfoTab.HorizontalScrollbarSize = 10
        Me.clientInfoTab.Location = New System.Drawing.Point(4, 41)
        Me.clientInfoTab.Name = "clientInfoTab"
        Me.clientInfoTab.Size = New System.Drawing.Size(276, 339)
        Me.clientInfoTab.TabIndex = 0
        Me.clientInfoTab.Text = "Details"
        Me.clientInfoTab.UseCustomBackColor = True
        Me.clientInfoTab.VerticalScrollbarBarColor = True
        Me.clientInfoTab.VerticalScrollbarHighlightOnWheel = False
        Me.clientInfoTab.VerticalScrollbarSize = 10
        '
        'btnForce7
        '
        Me.btnForce7.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce7.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce7.FlatAppearance.BorderSize = 0
        Me.btnForce7.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce7.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce7.Location = New System.Drawing.Point(140, 294)
        Me.btnForce7.Name = "btnForce7"
        Me.btnForce7.Size = New System.Drawing.Size(124, 18)
        Me.btnForce7.TabIndex = 78
        Me.btnForce7.Text = "───── force"
        Me.btnForce7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce7.UseVisualStyleBackColor = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label1.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(11, 291)
        Me.Label1.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label1.Size = New System.Drawing.Size(106, 19)
        Me.Label1.TabIndex = 77
        Me.Label1.Text = "Mouse"
        '
        'TextBox7
        '
        Me.TextBox7.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox7.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox7.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox7.Location = New System.Drawing.Point(118, 293)
        Me.TextBox7.Multiline = True
        Me.TextBox7.Name = "TextBox7"
        Me.TextBox7.ReadOnly = True
        Me.TextBox7.ShortcutsEnabled = False
        Me.TextBox7.Size = New System.Drawing.Size(35, 21)
        Me.TextBox7.TabIndex = 76
        Me.TextBox7.Text = "off"
        Me.TextBox7.WordWrap = False
        '
        'btnForce6
        '
        Me.btnForce6.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce6.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce6.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce6.FlatAppearance.BorderSize = 0
        Me.btnForce6.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce6.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce6.Location = New System.Drawing.Point(140, 275)
        Me.btnForce6.Name = "btnForce6"
        Me.btnForce6.Size = New System.Drawing.Size(124, 18)
        Me.btnForce6.TabIndex = 75
        Me.btnForce6.Text = "───── force"
        Me.btnForce6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce6.UseVisualStyleBackColor = False
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label13.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(11, 272)
        Me.Label13.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label13.Name = "Label13"
        Me.Label13.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label13.Size = New System.Drawing.Size(106, 19)
        Me.Label13.TabIndex = 74
        Me.Label13.Text = "Video Stream"
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox1.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox1.Location = New System.Drawing.Point(118, 274)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.ReadOnly = True
        Me.TextBox1.ShortcutsEnabled = False
        Me.TextBox1.Size = New System.Drawing.Size(35, 21)
        Me.TextBox1.TabIndex = 73
        Me.TextBox1.Text = "off"
        Me.TextBox1.WordWrap = False
        '
        'btnForce5
        '
        Me.btnForce5.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce5.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce5.FlatAppearance.BorderSize = 0
        Me.btnForce5.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce5.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce5.Location = New System.Drawing.Point(140, 255)
        Me.btnForce5.Name = "btnForce5"
        Me.btnForce5.Size = New System.Drawing.Size(124, 18)
        Me.btnForce5.TabIndex = 72
        Me.btnForce5.Text = "───── force"
        Me.btnForce5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce5.UseVisualStyleBackColor = False
        '
        'Label24
        '
        Me.Label24.AutoSize = True
        Me.Label24.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label24.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label24.Location = New System.Drawing.Point(11, 253)
        Me.Label24.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label24.Name = "Label24"
        Me.Label24.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label24.Size = New System.Drawing.Size(106, 19)
        Me.Label24.TabIndex = 71
        Me.Label24.Text = "Registry Edit"
        '
        'TextBox2
        '
        Me.TextBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox2.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox2.Location = New System.Drawing.Point(118, 254)
        Me.TextBox2.Multiline = True
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.ReadOnly = True
        Me.TextBox2.ShortcutsEnabled = False
        Me.TextBox2.Size = New System.Drawing.Size(35, 21)
        Me.TextBox2.TabIndex = 70
        Me.TextBox2.Text = "off"
        Me.TextBox2.WordWrap = False
        '
        'btnForce4
        '
        Me.btnForce4.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce4.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce4.FlatAppearance.BorderSize = 0
        Me.btnForce4.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce4.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce4.Location = New System.Drawing.Point(140, 236)
        Me.btnForce4.Name = "btnForce4"
        Me.btnForce4.Size = New System.Drawing.Size(124, 18)
        Me.btnForce4.TabIndex = 69
        Me.btnForce4.Text = "───── force"
        Me.btnForce4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce4.UseVisualStyleBackColor = False
        '
        'btnForce3
        '
        Me.btnForce3.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce3.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce3.FlatAppearance.BorderSize = 0
        Me.btnForce3.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce3.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce3.Location = New System.Drawing.Point(140, 215)
        Me.btnForce3.Name = "btnForce3"
        Me.btnForce3.Size = New System.Drawing.Size(124, 18)
        Me.btnForce3.TabIndex = 68
        Me.btnForce3.Text = "───── force"
        Me.btnForce3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce3.UseVisualStyleBackColor = False
        '
        'btnForce2
        '
        Me.btnForce2.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce2.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce2.FlatAppearance.BorderSize = 0
        Me.btnForce2.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce2.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce2.Location = New System.Drawing.Point(140, 193)
        Me.btnForce2.Name = "btnForce2"
        Me.btnForce2.Size = New System.Drawing.Size(124, 18)
        Me.btnForce2.TabIndex = 67
        Me.btnForce2.Text = "───── force"
        Me.btnForce2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce2.UseVisualStyleBackColor = False
        '
        'btnForce1
        '
        Me.btnForce1.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.btnForce1.Cursor = System.Windows.Forms.Cursors.Hand
        Me.btnForce1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnForce1.FlatAppearance.BorderSize = 0
        Me.btnForce1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnForce1.Font = New System.Drawing.Font("SimSun", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnForce1.Location = New System.Drawing.Point(140, 173)
        Me.btnForce1.Name = "btnForce1"
        Me.btnForce1.Size = New System.Drawing.Size(124, 18)
        Me.btnForce1.TabIndex = 66
        Me.btnForce1.Text = "───── force"
        Me.btnForce1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnForce1.UseVisualStyleBackColor = False
        '
        'Label25
        '
        Me.Label25.AutoSize = True
        Me.Label25.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label25.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label25.Location = New System.Drawing.Point(11, 233)
        Me.Label25.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label25.Name = "Label25"
        Me.Label25.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label25.Size = New System.Drawing.Size(106, 19)
        Me.Label25.TabIndex = 65
        Me.Label25.Text = "Download Mgr"
        '
        'TextBox3
        '
        Me.TextBox3.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox3.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox3.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox3.Location = New System.Drawing.Point(118, 234)
        Me.TextBox3.Multiline = True
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.ShortcutsEnabled = False
        Me.TextBox3.Size = New System.Drawing.Size(35, 21)
        Me.TextBox3.TabIndex = 64
        Me.TextBox3.Text = "off"
        Me.TextBox3.WordWrap = False
        '
        'Label26
        '
        Me.Label26.AutoSize = True
        Me.Label26.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label26.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label26.Location = New System.Drawing.Point(11, 213)
        Me.Label26.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label26.Name = "Label26"
        Me.Label26.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label26.Size = New System.Drawing.Size(106, 19)
        Me.Label26.TabIndex = 63
        Me.Label26.Text = "Uploading Mgr."
        '
        'TextBox4
        '
        Me.TextBox4.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox4.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox4.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox4.Location = New System.Drawing.Point(118, 214)
        Me.TextBox4.Multiline = True
        Me.TextBox4.Name = "TextBox4"
        Me.TextBox4.ReadOnly = True
        Me.TextBox4.ShortcutsEnabled = False
        Me.TextBox4.Size = New System.Drawing.Size(34, 21)
        Me.TextBox4.TabIndex = 62
        Me.TextBox4.Text = "off"
        Me.TextBox4.WordWrap = False
        '
        'Label27
        '
        Me.Label27.AutoSize = True
        Me.Label27.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label27.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label27.Location = New System.Drawing.Point(11, 193)
        Me.Label27.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label27.Name = "Label27"
        Me.Label27.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label27.Size = New System.Drawing.Size(106, 19)
        Me.Label27.TabIndex = 61
        Me.Label27.Text = "Ftp Status"
        '
        'Label28
        '
        Me.Label28.AutoSize = True
        Me.Label28.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label28.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label28.Location = New System.Drawing.Point(11, 173)
        Me.Label28.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label28.Name = "Label28"
        Me.Label28.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label28.Size = New System.Drawing.Size(106, 19)
        Me.Label28.TabIndex = 60
        Me.Label28.Text = "Terminal Status"
        '
        'TextBox5
        '
        Me.TextBox5.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox5.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox5.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox5.Location = New System.Drawing.Point(118, 195)
        Me.TextBox5.Multiline = True
        Me.TextBox5.Name = "TextBox5"
        Me.TextBox5.ReadOnly = True
        Me.TextBox5.ShortcutsEnabled = False
        Me.TextBox5.Size = New System.Drawing.Size(34, 21)
        Me.TextBox5.TabIndex = 59
        Me.TextBox5.Text = "off"
        Me.TextBox5.WordWrap = False
        '
        'TextBox6
        '
        Me.TextBox6.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.TextBox6.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TextBox6.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TextBox6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(214, Byte), Integer), CType(CType(69, Byte), Integer), CType(CType(38, Byte), Integer))
        Me.TextBox6.Location = New System.Drawing.Point(118, 175)
        Me.TextBox6.Multiline = True
        Me.TextBox6.Name = "TextBox6"
        Me.TextBox6.ReadOnly = True
        Me.TextBox6.ShortcutsEnabled = False
        Me.TextBox6.Size = New System.Drawing.Size(34, 21)
        Me.TextBox6.TabIndex = 58
        Me.TextBox6.Text = "off"
        Me.TextBox6.WordWrap = False
        '
        'txtUserInfoOs
        '
        Me.txtUserInfoOs.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoOs.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoOs.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoOs.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoOs.Location = New System.Drawing.Point(120, 142)
        Me.txtUserInfoOs.Multiline = True
        Me.txtUserInfoOs.Name = "txtUserInfoOs"
        Me.txtUserInfoOs.ReadOnly = True
        Me.txtUserInfoOs.ShortcutsEnabled = False
        Me.txtUserInfoOs.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoOs.TabIndex = 57
        Me.txtUserInfoOs.Text = "───────────────────"
        Me.txtUserInfoOs.WordWrap = False
        '
        'txtUserInfoVersion
        '
        Me.txtUserInfoVersion.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoVersion.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoVersion.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoVersion.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoVersion.Location = New System.Drawing.Point(120, 120)
        Me.txtUserInfoVersion.Multiline = True
        Me.txtUserInfoVersion.Name = "txtUserInfoVersion"
        Me.txtUserInfoVersion.ReadOnly = True
        Me.txtUserInfoVersion.ShortcutsEnabled = False
        Me.txtUserInfoVersion.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoVersion.TabIndex = 56
        Me.txtUserInfoVersion.Text = "───────────────────"
        Me.txtUserInfoVersion.WordWrap = False
        '
        'txtUserInfoPc
        '
        Me.txtUserInfoPc.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoPc.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoPc.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtUserInfoPc.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoPc.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoPc.Location = New System.Drawing.Point(120, 98)
        Me.txtUserInfoPc.Multiline = True
        Me.txtUserInfoPc.Name = "txtUserInfoPc"
        Me.txtUserInfoPc.ReadOnly = True
        Me.txtUserInfoPc.ShortcutsEnabled = False
        Me.txtUserInfoPc.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoPc.TabIndex = 55
        Me.txtUserInfoPc.Text = "───────────────────"
        Me.txtUserInfoPc.WordWrap = False
        '
        'txtUserInfoExtraData
        '
        Me.txtUserInfoExtraData.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoExtraData.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoExtraData.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtUserInfoExtraData.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoExtraData.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoExtraData.Location = New System.Drawing.Point(120, 76)
        Me.txtUserInfoExtraData.Multiline = True
        Me.txtUserInfoExtraData.Name = "txtUserInfoExtraData"
        Me.txtUserInfoExtraData.ReadOnly = True
        Me.txtUserInfoExtraData.ShortcutsEnabled = False
        Me.txtUserInfoExtraData.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoExtraData.TabIndex = 54
        Me.txtUserInfoExtraData.Text = "───────────────────"
        Me.txtUserInfoExtraData.WordWrap = False
        '
        'txtUserInfoAlias
        '
        Me.txtUserInfoAlias.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoAlias.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoAlias.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtUserInfoAlias.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoAlias.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoAlias.Location = New System.Drawing.Point(120, 54)
        Me.txtUserInfoAlias.Multiline = True
        Me.txtUserInfoAlias.Name = "txtUserInfoAlias"
        Me.txtUserInfoAlias.ReadOnly = True
        Me.txtUserInfoAlias.ShortcutsEnabled = False
        Me.txtUserInfoAlias.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoAlias.TabIndex = 53
        Me.txtUserInfoAlias.Text = "───────────────────"
        Me.txtUserInfoAlias.WordWrap = False
        '
        'txtUserInfoName
        '
        Me.txtUserInfoName.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoName.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoName.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoName.Location = New System.Drawing.Point(120, 32)
        Me.txtUserInfoName.Multiline = True
        Me.txtUserInfoName.Name = "txtUserInfoName"
        Me.txtUserInfoName.ReadOnly = True
        Me.txtUserInfoName.ShortcutsEnabled = False
        Me.txtUserInfoName.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoName.TabIndex = 52
        Me.txtUserInfoName.Text = "───────────────────"
        Me.txtUserInfoName.WordWrap = False
        '
        'txtUserInfoId
        '
        Me.txtUserInfoId.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.txtUserInfoId.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtUserInfoId.Font = New System.Drawing.Font("Nirmala UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUserInfoId.ForeColor = System.Drawing.Color.Silver
        Me.txtUserInfoId.Location = New System.Drawing.Point(120, 10)
        Me.txtUserInfoId.Multiline = True
        Me.txtUserInfoId.Name = "txtUserInfoId"
        Me.txtUserInfoId.ReadOnly = True
        Me.txtUserInfoId.ShortcutsEnabled = False
        Me.txtUserInfoId.Size = New System.Drawing.Size(146, 21)
        Me.txtUserInfoId.TabIndex = 51
        Me.txtUserInfoId.Text = "───────────────────"
        Me.txtUserInfoId.WordWrap = False
        '
        'Label29
        '
        Me.Label29.AutoSize = True
        Me.Label29.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label29.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label29.Location = New System.Drawing.Point(11, 75)
        Me.Label29.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label29.Name = "Label29"
        Me.Label29.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label29.Size = New System.Drawing.Size(106, 19)
        Me.Label29.TabIndex = 50
        Me.Label29.Text = "Extra data"
        '
        'Label30
        '
        Me.Label30.AutoSize = True
        Me.Label30.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label30.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label30.Location = New System.Drawing.Point(11, 141)
        Me.Label30.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label30.Name = "Label30"
        Me.Label30.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label30.Size = New System.Drawing.Size(106, 19)
        Me.Label30.TabIndex = 49
        Me.Label30.Text = "Os Version"
        '
        'Label31
        '
        Me.Label31.AutoSize = True
        Me.Label31.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label31.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label31.Location = New System.Drawing.Point(11, 119)
        Me.Label31.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label31.Name = "Label31"
        Me.Label31.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label31.Size = New System.Drawing.Size(106, 19)
        Me.Label31.TabIndex = 48
        Me.Label31.Text = "Version"
        '
        'Label32
        '
        Me.Label32.AutoSize = True
        Me.Label32.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label32.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label32.Location = New System.Drawing.Point(11, 54)
        Me.Label32.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label32.Name = "Label32"
        Me.Label32.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label32.Size = New System.Drawing.Size(106, 19)
        Me.Label32.TabIndex = 47
        Me.Label32.Text = "Custom Alias"
        '
        'Label33
        '
        Me.Label33.AutoSize = True
        Me.Label33.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label33.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label33.Location = New System.Drawing.Point(11, 97)
        Me.Label33.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label33.Name = "Label33"
        Me.Label33.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label33.Size = New System.Drawing.Size(106, 19)
        Me.Label33.TabIndex = 46
        Me.Label33.Text = "Pc description"
        Me.Label33.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label34
        '
        Me.Label34.AutoSize = True
        Me.Label34.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label34.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label34.Location = New System.Drawing.Point(11, 33)
        Me.Label34.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label34.Name = "Label34"
        Me.Label34.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label34.Size = New System.Drawing.Size(106, 19)
        Me.Label34.TabIndex = 45
        Me.Label34.Text = "Username"
        '
        'Label35
        '
        Me.Label35.AutoSize = True
        Me.Label35.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.Label35.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label35.Location = New System.Drawing.Point(11, 10)
        Me.Label35.MinimumSize = New System.Drawing.Size(106, 0)
        Me.Label35.Name = "Label35"
        Me.Label35.Padding = New System.Windows.Forms.Padding(10, 3, 0, 3)
        Me.Label35.Size = New System.Drawing.Size(106, 19)
        Me.Label35.TabIndex = 44
        Me.Label35.Text = "Identification"
        '
        'clientListTab
        '
        Me.clientListTab.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.clientListTab.Controls.Add(Me.clientList)
        Me.clientListTab.HorizontalScrollbarBarColor = True
        Me.clientListTab.HorizontalScrollbarHighlightOnWheel = False
        Me.clientListTab.HorizontalScrollbarSize = 10
        Me.clientListTab.Location = New System.Drawing.Point(4, 41)
        Me.clientListTab.Name = "clientListTab"
        Me.clientListTab.Size = New System.Drawing.Size(276, 339)
        Me.clientListTab.TabIndex = 1
        Me.clientListTab.Text = "Clients List"
        Me.clientListTab.UseCustomBackColor = True
        Me.clientListTab.VerticalScrollbarBarColor = True
        Me.clientListTab.VerticalScrollbarHighlightOnWheel = False
        Me.clientListTab.VerticalScrollbarSize = 10
        '
        'clientList
        '
        Me.clientList.BackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.clientList.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.clientList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.clientList.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.clientList.ForeColor = System.Drawing.Color.Firebrick
        Me.clientList.FormattingEnabled = True
        Me.clientList.ItemHeight = 18
        Me.clientList.Location = New System.Drawing.Point(0, 0)
        Me.clientList.Name = "clientList"
        Me.clientList.Size = New System.Drawing.Size(276, 339)
        Me.clientList.TabIndex = 24
        '
        'PingingTimer
        '
        Me.PingingTimer.Enabled = True
        Me.PingingTimer.Interval = 10000
        '
        'LumiereServer
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.DimGray
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center
        Me.ClientSize = New System.Drawing.Size(1803, 793)
        Me.Controls.Add(Me.clientsControlBoard)
        Me.Controls.Add(Me.panelEditStructureUserDescriptor)
        Me.Controls.Add(Me.panelComponentsList)
        Me.Controls.Add(Me.panelDebugView)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.mainMenu)
        Me.Cursor = System.Windows.Forms.Cursors.Arrow
        Me.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.ForeColor = System.Drawing.Color.Silver
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.mainMenu
        Me.MaximizeBox = False
        Me.Name = "LumiereServer"
        Me.Text = "LumiereServer"
        Me.mainMenu.ResumeLayout(False)
        Me.mainMenu.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel5.ResumeLayout(False)
        Me.Panel5.PerformLayout()
        Me.shellMenu.ResumeLayout(False)
        Me.panelDebugView.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel13.ResumeLayout(False)
        Me.Panel13.PerformLayout()
        Me.ftpFileMenu.ResumeLayout(False)
        Me.ftpBackgroundMenu.ResumeLayout(False)
        Me.Panel12.ResumeLayout(False)
        Me.Panel12.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel14.ResumeLayout(False)
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.Panel8.ResumeLayout(False)
        Me.Panel7.ResumeLayout(False)
        Me.panelComponentsList.ResumeLayout(False)
        Me.MetroTabPage2.ResumeLayout(False)
        Me.Panel4.ResumeLayout(False)
        Me.Panel4.PerformLayout()
        Me.MetroTabPage1.ResumeLayout(False)
        Me.MetroTabPage1.PerformLayout()
        Me.MetroTabPage3.ResumeLayout(False)
        Me.Panel10.ResumeLayout(False)
        Me.Panel10.PerformLayout()
        Me.panelEditStructureUserDescriptor.ResumeLayout(False)
        Me.panelEditStructureUserDescriptor.PerformLayout()
        Me.regKeyMenu.ResumeLayout(False)
        Me.regValueMenu.ResumeLayout(False)
        Me.clientsControlBoard.ResumeLayout(False)
        Me.clientInfoTab.ResumeLayout(False)
        Me.clientInfoTab.PerformLayout()
        Me.clientListTab.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents mainMenu As MenuStrip
    Friend WithEvents FileToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnViewClientLogs As ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HelpToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel5 As Panel
    Friend WithEvents Label10 As Label
    Friend WithEvents panelDebugView As Panel
    Friend WithEvents Panel13 As Panel
    Friend WithEvents Label14 As Label
    Friend WithEvents btnCam As Button
    Friend WithEvents btnBan As Button
    Friend WithEvents btnLocalDir As Button
    Friend WithEvents btnLive As Button
    Friend WithEvents ftpUrl As TextBox
    Friend WithEvents iconList As ImageList
    Friend WithEvents threadList As ListBox
    Friend WithEvents rtfCmdOuput As RichTextBox
    Friend WithEvents debugView As RichTextBox
    Friend WithEvents ftpFileMenu As ContextMenuStrip
    Friend WithEvents ftpBtnDownloadFile As ToolStripMenuItem
    Friend WithEvents ftpBtnDelete As ToolStripMenuItem
    Friend WithEvents ftpBtnRename As ToolStripMenuItem
    Friend WithEvents ftpBackgroundMenu As ContextMenuStrip
    Friend WithEvents btnFtpDownloadInterrupt As Button
    Friend WithEvents lblColumnFtp3 As Label
    Friend WithEvents lblColumnFtp2 As Label
    Friend WithEvents lblColumnFtp1 As Label
    Friend WithEvents Panel12 As Panel
    Friend WithEvents Panel14 As Panel
    Friend WithEvents Label16 As Label
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ftpNewToolstrip As ToolStripMenuItem
    Friend WithEvents ftpBtnNewFolder As ToolStripMenuItem
    Friend WithEvents ftpBtnNewFile As ToolStripMenuItem
    Friend WithEvents ftpBtnUpload As ToolStripMenuItem
    Friend WithEvents openFileDialog As OpenFileDialog
    Friend WithEvents btnShortcutWindows As Button
    Friend WithEvents btnShortcutTemp As Button
    Friend WithEvents btnShortcutAppData As Button
    Friend WithEvents btnShortcutDesktop As Button
    Friend WithEvents HorizonDovaLumiereSombreToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents btnShortcutStartup As Button
    Friend WithEvents btnShortcutStartupAll As Button
    Friend WithEvents btnFtpUploadInterrupt As Button
    Friend WithEvents btnFtpRun As ToolStripMenuItem
    Friend WithEvents btnFtpRunAs As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents btnDestroy As Button
    Friend WithEvents btnSwitchOut As Button
    Friend WithEvents panelComponentsList As MetroFramework.Controls.MetroTabControl
    Friend WithEvents MetroTabPage2 As MetroFramework.Controls.MetroTabPage
    Friend WithEvents Panel4 As Panel
    Friend WithEvents ftpBtnBack As Button
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents MetroTabPage1 As MetroFramework.Controls.MetroTabPage
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Panel7 As Panel
    Friend WithEvents ftpView As ListView
    Friend WithEvents clmnName As ColumnHeader
    Friend WithEvents clmnSize As ColumnHeader
    Friend WithEvents clmnLastWriteTime As ColumnHeader
    Friend WithEvents Label2 As Label
    Friend WithEvents btnShortcutHome As Button
    Friend WithEvents btnShortcutRecycleBin As Button
    Friend WithEvents Label11 As Label
    Friend WithEvents ftpBtnRefresh As ToolStripMenuItem
    Friend WithEvents txtEditStructureUserDescriptor As TextBox
    Friend WithEvents panelEditStructureUserDescriptor As Panel
    Friend WithEvents shellMenu As ContextMenuStrip
    Friend WithEvents shellBtnCancel As ToolStripMenuItem
    Friend WithEvents lblActivity As Label
    Friend WithEvents cmdHistoryList As ListBox
    Friend WithEvents ftpOperationHistory As ListBox
    Friend WithEvents lblUploadName As Label
    Friend WithEvents lblDownloadName As Label
    Friend WithEvents btnFtpOpen As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents MetroTabPage3 As MetroFramework.Controls.MetroTabPage
    Friend WithEvents regKeyHolder As TreeView
    Friend WithEvents regValueHolder As ListView
    Friend WithEvents rgClmnName As ColumnHeader
    Friend WithEvents rgClmnType As ColumnHeader
    Friend WithEvents rgClmnData As ColumnHeader
    Friend WithEvents Panel10 As Panel
    Friend WithEvents Label17 As Label
    Friend WithEvents Label19 As Label
    Friend WithEvents Label21 As Label
    Friend WithEvents Label22 As Label
    Friend WithEvents regKeyMenu As ContextMenuStrip
    Friend WithEvents regBtnKeyRefresh As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents regBtnKeyDelete As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents regBtnKeyNewSubkey As ToolStripMenuItem
    Friend WithEvents regValueMenu As ContextMenuStrip
    Friend WithEvents regBtnValueRefresh As ToolStripMenuItem
    Friend WithEvents regBtnValueDelete As ToolStripMenuItem
    Friend WithEvents regBtnValueNewValue As ToolStripMenuItem
    Friend WithEvents ftpUploadProgressBar As MetroFramework.Controls.MetroProgressBar
    Friend WithEvents ftpDownloadProgressBar As MetroFramework.Controls.MetroProgressBar
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents debugLowLevelView As RichTextBox
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EeererToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportingTimer As Timer
    Friend WithEvents clientsControlBoard As MetroFramework.Controls.MetroTabControl
    Friend WithEvents clientInfoTab As MetroFramework.Controls.MetroTabPage
    Friend WithEvents btnForce6 As Button
    Friend WithEvents Label13 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents btnForce5 As Button
    Friend WithEvents Label24 As Label
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents btnForce4 As Button
    Friend WithEvents btnForce3 As Button
    Friend WithEvents btnForce2 As Button
    Friend WithEvents btnForce1 As Button
    Friend WithEvents Label25 As Label
    Friend WithEvents TextBox3 As TextBox
    Friend WithEvents Label26 As Label
    Friend WithEvents TextBox4 As TextBox
    Friend WithEvents Label27 As Label
    Friend WithEvents Label28 As Label
    Friend WithEvents TextBox5 As TextBox
    Friend WithEvents TextBox6 As TextBox
    Friend WithEvents txtUserInfoOs As TextBox
    Friend WithEvents txtUserInfoVersion As TextBox
    Friend WithEvents txtUserInfoPc As TextBox
    Friend WithEvents txtUserInfoExtraData As TextBox
    Friend WithEvents txtUserInfoAlias As TextBox
    Friend WithEvents txtUserInfoName As TextBox
    Friend WithEvents txtUserInfoId As TextBox
    Friend WithEvents Label29 As Label
    Friend WithEvents Label30 As Label
    Friend WithEvents Label31 As Label
    Friend WithEvents Label32 As Label
    Friend WithEvents Label33 As Label
    Friend WithEvents Label34 As Label
    Friend WithEvents Label35 As Label
    Friend WithEvents clientListTab As MetroFramework.Controls.MetroTabPage
    Friend WithEvents btnShortcutPin3 As Button
    Friend WithEvents btnShortcutPin2 As Button
    Friend WithEvents btnShortcutPin1 As Button
    Friend WithEvents ftpPinPathButton As ToolStripMenuItem
    Friend WithEvents PingingTimer As Timer
    Friend WithEvents txtCmdInput As TextBox
    Friend WithEvents btnForce7 As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBox7 As TextBox
    Friend WithEvents clientList As ListBox
End Class
