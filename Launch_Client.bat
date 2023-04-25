@echo off

set local=%~dp0


set dotnet=%local%runtime\dotnet-7.0\dotnet.exe
set app=%local%Release\Client\GUILayoutTest1.dll

echo.
echo.
echo .NET root set as:
echo %dotnet%
echo.
echo.
echo Application root:
echo %app%
echo.
echo.

echo Testing for local dotnet
echo.
echo.

%dotnet% --info



echo Starting Application
echo.
echo.
@echo on

%dotnet% %app%

pause
