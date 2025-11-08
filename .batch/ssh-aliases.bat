@echo off
REM SSH Aliases that ignore permission warnings

:menu
echo ================================================
echo LeeTour SSH Quick Connect
echo ================================================
echo.
echo 1. Connect to Server
echo 2. MongoDB Tunnel
echo 3. Deploy Frontend
echo 4. Deploy API
echo 5. Check Server Status
echo 6. Exit
echo.
set /p choice="Select option (1-6): "

if "%choice%"=="1" goto connect
if "%choice%"=="2" goto mongodb
if "%choice%"=="3" goto deploy-frontend
if "%choice%"=="4" goto deploy-api
if "%choice%"=="5" goto status
if "%choice%"=="6" goto end
goto menu

:connect
echo Connecting to LeeTour server...
ssh -o StrictModes=no leetour
goto menu

:mongodb
echo Starting MongoDB tunnel...
echo Keep this window open!
ssh -o StrictModes=no -L 27017:localhost:27017 deployer@157.173.124.250 -N
goto menu

:deploy-frontend
echo Deploying frontend...
ssh -o StrictModes=no leetour "cd /var/www/leetour && git pull && cd apps/frontend && npm run build && pm2 restart leetour-frontend"
pause
goto menu

:deploy-api
echo Deploying API...
ssh -o StrictModes=no leetour "cd /var/www/leetour && git pull && cd apps/api && npm run build && pm2 restart leetour-api"
pause
goto menu

:status
echo Checking server status...
ssh -o StrictModes=no leetour "pm2 status"
pause
goto menu

:end
exit
