@echo off
REM ========================================
REM Server Connection Helper Script (Windows)
REM Quick SSH access to LeeTour server
REM ========================================

setlocal

set SERVER_IP=157.173.124.250
set SERVER_USER=deployer
set APP_DIR=/var/www/leetour

:menu
cls
echo ========================================
echo LeeTour Server Connection
echo ========================================
echo.
echo 1. Connect to server
echo 2. Connect and deploy
echo 3. View PM2 status
echo 4. View logs
echo 5. Restart applications
echo 6. Copy environment files to server
echo 7. Exit
echo.

set /p choice="Enter your choice [1-7]: "

if "%choice%"=="1" goto connect
if "%choice%"=="2" goto deploy
if "%choice%"=="3" goto status
if "%choice%"=="4" goto logs
if "%choice%"=="5" goto restart
if "%choice%"=="6" goto copy_env
if "%choice%"=="7" goto exit
goto invalid

:connect
echo.
echo Connecting to server...
ssh %SERVER_USER%@%SERVER_IP%
goto end

:deploy
echo.
echo Deploying application...
ssh %SERVER_USER%@%SERVER_IP% "cd %APP_DIR% && ./deploy.sh"
goto end

:status
echo.
echo Fetching PM2 status...
ssh %SERVER_USER%@%SERVER_IP% "pm2 status"
goto end

:logs
cls
echo.
echo Which logs do you want to view?
echo 1. All logs
echo 2. Admin logs
echo 3. API logs
echo 4. Frontend logs
echo.
set /p log_choice="Enter choice: "

if "%log_choice%"=="1" ssh %SERVER_USER%@%SERVER_IP% "pm2 logs --lines 50"
if "%log_choice%"=="2" ssh %SERVER_USER%@%SERVER_IP% "pm2 logs leetour-admin --lines 50"
if "%log_choice%"=="3" ssh %SERVER_USER%@%SERVER_IP% "pm2 logs leetour-api --lines 50"
if "%log_choice%"=="4" ssh %SERVER_USER%@%SERVER_IP% "pm2 logs leetour-frontend --lines 50"
goto end

:restart
cls
echo.
echo Which application to restart?
echo 1. All applications
echo 2. Admin only
echo 3. API only
echo 4. Frontend only
echo.
set /p restart_choice="Enter choice: "

if "%restart_choice%"=="1" ssh %SERVER_USER%@%SERVER_IP% "pm2 restart all"
if "%restart_choice%"=="2" ssh %SERVER_USER%@%SERVER_IP% "pm2 restart leetour-admin"
if "%restart_choice%"=="3" ssh %SERVER_USER%@%SERVER_IP% "pm2 restart leetour-api"
if "%restart_choice%"=="4" ssh %SERVER_USER%@%SERVER_IP% "pm2 restart leetour-frontend"
goto end

:copy_env
echo.
echo Copying environment files to server...

if exist "apps\admin\.env.local" (
    scp apps\admin\.env.local %SERVER_USER%@%SERVER_IP%:%APP_DIR%/apps/admin/
    echo Admin .env.local copied
) else (
    echo Warning: apps\admin\.env.local not found
)

if exist "apps\api\.env" (
    scp apps\api\.env %SERVER_USER%@%SERVER_IP%:%APP_DIR%/apps/api/
    echo API .env copied
) else (
    echo Warning: apps\api\.env not found
)

if exist "apps\frontend\.env" (
    scp apps\frontend\.env %SERVER_USER%@%SERVER_IP%:%APP_DIR%/apps/frontend/
    echo Frontend .env copied
) else (
    echo Warning: apps\frontend\.env not found
)

echo.
echo Environment files copied successfully
goto end

:invalid
echo.
echo Invalid option. Please choose 1-7
goto end

:exit
echo.
echo Goodbye!
exit /b

:end
echo.
pause
goto menu
