Imports System.IO
Imports System.Runtime.InteropServices

Public Class Form1

    <DllImport("gdi32.dll")>
    Private Shared Function CreateDIBSection(ByVal hdc As Int32,
    ByRef pbmi As BITMAPINFO, ByVal iUsage As System.UInt32,
    ByRef ppvBits As Int32, ByVal hSection As Int32,
    ByVal dwOffset As System.UInt32) As Int32
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function StretchDIBits(ByVal hdc As IntPtr, ByVal XDest As Integer, ByVal YDest As Integer, ByVal nDestWidth As Integer, ByVal nDestHeight As Integer, ByVal XSrc As Integer, ByVal YSrc As Integer, ByVal nSrcWidth As Integer, ByVal nSrcHeight As Integer, ByVal lpBits As Byte(),
<[In]> ByRef lpBitsInfo As BITMAPINFO, ByVal iUsage As UInteger, ByVal dwRop As UInteger) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function CreateCompatibleBitmap(hdc As IntPtr, nWidth As Integer, nHeight As Integer) As IntPtr
    End Function
    <DllImport("gdi32.dll", SetLastError:=True)>
    Private Shared Function CreateCompatibleDC(ByVal hRefDC As IntPtr) As IntPtr
    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function BitBlt(ByVal hdc As IntPtr, ByVal nXDest As Integer, ByVal nYDest As Integer, ByVal nWidth As Integer, ByVal nHeight As Integer, ByVal hdcSrc As IntPtr, ByVal nXSrc As Integer, ByVal nYSrc As Integer, ByVal dwRop As UInteger) As Boolean
    End Function
    <DllImport("Gdi32.dll")>
    Public Shared Function SelectObject(ByVal hdc As IntPtr, ByVal hObject As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetDC(ByVal hwnd As IntPtr) As IntPtr
    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function StretchBlt(hdcDest As IntPtr, nXOriginDest As Integer, nYOriginDest As Integer, nWidthDest As Integer, nHeightDest As Integer, hdcSrc As IntPtr, nXOriginSrc As Integer, nYOriginSrc As Integer, nWidthSrc As Integer, nHeightSrc As Integer, dwRop As UInteger) As Boolean
    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function SetStretchBltMode(hdc As IntPtr, iStretchMode As Integer) As Boolean
    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function DeleteObject(hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function
    <DllImport("gdi32.dll")>
    Private Shared Function DeleteDC(hdc As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
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


    ''Not used
    Sub method1()
        Dim bmp As Bitmap = bmpFromArray("img.bmp")
        Dim bmp1 As Bitmap = bmpFromArray("img1.bmp")

        Dim picBmp As Bitmap = New Bitmap(bmp.Width, 2000)

        CopyBitmap(bmp, picBmp, 0, 0, bmp.Width, 100, 0, 0)
        CopyBitmap(bmp1, picBmp, 0, 100, bmp.Width, 100, 0, 0)



        PictureBox1.Image = picBmp
    End Sub
    Function bmpFromArray(name As String) As Bitmap
        Dim bData As Byte()
        Dim br As BinaryReader = New BinaryReader(File.OpenRead(name))
        bData = br.ReadBytes(br.BaseStream.Length)

        Dim ms As MemoryStream = New MemoryStream(bData, 0, bData.Length)
        Dim bmp As Bitmap = Image.FromStream(ms)

        Return bmp
    End Function
    Public Sub CopyBitmap(ByRef uSource As Bitmap, ByRef uTarget As Bitmap, ByVal uDestX As Integer, ByVal uDestY As Integer, ByVal uSrcWidth As Integer, ByVal uSrcHeight As Integer, ByVal uSrcX As Integer, ByVal uSrcY As Integer)

        Dim nSrc As New Rectangle
        nSrc = Rectangle.FromLTRB(uSrcX, uSrcY, uSrcX + uSrcWidth, uSrcY + uSrcHeight)

        Dim nDst As New Rectangle
        nDst = Rectangle.FromLTRB(uDestX, uDestY, uDestX + uSrcWidth, uDestY + uSrcHeight)

        Using g As Graphics = Graphics.FromImage(uTarget)
            ' Draw the specified section of the source bitmap to the new one
            g.DrawImage(uSource, nDst, nSrc, GraphicsUnit.Pixel)
        End Using

    End Sub




    Class FrameData
        Public pic As PictureBox
        Public internalFrameHDC As IntPtr, internalFrameBMP As IntPtr
        Public picGraphics As Graphics
        Public picHDC As IntPtr
        Public maxWidth As Integer, maxHeight As Integer
        Public oldSelectObject As IntPtr

        Public Sub destroy()
            picGraphics.ReleaseHdc(picHDC)
            picGraphics.Dispose()

            SelectObject(internalFrameHDC, oldSelectObject)

            DeleteDC(internalFrameHDC)
            DeleteObject(internalFrameBMP)

        End Sub
    End Class

    Function initInternalFrame(pic As PictureBox, maxWidth As Integer, maxHeight As Integer) As FrameData
        Dim g As Graphics = pic.CreateGraphics()
        Dim picHDC As IntPtr = g.GetHdc()
        Dim res As New FrameData

        res.pic = pic
        res.picHDC=picHDC
        res.internalFrameHDC = CreateCompatibleDC(picHDC)
        res.internalFrameBMP = CreateCompatibleBitmap(picHDC, maxWidth, maxHeight)
        res.oldSelectObject = SelectObject(res.internalFrameHDC, res.internalFrameBMP)
        res.picGraphics = g
        res.maxWidth = maxWidth
        res.maxHeight = maxHeight

        SetStretchBltMode(picHDC, HALFTONE)

        Return res
    End Function

    Sub updateInternalFrame(data As FrameData, ByRef arr As Byte(), ByRef bmi As BITMAPINFO, frameShift As Integer)
        StretchDIBits(data.internalFrameHDC, 0, frameShift, bmi.bmiheader.biWidth, bmi.bmiheader.biHeight, 0, 0, bmi.bmiheader.biWidth, bmi.bmiheader.biHeight, arr, bmi, 0, SRCCOPY)
    End Sub


    ''Use this function to dynamically resize the array and write the output DIRECTLY on the picturebox skipping the internal buffer
    ''If you use this function you must not use 'updatePictureHDC'
    Sub updateDirectFrame(data As FrameData, ByRef arr As Byte(), ByRef bmi As BITMAPINFO, frameShift As Integer)
        Dim relativeShift = frameShift / data.maxHeight * data.pic.Height
        Dim relativeHeight = bmi.bmiheader.biHeight / data.maxHeight * data.pic.Height

        StretchDIBits(data.picHDC, 0, relativeShift, data.pic.Width, relativeHeight, 0, 0, bmi.bmiheader.biWidth, bmi.bmiheader.biHeight, arr, bmi, 0, SRCCOPY)
    End Sub

    Sub updatePictureHDC(data As FrameData)
        StretchBlt(data.picHDC, 0, 0, data.pic.Width, data.pic.Height, data.internalFrameHDC, 0, 0, data.maxWidth, data.maxHeight, SRCCOPY)
    End Sub



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'method1()

        ' Data accumulation. Doesn't matter hdd or tcp
        Dim bData As Byte()
        Dim br As BinaryReader = New BinaryReader(File.OpenRead("img1_arr.bmp"))
        bData = br.ReadBytes(br.BaseStream.Length)

        Dim bData2 As Byte()
        Dim br2 As BinaryReader = New BinaryReader(File.OpenRead("img2_arr.bmp"))
        bData2 = br2.ReadBytes(br2.BaseStream.Length)




        Dim bmi As New BITMAPINFO
        bmi.bmiheader = New BITMAPINFOHEADER

        'Now we fill up the bmi (Bitmap information variable) with all the necessary data
        bmi.bmiheader.biSize = 40 'Size, in bytes, of the header (always 40)
        bmi.bmiheader.biPlanes = 1 'Number of planes (always one)
        bmi.bmiheader.biBitCount = 24 'Bits per pixel (always 24 for image processing)
        bmi.bmiheader.biCompression = 0 'Compression: none or RLE (always zero)
        bmi.bmiheader.biWidth = 1920
        bmi.bmiheader.biHeight = 100
        bmi.bmiheader.biSizeImage = 576004



        Dim data As FrameData = initInternalFrame(PictureBox1, 1920, 1080)
        updateDirectFrame(data, bData, bmi, 0)
        updateDirectFrame(data, bData2, bmi, 100)
        updateDirectFrame(data, bData2, bmi, 200)
        updateDirectFrame(data, bData2, bmi, 300)
        updateDirectFrame(data, bData2, bmi, 400)
        updateDirectFrame(data, bData2, bmi, 500)
        updateDirectFrame(data, bData2, bmi, 700)


        'updateInternalFrame(data, bData, bmi, 0)
        'updateInternalFrame(data, bData2, bmi, 100)
        'updateInternalFrame(data, bData2, bmi, 200)
        'updateInternalFrame(data, bData2, bmi, 300)
        'updateInternalFrame(data, bData2, bmi, 400)
        'updateInternalFrame(data, bData2, bmi, 500)
        'updateInternalFrame(data, bData2, bmi, 700)
        'updatePictureHDC(data)


        data.destroy()

        Exit Sub

        'Dim g As Graphics = PictureBox1.CreateGraphics() 'or Me.CreateGraphics()
        'Dim dsthdc As IntPtr = g.GetHdc()

        'Dim bmi As New BITMAPINFO
        'bmi.bmiheader = New BITMAPINFOHEADER

        ''Now we fill up the bmi (Bitmap information variable) with all the necessary data
        'bmi.bmiheader.biSize = 40 'Size, in bytes, of the header (always 40)
        'bmi.bmiheader.biPlanes = 1 'Number of planes (always one)
        'bmi.bmiheader.biBitCount = 24 'Bits per pixel (always 24 for image processing)
        'bmi.bmiheader.biCompression = 0 'Compression: none or RLE (always zero)
        'bmi.bmiheader.biWidth = 1920
        'bmi.bmiheader.biHeight = 100
        'bmi.bmiheader.biSizeImage = 576004
        'Dim memHDC As IntPtr = CreateCompatibleDC(dsthdc)
        'Dim memBmp As IntPtr = CreateCompatibleBitmap(dsthdc, 1920, 1080)
        'SelectObject(memHDC, memBmp)


        'StretchDIBits(memHDC, 0, 0, 1920, 100, 0, 0, 1920, 100, bData, bmi, 0, SRCCOPY)
        'StretchDIBits(memHDC, 0, 100, 1920, 100, 0, 0, 1920, 100, bData2, bmi, 0, SRCCOPY)

        'SetStretchBltMode(dsthdc, HALFTONE)
        'StretchBlt(dsthdc, 0, 0, PictureBox1.Width, 200, memHDC, 0, 0, 1920, 200, SRCCOPY)

    End Sub
End Class
