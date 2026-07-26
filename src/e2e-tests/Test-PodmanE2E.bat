@echo off
setlocal
cd /d "%~dp0"

echo.
echo Podman E2E — scenarios in src\e2e-tests\scenarios\ (requires PODMAN_TEST_URL^).
echo Optional: -Scenario System  or  -Scenario '*Image*'
echo.

where pwsh >nul 2>&1
if errorlevel 1 (
  echo PowerShell 7+ ^(pwsh^) is required but was not found in PATH.
  echo Install from https://github.com/PowerShell/PowerShell/releases ^(.NET 10 host^)
  exit /b 1
)

pwsh -NoProfile -ExecutionPolicy Bypass -File "%~dp0Test-PodmanE2E.ps1" %*
exit /b %ERRORLEVEL%
