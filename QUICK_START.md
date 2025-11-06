# Quick Start Guide - Server Deployment

## 🚀 Fast Track Deployment

### Prerequisites
- Git Bash or WSL installed (Windows) or Terminal (Mac/Linux)
- SSH access configured to server

### Step 1: Test Server Connection

```bash
# Test if you can connect to the server
ssh deployer@157.173.124.250

# If successful, you'll see the server prompt
# If it asks for password, enter it
# Type 'exit' to disconnect
```

### Step 2: Setup SSH Key (Recommended - One Time Only)

**Windows (Git Bash or PowerShell):**
```bash
# Generate SSH key
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"
# Press Enter for all prompts (use defaults)

# Copy your public key
cat ~/.ssh/id_rsa.pub
# Or: type %USERPROFILE%\.ssh\id_rsa.pub
```

**Mac/Linux:**
```bash
# Generate SSH key
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"

# Copy SSH key to server
ssh-copy-id deployer@157.173.124.250
```

### Step 3: Use Helper Scripts

**Windows Users:**
```bash
# Double-click connect-server.bat
# OR in Command Prompt:
connect-server.bat
```

**Mac/Linux Users:**
```bash
# Make script executable (first time only)
chmod +x connect-server.sh

# Run the script
./connect-server.sh
```

---

## 📝 Manual Deployment (If Scripts Don't Work)

### 1. Connect to Server
```bash
ssh deployer@157.173.124.250
```

### 2. Navigate to App Directory
```bash
cd /var/www/leetour
```

### 3. Pull Latest Code
```bash
git pull origin main
```

### 4. Run Deploy Script
```bash
./deploy.sh
```

That's it! The script will handle:
- Installing dependencies
- Building applications
- Restarting PM2 processes
- Health checks

---

## 🔍 Check Deployment Status

```bash
# View all running applications
ssh deployer@157.173.124.250 "pm2 status"

# View logs
ssh deployer@157.173.124.250 "pm2 logs --lines 50"

# View specific app logs
ssh deployer@157.173.124.250 "pm2 logs leetour-admin --lines 50"
```

---

## 🌐 Access Your Applications

After successful deployment:

- **Admin Panel**: https://admin.goreise.com (or http://admin.goreise.com)
- **API Server**: https://api.goreise.com (or http://api.goreise.com)
- **Frontend**: https://tour.goreise.com (or http://tour.goreise.com)

**Note:** HTTPS will work only after SSL certificates are installed (see DEPLOYMENT_GUIDE.md Step 7)

---

## ⚡ Quick Commands Reference

```bash
# Connect to server
ssh deployer@157.173.124.250

# Deploy updates
ssh deployer@157.173.124.250 "cd /var/www/leetour && ./deploy.sh"

# Check status
ssh deployer@157.173.124.250 "pm2 status"

# View logs
ssh deployer@157.173.124.250 "pm2 logs"

# Restart all apps
ssh deployer@157.173.124.250 "pm2 restart all"

# Restart specific app
ssh deployer@157.173.124.250 "pm2 restart leetour-admin"
ssh deployer@157.173.124.250 "pm2 restart leetour-api"
ssh deployer@157.173.124.250 "pm2 restart leetour-frontend"
```

---

## 🔧 Common Issues

### Issue: "Permission denied (publickey)"
**Solution:** You need to set up SSH key or use password authentication

### Issue: "Connection refused"
**Solution:** Check if the server IP is correct and server is running

### Issue: "PM2 not found"
**Solution:** PM2 needs to be installed on server. See full DEPLOYMENT_GUIDE.md

### Issue: "Port already in use"
**Solution:**
```bash
ssh deployer@157.173.124.250
pm2 restart all
```

### Issue: "Application not responding"
**Solution:**
```bash
ssh deployer@157.173.124.250
pm2 logs leetour-admin --lines 50
# Check logs for errors
```

---

## 📚 Need More Help?

See detailed guides:
- [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md) - Complete deployment documentation
- [README.md](./README.md) - Application documentation

---

## 💡 Pro Tips

1. **Bookmark these URLs:**
   - https://admin.goreise.com
   - https://api.goreise.com
   - https://tour.goreise.com

2. **Save SSH alias** (Add to `~/.ssh/config`):
   ```
   Host leetour
       HostName 157.173.124.250
       User deployer
       IdentityFile ~/.ssh/id_rsa
   ```
   Then connect with: `ssh leetour`

3. **Create git alias for quick deploy:**
   ```bash
   git config alias.deploy '!git push origin main && ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && ./deploy.sh"'
   ```
   Then use: `git deploy`

4. **Use PM2 monitoring:**
   ```bash
   ssh deployer@157.173.124.250 "pm2 monit"
   ```

---

## 🎯 Deployment Checklist

- [ ] Can connect to server via SSH
- [ ] Server has Node.js, PM2, Nginx installed
- [ ] Repository cloned at /var/www/leetour
- [ ] Environment files (.env) created and configured
- [ ] DNS records pointing to server IP
- [ ] Nginx configuration set up
- [ ] PM2 applications running
- [ ] Applications accessible via URLs

Once all items are checked, your deployment is complete!
