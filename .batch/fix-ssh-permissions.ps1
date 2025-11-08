# Fix SSH Config File Permissions
# This script removes incorrect permissions and sets proper ownership

Write-Host "Fixing SSH config permissions..." -ForegroundColor Yellow
Write-Host ""

$sshConfigPath = "$env:USERPROFILE\.ssh\config"

if (-not (Test-Path $sshConfigPath)) {
    Write-Host "Error: SSH config file not found at $sshConfigPath" -ForegroundColor Red
    exit 1
}

Write-Host "SSH config file found: $sshConfigPath" -ForegroundColor Green

try {
    # Remove inheritance and all existing permissions
    Write-Host "Removing inheritance..." -ForegroundColor Cyan
    icacls $sshConfigPath /inheritance:r | Out-Null

    # Grant current user full control
    Write-Host "Granting full control to current user..." -ForegroundColor Cyan
    icacls $sshConfigPath /grant:r "$env:USERNAME:(F)" | Out-Null

    # Remove common groups that shouldn't have access
    $groupsToRemove = @(
        "NT AUTHORITY\Authenticated Users",
        "BUILTIN\Users",
        "BUILTIN\Administrators",
        "Everyone"
    )

    foreach ($group in $groupsToRemove) {
        icacls $sshConfigPath /remove $group 2>$null | Out-Null
    }

    Write-Host ""
    Write-Host "✅ Permissions fixed successfully!" -ForegroundColor Green
    Write-Host ""
    Write-Host "Current permissions:" -ForegroundColor Yellow
    icacls $sshConfigPath

    Write-Host ""
    Write-Host "You can now use SSH config without permission errors." -ForegroundColor Green

} catch {
    Write-Host ""
    Write-Host "❌ Error fixing permissions: $_" -ForegroundColor Red
    Write-Host ""
    Write-Host "Please run this script as Administrator:" -ForegroundColor Yellow
    Write-Host "Right-click PowerShell -> Run as Administrator" -ForegroundColor Yellow
    exit 1
}
