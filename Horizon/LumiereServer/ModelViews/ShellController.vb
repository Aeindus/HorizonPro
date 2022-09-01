Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading
Imports ServerCore

Public Class ShellController
    Inherits ComponentController

    Private user As UserWrapper
    Private cancellationToken As CancellationToken
    Private Const logo As String = "                                    Horizon Dova (◣_◢) LumiereSombre" + vbNewLine

    Private WithEvents rtfCmdOutput As RichTextBox
    Private WithEvents txtCmdInput As TextBox
    Private WithEvents cmdHistoryList As ListBox

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.shell))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()
        viewer.rtfCmdOuput.Text = logo
    End Sub

    Public Overrides Async Function Init() As Task
        AddHandlers()

        viewer.TextBox6.Text = "on"
        viewer.TextBox6.ForeColor = Color.LightGreen

        cancellationToken.Register(async Sub()
                                       await CancelOperation()
                                   End Sub)
        Await InitShell()
        Await RefreshConsole()
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox6.Text = "off"
        viewer.TextBox6.ForeColor = Color.FromArgb(214, 69, 38)
        rtfCmdOutput.Text = logo

        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        rtfCmdOutput = viewer.rtfCmdOuput
        txtCmdInput = viewer.txtCmdInput
        cmdHistoryList = viewer.cmdHistoryList
    End Sub
    Protected Overrides Sub RemoveHandlers()
        rtfCmdOutput = Nothing
        txtCmdInput = Nothing
        cmdHistoryList = Nothing
    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.shell
                Dim buffer(200) As Byte
                If Not Await component.RecvFragmented(buffer, buffer.Length, Sub(charCount As UInt64)
                                                                                 Dim offset As Integer = 0
                                                                                 If charCount > 3 AndAlso buffer(0) = 12 AndAlso buffer(1) = 13 AndAlso buffer(2) = 10 Then
                                                                                     '' The console will sometimes output vbFormFeed & VbCrLf on new commands.
                                                                                     '' These are filtered out here
                                                                                     offset = 3
                                                                                 End If

                                                                                 '' Also filter FormFeed in the middle of stream
                                                                                 rtfCmdOutput.AppendText(System.Text.Encoding.ASCII.GetString(buffer, offset, charCount).Replace(vbFormFeed, ""))
                                                                             End Sub, True) Then
                    Return
                End If

            Case DataTypes.flags_cancelled_operation
                rtfCmdOutput.AppendText(vbNewLine + vbNewLine)

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
    Private Async Function InitShell() As Task
        '' Execute only if the component is not processing
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.PROCESSING


        Dim infiniteToken As TokenSecurity = CreateLooseToken(DataTypes.shell).EnforceTime(30 * 60 * 1000)
        Await component.SendHeader(CreateHeader(DataTypes.shell_init, 0, ""), infiniteToken)
    End Function


    Private Async Function ExecuteCommand(constructedCommand As String) As Task
        If ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION Then
            Return
        End If

        If ControllerState = CONTROLLER_STATE.PAUSED Then
            Await InitShell()
        End If

        Await component.SendHeader(CreateHeader(DataTypes.shell, 0, constructedCommand), Nothing)
    End Function
    Private Async Function CancelOperation() As Task
        If Not ControllerState = CONTROLLER_STATE.PROCESSING Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.WAITING_CANCELLATION

        ''The execution will actually continue but in the flags_cancelled_operation response 
        Await component.SendHeader(CreateHeader(DataTypes.flags_cancel_operation, 0, ""), CreateDefaultToken(DataTypes.flags_cancelled_operation))
    End Function

    Private Async Function RefreshConsole() As Task
        Await component.SendHeader(CreateHeader(DataTypes.shell, 0, ""), Nothing)
    End Function
#End Region


#Region "View"

#End Region


#Region "Events"
    Private Sub cmdHistoryList_DoubleClick(sender As Object, e As EventArgs) Handles cmdHistoryList.DoubleClick
        txtCmdInput.Text = CStr(cmdHistoryList.SelectedItem).TrimStart(" ")
    End Sub
    Private Async Sub rtfCmdOutput_KeyPress(sender As Object, e As KeyPressEventArgs) Handles rtfCmdOutput.KeyPress
        If e.KeyChar = ChrW(Keys.Cancel) And rtfCmdOutput.SelectedText.Length = 0 Then
            Await CancelOperation()
            Return
        End If
    End Sub
    Private Async Sub txtCmdInput_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCmdInput.KeyPress
        If e.KeyChar = ChrW(Keys.Cancel) Then
            If txtCmdInput.SelectedText.Length = 0 Then
                Await CancelOperation()
            End If
        End If

        If e.KeyChar = ChrW(Keys.Enter) Then
            '' Clean all non-ascii characters
            txtCmdInput.Text = Regex.Replace(txtCmdInput.Text, "[^\u0020-\u007E]+", String.Empty)

            If txtCmdInput.Text.Replace(" ", "") = "" Then
                txtCmdInput.Text = ""
            End If

            '' Add this command to history only if it isn't empty
            If Not txtCmdInput.Text = "" Then
                Dim historyCommand As String = "   " + txtCmdInput.Text
                If Not cmdHistoryList.Items.Contains(historyCommand) Then
                    cmdHistoryList.Items.Add(historyCommand)
                End If
            End If

            Dim constructedCommand As String = txtCmdInput.Text + vbNewLine
            If txtCmdInput.Text = "cls" Then
                rtfCmdOutput.Text = ""
            End If
            txtCmdInput.Text = ""

            Await ExecuteCommand(constructedCommand)
            Return
        End If
    End Sub

#End Region


End Class
