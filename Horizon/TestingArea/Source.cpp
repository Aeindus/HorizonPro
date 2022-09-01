#pragma comment(lib,"gdi32.lib")

#include <Windows.h>
#include <iostream>
#include <wingdi.h>
#include <fstream>
#include <assert.h>



PBITMAPINFO CreateBitmapInfoStruct(HBITMAP hBmp)
{
	BITMAP bmp;
	PBITMAPINFO pbmi;
	WORD    cClrBits;

	// Retrieve the bitmap color format, width, and height.  
	assert(GetObject(hBmp, sizeof(BITMAP), (LPSTR)&bmp));
	
	// Convert the color format to a count of bits.  
	cClrBits = (WORD)(bmp.bmPlanes * bmp.bmBitsPixel);
	if (cClrBits == 1)
		cClrBits = 1;
	else if (cClrBits <= 4)
		cClrBits = 4;
	else if (cClrBits <= 8)
		cClrBits = 8;
	else if (cClrBits <= 16)
		cClrBits = 16;
	else if (cClrBits <= 24)
		cClrBits = 24;
	else cClrBits = 32;

	// Allocate memory for the BITMAPINFO structure. (This structure  
	// contains a BITMAPINFOHEADER structure and an array of RGBQUAD  
	// data structures.)  

	if (cClrBits < 24)
		pbmi = (PBITMAPINFO)LocalAlloc(LPTR,
			sizeof(BITMAPINFOHEADER) +
			sizeof(RGBQUAD) * (1 << cClrBits));

	// There is no RGBQUAD array for these formats: 24-bit-per-pixel or 32-bit-per-pixel 

	else
		pbmi = (PBITMAPINFO)LocalAlloc(LPTR,
			sizeof(BITMAPINFOHEADER));

	// Initialize the fields in the BITMAPINFO structure.  

	pbmi->bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	pbmi->bmiHeader.biWidth = bmp.bmWidth;
	pbmi->bmiHeader.biHeight = bmp.bmHeight;
	pbmi->bmiHeader.biPlanes = bmp.bmPlanes;
	pbmi->bmiHeader.biBitCount = bmp.bmBitsPixel;
	if (cClrBits < 24)
		pbmi->bmiHeader.biClrUsed = (1 << cClrBits);

	// If the bitmap is not compressed, set the BI_RGB flag.  
	pbmi->bmiHeader.biCompression = BI_RGB;

	// Compute the number of bytes in the array of color  
	// indices and store the result in biSizeImage.  
	// The width must be DWORD aligned unless the bitmap is RLE 
	// compressed. 
	pbmi->bmiHeader.biSizeImage = ((pbmi->bmiHeader.biWidth * cClrBits + 31) & ~31) / 8
		* pbmi->bmiHeader.biHeight;
	// Set biClrImportant to 0, indicating that all of the  
	// device colors are important.  
	pbmi->bmiHeader.biClrImportant = 0;
	return pbmi;
}

void CreateBMPFileModified(LPCSTR pszFile,BITMAPINFO bmpinf,BYTE *arr)
{
	HANDLE hf;                 // file handle  
	BITMAPFILEHEADER hdr;       // bitmap file-header  
	PBITMAPINFOHEADER pbih;     // bitmap info-header  
	LPBYTE lpBits;              // memory pointer  
	DWORD dwTotal;              // total count of bytes  
	DWORD cb;                   // incremental count of bytes  
	BYTE *hp;                   // byte pointer  
	DWORD dwTmp;
	PBITMAPINFO pbi;
	HDC hDC;

	pbih = (PBITMAPINFOHEADER)&bmpinf;

	// Create the .BMP file.  
	hf = CreateFileA(pszFile,
		GENERIC_READ | GENERIC_WRITE,
		(DWORD)0,
		NULL,
		CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL,
		(HANDLE)NULL);
	assert(hf != INVALID_HANDLE_VALUE);


	assert(WriteFile(hf, (LPSTR)arr, (DWORD)bmpinf.bmiHeader.biSizeImage, (LPDWORD)&dwTmp, NULL));

	// Close the .BMP file.  
	assert(CloseHandle(hf));

	// Free memory.  
	//GlobalFree((HGLOBAL)lpBits);
}

void CreateBMPFile(LPCSTR pszFile, BITMAPINFO bmpinf, BYTE *arr)
{
	HANDLE hf;                 // file handle  
	BITMAPFILEHEADER hdr;       // bitmap file-header  
	PBITMAPINFOHEADER pbih;     // bitmap info-header  
	LPBYTE lpBits;              // memory pointer  
	DWORD dwTotal;              // total count of bytes  
	DWORD cb;                   // incremental count of bytes  
	BYTE *hp;                   // byte pointer  
	DWORD dwTmp;
	PBITMAPINFO pbi;
	HDC hDC;

	pbih = (PBITMAPINFOHEADER)&bmpinf;
	lpBits = arr;//(LPBYTE)GlobalAlloc(GMEM_FIXED, pbih->biSizeImage);


	// Retrieve the color table (RGBQUAD array) and the bits  
	// (array of palette indices) from the DIB.  
	//GetDIBits(hDC, hBMP, 0, (WORD)pbih->biHeight, lpBits, pbi,DIB_RGB_COLORS);

	// Create the .BMP file.  
	hf = CreateFileA(pszFile,
		GENERIC_READ | GENERIC_WRITE,
		(DWORD)0,
		NULL,
		CREATE_ALWAYS,
		FILE_ATTRIBUTE_NORMAL,
		(HANDLE)NULL);
	assert(hf != INVALID_HANDLE_VALUE);

	hdr.bfType = 0x4d42;        // 0x42 = "B" 0x4d = "M"  
	// Compute the size of the entire file.  
	hdr.bfSize = (DWORD)(sizeof(BITMAPFILEHEADER) +pbih->biSize + pbih->biClrUsed* sizeof(RGBQUAD) + pbih->biSizeImage);
	hdr.bfReserved1 = 0;
	hdr.bfReserved2 = 0;

	// Compute the offset to the array of color indices.  
	hdr.bfOffBits = (DWORD) sizeof(BITMAPFILEHEADER) +pbih->biSize + pbih->biClrUsed* sizeof(RGBQUAD);

	// Copy the BITMAPFILEHEADER into the .BMP file.  
	assert(WriteFile(hf, (LPVOID)&hdr, sizeof(BITMAPFILEHEADER),(LPDWORD)&dwTmp, NULL));

	// Copy the BITMAPINFOHEADER and RGBQUAD array into the file.  
	assert(WriteFile(hf, (LPVOID)pbih, sizeof(BITMAPINFOHEADER)+ pbih->biClrUsed * sizeof(RGBQUAD),(LPDWORD)&dwTmp, (NULL)));

	// Copy the array of color indices into the .BMP file.  
	dwTotal = pbih->biSizeImage;
	hp = lpBits;
	assert(WriteFile(hf, (LPSTR)hp, (DWORD)pbih->biSizeImage, (LPDWORD)&dwTmp, NULL));

	// Close the .BMP file.  
	assert(CloseHandle(hf));

	// Free memory.  
	//GlobalFree((HGLOBAL)lpBits);
}

// Get the horizontal and vertical screen sizes in pixel
void GetDesktopResolution(int& horizontal, int& vertical)
{
	RECT desktop;

	// Get a handle to the desktop window
	const HWND hDesktop = GetDesktopWindow();

	// Get the size of screen to the variable desktop
	GetWindowRect(hDesktop, &desktop);

	// The top left corner will have coordinates (0,0)
	// and the bottom right corner will have coordinates
	// (horizontal, vertical)
	horizontal = desktop.right;
	vertical = desktop.bottom;
}

int multipleOfDWORD(int nr) {
	int ad = nr + 4;
	return (ad - (ad%4));
}
int main() {
	HDC screen = GetDC(NULL);
	HDC memHDC = CreateCompatibleDC(NULL);
	HBITMAP bmp;
	int screenW, screenH;

	GetDesktopResolution(screenW, screenH);
	BITMAPINFO bmpInfo;

	int imgHeight = 100;
	int heightOffset = 0;
	const char *file = "img1_arr.bmp";


	memset(&bmpInfo, 0, sizeof(bmpInfo));
	bmpInfo.bmiHeader.biSize = sizeof(BITMAPINFOHEADER);
	bmpInfo.bmiHeader.biWidth = screenW;
	bmpInfo.bmiHeader.biHeight = imgHeight;
	bmpInfo.bmiHeader.biPlanes = 1;
	bmpInfo.bmiHeader.biBitCount = 24;
	bmpInfo.bmiHeader.biCompression = BI_RGB;
	bmpInfo.bmiHeader.biSizeImage = multipleOfDWORD(abs(bmpInfo.bmiHeader.biWidth * bmpInfo.bmiHeader.biBitCount/8 * bmpInfo.bmiHeader.biHeight));// ((bmpInfo.bmiHeader.biWidth * bmpInfo.bmiHeader.biBitCount + 31) & ~31) / 8 * bmpInfo.bmiHeader.biHeight;
	
	void *imgBytes;
	bmp = CreateDIBSection(memHDC, &bmpInfo, DIB_RGB_COLORS, &imgBytes, NULL, NULL);
	HGDIOBJ oldSelect=SelectObject(memHDC, bmp);
	Sleep(2000);
	BitBlt(memHDC, 0, 0, screenW, imgHeight, screen, 0, heightOffset, SRCCOPY);
	GdiFlush();

	
	//CreateBMPFile(file, bmpInfo,(BYTE*)imgBytes);
	CreateBMPFileModified(file, bmpInfo, (BYTE*)imgBytes);

	//Restore original bitmap
	SelectObject(memHDC, oldSelect);
	DeleteDC(memHDC);
	DeleteObject(bmp);
	ReleaseDC(NULL,screen);

	return 0;

	ofstream fo("img.bmp",ios::binary);
	
	DWORD picSize = bmpInfo.bmiHeader.biSizeImage;
	char *src = (char*)imgBytes;

	while (picSize) {
		int sz = min(10000, picSize);

		fo.write((char*)src, sz);

		picSize -= sz;
		src += sz;
	}
	fo.flush();
	fo.close();
	return 0;
}