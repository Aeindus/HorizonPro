Imports System.ComponentModel
Imports System.Threading
Imports ServerCore

Public MustInherit Class ComponentController
    Inherits BasicController

    Public Enum CONTROLLER_STATE
        PROCESSING
        PAUSED
        WAITING_CANCELLATION
    End Enum

    Protected component As ComponentInstance
    Protected syncUse As New AsyncCountdownEvent
    Private internalControllerState As CONTROLLER_STATE = CONTROLLER_STATE.PAUSED

    ''' <summary>
    ''' A variable which monitors the state of the controller.
    ''' Used for determining when it's the proper time to initialize or send the first processing packet.
    ''' Also it automatically sets AutoCancellation of the underlying component
    ''' </summary>
    ''' <returns></returns>
    Protected Property ControllerState As CONTROLLER_STATE
        Set(value As CONTROLLER_STATE)
            internalControllerState = value

            If value = CONTROLLER_STATE.PROCESSING Then
                component.AutoCancellation = False
            End If
            If value = CONTROLLER_STATE.PAUSED Then
                component.AutoCancellation = True
            End If
        End Set
        Get
            Return internalControllerState
        End Get
    End Property



    Public Sub New(component As ComponentInstance)
        Me.component = component
    End Sub


    Public MustOverride Async Function HandlePacket(user As UserWrapper, packet As StructurePacketHeader, token As TokenSecurity) As Task

    ''' <summary>
    ''' Returns a task that waits for all asynchronous functions to complete.
    ''' This signals that no resources will be used after this completion IF the next call uninitializes the controller.
    ''' </summary>
    ''' <returns></returns>
    Public Function Wait() As Task
        Return syncUse.WaitAsync()
    End Function

End Class
