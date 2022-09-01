<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class WindowByteViewer
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.lblRegInfoIntro = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.hexview = New Be.Windows.Forms.HexBox()
        Me.lblRegPath = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ForeColor = System.Drawing.Color.Gray
        Me.btnSave.Location = New System.Drawing.Point(641, 15)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(147, 23)
        Me.btnSave.TabIndex = 31
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'lblRegInfoIntro
        '
        Me.lblRegInfoIntro.AutoSize = True
        Me.lblRegInfoIntro.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.lblRegInfoIntro.ForeColor = System.Drawing.Color.Gray
        Me.lblRegInfoIntro.Location = New System.Drawing.Point(12, 15)
        Me.lblRegInfoIntro.Name = "lblRegInfoIntro"
        Me.lblRegInfoIntro.Size = New System.Drawing.Size(156, 16)
        Me.lblRegInfoIntro.TabIndex = 32
        Me.lblRegInfoIntro.Text = "Registry key being edited"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ForeColor = System.Drawing.Color.Gray
        Me.btnCancel.Location = New System.Drawing.Point(566, 15)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 34
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'hexview
        '
        Me.hexview.BackColor = System.Drawing.Color.WhiteSmoke
        Me.hexview.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.hexview.ColumnInfoVisible = True
        Me.hexview.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.hexview.Location = New System.Drawing.Point(12, 53)
        Me.hexview.Name = "hexview"
        Me.hexview.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer), CType(CType(20, Byte), Integer))
        Me.hexview.ShadowSelectionColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.hexview.Size = New System.Drawing.Size(776, 385)
        Me.hexview.StringViewVisible = True
        Me.hexview.TabIndex = 35
        '
        'lblRegPath
        '
        Me.lblRegPath.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.lblRegPath.ForeColor = System.Drawing.Color.Gray
        Me.lblRegPath.Location = New System.Drawing.Point(173, 16)
        Me.lblRegPath.Multiline = True
        Me.lblRegPath.Name = "lblRegPath"
        Me.lblRegPath.Size = New System.Drawing.Size(385, 20)
        Me.lblRegPath.TabIndex = 36
        Me.lblRegPath.Text = "dfsdf"
        '
        'WindowByteViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.lblRegPath)
        Me.Controls.Add(Me.hexview)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.lblRegInfoIntro)
        Me.Controls.Add(Me.btnSave)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "WindowByteViewer"
        Me.ShowIcon = False
        Me.Text = "Hex Editor"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents btnSave As Button
    Friend WithEvents lblRegInfoIntro As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents hexview As Be.Windows.Forms.HexBox
    Friend WithEvents lblRegPath As TextBox
End Class
