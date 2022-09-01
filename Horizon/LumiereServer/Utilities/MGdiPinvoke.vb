Imports System.Runtime.InteropServices
Imports ServerCore.Definitions

Public Module MGdiPinvoke

    <DllImport("Gdi32.dll")>
    Private Function SelectObject(ByVal hdc As IntPtr, ByVal hObject As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("gdi32.dll")>
    Private Function DeleteDC(hdc As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("gdi32.dll")>
    Private Function CreateCompatibleBitmap(hdc As IntPtr, nWidth As Integer, nHeight As Integer) As IntPtr
    End Function
    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Function CreateCompatibleDC(ByVal hRefDC As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Function SetStretchBltMode(hdc As IntPtr, iStretchMode As Integer) As Boolean
    End Function

    <DllImport("gdi32.dll")>
    Private Function StretchDIBits(ByVal hdc As IntPtr, ByVal XDest As Integer, ByVal YDest As Integer, ByVal nDestWidth As Integer, ByVal nDestHeight As Integer, ByVal XSrc As Integer, ByVal YSrc As Integer, ByVal nSrcWidth As Integer, ByVal nSrcHeight As Integer, ByVal lpBits As Byte(),
<[In]> ByRef lpBitsInfo As BITMAPINFO, ByVal iUsage As UInteger, ByVal dwRop As UInteger) As Integer
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Structure RGBQUAD
        Public rgbBlue As Byte
        Public rgbGreen As Byte
        Public rgbRed As Byte
        Public rgbReserved As Byte
    End Structure

    <StructLayout(LayoutKind.Sequential)>
    Class BITMAPINFOHEADER
        Public biSize As Int32
        Public biWidth As Int32
        Public biHeight As Int32
        Public biPlanes As Int16
        Public biBitCount As Int16
        Public biCompression As Int32
        Public biSizeImage As Int32
        Public biXPelsPerMeter As Int32
        Public biYPelsPerMeter As Int32
        Public biClrUsed As Int32
        Public biClrImportant As Int32
    End Class

    <StructLayout(LayoutKind.Sequential)>
    Structure BITMAPINFO
        Dim bmiheader As BITMAPINFOHEADER
        Dim bmiColors() As RGBQUAD
    End Structure
    Const DIB_RGB_COLORS As Integer = 0
    Const SRCCOPY As Integer = 13369376
    Dim HALFTONE As Integer = 4


    Public Class FrameData
        Implements IDisposable

        Public canvas As Control
        Public picGraphics As Graphics
        Public picHDC As IntPtr
        Public clientScreenWidth As Integer = 0
        Public clientScreenHeight As Integer = 0

        Public cachedFrameBytesMaxSize As UInt64
        Public cachedRowHeight As UInt32
        Public cachedSectionsPerFrame As UInt32
        Public cachedBmi As BITMAPINFO
        Public cachedFrameBuffer As Byte()

        Private disposedValue As Boolean
        Private initialized As Boolean = False
        Private Const minimumFrameHeight As Integer = 100

        Public Sub New(pic As Control)
            Me.canvas = pic

            picGraphics = pic.CreateGraphics()
            picHDC = picGraphics.GetHdc()

            SetStretchBltMode(picHDC, HALFTONE)
        End Sub

        Public Function IsInitialized() As Boolean
            Return initialized
        End Function

        ''' <summary>
        ''' Initializes all the dimension variables needed.
        ''' Also performs checks on the given dimensions
        ''' </summary>
        Public Function InitDimensions(width As UInt32, height As UInt32) As ValidationResponse
            If width > 1920 Then
                Return New ValidationResponse With {.Successful = False, .Information = "Width parameter is larger than 1920px"}
            End If
            If height > 1080 Then
                Return New ValidationResponse With {.Successful = False, .Information = "Height parameter is larger than 1080px"}
            End If

            '' Resize the buffer to fit the entire screen
            Dim entireFrameBytesCount = CalculateStride(width, 24) * height
            ReDim cachedFrameBuffer(0 To entireFrameBytesCount - 1)

            clientScreenWidth = width
            clientScreenHeight = height

            initialized = True

            Return New ValidationResponse With {.Successful = True}
        End Function


        ''' <summary>
        ''' Checks given params and compares them to cached values
        ''' </summary>
        ''' <returns></returns>
        Public Function CheckConsistency(yOffset As UInt32, frameBytesSize As UInt64) As ValidationResponse
            '' Check that the stride size is a multiple of DWORD. Otherwise BSOD gets you
            '' This stride calculation was already done with cachedFrameBytesSize 
            If frameBytesSize <> cachedFrameBytesMaxSize Then
                Return New ValidationResponse With
                    {.Successful = False, .Information = "Received frame buffer is different than it should be, it's different than stride calculation"}
            End If

            If yOffset > clientScreenHeight - cachedRowHeight Then
                Return New ValidationResponse With {
                    .Successful = False,
                    .Information = "Frame Y axis shift is too large: overflow would occur"}
            End If

            Return New ValidationResponse With {.Successful = True}
        End Function

        ''' <summary>
        ''' Sets the necessary information for the first frame to be received.
        ''' Also performs checks on the given parameters.
        ''' </summary>
        Public Function SetVideoData(detailLevel As UInt32) As ValidationResponse
            Dim rowHeight As UInt32 = GetHeightFromDetailLevel(detailLevel, clientScreenHeight, minimumFrameHeight)

            '' Check if row height has the right dimensions
            If rowHeight < 100 Or rowHeight > clientScreenHeight Then
                Return New ValidationResponse With {.Successful = False, .Information = "Row height not within bounds"}
            End If

            '' Calculate the proper buffer for the video
            cachedFrameBytesMaxSize = CalculateStride(clientScreenWidth, 24) * rowHeight

            cachedBmi = New BITMAPINFO With {
               .bmiheader = New BITMAPINFOHEADER With {
               .biSize = 40,   'Size, in bytes, of the header (always 40)
               .biPlanes = 1,  'Number of planes (always one)
               .biBitCount = 24,    'Bits per pixel (always 24 for image processing)
               .biCompression = 0,  'Compression: none or RLE (always zero)
               .biWidth = clientScreenWidth,
               .biHeight = rowHeight,
               .biSizeImage = cachedFrameBytesMaxSize
               }
            }

            cachedRowHeight = rowHeight
            cachedSectionsPerFrame = clientScreenHeight / rowHeight
            clientScreenWidth = clientScreenWidth
            clientScreenHeight = clientScreenHeight

            Return New ValidationResponse With {.Successful = True}
        End Function


        ''' <summary>
        ''' Use this Function To write the output DIRECTLY On the picturebox skipping the internal buffer
        ''' </summary>
        Public Sub updateDirectFrame(yOffset As Integer)
            Dim relativeShift As Integer = Math.Floor(yOffset / clientScreenHeight * canvas.Height)
            Dim relativeHeight As Integer = Math.Floor(cachedBmi.bmiheader.biHeight / clientScreenHeight * canvas.Height)

            StretchDIBits(picHDC, 0, relativeShift, canvas.Width, relativeHeight, 0, 0, cachedBmi.bmiheader.biWidth, cachedBmi.bmiheader.biHeight, cachedFrameBuffer, cachedBmi, 0, SRCCOPY)
        End Sub


        Public Function GetPossibleFrameHeights()
            Return CalculateMultipliers(clientScreenHeight, minimumFrameHeight)
        End Function


        ''' <summary>
        ''' Calculate the size of a line if pixels aligned to DWORD. 
        ''' Returns value in bytes.
        ''' </summary>
        Private Function CalculateStride(width As UInt32, bitsPerPixel As UInt32) As UInt32
            Return ((width * bitsPerPixel + 31) And &HFFFFFFE0UL) / 8
        End Function

        Private Function GetHeightFromDetailLevel(detailLevel As Integer, height As UInt32, minimum As Integer) As UInt32
            Dim multipliers As List(Of Integer) = CalculateMultipliers(height, minimum)

            If detailLevel > multipliers.Count Then
                Return multipliers.Last()
            End If

            Return multipliers(detailLevel)
        End Function

        Private Function CalculateMultipliers(nr As UInt32, minimum As Integer) As List(Of Integer)
            Dim maxSteps As Integer = 10

            Dim result As New List(Of Integer)

            For divider As Integer = 1 To Math.Min(maxSteps, nr)
                If nr Mod divider = 0 And nr / divider > minimum Then
                    result.Add(nr / divider)
                End If
            Next

            result.Reverse()
            Return result
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects)

                    '' MSDN says setting to Nothing can speed up garbage collection
                    cachedFrameBuffer = Nothing

                    picGraphics.ReleaseHdc(picHDC)
                    picGraphics.Dispose()

                    canvas.Dispose()
                End If

                '' Here the existence of objects is NOT guaranteed
                ' TODO: free unmanaged resources (unmanaged objects) and override finalizer
                ' TODO: set large fields to null

                disposedValue = True
            End If
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
            Dispose(disposing:=True)
            GC.SuppressFinalize(Me)
        End Sub

        '' TODO: override finalizer only if 'Dispose(disposing As Boolean)' has code to free unmanaged resources
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code. Put cleanup code in 'Dispose(disposing As Boolean)' method
        '    Dispose(disposing:=False)
        '    MyBase.Finalize()
        'End Sub

    End Class
End Module
