@echo off
REM ========================================
REM SSH Setup Script for LeeTour Server (Windows)
REM ========================================

setlocal enabledelayedexpansion

set SERVER_IP=157.173.124.250
set SERVER_USER=deployer
set SSH_KEY=%USERPROFILE%\.ssh\leetour_server

echo ========================================
echo LeeTour Server SSH Setup
echo ========================================
echo.

REM Check if SSH key exists
if not exist "%SSH_KEY%" (
    echo [ERROR] SSH key not found!
    echo.
    echo Run this command first in Git Bash or PowerShell:
    echo ssh-keygen -t rsa -b 4096 -f ~/.ssh/leetour_server -C "leetour-deployment"
    echo.
    pause
    exit /b 1
)

echo [OK] SSH key found: %SSH_KEY%
echo.

REM Display public key
echo Your SSH public key:
echo ─────────────────────────────────────────────────────────
type "%SSH_KEY%.pub"
echo.
echo ─────────────────────────────────────────────────────────
echo.

echo IMPORTANT: You need to add this public key to the server
echo.
echo Option 1: Automatic (requires password)
echo   Run: ssh-copy-id -i %SSH_KEY%.pub %SERVER_USER%@%SERVER_IP%
echo.
echo Option 2: Manual setup
echo   1. Copy the public key shown above (Ctrl+C)
echo   2. Connect to server: ssh %SERVER_USER%@%SERVER_IP%
echo   3. Run: mkdir -p ~/.ssh ^&^& nano ~/.ssh/authorized_keys
echo   4. Paste the public key (Right-click or Shift+Insert)
echo   5. Save and exit (Ctrl+X, Y, Enter)
echo   6. Run: chmod 600 ~/.ssh/authorized_keys
echo   7. Exit and test connection
echo.

set /p choice="Do you want to try automatic setup? (y/n): "

if /i "%choice%"=="y" (
    echo.
    echo Attempting to copy SSH key to server...
    echo You will be prompted for the server password.
    echo.

    REM Try ssh-copy-id if available (Git Bash)
    where ssh-copy-id >nul 2>&1
    if %errorlevel% equ 0 (
        ssh-copy-id -i "%SSH_KEY%.pub" %SERVER_USER%@%SERVER_IP%
    ) else (
        REM Manual copy using PowerShell
        powershell -Command "Get-Content '%SSH_KEY%.pub' | ssh %SERVER_USER%@%SERVER_IP% 'mkdir -p ~/.ssh; cat >> ~/.ssh/authorized_keys; chmod 600 ~/.ssh/authorized_keys'"
    )

    if %errorlevel% equ 0 (
        echo.
        echo [OK] SSH key copied successfully!
    ) else (
        echo.
        echo [ERROR] Failed to copy SSH key
        echo Please follow the manual setup instructions above
        pause
        exit /b 1
    )
) else (
    echo.
    echo Please follow the manual setup instructions above
    echo Run this script again after completing the setup
    pause
    exit /b 0
)

REM Test connection
echo.
echo Testing SSH connection...
ssh -i "%SSH_KEY%" -o ConnectTimeout=5 %SERVER_USER%@%SERVER_IP% "echo Connection successful!" 2>nul

if %errorlevel% equ 0 (
    echo [OK] SSH connection works!
) else (
    echo [ERROR] SSH connection failed
    echo Please check your setup and try again
    pause
    exit /b 1
)

REM Setup SSH config
echo.
echo Setting up SSH config for easy access...

set SSH_CONFIG=%USERPROFILE%\.ssh\config

REM Backup existing config
if exist "%SSH_CONFIG%" (
    copy "%SSH_CONFIG%" "%SSH_CONFIG%.backup.%date:~-4%%date:~3,2%%date:~0,2%" >nul
    echo Backed up existing SSH config
)

REM Check if leetour config exists and remove it
findstr /C:"Host leetour" "%SSH_CONFIG%" >nul 2>&1
if %errorlevel% equ 0 (
    echo LeeTour SSH config already exists, updating...
    REM Create temp file without leetour config
    powershell -Command "(Get-Content '%SSH_CONFIG%') | Where-Object { $_ -notmatch 'Host leetour' } | Set-Content '%SSH_CONFIG%.tmp'"
    move /y "%SSH_CONFIG%.tmp" "%SSH_CONFIG%" >nul
)

REM Add new config
(
echo.
echo # LeeTour Server Configuration
echo Host leetour
echo     HostName %SERVER_IP%
echo     User %SERVER_USER%
echo     IdentityFile ~/.ssh/leetour_server
echo     IdentitiesOnly yes
echo     ServerAliveInterval 60
echo     ServerAliveCountMax 3
echo.
) >> "%SSH_CONFIG%"

echo [OK] SSH config updated!

echo.
echo ========================================
echo SSH Setup Complete!
echo ========================================
echo.
echo You can now connect to the server using:
echo.
echo   ssh leetour
echo.
echo Or the full command:
echo.
echo   ssh -i %SSH_KEY% %SERVER_USER%@%SERVER_IP%
echo.

set /p connect="Do you want to connect to the server now? (y/n): "

if /i "%connect%"=="y" (
    echo.
    echo Connecting to server...
    ssh leetour
)

echo.
pause
