@echo off
echo ========================================
echo Building Sidequest Municipal App
echo ========================================
echo.

echo Closing any running instances...
taskkill /F /IM "Sidequest municiple app.exe" 2>nul
if %errorlevel% equ 0 (
    echo Application closed successfully
) else (
    echo No running instance found
)
echo.

call "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\Tools\VsDevCmd.bat"

echo.
echo Restoring NuGet packages...
msbuild "Sidequest municiple app.sln" /t:Restore /p:RestorePackagesConfig=true

echo.
echo Building solution...
msbuild "Sidequest municiple app.sln" /p:Configuration=Debug /v:minimal

echo.
echo ========================================
echo Build Complete
echo ========================================
echo.

