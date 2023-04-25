@echo off

set local=C:\Users\%USERNAME%\Downloads

set dotnet=%local\dotnet
set app=%local\CCET4610_Project-objective_1a\visual_studio\GUILayoutTest1\GUILayoutTest1\bin\Debug\net7.0-windows\

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
echo Starting Application
@echo on




%dotnet%\dotnet.exe %app%\GUILayoutTest1.dll
pause
