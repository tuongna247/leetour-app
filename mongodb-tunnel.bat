@echo off
REM MongoDB SSH Tunnel for Local Development
echo ================================================
echo MongoDB SSH Tunnel
echo ================================================
echo.
echo MongoDB on server will be available at:
echo   localhost:27017
echo.
echo Keep this window open while developing.
echo Press Ctrl+C to close the tunnel.
echo.
echo ================================================
echo.

ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N -o ServerAliveInterval=60 -o ServerAliveCountMax=3
