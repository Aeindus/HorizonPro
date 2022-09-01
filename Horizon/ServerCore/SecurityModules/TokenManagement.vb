Imports System.Threading

Public Structure TokenSecurity
    Dim id As Byte()

    Dim enforceExpiration As Boolean
    Dim enforceExpectedDatatype As Boolean   ' Enables checks based on the received header's datatype
    Dim enforceAllowedUses As Boolean

    Dim creationDatatype As DataTypes
    Dim creationTime As DateTime

    Dim expirationTime As DateTime
    Dim expectedDatatype As DataTypes
    Dim allowedUses As Integer

    '' Persistent data stored between requests
    Dim metadata As Object

    Public Function EnforceUses(allowedUses As Integer) As TokenSecurity
        enforceAllowedUses = True
        Me.allowedUses = allowedUses
        Return Me
    End Function
    Public Function EnforceTime(expirationDelayMs As Integer) As TokenSecurity
        enforceExpiration = True
        expirationTime = DateTime.UtcNow.AddMilliseconds(expirationDelayMs)
        Return Me
    End Function
End Structure


Public Class TokenManagement
    Private Class ArrayComparer
        Inherits EqualityComparer(Of Byte())

        Public Overrides Function Equals(x() As Byte, y() As Byte) As Boolean
            If x.Length <> y.Length Then
                Return False
            End If
            For i As Integer = 0 To x.Length - 1
                If x(i) <> y(i) Then
                    Return False
                End If
            Next
            Return True
        End Function

        Public Overrides Function GetHashCode(obj() As Byte) As Integer
            Dim result As Integer = 0
            For i As Integer = 0 To obj.Length - 1
                ' Do some cutting of the int64 numbers
                ' The last number should be 0x7FFFFFFF because if the last bit is set the number is considered
                ' positive but with a larger than signed int32 vpossible value. So the conversion would fail
                result = CInt(((CLng(result) * 23 + obj(i)) Xor &HA0E0L) And &H7FFFFFFFL)
            Next
            Return result
        End Function
    End Class

    Public Const defaultTokenExpirationTime As Integer = 20000
    Private Const garbageExpirationDelay As Integer = 120000    ''Preserve tokens in memory for longer so that an expiration message is given instead of not found
    Private Shared randomGenerator As New Random
    Private Shared cleanTimer As New Timer(AddressOf TickHandler, Nothing, 0, 2000)
    Private Shared listTokenInstances As New List(Of TokenManagement)
    Private registeredTokens As New Dictionary(Of Byte(), TokenSecurity)(New ArrayComparer())

    Public Sub New()
        SyncLock listTokenInstances
            listTokenInstances.Add(Me)
        End SyncLock
    End Sub


    Private Shared Sub TickHandler(state As Object)
        SyncLock listTokenInstances
            For Each instance In listTokenInstances
                SyncLock instance.registeredTokens
                    With instance
                        For i As Integer = .registeredTokens.Count - 1 To 0 Step -1
                            Dim tokenEntry = .registeredTokens.ElementAt(i)

                            '' I want to remove tokens as late as possible so that a call to IsValid
                            '' will return token expired instead of not found for better debugging
                            If tokenEntry.Value.enforceExpiration AndAlso Date.UtcNow.Subtract(tokenEntry.Value.expirationTime).TotalMilliseconds > garbageExpirationDelay Then
                                .registeredTokens.Remove(tokenEntry.Key)
                            End If
                        Next
                    End With
                End SyncLock
            Next
        End SyncLock
    End Sub

    ''' <summary>
    ''' Removes the token from the database.
    ''' </summary>
    Public Sub RevokeToken(token As TokenSecurity)
        SyncLock registeredTokens
            If token.id Is Nothing Then
                Return
            End If

            registeredTokens.Remove(token.id)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Disables all created tokens for security reasons
    ''' </summary>
    Public Sub DisableAllTokens()
        SyncLock registeredTokens
            registeredTokens.Clear()
        End SyncLock
    End Sub

    ''' <summary>
    ''' Registers the token and associates it with the packet.
    ''' If succesfull, the packet is assigned the token's id
    ''' </summary>
    Public Sub RegisterPacket(ByRef header As StructurePacketHeader, ByRef token As TokenSecurity)
        header.tokenId = token.id

        '' Register the packet datatype to the token
        RegisterPacket(header.dataType, token)
    End Sub

    ''' <summary>
    ''' Registers the token and assigns it the associated origin-packet datatype.
    ''' </summary>
    Public Sub RegisterPacket(packetOriginDatatype As DataTypes, ByRef token As TokenSecurity)
        SyncLock registeredTokens
            '' Do not replace the token if it aleady exists
            '' Othewise the timeout could be cicumvented
            If registeredTokens.ContainsKey(token.id) Then
                Throw New Exception("Token id was reused and token is still valid and registered")
            End If

            '' An already bound token should not be used for two different packets
            '' Curently no specific reason for this but just to preserve logic
            If token.creationDatatype <> 0 And token.creationDatatype <> packetOriginDatatype Then
                Throw New Exception("Already bound token is being registered to another datatype")
            End If

            token.creationDatatype = packetOriginDatatype

            '' Safe method of adding keys to the dictionary without exception on dublicates
            '' This also overrides the value
            registeredTokens(token.id) = token
        End SyncLock
    End Sub

    ''' <summary>
    ''' Checks if the header's token is registered on the system and if security conditions apply or fail.
    ''' Overall ensures the token is enforced. If one-time use is set, the token is automatically removed even 
    ''' if the packet does not respect all the imposed conditions.
    ''' This method is forced to take as another param the tokenSecurity because in this way we assure 
    ''' whoever calls this method will have made a copy of it before passing to this function. This is so because this method
    ''' will remove the token from the database and will render it inaccesible after this call.
    ''' </summary>
    Public Function ValidateToken(tokenSecurity As TokenSecurity, packet As StructurePacketHeader) As ValidationResponse
        SyncLock registeredTokens
            If tokenSecurity.enforceExpiration And Date.UtcNow > tokenSecurity.expirationTime Then
                registeredTokens.Remove(packet.tokenId)
                Return New ValidationResponse With {.Successful = False, .Information = "Token expired"}
            End If

            If tokenSecurity.enforceAllowedUses Then
                If tokenSecurity.allowedUses = 0 Then
                    registeredTokens.Remove(packet.tokenId)
                    Return New ValidationResponse With {.Successful = False, .Information = "Token uses expired"}
                Else
                    tokenSecurity.allowedUses -= 1

                    '' Update with the new value
                    registeredTokens.Item(packet.tokenId) = tokenSecurity
                End If
            End If

            If tokenSecurity.enforceExpectedDatatype And Not tokenSecurity.expectedDatatype = packet.dataType Then
                Return New ValidationResponse With {.Successful = False, .Information = $"Expected type {tokenSecurity.expectedDatatype.ToString()} doesn't match {CType(packet.dataType, DataTypes).ToString()}"}
            End If

            Return New ValidationResponse With {.Successful = True}
        End SyncLock
    End Function

    ''' <summary>
    ''' Removes this instance of the token management system from static memory
    ''' </summary>
    Public Sub ReleaseInstance()
        SyncLock listTokenInstances
            listTokenInstances.Remove(Me)
        End SyncLock
    End Sub

    ''' <summary>
    ''' Returns the token associated with a packet.
    ''' Note: The token can as well be expired or invalidated but still remaining in the database
    ''' </summary>
    ''' <param name="packet"></param>
    ''' <returns></returns>
    Public Function GetTokenFromDatabase(packet As StructurePacketHeader) As StructHolder(Of TokenSecurity)
        If Not registeredTokens.ContainsKey(packet.tokenId) Then
            Return New StructHolder(Of TokenSecurity) With {.HasInformation = False}
        End If

        Dim token As TokenSecurity = registeredTokens.Item(packet.tokenId)
        Return New StructHolder(Of TokenSecurity) With {.HasInformation = True, .Data = token}
    End Function

    Public Shared Function GenerateTokenId() As Byte()
        Dim count As Integer = StructurePacketHeader.GetFieldSize(StructurePacketHeader.FieldNames.tokenId)
        Dim result(count - 1) As Byte

        randomGenerator.NextBytes(result)
        Return result
    End Function
End Class


