# 🚀 LeeTour Deployment Documentation

Complete guide for deploying LeeTour application to production server.

---

## 📑 Table of Contents

1. [Quick Overview](#quick-overview)
2. [Server Configuration](#server-configuration)
3. [Documentation Files](#documentation-files)
4. [Deployment Process](#deployment-process)
5. [Helper Scripts](#helper-scripts)
6. [Useful Commands](#useful-commands)

---

## 🎯 Quick Overview

### Server Details

- **Server IP**: `157.173.124.250`
- **User**: `deployer`
- **App Directory**: `/var/www/leetour`

### Application URLs

| Service  | Domain              | Port |
|----------|---------------------|------|
| Admin    | admin.goreise.com   | 3000 |
| API      | api.goreise.com     | 3001 |
| Frontend | tour.goreise.com    | 3002 |

### Technology Stack

- **Runtime**: Node.js 18+
- **Process Manager**: PM2
- **Web Server**: Nginx
- **Database**: MongoDB
- **SSL**: Let's Encrypt (Certbot)

---

## 📚 Documentation Files

This repository includes comprehensive deployment documentation:

### 1. **[QUICK_START.md](./QUICK_START.md)** ⚡
   **For:** First-time setup and quick deployments

   **Includes:**
   - Fast track deployment steps
   - SSH setup instructions
   - Quick command reference
   - Common issues and solutions

### 2. **[DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md)** 📖
   **For:** Complete deployment process

   **Includes:**
   - Detailed server setup steps
   - Nginx configuration
   - SSL certificate installation
   - PM2 process management
   - Troubleshooting guide
   - Security best practices

### 3. **[ENV_SETUP.md](./ENV_SETUP.md)** 🔐
   **For:** Environment variables configuration

   **Includes:**
   - Environment file templates
   - Secret key generation
   - MongoDB setup (Local & Atlas)
   - Cloudinary configuration
   - Google Maps API setup

---

## 🛠️ Helper Scripts

### For Windows Users

**connect-server.bat** - Interactive menu for:
- Connecting to server
- Deploying updates
- Viewing status and logs
- Restarting applications
- Copying environment files

**Usage:**
```batch
REM Double-click the file, or:
connect-server.bat
```

### For Mac/Linux Users

**connect-server.sh** - Same features as Windows version

**Usage:**
```bash
# Make executable (first time)
chmod +x connect-server.sh

# Run
./connect-server.sh
```

### Deployment Scripts on Server

**deploy.sh** - Automated deployment script that:
- Pulls latest code from Git
- Installs dependencies
- Builds all applications
- Restarts PM2 processes
- Performs health checks
- Creates backups

---

## 🚀 Deployment Process

### First Time Deployment

```bash
# 1. Connect to server
ssh deployer@157.173.124.250

# 2. Create app directory
sudo mkdir -p /var/www/leetour
sudo chown -R $USER:$USER /var/www/leetour

# 3. Clone repository
cd /var/www/leetour
git clone https://github.com/tuongna247/leetour-app.git .

# 4. Follow detailed setup
# See DEPLOYMENT_GUIDE.md Step 2-7
```

### Regular Deployments

**Option 1: Use Helper Script (Easiest)**
```bash
# Windows
connect-server.bat
# Choose option 2 (Connect and deploy)

# Mac/Linux
./connect-server.sh
# Choose option 2
```

**Option 2: Single Command**
```bash
ssh deployer@157.173.124.250 "cd /var/www/leetour && ./deploy.sh"
```

**Option 3: Manual Steps**
```bash
# Connect
ssh deployer@157.173.124.250

# Deploy
cd /var/www/leetour
./deploy.sh
```

---

## 📊 Monitoring & Maintenance

### Check Application Status

```bash
# View all applications
ssh deployer@157.173.124.250 "pm2 status"

# Detailed monitoring
ssh deployer@157.173.124.250 "pm2 monit"
```

### View Logs

```bash
# All logs
ssh deployer@157.173.124.250 "pm2 logs"

# Specific application
ssh deployer@157.173.124.250 "pm2 logs leetour-admin"

# Last 100 lines
ssh deployer@157.173.124.250 "pm2 logs --lines 100"

# Nginx logs
ssh deployer@157.173.124.250 "sudo tail -f /var/log/nginx/leetour-admin-error.log"
```

### Restart Applications

```bash
# Restart all
ssh deployer@157.173.124.250 "pm2 restart all"

# Restart specific app
ssh deployer@157.173.124.250 "pm2 restart leetour-admin"
ssh deployer@157.173.124.250 "pm2 restart leetour-api"
ssh deployer@157.173.124.250 "pm2 restart leetour-frontend"

# Reload (zero-downtime)
ssh deployer@157.173.124.250 "pm2 reload all"
```

---

## 🔧 Useful Commands

### PM2 Commands

```bash
# List all processes
pm2 list

# Show process details
pm2 show leetour-admin

# Stop all
pm2 stop all

# Delete all processes
pm2 delete all

# Save current list
pm2 save

# Resurrect saved list
pm2 resurrect

# Update PM2
pm2 update
```

### Nginx Commands

```bash
# Test configuration
sudo nginx -t

# Reload nginx
sudo systemctl reload nginx

# Restart nginx
sudo systemctl restart nginx

# Check status
sudo systemctl status nginx

# View error logs
sudo tail -f /var/log/nginx/error.log
```

### Git Commands

```bash
# Pull latest changes
git pull origin main

# Check current branch
git branch

# View recent commits
git log --oneline -10

# Discard local changes
git reset --hard origin/main

# View remote URL
git remote -v
```

### System Commands

```bash
# Check disk space
df -h

# Check memory usage
free -m

# Check CPU usage
top

# Check running processes
ps aux | grep node

# Check port usage
sudo netstat -tlnp | grep :3000
```

---

## 🔐 Security Checklist

- [ ] SSH key authentication enabled
- [ ] Password authentication disabled (optional)
- [ ] UFW firewall configured
- [ ] SSL certificates installed
- [ ] Environment variables secured
- [ ] MongoDB access restricted
- [ ] Regular backups scheduled
- [ ] Server updates automated
- [ ] Application logs monitored
- [ ] PM2 startup configured

---

## 🆘 Troubleshooting Quick Reference

### Problem: Can't connect to server
```bash
# Test connection
ping 157.173.124.250

# Check SSH
ssh -v deployer@157.173.124.250
```

### Problem: Application not starting
```bash
# Check PM2 logs
pm2 logs leetour-admin --lines 50

# Check if port is in use
sudo lsof -i :3000

# Restart application
pm2 restart leetour-admin
```

### Problem: 502 Bad Gateway
```bash
# Check if applications are running
pm2 status

# Check nginx error logs
sudo tail -f /var/log/nginx/error.log

# Restart nginx
sudo systemctl restart nginx
```

### Problem: MongoDB connection failed
```bash
# Check MongoDB status
sudo systemctl status mongod

# Start MongoDB
sudo systemctl start mongod

# Check connection string in .env files
```

### Problem: SSL certificate issues
```bash
# Renew certificates
sudo certbot renew

# Check certificate status
sudo certbot certificates
```

---

## 📞 Support & Resources

### Internal Documentation
- [QUICK_START.md](./QUICK_START.md) - Quick deployment guide
- [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md) - Comprehensive guide
- [ENV_SETUP.md](./ENV_SETUP.md) - Environment configuration

### External Resources
- **PM2**: https://pm2.keymetrics.io/docs/
- **Nginx**: https://nginx.org/en/docs/
- **Next.js**: https://nextjs.org/docs/deployment
- **MongoDB**: https://www.mongodb.com/docs/
- **Let's Encrypt**: https://letsencrypt.org/docs/

### Server Management
- **PM2 GUI**: Consider installing PM2 Plus for web monitoring
- **Monitoring**: Set up Uptime monitoring (UptimeRobot, Pingdom)
- **Backups**: Automated database backups recommended

---

## 🎯 Deployment Checklist

### Pre-Deployment
- [ ] Code committed and pushed to repository
- [ ] All tests passing locally
- [ ] Environment variables updated (if needed)
- [ ] Database migrations prepared (if any)
- [ ] Changelog updated

### Deployment
- [ ] Connect to server
- [ ] Pull latest code
- [ ] Run deployment script
- [ ] Verify all applications started
- [ ] Check logs for errors
- [ ] Test all URLs

### Post-Deployment
- [ ] Verify admin panel functionality
- [ ] Test API endpoints
- [ ] Check frontend pages
- [ ] Monitor error logs
- [ ] Update team on deployment
- [ ] Document any issues

---

## 💡 Pro Tips

1. **Set up SSH alias** for quick access:
   ```bash
   # Add to ~/.ssh/config
   Host leetour
       HostName 157.173.124.250
       User deployer

   # Then use: ssh leetour
   ```

2. **Create deployment alias**:
   ```bash
   # Add to ~/.bashrc or ~/.zshrc
   alias deploy-leetour='ssh deployer@157.173.124.250 "cd /var/www/leetour && ./deploy.sh"'

   # Then use: deploy-leetour
   ```

3. **Monitor logs in real-time**:
   ```bash
   pm2 logs --lines 0
   ```

4. **Quick health check**:
   ```bash
   curl -I https://admin.goreise.com
   curl -I https://api.goreise.com
   curl -I https://tour.goreise.com
   ```

5. **Backup before major updates**:
   ```bash
   # Backup is automatic in deploy.sh
   # Manual backup:
   cd /var/www/leetour
   tar -czf ~/backup-$(date +%Y%m%d).tar.gz --exclude=node_modules --exclude=.next .
   ```

---

## 📅 Maintenance Schedule

### Daily
- Monitor application logs
- Check PM2 status
- Review error rates

### Weekly
- Review server resource usage
- Check for security updates
- Review application logs for patterns

### Monthly
- Update npm packages
- Review and rotate logs
- Test backup restoration
- Security audit

### Quarterly
- Rotate secrets and API keys
- Review and optimize database
- Update Node.js version (if needed)
- Review and update documentation

---

## 🏁 Getting Started

1. **First Time?** Start with [QUICK_START.md](./QUICK_START.md)
2. **Need Details?** Read [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md)
3. **Setting up .env?** Check [ENV_SETUP.md](./ENV_SETUP.md)
4. **Ready to Deploy?** Use `connect-server.bat` or `connect-server.sh`

Happy Deploying! 🚀
