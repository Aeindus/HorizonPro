Imports System.Threading
Imports ServerCore

Public Class Controller
    Inherits ComponentController


    Private user As UserWrapper
    Private cancellationToken As CancellationToken



    Public Sub New(user As UserWrapper, cancellationToken As CancellationToken)
        MyBase.New(user.GetComponent(ComponentTypes.))
        Me.user = user
        Me.cancellationToken = cancellationToken
    End Sub

    Public Shared Sub StartupInit()

    End Sub

    Public Overrides Function Init() As Task
        AddHandlers()

        Return Task.CompletedTask
    End Function

    Public Overrides Function UnInit() As Task

        RemoveHandlers()
        Return Task.CompletedTask
    End Function

    Protected Overrides Sub AddHandlers()


    End Sub
    Protected Overrides Sub RemoveHandlers()

    End Sub


#Region "PacketProcessor"
    Public Overrides Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task

    End Function
#End Region



#Region "Model"

#End Region


#Region "View"

#End Region


#Region "Events"

#End Region
End Class
