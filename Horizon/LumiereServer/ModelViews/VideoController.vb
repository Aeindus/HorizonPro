Imports System.IO
Imports System.Threading
Imports ServerCore

Public Class VideoController
    Inherits ComponentController

    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Private videoData As FrameData = Nothing
    Private fpsCounter As New Stopwatch

    Private WithEvents videoForm As WindowVideo
    Private WithEvents btnLive As Button


    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.screenCapture))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Async Function Init() As Task
        AddHandlers()

        viewer.TextBox1.Text = "on"
        viewer.TextBox1.ForeColor = Color.LightGreen

        videoForm = New WindowVideo
        videoData = New FrameData(videoForm.GetCanvasInstance())

        fpsCounter.Start()

        Await SendInitPacket()
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox1.Text = "off"
        viewer.TextBox1.ForeColor = Color.FromArgb(214, 69, 38)

        videoForm.Hide()

        videoData.Dispose()
        videoForm.Dispose()

        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        btnLive = viewer.btnLive
    End Sub
    Protected Overrides Sub RemoveHandlers()
        btnLive = Nothing
    End Sub

    '' I can only use integer divisors of the full height of a frame.
    '' Otherwise I have to handle leftover at the end of a full frame which I cannot do
    '' because StretchDIBits restricts me.
    '' This function accespt a structure with the height of the DIB section to be drawn which must 
    '' contain the height of the area. But this height is set in the caching method.
    '' Also I cannot use two different structures for StretchDIBits (for full row height and for the left over)
    '' (perhaps only with a hack)

#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.video_init
                Dim width As UInt32
                Dim height As UInt32
                Dim videoParametersBuffer(8) As Byte

                If Not Await component.RecvData(videoParametersBuffer, 8) Then Return

                width = BitConverter.ToUInt32(videoParametersBuffer, 0)
                height = BitConverter.ToUInt32(videoParametersBuffer, 4)

                '' Cache important data used for drawing
                Dim validationResult As ValidationResponse = videoData.InitDimensions(width, height)
                If Not validationResult.Successful Then
                    Throw New Exception(validationResult.Information)
                End If

                '' Adjust window controls with optimum settings
                videoForm.OptimizeControls(videoData.clientScreenHeight, videoData.clientScreenWidth, videoData.GetPossibleFrameHeights())


            Case DataTypes.video_frame
                Dim yOffset As UInt32
                Dim frameBytesSize As UInt64
                Dim videoParametersBuffer(24) As Byte

                If Not Await component.RecvData(videoParametersBuffer, 4) Then Return
                yOffset = BitConverter.ToUInt32(videoParametersBuffer, 0)

                frameBytesSize = packet.packetSize - 4

                '' Check received data against the cached structures
                Dim validationResult As ValidationResponse = videoData.CheckConsistency(yOffset, frameBytesSize)
                If Not validationResult.Successful Then
                    Throw New Exception(validationResult.Information)
                End If

                If Not Await component.RecvData(videoData.cachedFrameBuffer, frameBytesSize) Then Return

                Try
                    videoData.updateDirectFrame(yOffset)
                Catch ex As Exception
                    Throw New Exception("Error updating frame: " + ex.Message)
                End Try

                '' Calculate the FPS on new frame
                If yOffset = 0 Then
                    videoForm.FPS = Math.Floor(1000 / fpsCounter.ElapsedMilliseconds())
                    fpsCounter.Restart()
                End If

            Case DataTypes.video_end
                '' Revoke all tokens so that there are no leftover from packets of type video_frame
                '' In total there are <frameCount>+1 tokens created and in this state all of them should be used up
                component.GetTokenManager().DisableAllTokens()

                '' Signal that no more frames are expected
                ControllerState = CONTROLLER_STATE.PAUSED

                '' Allow cancellation in this short time window
                cancellationToken.ThrowIfCancellationRequested()

                If videoForm.Visible Then
                    Await StartStreaming()
                End If
        End Select
    End Function
#End Region



#Region "Model"
    Private Function StartStreaming() As Task
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return Task.CompletedTask
        End If

        '' Only start the video stream if the videoData was initialized
        If Not videoData.IsInitialized() Then
            Return Task.CompletedTask
        End If

        '' The component now should be self-sustainable
        '' If it does not receive the ordered amount of frames then it will disconnect itself on uninitialization
        ''  (because it enters the hard reset mode)
        '' Also the frames token is short lived
        ControllerState = CONTROLLER_STATE.PROCESSING

        '' Set the video data needed for the next video session
        Dim validationResult As ValidationResponse = videoData.SetVideoData(videoForm.LevelOfDetail)
        If Not validationResult.Successful Then
            Return Task.CompletedTask
        End If

        Return SendStartVideoPacket(videoForm.FramesCountValue, videoData.cachedSectionsPerFrame, videoData.cachedRowHeight)
    End Function
    Private Async Function SendStartVideoPacket(frameCount As Integer, sectionsPerFrame As Integer, rowHeight As Integer) As Task
        Dim buffer(7) As Byte
        Dim minimumFPS As Integer = 1
        Dim expectedTotalTimeMS As Integer = Math.Floor(frameCount * 1 / minimumFPS * 1000)

        '' Allow one token use for the video_end packet
        Dim videoEndToken As TokenSecurity = CreateDefaultToken(DataTypes.video_end, 1, expectedTotalTimeMS)
        Dim videoEndPacket As StructurePacketHeader = CreateHeader(DataTypes.video_start, 0)
        component.GetTokenManager().RegisterPacket(videoEndPacket, videoEndToken)

        '' Restart fps counter for new frames
        fpsCounter.Restart()

        Await component.SendHeader(CreateHeader(DataTypes.video_start, 8 + videoEndPacket.tokenId.Length), CreateDefaultToken(DataTypes.video_frame, frameCount * sectionsPerFrame, expectedTotalTimeMS))

        BitConverter.GetBytes(frameCount).CopyTo(buffer, 0)
        BitConverter.GetBytes(rowHeight).CopyTo(buffer, 4)
        Await component.SendData(buffer, 8)
        Await component.SendData(videoEndPacket.tokenId, videoEndPacket.tokenId.Length)
    End Function

    Private Async Function SendInitPacket() As Task
        Await component.SendHeader(CreateHeader(DataTypes.video_init, 0), CreateDefaultToken(DataTypes.video_init))
    End Function
#End Region


#Region "View"

#End Region


#Region "Events"
    Private Async Sub btnLive_Click(sender As Object, e As EventArgs) Handles btnLive.Click
        If Not videoForm.Visible Then
            videoForm.Show()
            Await StartStreaming()

            '' Get mouse controller to also start it
            Dim mouseController As MouseController = viewer.GetController(ComponentTypes.mouse)
            If mouseController IsNot Nothing Then
                Await mouseController.StartMouse()
            End If
        End If
    End Sub

    ''' <summary>
    ''' It makes absolute sense to not allow the form to close
    ''' The form has the same lifetime as this controller so it should not be closed and reopened etc...
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub videoForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles videoForm.FormClosing
        '' Don't close form but keep in memory 
        e.Cancel = True

        videoForm.Hide()
    End Sub
#End Region


End Class
