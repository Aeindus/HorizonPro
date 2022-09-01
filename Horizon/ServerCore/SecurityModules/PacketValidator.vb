Public Class PacketValidator
    Private tokenManagement As TokenManagement

    Sub New(tokenManagement As TokenManagement)
        Me.tokenManagement = tokenManagement
    End Sub

    ''' <summary>
    ''' Checks if the packet structure conforms to the standard and if the token is valid then returns it. 
    ''' </summary>
    ''' <param name="packet"></param>
    ''' <returns></returns>
    Public Function ValidatePacket(packet As StructurePacketHeader) As QueryResponse(Of TokenSecurity)
        Dim basicValidation As ValidationResponse = IsBasicValid(packet)
        Dim tokenHolder As StructHolder(Of TokenSecurity)

        If Not basicValidation.Successful Then
            Return New QueryResponse(Of TokenSecurity) With {.Successful = False, .Information = basicValidation.Information}
        End If

        If IsAllowedUnauthorized(packet) Then
            '' Generate unauthorized token
            tokenHolder = GetUnrestrictedTokenDummy(packet)

            If Not tokenHolder.HasInformation Then
                Return New QueryResponse(Of TokenSecurity) With
                    {.Successful = False, .Information = "Unauthorized token could not be generated"}
            End If
        Else
            tokenHolder = tokenManagement.GetTokenFromDatabase(packet)

            If Not tokenHolder.HasInformation Then
                Return New QueryResponse(Of TokenSecurity) With
                    {.Successful = False, .Information = "Token not found in database"}
            End If

            '' Validate the found token against the database
            Dim tokenValidation As ValidationResponse = tokenManagement.ValidateToken(tokenHolder.Data, packet)
            If Not tokenValidation.Successful Then
                Return New QueryResponse(Of TokenSecurity) With
                    {.Successful = False, .Information = tokenValidation.Information}
            End If
        End If

        Return New QueryResponse(Of TokenSecurity) With {.Successful = True, .Data = tokenHolder.Data}
    End Function


    ''' <summary>
    ''' Checks if the basic packet information is correct. 
    ''' Does not validate against the token engine.
    ''' </summary>
    ''' <returns></returns>
    Private Function IsBasicValid(packet As StructurePacketHeader) As ValidationResponse
        If Not packet.checksum = packet.ComputeChecksum() Then
            Return New ValidationResponse With {.Successful = False, .Information = "Incorrect checksum"}
        End If
        If Not packet.magicStart = StructurePacketHeader.MAGIC_START Then
            Return New ValidationResponse With {.Successful = False, .Information = "Incorrect MAGIC_START"}
        End If

        If IsAllowedUnauthorized(packet) Then
            If packet.tokenId.Any(Function(v) v <> 0) Then
                Return New ValidationResponse With {.Successful = False, .Information = "Error packet must have a null token id"}
            End If
        End If

        Return New ValidationResponse With {.Successful = True}
    End Function

    Private Function IsAllowedUnauthorized(packet As StructurePacketHeader) As Boolean
        '' Error packets do not have registered tokens but must be allowed
        Return (packet.dataType = DataTypes.com_error_debugging Or packet.dataType = DataTypes.tcp_error_debugging Or packet.dataType = DataTypes.com_notification)
    End Function


    ''' <summary>
    ''' Returns a dummy but unrestricted token for the error messages.
    ''' The tokens returned were never and will never be stored in the tokens database
    ''' </summary>
    ''' <returns></returns>
    Private Function GetUnrestrictedTokenDummy(packet As StructurePacketHeader) As StructHolder(Of TokenSecurity)
        Dim tokenSecurity As New TokenSecurity

        With tokenSecurity
            .id = TokenManagement.GenerateTokenId()
            .creationTime = DateTime.UtcNow

            .enforceAllowedUses = False
            .enforceExpectedDatatype = True
            .enforceExpiration = False

            .allowedUses = 0
            .expectedDatatype = packet.dataType
            .expirationTime = DateTime.MaxValue
        End With

        Return New StructHolder(Of TokenSecurity) With {.HasInformation = True, .Data = tokenSecurity}
    End Function
End Class
