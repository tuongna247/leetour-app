# LeeTour Batch Scripts

This folder contains helper scripts for common development and deployment tasks.

## Available Scripts

### Windows Batch Files (.bat)

1. **connect-server.bat**
   - Connect to LeeTour server via SSH
   - Usage: Double-click or run `.\connect-server.bat`

2. **mongodb-tunnel.bat**
   - Create SSH tunnel to MongoDB on server
   - Makes remote MongoDB available at `localhost:27017`
   - Keep window open while developing
   - Usage: Double-click or run `.\mongodb-tunnel.bat`

3. **deploy-frontend-simple.bat**
   - Simple frontend deployment script
   - Usage: Double-click or run `.\deploy-frontend-simple.bat`

4. **ssh-aliases.bat**
   - Interactive menu for SSH tasks
   - Options: Connect, MongoDB Tunnel, Deploy, Status check
   - Usage: Double-click or run `.\ssh-aliases.bat`

### PowerShell Scripts (.ps1)

1. **create-ssh-config.ps1**
   - Creates SSH config with correct permissions
   - Usage: `powershell -ExecutionPolicy Bypass -File .\create-ssh-config.ps1`

2. **fix-ssh-permissions.ps1**
   - Fixes SSH config file permissions on Windows
   - Usage: `powershell -ExecutionPolicy Bypass -File .\fix-ssh-permissions.ps1`

## How to Run from Root Directory

You can run these scripts from the project root:

```bash
# Windows Command Prompt
.\.batch\connect-server.bat
.\.batch\mongodb-tunnel.bat
.\.batch\ssh-aliases.bat

# PowerShell
.\.batch\create-ssh-config.ps1
.\.batch\fix-ssh-permissions.ps1
```

## Notes

- All scripts work from any directory
- No path updates needed after moving to .batch folder
- Scripts use absolute server addresses and SSH configs
- MongoDB tunnel requires SSH access to server
