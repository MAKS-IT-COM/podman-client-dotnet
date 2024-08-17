@echo off

REM Change directory to the location of the script
cd /d %~dp0

REM Invoke the PowerShell script (Release-NuGetPackage.ps1) in the same directory
powershell -ExecutionPolicy Bypass -File "%~dp0Release-NuGetPackage.ps1"
