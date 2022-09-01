Public Module NetworkTools

    ''' <summary>
    ''' Creates a new packet header which does NOT contains a random token
    ''' </summary>
    Public Function CreateHeader(dataType As DataTypes, packetSize As UInt64) As StructurePacketHeader
        Return CreateHeader(dataType, packetSize, "")
    End Function
    ''' <summary>
    ''' Creates a new packet header which does NOT contains a random token
    ''' </summary>
    Public Function CreateHeader(dataType As DataTypes, packetSize As UInt64, arguments As String) As StructurePacketHeader
        Dim sPacket As New StructurePacketHeader
        sPacket.magicStart = StructurePacketHeader.MAGIC_START
        sPacket.dataType = dataType
        sPacket.packetSize = packetSize
        sPacket.arguments = arguments
        sPacket.checksum = sPacket.ComputeChecksum()

        Return sPacket
    End Function


    ''' <summary>
    ''' Creates an unregistered and unbounded security token.
    ''' This token must be bounded and registered to a packet header in a call to SendHeader().
    ''' All security checks are enforced for this token.
    ''' </summary>
    Public Function CreateDefaultToken(expectedDatatype As DataTypes, Optional allowedUses As Integer = 1, Optional expirationDelayMs As Integer = TokenManagement.defaultTokenExpirationTime) As TokenSecurity
        Dim tokenSecurity As New TokenSecurity

        With tokenSecurity
            .id = TokenManagement.GenerateTokenId()
            .creationTime = DateTime.UtcNow

            .enforceAllowedUses = True
            .enforceExpectedDatatype = True
            .enforceExpiration = True

            .allowedUses = allowedUses
            .expectedDatatype = expectedDatatype
            .expirationTime = DateTime.UtcNow.AddMilliseconds(expirationDelayMs)
        End With

        Return tokenSecurity
    End Function

    ''' <summary>
    ''' Creates an unregistered and unbounded security token.
    ''' This token must be bounded and registered to a packet header in a call to SendHeader().
    ''' Caution: Only specified security checks are enforced. All other are disabled.
    ''' </summary>
    Public Function CreateLooseToken(expectedDatatype As DataTypes) As TokenSecurity
        Dim tokenSecurity As New TokenSecurity

        With tokenSecurity
            .id = TokenManagement.GenerateTokenId()
            .creationTime = DateTime.UtcNow

            .enforceAllowedUses = False
            .enforceExpectedDatatype = True
            .enforceExpiration = False

            .allowedUses = 0
            .expectedDatatype = expectedDatatype
            .expirationTime = DateTime.UtcNow
        End With

        Return tokenSecurity
    End Function

End Module
