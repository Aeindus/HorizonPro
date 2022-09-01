Imports System.IO
Imports System.Net.Sockets
Imports System.Threading

Public Class ComponentInstance
    Private lockHeaderSending As New SemaphoreSlim(1, 1)
    Private lockHeaderReceiving As New SemaphoreSlim(1, 1)
    Private userdescription As New StructureUserDescriptor
    Private tokenManagement As New TokenManagement()
    Private packetValidator As PacketValidator

    Private authentificateRecvTimeout As Integer = 5000
    Private hardCancellationTimeout As Integer = 5000
    Private netclient As TcpClient
    Private netstream As NetworkStream
    Private report As Reporting = Reporting.GetInstance()
    Private internalIsClosed As Boolean = False

    Private delayedCancellationSource As CancellationTokenSource

    Private mustRecv As UInt64
    Private mustSend As UInt64

    '' Callbacks
    '' Throws OperationCanceledException if component can be reused or any exception whose message will be recorded
    Public OnReceiveCallback As Func(Of ComponentInstance, StructurePacketHeader, TokenSecurity, Task)
    Public OnComponentInit As Func(Of ComponentInstance, CancellationToken, Task)
    Public OnComponentUnInit As Func(Of ComponentInstance, Task)
    Public OnComponentDisconnect As Action(Of ComponentInstance)
    Public OnComponentFinished As Action(Of ComponentInstance)

    ''' <summary>
    ''' Sets wether the internal packet handler can automatically cancel the receive operation 
    ''' </summary>
    Public Property AutoCancellation As Boolean = True
    Public Property Authenticated As Boolean
    Public ReadOnly Property IsClosed As Boolean
        Get
            Return internalIsClosed
        End Get
    End Property

    Public ReadOnly Property CanUse As Boolean
        Get
            Return Not IsClosed() And Authenticated = True
        End Get
    End Property
    Public ReadOnly Property Ip As String
        Get
            Return Address.Split(":")(0)
        End Get
    End Property
    Public ReadOnly Property Address As String
    Public ReadOnly Property ComponentType As ComponentTypes
        Get
            Return CType(Descriptor.clientType, ComponentTypes)
        End Get
    End Property
    Friend ReadOnly Property Descriptor As StructureUserDescriptor
        Get
            Return userdescription
        End Get
    End Property


    Public Sub New(netclient As TcpClient)
        Me.netclient = netclient
        packetValidator = New PacketValidator(tokenManagement)

        netstream = netclient.GetStream()
        Address = netclient.Client.RemoteEndPoint.ToString()
        SetKeepAlive(True)
    End Sub
    Protected Overrides Sub Finalize()
        CloseClient()
        tokenManagement.ReleaseInstance()
    End Sub

    Public Sub CloseClient()
        If Not IsClosed Then
            netclient.Client.Shutdown(SocketShutdown.Both)
            internalIsClosed = True
        End If

        '' TODO: it would make sense to enable the line below in order to catch after-use bugs.
        '' But this function may be called on UI thread while WaitForData loops in another thread and will throw null exception
        'netclient = Nothing
    End Sub

    ''' <summary>
    ''' Sends a header and registers the associated token in the database
    ''' Locks the component and this function for any further header-sending until all bytes are sent.
    ''' The fixed amount of body-data must be sent to unlock the component
    ''' </summary>
    ''' <param name="tokenInfo">Set to nothing to prevent registration of the token</param>
    Public Async Function SendHeader(sPacket As StructurePacketHeader, tokenInfo As Nullable(Of TokenSecurity)) As Task(Of Boolean)
        If IsClosed Then
            Return False
        End If

        '' Wait for the semaphore to unlock and allow another packet to be sent. 
        Await lockHeaderSending.WaitAsync()

        If mustSend <> 0 Then
            CloseClient()
            Throw New Exception("Header was being sent with data pending")
        End If

        If tokenInfo IsNot Nothing Then
            '' Associates the packet's token to the token security descriptor
            tokenManagement.RegisterPacket(sPacket, tokenInfo)
        End If

        '' Set how much data must be sent
        mustSend = sPacket.packetSize

        Dim bufferedPacket() As Byte = sPacket.SerializeAndEncrypt()
        If Not Await SendRaw(bufferedPacket, bufferedPacket.Length) Then
            Return False
        End If

        '' If the packet is empty release the lock right away
        If sPacket.packetSize = 0 Then
            lockHeaderSending.Release()
        End If
        Return True
    End Function

    ''' <summary>
    ''' Sends body data. Handles common exceptions
    ''' Closes client on some exceptions
    ''' </summary>
    Public Function SendData(buffer() As Byte, cbSize As UInt64) As Task(Of Boolean)
        If IsClosed Then
            Return Task.FromResult(False)
        End If

        If cbSize = 0 Then
            Return Task.FromResult(True)
        End If

        If lockHeaderSending.CurrentCount <> 0 Then
            CloseClient()
            Throw New Exception("Body data was sent without a packet header")
        End If
        If cbSize > mustSend Then
            CloseClient()
            Throw New Exception("More body data was being sent than allotted for")
        End If

        mustSend -= cbSize
        If mustSend = 0 Then
            '' Unlock header-sending semaphore
            lockHeaderSending.Release()
        End If
        Return SendRaw(buffer, cbSize)
    End Function

    ''' <summary>
    ''' Send raw data to the network. Handles client disconnection
    ''' Closes client on some exceptions
    ''' </summary>
    Private Async Function SendRaw(buffer() As Byte, cbSize As UInt64) As Task(Of Boolean)
        Dim offset As UInt64 = 0

        Try
            While cbSize > 0
                '' Use this technique because WriteAsync only accepts Int as count so would fail for larger messages
                '' Also the driver may refuse to atomically move large data so limit to a reasonable constant

                Dim limitedCount As UInt64 = Math.Min(65000, cbSize)

                Await netstream.WriteAsync(buffer, offset, limitedCount)
                offset += limitedCount
                cbSize -= limitedCount
            End While

        Catch ex As IOException
            '' This exception should ONLY handle disconnection events or timeouts
            '' All other exceptions must be passed along to be analyzed
            CloseClient()
            Return False
        End Try

        Return True
    End Function


    ''' <summary>
    ''' Waits for data to be available while allowing interruption.
    ''' Throws OperationCanceledException
    ''' </summary>
    Private Async Function WaitForData(cancellationToken As CancellationToken) As Task
        '' Include some speed up if the cancellation is none
        If Not cancellationToken.CanBeCanceled Then
            Return
        End If

        Try
            '' Issue first a fast Available call because it does not block
            '' NetworkStream.DataAvailable is the same as socket.Available <> 0
            While netclient.Client.Available = 0
                If AutoCancellation Then
                    cancellationToken.ThrowIfCancellationRequested()
                End If

                '' Use poll because it returns true when the socket is closed
                '' which will allow the execution to continue but will fail on the next receive
                '' which will be used to detect the disconnection
                If Await Task.Run(Function() As Boolean
                                      Return netclient.Client.Poll(20000, SelectMode.SelectRead)
                                  End Function) Then
                    Return
                End If
            End While
        Catch ex As SocketException
            '' Do not allow this exception to escape
            '' This exception signals the socket is closed
            CloseClient()
        End Try
    End Function

    ''' <summary>
    ''' Receives a header from the network and fills the passed object with the data. Also checks the corresponding token
    ''' Locks the component and this function for any further header-receiving until all bytes are read.
    ''' The fixed amount of body-data must be received to unlock the component.
    ''' Throws OperationCanceledException if the read is succesfully cancelled without reading any byte from the network
    ''' </summary>
    Public Async Function RecvHeader(sPacket As StructurePacketHeader, cancellationToken As CancellationToken) As Task(Of QueryResponse(Of TokenSecurity))
        If IsClosed Then
            Return New QueryResponse(Of TokenSecurity) With {.Successful = False}
        End If

        '' Wait for the semaphore to unlock and allow another packet to be sent. 
        Await lockHeaderReceiving.WaitAsync()

        If mustRecv <> 0 Then
            CloseClient()
            Throw New Exception("Header was being received with data pending")
        End If

        Dim buffer(sPacket.GetSize() - 1) As Byte

        '' Allow this operation to be cancelled
        Await WaitForData(cancellationToken)

        If Not Await RecvRaw(buffer, buffer.Length) Then
            Return New QueryResponse(Of TokenSecurity) With {.Successful = False}
        End If

        sPacket.DecryptAndDeserialize(buffer, 0)
        mustRecv = sPacket.packetSize

        Dim tokenValidationResult As QueryResponse(Of TokenSecurity) = packetValidator.ValidatePacket(sPacket)
        If Not tokenValidationResult.Successful Then
            CloseClient()
            report.Low(ComponentType.ToString(), "RecvHeader", tokenValidationResult.Information, True)
            Return New QueryResponse(Of TokenSecurity) With {.Successful = False}
        End If

        '' If the packet is empty release the lock right away
        If sPacket.packetSize = 0 Then
            lockHeaderReceiving.Release()
        End If

        Return tokenValidationResult
    End Function

    ''' <summary>
    ''' Receives a structure as defined by the parameter.
    ''' This fails if no data can be read to fill the structure.
    ''' </summary>
    Public Function RecvStructure(sPacket As VariableBasicStructure) As Task(Of Boolean)
        If IsClosed Then
            Return Task.FromResult(False)
        End If

        If lockHeaderReceiving.CurrentCount <> 0 Then
            CloseClient()
            Throw New Exception("Body data was received without a packet header")
        End If

        If Not sPacket.Deserialize(netstream, mustRecv) Then
            CloseClient()
            Throw New Exception("More body data was being received than allotted for")
        End If

        mustRecv -= sPacket.GetSize()
        If mustRecv = 0 Then
            '' Unlock header-receiving semaphore
            lockHeaderReceiving.Release()
        End If
        Return Task.FromResult(True)
    End Function


    ''' <summary>
    ''' Receives body data in a fragmented manner.
    ''' If readRemaining is True then it stops reading the data if it does not fit in the fragment size without disconnecting the user.
    ''' Otherwise it also reads the remaining bytes.
    ''' </summary>
    ''' <param name="buffer"></param>
    ''' <param name="fragmentSize"></param>
    ''' <param name="OnFragmentReceived">Callback which is executed for each possible fragment. 
    ''' If readRemaining is true then the argument represents the read bytes in the last packet</param>
    ''' <param name="readRemaining"></param>
    ''' <returns></returns>
    Public Async Function RecvFragmented(buffer() As Byte, fragmentSize As UInt64, OnFragmentReceived As Action(Of UInt64), Optional readRemaining As Boolean = False) As Task(Of Boolean)
        If IsClosed Then
            Return False
        End If

        If fragmentSize = 0 Then
            Return True
        End If

        '' Nothing to receive
        If mustRecv = 0 Then
            Return True
        End If

        If lockHeaderReceiving.CurrentCount <> 0 Then
            CloseClient()
            Throw New Exception("Body data was received without a packet header")
        End If

        While True
            If fragmentSize > mustRecv Then
                If readRemaining Then
                    fragmentSize = mustRecv
                Else
                    Exit While
                End If

            End If

            If Not Await RecvData(buffer, fragmentSize) Then
                Return False
            End If

            OnFragmentReceived(fragmentSize)

            If mustRecv = 0 Then
                Exit While
            End If
        End While

        Return True
    End Function

    ''' <summary>
    ''' Receives body data. Handles common exceptions
    ''' Closes client on some exceptions
    ''' </summary>
    Public Function RecvData(buffer() As Byte, cbSize As UInt64) As Task(Of Boolean)
        If IsClosed Then
            Return Task.FromResult(False)
        End If

        If cbSize = 0 Then
            Return Task.FromResult(True)
        End If

        If lockHeaderReceiving.CurrentCount <> 0 Then
            CloseClient()
            Throw New Exception("Body data was received without a packet header")
        End If
        If cbSize > mustRecv Then
            CloseClient()
            Throw New Exception("More body data was being received than allotted for")
        End If

        mustRecv -= cbSize
        If mustRecv = 0 Then
            '' Unlock header-receiving semaphore
            lockHeaderReceiving.Release()
        End If
        Return RecvRaw(buffer, cbSize)
    End Function

    ''' <summary>
    ''' Receives raw data from the network. Handles client disconnection
    ''' Closes client on some exceptions
    ''' </summary>
    Private Async Function RecvRaw(buffer() As Byte, cbSize As UInt64) As Task(Of Boolean)
        Dim offset As UInt64 = 0
        Try
            While cbSize > 0
                Dim recvlen As UInt64 = Await netstream.ReadAsync(buffer, offset, cbSize)
                If recvlen = 0 Then
                    '' Socket closed
                    CloseClient()
                    Return False
                End If

                offset += recvlen
                cbSize -= recvlen
            End While
        Catch ex As IOException
            '' This exception should ONLY handle disconnection events or timeouts
            '' All other exceptions must be passed along to be analyzed
            CloseClient()
            Return False
        End Try

        Return True
    End Function




    ''' <summary>
    ''' Send an authentification packet to the client.
    ''' Safe method - uses timeout for receive function calls
    ''' Creates a corectly named folder based on the client user id.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function Authentificate() As Task(Of Boolean)
        Dim syncDate As Date = Date.Now

        If IsClosed Then
            Return False
        End If

        '' Code flow: the first time an user connects I send him a StructureUserDescriptor
        '' and wait for a StructureUserDescriptor back again.

        '' Assign a random id. The user may already have an id so will ignore this one
        userdescription.ApplyRandomUID()

        Dim authHeader As StructurePacketHeader = CreateHeader(DataTypes.authentificate, userdescription.GetSize())
        If Not Await SendHeader(authHeader, CreateDefaultToken(DataTypes.authentificate)) Then
            Return False
        End If

        If Not Await SendData(userdescription.Serialize(), userdescription.GetSize()) Then
            Return False
        End If

        '' Activate self destroy in case receive does not complete in time
        ActivateSelfDestroy(authentificateRecvTimeout)

        '' The received packet header can only use the token previously created
        '' No need to recheck the received datatype
        Dim receivedHeader As New StructurePacketHeader
        If Not (Await RecvHeader(receivedHeader, CancellationToken.None)).Successful Then
            Return False
        End If

        Dim buffer(userdescription.GetSize() - 1) As Byte
        If Not Await RecvData(buffer, buffer.Length) Then
            Return False
        End If
        userdescription.Deserialize(buffer, 0)

        '' Cancel the timeout because the operation completed succesfully
        DeactivateSelfDestroy()

        '' Check the validity of the user descriptor
        If Not userdescription.IsValid() Then
            CloseClient()
            Return False
        End If

        '' Set as authentificated
        Authenticated = True

        Return True
    End Function


    ''' <summary>
    ''' Starts this component and its thread for listening to messages
    ''' This function does NOT throw any exception
    ''' </summary>
    Public Async Function ProcessComponent(cancellationToken As CancellationToken) As Task
        If IsClosed Then
            Return
        End If

        '' Prepare the self destruct code
        cancellationToken.Register(Sub()
                                       ActivateSelfDestroy(hardCancellationTimeout)
                                   End Sub)

        '' Return control to caller in order for it to release any locks
        Await Task.Yield

        Await RaiseOnComponentInit(cancellationToken)

        If Not IsClosed Then
            Await InternalProcessComponent(cancellationToken)
        End If


        Await RaiseOnComponentUnInit()

        If IsClosed Then
            RaiseOnComponentDisconnect()
        End If

        '' Event called after OnComponentUnInit and after OnDisconnect events
        RaiseOnComponentFinished()

        '' The component ended succesfully - either disconnected or in a stable state
        '' Deactivate here in order to also cover the callbacks as they may block in a recv command
        DeactivateSelfDestroy()
    End Function
    Private Async Function InternalProcessComponent(cancellationToken As CancellationToken) As Task
        Dim rPacket As New StructurePacketHeader

        Try
            While True
                Dim recvHeaderQuery As QueryResponse(Of TokenSecurity)

                Try
                    recvHeaderQuery = Await RecvHeader(rPacket, cancellationToken)
                Catch ex As OperationCanceledException
                    '' The operation was cancelled - sign that this component can still be used
                    '' The client is not closed
                    '' Release the locks as we assume the component is in a pristine state - no to-read data or to-receive
                    ResetSocketLocks()
                    Return
                Catch ex As Exception
                    CloseClient()
                    report.Low(ComponentType.ToString(), "InternalProcessComponent - RecvHeader", ex.Message, True)
                    Return
                End Try

                If Not recvHeaderQuery.Successful Then
                    Return
                End If

                '' Filter core packets here
                Select Case rPacket.dataType
                    Case DataTypes.ping_acknowledged
                        Continue While

                    Case DataTypes.tcp_error_debugging
                        Dim err_param As String = rPacket.GetPacketParamValue("err")
                        report.Low(ComponentType.ToString(), "Tcp Debug", err_param, True)

                        '' On error no token can remain valid
                        tokenManagement.DisableAllTokens()
                        Continue While

                    Case DataTypes.com_error_debugging
                        Dim err_param As String = rPacket.GetPacketParamValue("err")
                        report.Low(ComponentType.ToString(), "Component Debug", err_param, True)

                        '' On error no token can remain valid
                        tokenManagement.DisableAllTokens()
                        Continue While

                    Case DataTypes.com_notification
                        report.Low(ComponentType.ToString(), "Component notification", rPacket.GetSafeArguments(), False)
                        Continue While

                    Case DataTypes.flags_cancelled_operation
                        '' On cancellation no token can remain valid
                        tokenManagement.DisableAllTokens()
                End Select

                Try
                    '' This could throw an exception 
                    Await OnReceiveCallback(Me, rPacket, recvHeaderQuery.Data)

                    '' Check if RecvHeader() is locked down after the message was processed
                    If lockHeaderReceiving.CurrentCount = 0 Then
                        Throw New Exception("Receiving lock was still taken after packet processing finished. This could indicate a forced socket shutdown triggered by cancellation or unprocessed packet")
                    End If

                Catch ex As OperationCanceledException
                    '' The operation was cancelled - sign that this component can still be used
                    '' The client is not closed

                    '' No reading or sending locks should be hold open at this stage as we assume the
                    ''  component is in a pristine state
                    If lockHeaderReceiving.CurrentCount = 0 Or lockHeaderSending.CurrentCount = 0 Then
                        CloseClient()   '' Perhaps OnReceiveCallback produced an exception without closing the client
                        report.Low(ComponentType.ToString(), "InternalProcessComponent - OnReceiveCallback", "Component was cancelled but did not release socket locks", True)
                    End If
                    Return

                Catch ex As Exception
                    CloseClient()   '' Perhaps OnReceiveCallback produced an exception without closing the client
                    report.Low(ComponentType.ToString(), "InternalProcessComponent - OnReceiveCallback", ex.Message, True)
                    Return
                End Try
            End While

        Catch ex As Exception
            CloseClient()

            '' Capture exceptions from other unshielded function in the loop like GetPacketParamValue
            report.Low(ComponentType.ToString(), "InternalProcessComponent - Default", ex.Message, True)
        End Try
    End Function


    Private Sub ActivateSelfDestroy(timeout As Integer)
        delayedCancellationSource = New CancellationTokenSource(timeout)
        delayedCancellationSource.Token.Register(Sub()
                                                     CloseClient()
                                                 End Sub)
    End Sub
    Private Sub DeactivateSelfDestroy()
        If delayedCancellationSource IsNot Nothing Then
            delayedCancellationSource.Dispose()
        End If
    End Sub
    ''' <summary>
    ''' Resets the locks used for sending and receiving packet headers
    ''' </summary>
    Private Sub ResetSocketLocks()
        If lockHeaderReceiving.CurrentCount = 0 Then
            lockHeaderReceiving.Release()
        End If
        If lockHeaderSending.CurrentCount = 0 Then
            lockHeaderSending.Release()
        End If
    End Sub


    Private Async Function RaiseOnComponentInit(cancellationToken As CancellationToken) As Task
        Try
            Await OnComponentInit(Me, cancellationToken)
        Catch ex As Exception
            CloseClient()
            report.Low(ComponentType.ToString(), "OnComponentInit", ex.Message, False)
        End Try
    End Function
    Private Async Function RaiseOnComponentUnInit() As Task
        Try
            Await OnComponentUnInit(Me)
        Catch ex As Exception
            CloseClient()
            report.Low(ComponentType.ToString(), "OnComponentUnInit", ex.Message, False)
        End Try
    End Function
    Private Sub RaiseOnComponentDisconnect()
        Try
            OnComponentDisconnect(Me)
        Catch ex As Exception
            '' This should not throw exception so it's an error
            report.Low(ComponentType.ToString(), "OnComponentDisconnect", ex.Message, True)
        End Try
    End Sub
    Private Sub RaiseOnComponentFinished()
        Try
            OnComponentFinished(Me)
        Catch ex As Exception
            '' This should not throw exception so it's an error
            report.Low(ComponentType.ToString(), "OnComponentFinished", ex.Message, True)
        End Try
    End Sub



    Public Function GetTokenManager() As TokenManagement
        Return tokenManagement
    End Function

    ''' <summary>
    ''' Enables keepalive on this socket and configures the timeouts
    ''' </summary>
    ''' <param name="activate"></param>
    Public Sub SetKeepAlive(activate As Boolean)
        '' This call shouldn't be needed to activate keepalive
        netclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, activate)

        If activate Then
            '' Set the timeouts. By default its 2 hours
            Dim keepaliveTimeouts As New StructureTcpKeepAlive
            keepaliveTimeouts.onoff = 1
            keepaliveTimeouts.keepaliveinterval = 5000
            keepaliveTimeouts.keepalivetime = 5000

            If Not netclient.Client.IOControl(IOControlCode.KeepAliveValues, keepaliveTimeouts.Serialize(), New Byte() {}) = 0 Then
                Throw New Exception("Keepalive settings failed")
            End If

        End If
    End Sub

    ''' <summary>
    ''' Sets the SOCKET's receive timeout
    ''' Does NOT affect networkstream operations like recvHeader,recvData
    ''' </summary>
    Public Function SetReceiveTimeout(tmt As Integer) As Integer
        Dim bkTimeout As Integer = netclient.Client.ReceiveTimeout

        netclient.Client.ReceiveTimeout = tmt

        Return bkTimeout
    End Function

    ''' <summary>
    ''' Sets the SOCKET's send timeout
    ''' Does not affect networkstream operations like sendHeader,sendData
    ''' </summary>
    Public Function SetSendTimeout(tmt As Integer) As Integer
        Dim bkTimeout As Integer = netclient.Client.SendTimeout

        netclient.Client.SendTimeout = tmt

        Return bkTimeout
    End Function

End Class
