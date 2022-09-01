Public Class WindowVideo
    Private Shared windowVideoInstance As WindowVideo
    Private enableMouseSync As Boolean = False
    Private screenPos As New Point
    Private clickEvent As Integer
    Private rowFramePossibleSizes As List(Of Integer) = New List(Of Integer)()

    Public Property FPS As Double = 0
    Public Property FramesCountValue As UInt32 = 20
    Public ReadOnly Property LevelOfDetail As UInt32
        Get
            Return rowHeightControl.Value
        End Get
    End Property


    Public Shared Function GetVideoWindow() As WindowVideo
        Return windowVideoInstance
    End Function
    Public Function GetLiveMouse() As Point
        If enableMouseSync Then
            Return screenPos
        Else
            Return New Point With {.X = -1, .Y = -1}
        End If
    End Function

    ''' <summary>
    ''' 0 -nothing; 1 left click; 2 right click
    ''' </summary>
    ''' <returns></returns>
    Public Function GetLiveClick() As Integer
        Dim temp As Integer = clickEvent
        clickEvent = 0

        Return temp
    End Function
    Public Function GetCanvasInstance() As Control
        Return frameBuffer
    End Function



    ''' <summary>
    ''' Adjust controls for best performance
    ''' </summary>
    Public Sub OptimizeControls(maxHeight As UInt32, maxWidth As UInt32, heightLevels As List(Of Integer))
        '' Calculate a series of dividers of the image's height
        rowFramePossibleSizes = heightLevels

        '' Show the dividers on the trackbar
        rowHeightControl.SetRange(0, rowFramePossibleSizes.Count - 1)
        If rowHeightControl.Value > rowHeightControl.Maximum Then
            rowHeightControl.Value = rowHeightControl.Maximum
        End If


        UpdateSettingsControls()
    End Sub
    Public Sub UpdateSettingsControls()
        Label2.Text = "Row height: " + rowFramePossibleSizes(LevelOfDetail).ToString()

        framesCountControl.Value = FramesCountValue
        Label1.Text = "Frames count: " + framesCountControl.Value.ToString
    End Sub
    Private Function CalculateMultipliers(nr As UInt32) As List(Of Integer)
        Dim maxSteps As Integer = 7

        Dim result As New List(Of Integer)

        For multiplier As Integer = 1 To maxSteps
            If nr Mod multiplier = 0 Then
                result.Add(nr / multiplier)
            End If
        Next
        result.Reverse()
        Return result
    End Function

    Private Sub rowHeightControl_Scroll(sender As Object, e As EventArgs) Handles rowHeightControl.Scroll
        UpdateSettingsControls()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        enableMouseSync = Not enableMouseSync

        If enableMouseSync Then
            Button1.BackColor = Color.Red
        Else
            Button1.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub framesCountControl_Scroll(sender As Object, e As EventArgs) Handles framesCountControl.Scroll
        FramesCountValue = framesCountControl.Value
        UpdateSettingsControls()
    End Sub




    Private Sub WindowVideo_Load(sender As Object, e As EventArgs) Handles Me.Load
        rowHeightControl.Minimum = 0
        rowHeightControl.Maximum = rowFramePossibleSizes.Count() - 1
        rowHeightControl.Value = 0
        rowHeightControl.TickFrequency = 1 'rowFramePossibleSizes.Count()
        rowHeightControl.SmallChange = 1
        rowHeightControl.LargeChange = 1


        framesCountControl.Minimum = 20
        framesCountControl.Maximum = 100
        framesCountControl.SmallChange = 5
        framesCountControl.LargeChange = 5
        framesCountControl.Value = FramesCountValue
        UpdateSettingsControls()

        windowVideoInstance = Me
    End Sub

    Private Sub ReadFPSTimer_Tick(sender As Object, e As EventArgs) Handles ReadFPSTimer.Tick
        Label3.Text = "FPS: " + FPS.ToString()
    End Sub

    Private Sub frameBuffer_MouseMove(sender As Object, e As MouseEventArgs) Handles frameBuffer.MouseMove
        screenPos.X = e.X
        screenPos.Y = e.Y
    End Sub

    Private Sub frameBuffer_MouseClick(sender As Object, e As MouseEventArgs) Handles frameBuffer.MouseClick
        If e.Button = MouseButtons.Left Then
            clickEvent = 1
        Else
            clickEvent = 2
        End If
    End Sub
End Class