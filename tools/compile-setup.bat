@echo off

echo This script is obsolete. It was originally used for compiling and packing System Manager Setup, but the Setup has become uncompressible when the .NET Framework installer was added.
echo You should not use this script. You can compile the setup yourself using Visual Studio and/or MSBuild.
echo.

choice /M:"Do you want to continue?"

IF %errorlevel% neq 1 EXIT /b

echo.
echo This script will compile the System Manager Setup application.
echo Before running this script, make sure that UPX is installed and is included in PATH.
echo.
REM echo You can install UPX at https://upx.github.io

pause

echo.
set /p SCRIPT_SYSTEM_MANAGER_COMPILE_PLATFORM=Please enter the platform to compile for: 

REM Push the current directory to the stack and navigate to the Setup
pushd ..\src\Setup

msbuild Setup.vcxproj /p:Configuration=Release /p:Platform=%SCRIPT_SYSTEM_MANAGER_COMPILE_PLATFORM% /t:Rebuild

echo.

cd %SCRIPT_SYSTEM_MANAGER_COMPILE_PLATFORM%\Release
upx SystemManagerSetup.exe -4 -v --keep-resource=IDR_NETFRMWRK_452_SETUP --keep-resource=115
REM Pop the directory back
popd
cd ..

echo The setup application has been compiled and packed.
echo The output is available at %CD%\src\Setup\%SCRIPT_SYSTEM_MANAGER_COMPILE_PLATFORM%\Release\SystemManagerSetup.exe

cd tools

echo.
pause