Imports System.IO
Imports System.Threading
Imports ServerCore

Public Class UserInfoController
    Inherits BasicController


    Private user As UserWrapper
    Private descriptorSnapshot As StructureUserDescriptor

    Private WithEvents txtUserInfoOs As TextBox
    Private WithEvents txtUserInfoVersion As TextBox
    Private WithEvents txtUserInfoPc As TextBox
    Private WithEvents txtUserInfoExtraData As TextBox
    Private WithEvents txtUserInfoAlias As TextBox
    Private WithEvents txtUserInfoName As TextBox
    Private WithEvents txtUserInfoId As TextBox

    Public Sub New(user As UserWrapper)
        Me.user = user
    End Sub

    Public Shared Sub StartupInit()
        ClearUserInfo()
    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()

        ClearUserInfo()
        PopulateUserInfo()

        descriptorSnapshot = user.Descriptor

        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task
        ClearUserInfo()
        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()
        txtUserInfoId = viewer.txtUserInfoId
        txtUserInfoName = viewer.txtUserInfoName
        txtUserInfoAlias = viewer.txtUserInfoAlias
        txtUserInfoExtraData = viewer.txtUserInfoExtraData
        txtUserInfoPc = viewer.txtUserInfoPc
        txtUserInfoVersion = viewer.txtUserInfoVersion
        txtUserInfoOs = viewer.txtUserInfoOs
    End Sub

    Protected Overrides Sub RemoveHandlers()
        txtUserInfoId = Nothing
        txtUserInfoName = Nothing
        txtUserInfoAlias = Nothing
        txtUserInfoExtraData = Nothing
        txtUserInfoPc = Nothing
        txtUserInfoVersion = Nothing
        txtUserInfoOs = Nothing
    End Sub



#Region "Model"
    ''' <summary>
    ''' Sends the updated information to the user
    ''' </summary>
    Private Sub UpdateUserDescriptor(value As String, entryType As ModifiedUserEntry)
        If entryType = ModifiedUserEntry.CustomAlias Then
            descriptorSnapshot.customName = value
        ElseIf entryType = ModifiedUserEntry.ExtraData Then
            descriptorSnapshot.extraData = value
        ElseIf entryType = ModifiedUserEntry.PcDescription Then
            descriptorSnapshot.pcDescription = value
        End If

        '' This will not be awaited for. No issues because the sendHeader method used here is synchronized
        user.UpdateDescriptor(descriptorSnapshot).ContinueWith(Sub()
                                                                   '' Refresh the user info
                                                                   PopulateUserInfo()
                                                               End Sub, TaskScheduler.FromCurrentSynchronizationContext)

    End Sub
#End Region


#Region "View"

    ''' <summary>
    ''' Populate the client tab with information about the selected user
    ''' </summary>
    Private Sub PopulateUserInfo()
        txtUserInfoId.Text = user.SystemId
        txtUserInfoName.Text = user.Descriptor.userName
        txtUserInfoAlias.Text = user.Descriptor.customName
        txtUserInfoExtraData.Text = user.Descriptor.extraData
        txtUserInfoPc.Text = user.Descriptor.pcDescription
        txtUserInfoVersion.Text = user.Descriptor.version
        txtUserInfoOs.Text = user.Descriptor.osVersion
    End Sub
    Private Shared Sub ClearUserInfo()
        Dim emptystr As String = "───────────────────"
        viewer.txtUserInfoId.Text = emptystr
        viewer.txtUserInfoName.Text = emptystr
        viewer.txtUserInfoAlias.Text = emptystr
        viewer.txtUserInfoExtraData.Text = emptystr
        viewer.txtUserInfoPc.Text = emptystr
        viewer.txtUserInfoVersion.Text = emptystr
        viewer.txtUserInfoOs.Text = emptystr
    End Sub

    Private Sub OpenUserEditor(entryType As ModifiedUserEntry)
        Dim modalWindow As New UserEditor
        Dim descriptor As StructureUserDescriptor = user.Descriptor

        If entryType = ModifiedUserEntry.CustomAlias Then
            modalWindow.Value = descriptor.customName
        ElseIf entryType = ModifiedUserEntry.ExtraData Then
            modalWindow.Value = descriptor.extraData
        ElseIf entryType = ModifiedUserEntry.PcDescription Then
            modalWindow.Value = descriptor.pcDescription
        End If

        If modalWindow.ShowDialog() = DialogResult.OK Then
            UpdateUserDescriptor(modalWindow.Value, entryType)
        End If
        modalWindow.Dispose()
    End Sub
#End Region


#Region "Events"
    Private Sub txtUserInfoPc_DoubleClick(sender As Object, e As EventArgs) Handles txtUserInfoPc.DoubleClick
        OpenUserEditor(ModifiedUserEntry.PcDescription)
    End Sub
    Private Sub txtUserInfoExtraData_DoubleClick(sender As Object, e As EventArgs) Handles txtUserInfoExtraData.DoubleClick
        OpenUserEditor(ModifiedUserEntry.ExtraData)
    End Sub
    Private Sub txtUserInfoAlias_DoubleClick(sender As Object, e As EventArgs) Handles txtUserInfoAlias.DoubleClick
        OpenUserEditor(ModifiedUserEntry.CustomAlias)
    End Sub
#End Region


End Class
