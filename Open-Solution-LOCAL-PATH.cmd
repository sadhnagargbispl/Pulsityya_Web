@echo off
REM ===========================================================================
REM  Puslstiya_web - open the solution from the LOCAL drive path.
REM
REM  Do NOT open this solution from \\localhost\Sadhna\... .NET treats a UNC
REM  path as a remote zone and refuses to probe the bin folder, which gives
REM  "Could not load file or assembly 'Antlr3.Runtime'" even though the DLL
REM  is right there. \\localhost\Sadhna and D:\Sadhna are the same folder.
REM
REM  Always start Visual Studio with this file, or via
REM  File > Open > Project/Solution and browse to the D:\ path by hand.
REM  Never use the Recent Projects entry - it still points at the UNC path.
REM ===========================================================================

set "SLN=D:\Sadhna\New Github\Puslstiya_web\Puslstiya_web.sln"

if not exist "%SLN%" (
    echo Solution not found: %SLN%
    pause
    exit /b 1
)

tasklist /fi "imagename eq devenv.exe" | find /i "devenv.exe" >nul
if not errorlevel 1 (
    echo.
    echo   Visual Studio is already running.
    echo   Close it completely ^(File ^> Exit^) and run this file again,
    echo   otherwise the old UNC path stays in effect.
    echo.
    pause
    exit /b 1
)

echo Opening: %SLN%
start "" "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\devenv.exe" "%SLN%"
