# LeeTour Deployment - Quick Reference Card

Print this page for quick access to common commands and troubleshooting.

---

## 📞 Server Connection

```bash
ssh deployer@YOUR_SERVER_IP
cd /var/www/leetour
```

---

## 🔄 Common PM2 Commands

```bash
pm2 status                          # View status of all apps
pm2 logs                            # View logs (all apps)
pm2 logs leetour-admin              # View specific app logs
pm2 logs --lines 100                # View last 100 log lines
pm2 restart all                     # Restart all apps
pm2 restart leetour-admin           # Restart specific app
pm2 stop all                        # Stop all apps
pm2 start ecosystem.config.js       # Start all apps
pm2 monit                           # Monitor CPU/Memory usage
pm2 flush                           # Clear logs
pm2 save                            # Save current process list
```

---

## 🌐 Nginx Commands

```bash
sudo nginx -t                       # Test configuration
sudo systemctl status nginx         # Check Nginx status
sudo systemctl reload nginx         # Reload config (no downtime)
sudo systemctl restart nginx        # Full restart
sudo tail -f /var/log/nginx/error.log              # View errors
sudo tail -f /var/log/nginx/leetour-admin-access.log   # View access logs
```

---

## 🚀 Deployment Commands

```bash
# Full deployment (build + restart)
cd /var/www/leetour
./deploy.sh

# Quick update (no rebuild)
git pull origin main
pm2 restart all

# Update dependencies only
cd apps/admin && npm install && cd ../..
cd apps/api && npm install && cd ../..
cd apps/frontend && npm install && cd ../..
pm2 restart all
```

---

## 🔐 SSL Certificate Commands

```bash
sudo certbot certificates           # View all certificates
sudo certbot renew                  # Renew certificates manually
sudo certbot renew --dry-run        # Test renewal
sudo systemctl status certbot.timer # Check auto-renewal status
```

---

## 📊 System Monitoring

```bash
# CPU & Memory
htop                                # Interactive process viewer
free -h                             # Memory usage
df -h                               # Disk usage
du -sh /var/www/leetour             # App directory size

# Network
sudo netstat -tulpn                 # All listening ports
sudo netstat -tulpn | grep :3000    # Check specific port
ss -tulpn                           # Socket statistics

# Processes
ps aux | grep node                  # Node.js processes
ps aux | grep nginx                 # Nginx processes
```

---

## 🔍 Log Locations

```bash
# Application logs
/var/www/leetour/logs/admin-error.log
/var/www/leetour/logs/admin-out.log
/var/www/leetour/logs/api-error.log
/var/www/leetour/logs/api-out.log
/var/www/leetour/logs/frontend-error.log
/var/www/leetour/logs/frontend-out.log

# Nginx logs
/var/log/nginx/error.log
/var/log/nginx/access.log
/var/log/nginx/leetour-admin-access.log
/var/log/nginx/leetour-admin-error.log
/var/log/nginx/leetour-api-access.log
/var/log/nginx/leetour-frontend-access.log

# System logs
/var/log/syslog                     # System messages
/var/log/auth.log                   # Authentication logs
```

---

## 🛠️ Troubleshooting Quick Fixes

### Apps won't start
```bash
pm2 logs leetour-admin --lines 50   # Check logs
pm2 delete all && pm2 start ecosystem.config.js  # Fresh start
```

### 502 Bad Gateway
```bash
pm2 status                          # Check if apps are running
pm2 restart all                     # Restart apps
sudo systemctl restart nginx        # Restart Nginx
```

### Database connection error
```bash
# Check environment variables
cat apps/admin/.env.local | grep MONGODB_URI
cat apps/api/.env | grep MONGODB_URI

# Restart apps
pm2 restart all
```

### High memory usage
```bash
pm2 monit                           # Check which app uses memory
pm2 restart leetour-admin           # Restart specific app
```

### Can't access via domain
```bash
nslookup admin.yourdomain.com       # Check DNS
sudo nginx -t                       # Test Nginx config
sudo ufw status                     # Check firewall
ping admin.yourdomain.com           # Test connectivity
```

### SSL certificate expired
```bash
sudo certbot renew                  # Renew certificates
sudo systemctl reload nginx         # Reload Nginx
```

---

## 📁 Important File Locations

```bash
# Application
/var/www/leetour/                   # App root
/var/www/leetour/apps/admin/        # Admin app
/var/www/leetour/apps/api/          # API app
/var/www/leetour/apps/frontend/     # Frontend app

# Configuration
/var/www/leetour/ecosystem.config.js        # PM2 config
/etc/nginx/sites-available/leetour          # Nginx config
/var/www/leetour/apps/admin/.env.local      # Admin env
/var/www/leetour/apps/api/.env              # API env
/var/www/leetour/apps/frontend/.env         # Frontend env

# Backups
/var/www/leetour/backups/           # Application backups
```

---

## 🔐 Security Commands

```bash
# Firewall (UFW)
sudo ufw status                     # Check firewall status
sudo ufw allow 80/tcp               # Allow HTTP
sudo ufw allow 443/tcp              # Allow HTTPS
sudo ufw enable                     # Enable firewall

# Fail2ban (SSH protection)
sudo fail2ban-client status         # Check status
sudo fail2ban-client status sshd    # Check SSH jail

# System updates
sudo apt update                     # Update package list
sudo apt upgrade -y                 # Upgrade packages
sudo apt autoremove                 # Remove unused packages
```

---

## 💾 Backup & Restore

### Create backup
```bash
cd /var/www/leetour
tar -czf backup-$(date +%Y%m%d).tar.gz \
    --exclude='node_modules' \
    --exclude='.next' \
    --exclude='logs' \
    --exclude='backups' \
    .
```

### Restore from backup
```bash
cd /var/www/leetour
tar -xzf backups/backup-YYYYMMDD.tar.gz
pm2 restart all
```

### Database backup (if using mongodump)
```bash
mongodump --uri="YOUR_MONGODB_URI" --out=./backups/db-$(date +%Y%m%d)
```

---

## 🌍 Environment Variables Quick Reference

### Required in all .env files:
- `NODE_ENV=production`
- `MONGODB_URI` (admin & API must match)
- `JWT_SECRET` (admin & API must match)
- `NEXTAUTH_SECRET` (admin & API must match)

### Admin specific:
- `NEXTAUTH_URL=https://admin.yourdomain.com`
- `GOOGLE_CLIENT_ID`
- `GOOGLE_CLIENT_SECRET`

### API specific:
- `ADMIN_URL=https://admin.yourdomain.com`
- `FRONTEND_URL=https://yourdomain.com`
- `ALLOWED_ORIGINS`

### Frontend specific:
- `NEXT_PUBLIC_API_URL=https://api.yourdomain.com`

---

## 📱 URLs

- **Admin**: https://admin.yourdomain.com
- **API**: https://api.yourdomain.com
- **Frontend**: https://yourdomain.com

---

## 🆘 Emergency Contacts

- **Contabo Support**: https://contabo.com/support
- **MongoDB Atlas Support**: https://support.mongodb.com
- **Let's Encrypt**: https://letsencrypt.org/docs/

---

## 📋 Health Check Checklist

```bash
□ All apps showing "online" in PM2
□ No errors in pm2 logs
□ All URLs accessible via HTTPS
□ SSL certificates valid
□ Database connections working
□ OAuth login working (if configured)
□ Image uploads working
□ Nginx status: active (running)
□ Disk space > 20% free
□ Memory usage < 80%
```

---

## 🔢 Port Reference

- **Admin**: 3000
- **API**: 3001
- **Frontend**: 3002
- **HTTP**: 80
- **HTTPS**: 443
- **SSH**: 22 (or custom)

---

**Print Date**: _______________
**Server IP**: _______________
**Deployed By**: _______________
