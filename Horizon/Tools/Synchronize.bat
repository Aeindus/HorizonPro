@echo off

echo.
echo                 SYNCHRONIZE SPECIFIC FILES
echo.
echo.
echo.
set src_compiled_folder=..\Debug\
set src_tool_folder=.
set dst_folder=G:\HorizonTests

set compiled_file_list=shared_compiled_files.txt
set tool_file_list=shared_tool_files.txt

echo ..... Copying compiled files..........
if not exist %compiled_file_list% (
	echo File list for synchronization not found!
	pause
)

for /f "delims=" %%f in (%compiled_file_list%) do (
    xcopy /Y "%src_compiled_folder%\%%f" "%dst_folder%\"
)
echo.
echo.

echo ..... Copying tool files..........
if not exist %tool_file_list% (
	echo File list for synchronization not found!
	pause
)

for /f "delims=" %%f in (%tool_file_list%) do (
    xcopy /Y "%src_tool_folder%\%%f" "%dst_folder%\"
)
echo.
timeout 10