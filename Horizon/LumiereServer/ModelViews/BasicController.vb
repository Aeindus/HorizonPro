Imports System.ComponentModel
Imports System.Threading
Imports ServerCore

Public MustInherit Class BasicController
    Protected Shared viewer As LumiereServer

    Public Shared Sub SetViewer(viewerForm As LumiereServer)
        viewer = viewerForm
    End Sub


    Public MustOverride Async Function Init() As Task
    Public MustOverride Async Function UnInit() As Task
    Protected MustOverride Sub AddHandlers()
    Protected MustOverride Sub RemoveHandlers()

    Protected Sub GetControlByName(name As String, ByRef outObject As Object)
        For Each control As Control In viewer.Controls
            If control.Name = name Then
                outObject = control
            End If
        Next
        For Each control As Component In viewer.Container.Components
            If control.Site.Name = name Then
                outObject = control
            End If
        Next
    End Sub

End Class
