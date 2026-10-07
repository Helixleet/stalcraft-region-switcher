@echo off
setlocal

echo ========================================
echo  STALZONE Region Switcher - Build
echo ========================================
echo.

set "CSC="
if exist "%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" (
    set "CSC=%SystemRoot%\Microsoft.NET\Framework64\v4.0.30319\csc.exe"
    goto :found
)
if exist "%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe" (
    set "CSC=%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\csc.exe"
    goto :found
)

echo [ERROR] csc.exe not found
echo Install .NET Framework 4.x
pause
exit /b 1

:found
echo Compiler: %CSC%
echo.

if not exist "bin" mkdir bin

"%CSC%" /nologo /target:winexe /platform:anycpu /optimize+ /out:bin\STALZONERegionSwitcher.exe /win32icon:icon.ico /reference:System.dll /reference:System.Core.dll /reference:System.Drawing.dll /reference:System.Windows.Forms.dll /reference:Microsoft.CSharp.dll Program.cs MainForm.cs MainForm.Designer.cs SteamHelper.cs Properties\AssemblyInfo.cs

if errorlevel 1 (
    echo.
    echo [ERROR] Build failed
    pause
    exit /b 1
)

echo.
echo ========================================
echo  Done!
echo  bin\STALZONERegionSwitcher.exe
echo ========================================
echo.
pause
