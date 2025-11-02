# LeeTour Deployment Guide - Contabo VPS (Ubuntu)

Complete step-by-step guide for deploying LeeTour to a Contabo VPS running Ubuntu without Docker.

## Table of Contents
1. [Prerequisites](#prerequisites)
2. [Server Initial Setup](#server-initial-setup)
3. [Application Deployment](#application-deployment)
4. [Domain & SSL Configuration](#domain--ssl-configuration)
5. [Post-Deployment](#post-deployment)
6. [Maintenance](#maintenance)
7. [Troubleshooting](#troubleshooting)

---

## Prerequisites

### Server Requirements
- **VPS**: Contabo VPS (minimum 4GB RAM recommended)
- **OS**: Ubuntu 20.04 LTS or 22.04 LTS
- **Storage**: Minimum 20GB SSD
- **Network**: Public IP address

### Domain Requirements (Optional but Recommended)
- 3 subdomains pointing to your server IP:
  - `admin.yourdomain.com` → Admin Dashboard
  - `api.yourdomain.com` → API Server
  - `yourdomain.com` or `www.yourdomain.com` → Frontend

### Access Requirements
- Root or sudo access to the server
- SSH key pair for secure access
- Git repository access (GitHub, GitLab, etc.)

---

## Server Initial Setup

### Step 1: Connect to Your Server

```bash
# Connect via SSH
ssh root@YOUR_SERVER_IP

# Or if using a non-root user with sudo
ssh youruser@YOUR_SERVER_IP
```

### Step 2: Run Server Setup Script

```bash
# Download the setup script (if not already on server)
# If you have the files locally, upload them first:
# scp setup-server.sh root@YOUR_SERVER_IP:/root/

# Make the script executable
chmod +x setup-server.sh

# Run the setup script
sudo ./setup-server.sh
```

This script will:
- ✓ Update system packages
- ✓ Install Node.js 20.x LTS
- ✓ Install PM2 process manager
- ✓ Install Nginx web server
- ✓ Configure firewall (UFW)
- ✓ Setup fail2ban for SSH protection
- ✓ Create 'deployer' user
- ✓ Configure system limits
- ✓ Enable swap space
- ✓ Setup log rotation

**Expected Duration**: 10-15 minutes

### Step 3: Secure SSH Access (Recommended)

```bash
# Generate SSH key on your local machine (if you haven't)
ssh-keygen -t ed25519 -C "your_email@example.com"

# Copy SSH key to server
ssh-copy-id deployer@YOUR_SERVER_IP

# Test SSH key login
ssh deployer@YOUR_SERVER_IP

# Once confirmed, disable password authentication
sudo nano /etc/ssh/sshd_config
# Set: PasswordAuthentication no
# Save and exit (Ctrl+X, Y, Enter)

# Restart SSH service
sudo systemctl restart sshd
```

---

## Application Deployment

### Step 4: Clone Repository

```bash
# Switch to deployer user
su - deployer

# Navigate to application directory
cd /var/www/leetour

# Clone your repository
git clone https://github.com/yourusername/leetour-app.git .

# Or if already cloned, pull latest changes
git pull origin main
```

### Step 5: Configure Environment Variables

```bash
# Admin App
cd /var/www/leetour/apps/admin
cp .env.production.example .env.local

# Edit the file
nano .env.local
```

**Required configurations for `.env.local`**:
```env
MONGODB_URI=mongodb+srv://leetour:YOUR_PASSWORD@cluster0.nz7bupo.mongodb.net/leetour
JWT_SECRET=$(openssl rand -base64 32)
NEXTAUTH_SECRET=$(openssl rand -base64 32)
NEXTAUTH_URL=https://admin.yourdomain.com
GOOGLE_CLIENT_ID=your_google_client_id
GOOGLE_CLIENT_SECRET=your_google_client_secret
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=dfmq2saqc
NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET=leetour_preset
```

```bash
# API Server
cd /var/www/leetour/apps/api
cp .env.production.example .env

# Edit the file
nano .env
```

**Required configurations for `.env`**:
```env
MONGODB_URI=mongodb+srv://leetour:YOUR_PASSWORD@cluster0.nz7bupo.mongodb.net/leetour
JWT_SECRET=<SAME_AS_ADMIN>
NEXTAUTH_SECRET=<SAME_AS_ADMIN>
NEXTAUTH_URL=https://api.yourdomain.com
ADMIN_URL=https://admin.yourdomain.com
FRONTEND_URL=https://yourdomain.com
ALLOWED_ORIGINS=https://admin.yourdomain.com,https://yourdomain.com
```

```bash
# Frontend App
cd /var/www/leetour/apps/frontend
cp .env.production.example .env

# Edit the file
nano .env
```

**Required configurations for `.env`**:
```env
NEXT_PUBLIC_API_URL=https://api.yourdomain.com
NEXT_PUBLIC_ADMIN_URL=https://admin.yourdomain.com
NEXT_PUBLIC_SITE_URL=https://yourdomain.com
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=dfmq2saqc
```

**IMPORTANT**:
- Generate unique secrets: `openssl rand -base64 32`
- Use the **same** JWT_SECRET and NEXTAUTH_SECRET for both admin and API
- Never commit .env files to git

### Step 6: Run Deployment Script

```bash
# Navigate to project root
cd /var/www/leetour

# Make deployment script executable
chmod +x deploy.sh

# Run the deployment
./deploy.sh
```

This script will:
- ✓ Create necessary directories
- ✓ Create backup of current deployment
- ✓ Check environment files
- ✓ Pull latest code from git
- ✓ Install dependencies for all apps
- ✓ Build all applications
- ✓ Start/restart PM2 processes
- ✓ Run health checks

**Expected Duration**: 15-20 minutes (first deployment)

### Step 7: Verify Applications

```bash
# Check PM2 status
pm2 status

# View logs
pm2 logs

# Check if apps are responding
curl http://localhost:3000  # Admin
curl http://localhost:3001  # API
curl http://localhost:3002  # Frontend
```

---

## Domain & SSL Configuration

### Step 8: Configure DNS Records

In your domain registrar (GoDaddy, Namecheap, Cloudflare, etc.), create these DNS records:

| Type | Name | Value | TTL |
|------|------|-------|-----|
| A | admin | YOUR_SERVER_IP | 3600 |
| A | api | YOUR_SERVER_IP | 3600 |
| A | @ | YOUR_SERVER_IP | 3600 |
| A | www | YOUR_SERVER_IP | 3600 |

Wait 5-15 minutes for DNS propagation.

### Step 9: Configure Nginx

```bash
# Copy Nginx configuration
sudo cp /var/www/leetour/nginx-leetour.conf /etc/nginx/sites-available/leetour

# Edit the configuration with your domains
sudo nano /etc/nginx/sites-available/leetour

# Find and replace:
# - admin.yourdomain.com → admin.youractualdomain.com
# - api.yourdomain.com → api.youractualdomain.com
# - yourdomain.com → youractualdomain.com

# Remove default site
sudo rm /etc/nginx/sites-enabled/default

# Enable LeeTour site
sudo ln -s /etc/nginx/sites-available/leetour /etc/nginx/sites-enabled/

# Test Nginx configuration
sudo nginx -t

# If test passes, reload Nginx
sudo systemctl reload nginx
```

### Step 10: Install SSL Certificates

```bash
# Install SSL certificates for all domains
sudo certbot --nginx -d admin.yourdomain.com -d api.yourdomain.com -d yourdomain.com -d www.yourdomain.com

# Follow the prompts:
# 1. Enter your email
# 2. Agree to terms of service
# 3. Choose to redirect HTTP to HTTPS (option 2)

# Certbot will automatically:
# - Obtain SSL certificates
# - Configure Nginx for HTTPS
# - Set up auto-renewal
```

### Step 11: Update OAuth Redirect URIs

**Google OAuth**:
1. Go to [Google Cloud Console](https://console.cloud.google.com)
2. Select your project
3. Navigate to: APIs & Services → Credentials
4. Edit your OAuth 2.0 Client ID
5. Add authorized redirect URIs:
   - `https://admin.yourdomain.com/api/auth/callback/google`
6. Save

**Update Admin .env.local**:
```bash
cd /var/www/leetour/apps/admin
nano .env.local
# Update NEXTAUTH_URL=https://admin.yourdomain.com
```

**Restart admin app**:
```bash
pm2 restart leetour-admin
```

---

## Post-Deployment

### Step 12: Test All Functionality

Visit your applications:
- **Admin**: https://admin.yourdomain.com
- **API**: https://api.yourdomain.com
- **Frontend**: https://yourdomain.com

Test:
- ✓ Login functionality
- ✓ Google OAuth
- ✓ Database connections
- ✓ Image uploads (Cloudinary)
- ✓ API endpoints
- ✓ Tour creation/editing
- ✓ Booking system

### Step 13: Setup Monitoring

```bash
# Install PM2 monitoring
pm2 install pm2-logrotate

# Configure log rotation
pm2 set pm2-logrotate:max_size 10M
pm2 set pm2-logrotate:retain 30

# Enable PM2 web dashboard (optional)
pm2 web
# Access at http://YOUR_SERVER_IP:9615
```

### Step 14: Setup Automated Backups

```bash
# Create backup script
sudo nano /usr/local/bin/backup-leetour.sh
```

```bash
#!/bin/bash
BACKUP_DIR="/var/www/leetour/backups"
DATE=$(date +%Y%m%d-%H%M%S)

# Backup database
mongodump --uri="YOUR_MONGODB_URI" --out="$BACKUP_DIR/db-$DATE"

# Backup application files
tar -czf "$BACKUP_DIR/app-$DATE.tar.gz" -C /var/www/leetour \
    --exclude='node_modules' \
    --exclude='.next' \
    --exclude='logs' \
    --exclude='backups' \
    .

# Delete backups older than 7 days
find $BACKUP_DIR -mtime +7 -delete

echo "Backup completed: $DATE"
```

```bash
# Make executable
sudo chmod +x /usr/local/bin/backup-leetour.sh

# Add to crontab (daily at 2 AM)
crontab -e
# Add: 0 2 * * * /usr/local/bin/backup-leetour.sh >> /var/log/leetour-backup.log 2>&1
```

---

## Maintenance

### Regular Updates

```bash
# SSH to server
ssh deployer@YOUR_SERVER_IP

# Navigate to project
cd /var/www/leetour

# Pull latest changes and deploy
./deploy.sh
```

### Useful PM2 Commands

```bash
# View status
pm2 status

# View logs (all apps)
pm2 logs

# View logs (specific app)
pm2 logs leetour-admin

# Restart all apps
pm2 restart all

# Restart specific app
pm2 restart leetour-admin

# Stop all apps
pm2 stop all

# Monitor resources
pm2 monit

# Clear logs
pm2 flush
```

### Nginx Commands

```bash
# Test configuration
sudo nginx -t

# Reload configuration
sudo systemctl reload nginx

# Restart Nginx
sudo systemctl restart nginx

# View error logs
sudo tail -f /var/log/nginx/error.log

# View access logs
sudo tail -f /var/log/nginx/leetour-admin-access.log
```

### SSL Renewal

```bash
# Test renewal
sudo certbot renew --dry-run

# Renew manually (if needed)
sudo certbot renew

# Auto-renewal is configured by default
```

---

## Troubleshooting

### Application Won't Start

```bash
# Check PM2 logs
pm2 logs leetour-admin --lines 100

# Check if ports are in use
sudo netstat -tulpn | grep :3000

# Restart app
pm2 restart leetour-admin

# Check .env files exist
ls -la /var/www/leetour/apps/admin/.env.local
ls -la /var/www/leetour/apps/api/.env
```

### Database Connection Issues

```bash
# Test MongoDB connection
mongo "mongodb+srv://YOUR_CONNECTION_STRING"

# Check if MONGODB_URI is set correctly in .env files
grep MONGODB_URI /var/www/leetour/apps/admin/.env.local
grep MONGODB_URI /var/www/leetour/apps/api/.env
```

### Nginx 502 Bad Gateway

```bash
# Check if apps are running
pm2 status

# Check Nginx error logs
sudo tail -f /var/log/nginx/error.log

# Verify Nginx configuration
sudo nginx -t

# Check if firewall is blocking ports
sudo ufw status
```

### High Memory Usage

```bash
# Check memory
free -h

# View PM2 resource usage
pm2 monit

# Restart specific app to clear memory
pm2 restart leetour-admin

# Check for memory leaks in logs
pm2 logs --lines 200
```

### SSL Certificate Issues

```bash
# Check certificate status
sudo certbot certificates

# Renew certificate
sudo certbot renew

# Test certificate
openssl s_client -connect admin.yourdomain.com:443
```

---

## Security Best Practices

1. **Keep System Updated**
   ```bash
   sudo apt update && sudo apt upgrade -y
   ```

2. **Change Default SSH Port** (Optional)
   ```bash
   sudo nano /etc/ssh/sshd_config
   # Change: Port 22 → Port 2222
   sudo systemctl restart sshd
   sudo ufw allow 2222/tcp
   ```

3. **Enable Automatic Security Updates**
   ```bash
   sudo apt install unattended-upgrades
   sudo dpkg-reconfigure -plow unattended-upgrades
   ```

4. **Monitor Failed Login Attempts**
   ```bash
   sudo fail2ban-client status sshd
   ```

5. **Regular Backups**
   - Database: Daily
   - Application files: Weekly
   - Store backups off-server

---

## Performance Optimization

1. **Enable PM2 Clustering** (for higher traffic)
   ```javascript
   // Edit ecosystem.config.js
   instances: 'max',  // Use all CPU cores
   exec_mode: 'cluster'
   ```

2. **Enable Nginx Caching**
   ```nginx
   # Add to nginx config
   proxy_cache_path /var/cache/nginx levels=1:2 keys_zone=my_cache:10m;
   proxy_cache my_cache;
   ```

3. **Enable Gzip Compression**
   ```bash
   sudo nano /etc/nginx/nginx.conf
   # Uncomment gzip settings
   ```

4. **Monitor with Netdata** (Optional)
   ```bash
   bash <(curl -Ss https://my-netdata.io/kickstart.sh)
   # Access at http://YOUR_SERVER_IP:19999
   ```

---

## Support & Resources

- **LeeTour Documentation**: [Your internal docs]
- **Next.js Documentation**: https://nextjs.org/docs
- **PM2 Documentation**: https://pm2.keymetrics.io/docs
- **Nginx Documentation**: https://nginx.org/en/docs/
- **Certbot Documentation**: https://certbot.eff.org/docs/

---

## Deployment Checklist

- [ ] Server setup completed
- [ ] All 3 apps deployed and running
- [ ] Environment variables configured
- [ ] DNS records created
- [ ] SSL certificates installed
- [ ] OAuth redirect URIs updated
- [ ] Database connection verified
- [ ] Image uploads working
- [ ] Login functionality tested
- [ ] Backups configured
- [ ] Monitoring enabled
- [ ] Firewall configured
- [ ] Documentation updated

---

**Deployment Date**: _________________

**Deployed By**: _________________

**Notes**: _________________
