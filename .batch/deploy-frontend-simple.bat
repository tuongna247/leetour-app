@echo off
REM Simple Frontend Deployment
echo ================================================
echo Deploying Frontend to Server
echo ================================================
echo.

ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/frontend && npm run build && pm2 restart leetour-frontend && pm2 status"

echo.
echo ================================================
echo Deployment Complete!
echo ================================================
pause
