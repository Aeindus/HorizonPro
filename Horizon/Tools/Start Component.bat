@echo off

mode con: cols=80 lines=10

:start

start /WAIT VMSynchronize.bat

cls

echo Select which component to start:
echo.

echo  0) All
echo  1) Synchronize
echo  2) Ftp
echo  3) Shell
echo  4) Video
echo  5) Mouse
echo  6) Ftp Download + Ftp
echo  7) Ftp Upload + Ftp
echo  8) Regedit

set option=0
set /p option=Choose:

if %option%==0 (
	start rundll32.exe FtpComponent.dll,debug
	echo Started ftp
	start rundll32.exe ShellComponent.dll,debug
	echo Started shell
	start rundll32.exe VideoComponent.dll,debug
	echo Started video
	start rundll32.exe MouseComponent.dll,debug
	echo Started mouse
	start rundll32.exe FtpDownloadComponent.dll,debug
	echo Started Ftp Download
	start rundll32.exe FtpUploadComponent.dll,debug
	echo Started Ftp Upload
	start rundll32.exe RegComponent.dll,debug
	echo Started Regedit
)
if %option%==1 (
	goto start
)
if %option%==2 (
	start rundll32.exe FtpComponent.dll,debug
	echo Started ftp
)
if %option%==3 (
	start rundll32.exe ShellComponent.dll,debug
	echo Started shell
)
if %option%==4 (
	start rundll32.exe VideoComponent.dll,debug
	echo Started video
)
if %option%==5 (
	start rundll32.exe MouseComponent.dll,debug
	echo Started mouse
)
if %option%==6 (
	start rundll32.exe FtpComponent.dll,debug
	echo Started ftp
	start rundll32.exe FtpDownloadComponent.dll,debug
	echo Started ftp download
)
if %option%==7 (
	start rundll32.exe FtpComponent.dll,debug
	echo Started ftp
	start rundll32.exe FtpUploadComponent.dll,debug
	echo Started ftp upload
)
if %option%==8 (
	start rundll32.exe RegComponent.dll,debug
	echo Started Regedit
)

echo Press any key to stop all components and to update them
pause>nul

taskkill /f /im rundll32.exe > nul
goto start 