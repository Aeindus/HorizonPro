<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WindowViewClientLogs
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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.tableRegistered = New System.Windows.Forms.DataGridView()
        Me.ServerClientManagement2003DataSetBindingSource = New System.Windows.Forms.BindingSource(Me.components)
        Me.tableBlacklist = New System.Windows.Forms.DataGridView()
        Me.btnmvtoblacklist = New System.Windows.Forms.Button()
        Me.btnremofrmblacklist = New System.Windows.Forms.Button()
        Me.btnmvtoblacklist_txt = New System.Windows.Forms.Button()
        Me.txtip = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.tableRegistered, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ServerClientManagement2003DataSetBindingSource, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.tableBlacklist, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tableRegistered
        '
        Me.tableRegistered.BackgroundColor = System.Drawing.Color.White
        Me.tableRegistered.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tableRegistered.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tableRegistered.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle7
        Me.tableRegistered.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.tableRegistered.DefaultCellStyle = DataGridViewCellStyle8
        Me.tableRegistered.GridColor = System.Drawing.Color.Silver
        Me.tableRegistered.Location = New System.Drawing.Point(6, 58)
        Me.tableRegistered.Name = "tableRegistered"
        DataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.Color.White
        Me.tableRegistered.RowsDefaultCellStyle = DataGridViewCellStyle9
        Me.tableRegistered.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tableRegistered.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tableRegistered.Size = New System.Drawing.Size(369, 424)
        Me.tableRegistered.TabIndex = 0
        '
        'tableBlacklist
        '
        Me.tableBlacklist.BackgroundColor = System.Drawing.Color.White
        Me.tableBlacklist.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.tableBlacklist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        DataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle10.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle10.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle10.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle10.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.tableBlacklist.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle10
        Me.tableBlacklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle11.BackColor = System.Drawing.Color.White
        DataGridViewCellStyle11.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle11.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle11.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.tableBlacklist.DefaultCellStyle = DataGridViewCellStyle11
        Me.tableBlacklist.GridColor = System.Drawing.Color.Silver
        Me.tableBlacklist.Location = New System.Drawing.Point(6, 58)
        Me.tableBlacklist.Name = "tableBlacklist"
        DataGridViewCellStyle12.ForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle12.SelectionBackColor = System.Drawing.Color.Silver
        DataGridViewCellStyle12.SelectionForeColor = System.Drawing.Color.White
        Me.tableBlacklist.RowsDefaultCellStyle = DataGridViewCellStyle12
        Me.tableBlacklist.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.tableBlacklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.tableBlacklist.Size = New System.Drawing.Size(324, 424)
        Me.tableBlacklist.TabIndex = 1
        '
        'btnmvtoblacklist
        '
        Me.btnmvtoblacklist.BackColor = System.Drawing.Color.Black
        Me.btnmvtoblacklist.FlatAppearance.BorderSize = 0
        Me.btnmvtoblacklist.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnmvtoblacklist.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnmvtoblacklist.ForeColor = System.Drawing.Color.White
        Me.btnmvtoblacklist.Location = New System.Drawing.Point(6, 29)
        Me.btnmvtoblacklist.Name = "btnmvtoblacklist"
        Me.btnmvtoblacklist.Size = New System.Drawing.Size(369, 23)
        Me.btnmvtoblacklist.TabIndex = 2
        Me.btnmvtoblacklist.Text = "Move to blacklist"
        Me.btnmvtoblacklist.UseVisualStyleBackColor = False
        '
        'btnremofrmblacklist
        '
        Me.btnremofrmblacklist.BackColor = System.Drawing.Color.Black
        Me.btnremofrmblacklist.FlatAppearance.BorderSize = 0
        Me.btnremofrmblacklist.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnremofrmblacklist.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnremofrmblacklist.ForeColor = System.Drawing.Color.White
        Me.btnremofrmblacklist.Location = New System.Drawing.Point(6, 29)
        Me.btnremofrmblacklist.Name = "btnremofrmblacklist"
        Me.btnremofrmblacklist.Size = New System.Drawing.Size(324, 23)
        Me.btnremofrmblacklist.TabIndex = 3
        Me.btnremofrmblacklist.Text = "Remove selected item from blacklist x"
        Me.btnremofrmblacklist.UseVisualStyleBackColor = False
        '
        'btnmvtoblacklist_txt
        '
        Me.btnmvtoblacklist_txt.BackColor = System.Drawing.Color.Black
        Me.btnmvtoblacklist_txt.FlatAppearance.BorderSize = 0
        Me.btnmvtoblacklist_txt.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnmvtoblacklist_txt.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnmvtoblacklist_txt.ForeColor = System.Drawing.Color.White
        Me.btnmvtoblacklist_txt.Location = New System.Drawing.Point(177, 0)
        Me.btnmvtoblacklist_txt.Name = "btnmvtoblacklist_txt"
        Me.btnmvtoblacklist_txt.Size = New System.Drawing.Size(168, 23)
        Me.btnmvtoblacklist_txt.TabIndex = 4
        Me.btnmvtoblacklist_txt.Text = "Move to blacklist >>> "
        Me.btnmvtoblacklist_txt.UseVisualStyleBackColor = False
        '
        'txtip
        '
        Me.txtip.BackColor = System.Drawing.Color.Black
        Me.txtip.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtip.ForeColor = System.Drawing.Color.White
        Me.txtip.Location = New System.Drawing.Point(3, 4)
        Me.txtip.Multiline = True
        Me.txtip.Name = "txtip"
        Me.txtip.Size = New System.Drawing.Size(168, 16)
        Me.txtip.TabIndex = 5
        Me.txtip.Text = "127.0.0.1"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.tableRegistered)
        Me.GroupBox1.Controls.Add(Me.btnmvtoblacklist)
        Me.GroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox1.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.ForeColor = System.Drawing.Color.Black
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(381, 492)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Registered Users"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnremofrmblacklist)
        Me.GroupBox2.Controls.Add(Me.tableBlacklist)
        Me.GroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox2.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(399, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(340, 492)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Blacklisted users"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Panel1)
        Me.GroupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.GroupBox3.Font = New System.Drawing.Font("Verdana", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox3.Location = New System.Drawing.Point(12, 510)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(381, 51)
        Me.GroupBox3.TabIndex = 8
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Blacklist a custom user"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Black
        Me.Panel1.Controls.Add(Me.txtip)
        Me.Panel1.Controls.Add(Me.btnmvtoblacklist_txt)
        Me.Panel1.Location = New System.Drawing.Point(6, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(369, 23)
        Me.Panel1.TabIndex = 9
        '
        'WindowViewClientLogs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(749, 567)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.ForeColor = System.Drawing.Color.Black
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "WindowViewClientLogs"
        Me.ShowIcon = False
        Me.Text = "Users Central Database"
        CType(Me.tableRegistered, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ServerClientManagement2003DataSetBindingSource, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.tableBlacklist, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tableRegistered As DataGridView
    Friend WithEvents ServerClientManagement2003DataSetBindingSource As BindingSource
    Friend WithEvents tableBlacklist As DataGridView
    Friend WithEvents btnmvtoblacklist As Button
    Friend WithEvents btnremofrmblacklist As Button
    Friend WithEvents btnmvtoblacklist_txt As Button
    Friend WithEvents txtip As TextBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Panel1 As Panel
End Class
