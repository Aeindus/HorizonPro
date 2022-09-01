@echo off

echo.
echo                 SYNCHRONIZE ALL SHARED FILES TO HERE
echo.
echo.
echo.


if NOT %username%==Test (
	echo WRONG MACHINE!
	timeout 10
	exit 1
)

xcopy /S /Y \\tsclient\G\HorizonTests .

timeout 2
exit 0