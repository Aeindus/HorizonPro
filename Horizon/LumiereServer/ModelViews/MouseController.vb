Imports System.IO
Imports System.Threading
Imports ServerCore

Public Class MouseController
    Inherits ComponentController

    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Private Shared cachedPen As New System.Drawing.Pen(Color.Red, 4)

    Private WithEvents btnLive As Button

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.mouse))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()

        viewer.TextBox7.Text = "on"
        viewer.TextBox7.ForeColor = Color.LightGreen

        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox7.Text = "off"
        viewer.TextBox7.ForeColor = Color.FromArgb(214, 69, 38)

        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        btnLive = viewer.btnLive

    End Sub
    Protected Overrides Sub RemoveHandlers()
        btnLive = Nothing

    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.mouse_pos
                Dim init_buff(24) As Byte

                If Not Await component.RecvData(init_buff, 16) Then
                    Return
                End If

                '' Do not continue processing if we are already waiting for this signal
                '' Otherwise we may in the end send a mouse click event or something else
                '' would break the protocol - we don't want to client to send any more packets
                If ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION Then
                    Return
                End If

                Dim screenW As UInt32 = BitConverter.ToUInt32(init_buff, 0)
                Dim screenH As UInt32 = BitConverter.ToUInt32(init_buff, 4)
                Dim x As UInt32 = BitConverter.ToUInt32(init_buff, 8)
                Dim y As UInt32 = BitConverter.ToUInt32(init_buff, 12)

                Dim videoForm As WindowVideo = WindowVideo.GetVideoWindow()
                If videoForm Is Nothing OrElse videoForm.IsDisposed OrElse Not videoForm.Visible OrElse CancellationToken.IsCancellationRequested Then
                    '' Cancel the current operation because the window is not active
                    '' The client is not really forced to stop sending packets and a rogue one may do that
                    '' However the receiving packets do not affect us because we "cut" the execution tree here
                    '' If cancellation is requested then the rogue client will be closed
                    Dim mouseEndToken As TokenSecurity = CreateDefaultToken(DataTypes.flags_cancelled_operation, 1, 5 * 1000)
                    Await component.SendHeader(CreateHeader(DataTypes.flags_cancel_operation, 0), mouseEndToken)

                    ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION
                    Return
                End If

                Using canvasGraphics As Graphics = videoForm.GetCanvasInstance().CreateGraphics()
                    canvasGraphics.DrawEllipse(cachedPen,
                              CInt(videoForm.GetCanvasInstance().Width * x / screenW),
                              CInt(videoForm.GetCanvasInstance.Height * y / screenH), 3, 3)
                End Using

                'Send live mouse position
                Dim mouse As Point = videoForm.GetLiveMouse()

                '' No movement registered
                If mouse.X < 0 Or mouse.Y < 0 Then
                    Return
                End If

                If Not Await component.SendHeader(CreateHeader(DataTypes.mouse_pos, 8), Nothing) Then
                    Return
                End If

                Dim buffer(7) As Byte
                BitConverter.GetBytes(Convert.ToUInt32(screenW * mouse.X / videoForm.GetCanvasInstance.Width)).CopyTo(buffer, 0)
                BitConverter.GetBytes(Convert.ToUInt32(screenH * mouse.Y / videoForm.GetCanvasInstance.Height)).CopyTo(buffer, 4)

                If Not Await component.SendData(buffer, 8) Then Return

                'Send live mouse clicks
                Dim tempClick As Integer = videoForm.GetLiveClick()
                If tempClick = 0 Then Return

                Dim mouseClickDataType As DataTypes = IIf(tempClick = 1, DataTypes.mouse_click_left, DataTypes.mouse_click_right)
                If Not Await component.SendHeader(CreateHeader(mouseClickDataType, 0), Nothing) Then Return

            Case DataTypes.flags_cancelled_operation
                '' Here I can allow a few more packet to go thru or directly cancel the token which can cause 
                ''   the component to disconnect prematurely.
                component.GetTokenManager().DisableAllTokens()

                '' If no exception was thrown till now it means:
                ''  this component will process no more packets so will block in internal receive
                ''  the cancellation wasn't requested for now but CAN be requested in the future
                '' Do this before throwing exception
                ControllerState = CONTROLLER_STATE.PAUSED

                '' Check if the reason for the window closing was a cancellation
                cancellationToken.ThrowIfCancellationRequested()
        End Select
    End Function
#End Region



#Region "Model"

#End Region


#Region "View"

#End Region


#Region "Events"
    Public Async Function StartMouse() As Task
        Dim videoForm As WindowVideo = WindowVideo.GetVideoWindow()

        '' Execute only if the component is not processing
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return
        End If

        '' Check if the window exists
        If videoForm Is Nothing OrElse videoForm.IsDisposed OrElse Not videoForm.Visible Then
            Return
        End If

        '' We don't want to be cancelled in the middle of the operation
        '' because on the next reconnection we will either receive a lot of packets or the component will close 
        '' because of expired token
        ControllerState = CONTROLLER_STATE.PROCESSING

        '' Start the mouse operation on the client side
        Await component.SendHeader(CreateHeader(DataTypes.mouse_start, 0), CreateLooseToken(DataTypes.mouse_pos).EnforceTime(2 * 60 * 60 * 1000))
    End Function
#End Region


End Class
