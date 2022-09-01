<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WindowRegNewValue
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
        Me.txtValName = New System.Windows.Forms.TextBox()
        Me.txtValueData = New System.Windows.Forms.TextBox()
        Me.cmbValType = New System.Windows.Forms.ComboBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtValName
        '
        Me.txtValName.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtValName.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtValName.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.txtValName.Location = New System.Drawing.Point(11, 37)
        Me.txtValName.Multiline = True
        Me.txtValName.Name = "txtValName"
        Me.txtValName.Size = New System.Drawing.Size(207, 21)
        Me.txtValName.TabIndex = 0
        '
        'txtValueData
        '
        Me.txtValueData.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.txtValueData.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.txtValueData.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.txtValueData.Location = New System.Drawing.Point(401, 37)
        Me.txtValueData.Multiline = True
        Me.txtValueData.Name = "txtValueData"
        Me.txtValueData.Size = New System.Drawing.Size(308, 240)
        Me.txtValueData.TabIndex = 1
        '
        'cmbValType
        '
        Me.cmbValType.BackColor = System.Drawing.SystemColors.ButtonFace
        Me.cmbValType.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.cmbValType.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.cmbValType.FormattingEnabled = True
        Me.cmbValType.Items.AddRange(New Object() {"REG_SZ - 1", "REG_BINARY - 3", "REG_DWORD - 4", "REG_QWORD - 11", "REG_MULTI_SZ - 7", "REG_EXPAND_SZ - 2"})
        Me.cmbValType.Location = New System.Drawing.Point(224, 37)
        Me.cmbValType.Name = "cmbValType"
        Me.cmbValType.Size = New System.Drawing.Size(171, 24)
        Me.cmbValType.TabIndex = 2
        Me.cmbValType.Text = "REG_SZ - 1"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.Label1.ForeColor = System.Drawing.Color.Gray
        Me.Label1.Location = New System.Drawing.Point(8, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(76, 16)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Value name"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.Label2.ForeColor = System.Drawing.Color.Gray
        Me.Label2.Location = New System.Drawing.Point(221, 18)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Type"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Tai Le", 9.75!)
        Me.Label3.ForeColor = System.Drawing.Color.Gray
        Me.Label3.Location = New System.Drawing.Point(398, 18)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(35, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Data"
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnCancel.FlatAppearance.BorderSize = 0
        Me.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnCancel.ForeColor = System.Drawing.Color.Gray
        Me.btnCancel.Location = New System.Drawing.Point(169, 254)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 36
        Me.btnCancel.Text = "&Cancel"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.WhiteSmoke
        Me.btnSave.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        Me.btnSave.FlatAppearance.BorderSize = 0
        Me.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnSave.ForeColor = System.Drawing.Color.Gray
        Me.btnSave.Location = New System.Drawing.Point(248, 254)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(147, 23)
        Me.btnSave.TabIndex = 35
        Me.btnSave.Text = "&Save"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'WindowRegNewValue
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(716, 284)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbValType)
        Me.Controls.Add(Me.txtValueData)
        Me.Controls.Add(Me.txtValName)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "WindowRegNewValue"
        Me.ShowIcon = False
        Me.Text = "New Value"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtValName As TextBox
    Friend WithEvents txtValueData As TextBox
    Friend WithEvents cmbValType As ComboBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents btnCancel As Button
    Friend WithEvents btnSave As Button
End Class
