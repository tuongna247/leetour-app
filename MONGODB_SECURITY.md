# MongoDB Security Guide

## Security Comparison

### Method 1: Direct Connection
```
Your PC ──────> Internet ──────> MongoDB Port 27017
          (encrypted)        (EXPOSED)
```
**Risk Level: ⚠️ MODERATE**
- MongoDB port is visible to the internet
- Password is the only protection
- Vulnerable to brute force attacks

### Method 2: SSH Tunnel (RECOMMENDED)
```
Your PC ──────> SSH Tunnel ──────> Localhost:27017
          (encrypted)          (NOT EXPOSED)
```
**Risk Level: ✅ HIGH SECURITY**
- MongoDB completely hidden from internet
- Double encryption (SSH + MongoDB)
- Requires SSH key compromise to attack

---

## Secure Method 1 Implementation (If You Must)

If you need to use direct connection, here's how to make it more secure:

### Step 1: Restrict to Your IP Only

**Create this script: `setup-mongodb-secure.sh`**

```bash
#!/bin/bash

# Get your current IP
YOUR_IP=$(curl -s ifconfig.me)

echo "Your current IP: $YOUR_IP"
echo "This script will:"
echo "1. Enable MongoDB remote access"
echo "2. Create firewall rules to ONLY allow your IP"
echo "3. Enable MongoDB authentication"
echo ""
read -p "Continue? (y/n) " -n 1 -r
echo
if [[ ! $REPONSE =~ ^[Yy]$ ]]; then
    exit 1
fi

# Backup config
sudo cp /etc/mongod.conf /etc/mongod.conf.backup

# Configure MongoDB
sudo sed -i 's/bindIp: 127.0.0.1/bindIp: 0.0.0.0/' /etc/mongod.conf

# Enable authentication
if ! grep -q "security:" /etc/mongod.conf; then
    echo "security:" | sudo tee -a /etc/mongod.conf
    echo "  authorization: enabled" | sudo tee -a /etc/mongod.conf
fi

# Restart MongoDB
sudo systemctl restart mongod

# Configure firewall - ONLY allow your IP
sudo ufw delete allow 27017/tcp 2>/dev/null || true
sudo ufw allow from $YOUR_IP to any port 27017 proto tcp

echo ""
echo "✅ MongoDB is now accessible ONLY from your IP: $YOUR_IP"
echo ""
echo "⚠️  WARNING: If your IP changes, you'll need to update the firewall rule:"
echo "   sudo ufw allow from NEW_IP to any port 27017 proto tcp"
echo "   sudo ufw delete allow from $YOUR_IP to any port 27017 proto tcp"
```

### Step 2: Use Strong Passwords

**Password Requirements:**
- Minimum 16 characters
- Mix of uppercase, lowercase, numbers, symbols
- No dictionary words
- Unique (not used anywhere else)

**Good Example:**
```
Tr9$mK2!pL5#vN8@qW4%yR7&
```

**Bad Examples:**
```
password123          ❌ Too simple
LeetourDB2024       ❌ Too predictable
admin               ❌ Common word
```

### Step 3: Additional Security Layers

```bash
# Install fail2ban to block brute force attempts
sudo apt-get install fail2ban

# Create MongoDB jail
sudo nano /etc/fail2ban/jail.local

# Add this:
[mongodb]
enabled = true
port = 27017
filter = mongodb-auth
logpath = /var/log/mongodb/mongod.log
maxretry = 3
bantime = 3600
```

---

## Method 2: SSH Tunnel (RECOMMENDED)

### Why This is THE BEST Option

1. **No Internet Exposure**
   - MongoDB listens only on localhost
   - Impossible to connect directly from internet
   - Zero attack surface for MongoDB

2. **SSH Security**
   - Industry standard encryption
   - Public/private key authentication
   - Built-in brute force protection
   - Audited and trusted by millions

3. **No Configuration Needed**
   - MongoDB stays on default settings
   - No firewall rules to manage
   - No passwords to remember

### Setup SSH Tunnel (Secure Method)

#### Option A: One-Time Connection
```bash
# Windows Command Prompt or PowerShell
ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
```

#### Option B: Background Process (Windows)

**Create: `start-mongo-tunnel.bat`**
```batch
@echo off
echo Starting MongoDB SSH Tunnel...
echo MongoDB will be available at localhost:27017
echo.
echo Press Ctrl+C to stop
echo.

ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N -o ServerAliveInterval=60 -o ServerAliveCountMax=3
```

#### Option C: Auto-Reconnect PowerShell Script

**Create: `start-mongo-tunnel.ps1`**
```powershell
# MongoDB SSH Tunnel with Auto-Reconnect
$serverHost = "deployer@157.173.124.250"
$localPort = 27017
$remotePort = 27017

Write-Host "MongoDB SSH Tunnel Manager" -ForegroundColor Green
Write-Host "===========================" -ForegroundColor Green
Write-Host ""

while ($true) {
    try {
        Write-Host "Connecting to MongoDB via SSH tunnel..." -ForegroundColor Yellow
        Write-Host "Local: localhost:$localPort -> Remote: localhost:$remotePort" -ForegroundColor Cyan
        Write-Host ""
        Write-Host "Press Ctrl+C to stop" -ForegroundColor Gray
        Write-Host ""

        # Start SSH tunnel
        ssh -L "${localPort}:localhost:${remotePort}" $serverHost -N -o ServerAliveInterval=60 -o ServerAliveCountMax=3

    } catch {
        Write-Host "Connection lost! Reconnecting in 5 seconds..." -ForegroundColor Red
        Start-Sleep -Seconds 5
    }
}
```

**Run it:**
```powershell
powershell -ExecutionPolicy Bypass -File start-mongo-tunnel.ps1
```

---

## Security Checklist

### For Method 1 (Direct Connection):
- [ ] MongoDB authentication enabled
- [ ] Strong passwords (16+ characters)
- [ ] Firewall restricted to your IP only
- [ ] fail2ban installed and configured
- [ ] Regular password rotation (every 90 days)
- [ ] Monitor MongoDB logs for suspicious activity
- [ ] Use VPN for additional security
- [ ] SSL/TLS encryption enabled

### For Method 2 (SSH Tunnel):
- [ ] SSH key authentication enabled
- [ ] Password authentication disabled on SSH
- [ ] SSH keys stored securely
- [ ] Server firewall allows only SSH (port 22)
- [ ] fail2ban protecting SSH
- [ ] SSH port changed from default (optional)

---

## Current Security Status of Your Server

### Check Current MongoDB Configuration

```bash
# Check if MongoDB is exposed to internet
ssh deployer@157.173.124.250 "sudo netstat -tlnp | grep 27017"

# Expected SECURE output (localhost only):
tcp  0  0  127.0.0.1:27017  0.0.0.0:*  LISTEN  1234/mongod

# INSECURE output (exposed to internet):
tcp  0  0  0.0.0.0:27017    0.0.0.0:*  LISTEN  1234/mongod

# Check firewall status
ssh deployer@157.173.124.250 "sudo ufw status | grep 27017"

# Check MongoDB config
ssh deployer@157.173.124.250 "grep bindIp /etc/mongod.conf"
```

---

## My Strong Recommendation

### For Local Development: Use SSH Tunnel (Method 2)

**Pros:**
- ✅ Maximum security
- ✅ No configuration needed
- ✅ No password management
- ✅ Industry best practice
- ✅ Works from any location
- ✅ No firewall changes needed

**Cons:**
- ⚠️ Need to keep tunnel running
- ⚠️ One extra terminal window

**Setup Time:** 2 minutes
**Security Level:** 🔒🔒🔒🔒🔒 (5/5)

---

## Real-World Security Scenarios

### Scenario 1: Method 1 (Direct Connection)
**What happens if:**
- Password leaked? → Database fully compromised ❌
- IP changes? → Can't connect, need firewall update ⚠️
- Brute force attack? → Possible with weak password ❌
- Port scanner finds you? → MongoDB visible to attacker ❌

### Scenario 2: SSH Tunnel
**What happens if:**
- Password leaked? → No MongoDB password exists ✅
- IP changes? → Still works, no changes needed ✅
- Brute force attack? → SSH has built-in protection ✅
- Port scanner finds you? → Only SSH visible, MongoDB hidden ✅

---

## Conclusion

### Use SSH Tunnel (Method 2) Because:

1. **Security**: Military-grade encryption
2. **Simplicity**: One command to connect
3. **Flexibility**: Works from anywhere
4. **Zero Risk**: MongoDB never exposed
5. **Industry Standard**: What professionals use

### Only Use Direct Connection (Method 1) If:

- You absolutely need persistent connection
- You can maintain strong passwords
- You have a static IP address
- You can monitor logs regularly
- You understand the risks

---

## Quick Start (Secure Way)

1. **Open terminal and start tunnel:**
   ```bash
   ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
   ```

2. **Create local .env.local:**
   ```bash
   # apps/api/.env.local
   MONGODB_URI=mongodb://localhost:27017/leetour
   ```

3. **Start your app:**
   ```bash
   npm run dev
   ```

**That's it! Secure and simple.** 🔒
