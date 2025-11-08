# Create SSH Config File with Correct Permissions
# This script creates a new SSH config file with proper permissions

Write-Host "SSH Config File Creator" -ForegroundColor Green
Write-Host "=======================" -ForegroundColor Green
Write-Host ""

$sshDir = "$env:USERPROFILE\.ssh"
$configPath = "$sshDir\config"
$backupPath = "$sshDir\config.backup"

# Create .ssh directory if it doesn't exist
if (-not (Test-Path $sshDir)) {
    Write-Host "Creating .ssh directory..." -ForegroundColor Cyan
    New-Item -ItemType Directory -Path $sshDir -Force | Out-Null
}

# Backup existing config if it exists
if (Test-Path $configPath) {
    Write-Host "Backing up existing config to config.backup..." -ForegroundColor Yellow
    Copy-Item $configPath $backupPath -Force
    Remove-Item $configPath -Force
}

# Create SSH config content for your server
$configContent = @"
# LeeTour Server Configuration
Host leetour
    HostName 157.173.124.250
    User deployer
    Port 22
    IdentityFile ~/.ssh/id_rsa
    ServerAliveInterval 60
    ServerAliveCountMax 3

# Short alias for the server
Host leetour-server
    HostName 157.173.124.250
    User deployer

# MongoDB SSH Tunnel Configuration
Host leetour-mongodb
    HostName 157.173.124.250
    User deployer
    LocalForward 27017 localhost:27017
    ServerAliveInterval 60
    ServerAliveCountMax 3
"@

# Create the config file
Write-Host "Creating new SSH config file..." -ForegroundColor Cyan
$configContent | Out-File -FilePath $configPath -Encoding ASCII -Force

# Set correct permissions
Write-Host "Setting correct permissions..." -ForegroundColor Cyan

# Remove inheritance
icacls $configPath /inheritance:r | Out-Null

# Grant current user full control
icacls $configPath /grant:r "$env:USERNAME:(F)" | Out-Null

Write-Host ""
Write-Host "✅ SSH config created successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Config file location: $configPath" -ForegroundColor Yellow
Write-Host ""
Write-Host "You can now use these SSH shortcuts:" -ForegroundColor Cyan
Write-Host "  ssh leetour              - Connect to server" -ForegroundColor White
Write-Host "  ssh leetour-mongodb      - Connect with MongoDB tunnel" -ForegroundColor White
Write-Host ""
Write-Host "Current permissions:" -ForegroundColor Yellow
icacls $configPath
