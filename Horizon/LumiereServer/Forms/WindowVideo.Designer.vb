<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class WindowVideo
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(WindowVideo))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.rowHeightControl = New System.Windows.Forms.TrackBar()
        Me.framesCountControl = New System.Windows.Forms.TrackBar()
        Me.ReadFPSTimer = New System.Windows.Forms.Timer(Me.components)
        Me.frameBuffer = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        CType(Me.rowHeightControl, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.framesCountControl, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.Button1)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.rowHeightControl)
        Me.Panel1.Controls.Add(Me.framesCountControl)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 691)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1447, 43)
        Me.Panel1.TabIndex = 14
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.White
        Me.Button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.Button1.Location = New System.Drawing.Point(567, 5)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(171, 35)
        Me.Button1.TabIndex = 20
        Me.Button1.Text = "Enable live movement"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label3.Location = New System.Drawing.Point(744, 16)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(39, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Label3"
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.Label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label1.Location = New System.Drawing.Point(418, 27)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 18)
        Me.Label1.TabIndex = 17
        '
        'Label2
        '
        Me.Label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight
        Me.Label2.Location = New System.Drawing.Point(121, 27)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 18)
        Me.Label2.TabIndex = 16
        '
        'rowHeightControl
        '
        Me.rowHeightControl.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.rowHeightControl.Location = New System.Drawing.Point(0, 0)
        Me.rowHeightControl.Maximum = 20
        Me.rowHeightControl.Name = "rowHeightControl"
        Me.rowHeightControl.Size = New System.Drawing.Size(368, 45)
        Me.rowHeightControl.SmallChange = 100
        Me.rowHeightControl.TabIndex = 15
        Me.rowHeightControl.TickFrequency = 10
        Me.rowHeightControl.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'framesCountControl
        '
        Me.framesCountControl.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.framesCountControl.Location = New System.Drawing.Point(374, 0)
        Me.framesCountControl.Maximum = 100
        Me.framesCountControl.Minimum = 10
        Me.framesCountControl.Name = "framesCountControl"
        Me.framesCountControl.Size = New System.Drawing.Size(187, 45)
        Me.framesCountControl.SmallChange = 5
        Me.framesCountControl.TabIndex = 14
        Me.framesCountControl.TickFrequency = 5
        Me.framesCountControl.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.framesCountControl.Value = 10
        '
        'ReadFPSTimer
        '
        Me.ReadFPSTimer.Enabled = True
        Me.ReadFPSTimer.Interval = 200
        '
        'frameBuffer
        '
        Me.frameBuffer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.frameBuffer.Location = New System.Drawing.Point(0, 0)
        Me.frameBuffer.Name = "frameBuffer"
        Me.frameBuffer.Size = New System.Drawing.Size(1447, 691)
        Me.frameBuffer.TabIndex = 15
        '
        'WindowVideo
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(38, Byte), Integer), CType(CType(50, Byte), Integer), CType(CType(56, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1447, 734)
        Me.Controls.Add(Me.frameBuffer)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "WindowVideo"
        Me.Text = "Video Feed"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.rowHeightControl, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.framesCountControl, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents rowHeightControl As TrackBar
    Friend WithEvents framesCountControl As TrackBar
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ReadFPSTimer As Timer
    Friend WithEvents Label3 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents frameBuffer As Panel
End Class
