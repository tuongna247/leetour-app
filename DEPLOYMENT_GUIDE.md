# LeeTour Application Deployment Guide

## Server Information

- **Server IP**: 157.173.124.250
- **Admin Panel**: admin.goreise.com → Port 3000
- **API Server**: api.goreise.com → Port 3001
- **Frontend**: tour.goreise.com → Port 3002

## Prerequisites

### 1. SSH Key Setup (Local Machine)

```bash
# Generate SSH key if you don't have one
ssh-keygen -t rsa -b 4096 -C "your_email@example.com"

# Copy your public key
cat ~/.ssh/id_rsa.pub
# Or on Windows:
# type %USERPROFILE%\.ssh\id_rsa.pub
```

### 2. Server Requirements

The server should have:
- Node.js (v18+)
- npm or yarn
- PM2 (process manager)
- Nginx (reverse proxy)
- Git
- MongoDB (database)

---

## Step 1: Connect to Server

### Option A: Password Authentication (First Time)

```bash
ssh deployer@157.173.124.250
# Enter password when prompted
```

### Option B: SSH Key Authentication (Recommended)

```bash
# On your local machine, copy your SSH public key to the server
ssh-copy-id deployer@157.173.124.250

# Or manually add it
ssh deployer@157.173.124.250
mkdir -p ~/.ssh
chmod 700 ~/.ssh
nano ~/.ssh/authorized_keys
# Paste your public key, save and exit (Ctrl+X, Y, Enter)
chmod 600 ~/.ssh/authorized_keys
exit

# Now you can connect without password
ssh deployer@157.173.124.250
```

---

## Step 2: Initial Server Setup (One-Time)

Connect to your server and run these commands:

```bash
# Update system packages
sudo apt update && sudo apt upgrade -y

# Install Node.js 18.x
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt install -y nodejs

# Install PM2 globally
sudo npm install -g pm2

# Install Nginx
sudo apt install -y nginx

# Install Git
sudo apt install -y git

# Create application directory
sudo mkdir -p /var/www/leetour
sudo chown -R $USER:$USER /var/www/leetour

# Clone your repository
cd /var/www/leetour
git clone https://github.com/tuongna247/leetour-app.git .

# Or if you already have the repo
cd /var/www/leetour
git remote add origin https://github.com/tuongna247/leetour-app.git
```

---

## Step 3: Configure Environment Variables

Create `.env` files for each application:

### Admin App (.env.local)

```bash
nano /var/www/leetour/apps/admin/.env.local
```

Paste and update:

```env
# MongoDB Connection
MONGODB_URI=mongodb://localhost:27017/leetour
# Or MongoDB Atlas
# MONGODB_URI=mongodb+srv://username:password@cluster.mongodb.net/leetour

# NextAuth Configuration
NEXTAUTH_URL=https://admin.goreise.com
NEXTAUTH_SECRET=your-super-secret-key-here-min-32-chars

# API Configuration
NEXT_PUBLIC_API_URL=https://api.goreise.com

# Cloudinary (Optional - for image uploads)
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=your-cloud-name
NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET=your-preset
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret

# Google OAuth (Optional)
GOOGLE_CLIENT_ID=your-google-client-id
GOOGLE_CLIENT_SECRET=your-google-client-secret
```

### API App (.env)

```bash
nano /var/www/leetour/apps/api/.env
```

Paste and update:

```env
# MongoDB Connection
MONGODB_URI=mongodb://localhost:27017/leetour

# API Configuration
PORT=3001
NODE_ENV=production

# CORS Origins
ALLOWED_ORIGINS=https://admin.goreise.com,https://tour.goreise.com

# JWT Secret
JWT_SECRET=your-jwt-secret-key-here

# Cloudinary (Optional)
CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret
```

### Frontend App (.env)

```bash
nano /var/www/leetour/apps/frontend/.env
```

Paste and update:

```env
# API Configuration
NEXT_PUBLIC_API_URL=https://api.goreise.com

# App Configuration
NEXT_PUBLIC_APP_NAME=LeeTour
NEXT_PUBLIC_APP_URL=https://tour.goreise.com

# Optional Services
NEXT_PUBLIC_GOOGLE_MAPS_API_KEY=your-google-maps-key
```

---

## Step 4: Configure Nginx

```bash
# Copy the nginx configuration
sudo cp /var/www/leetour/nginx-leetour.conf /etc/nginx/sites-available/leetour

# Create symbolic link
sudo ln -s /etc/nginx/sites-available/leetour /etc/nginx/sites-enabled/

# Test nginx configuration
sudo nginx -t

# If test is successful, reload nginx
sudo systemctl reload nginx

# Enable nginx to start on boot
sudo systemctl enable nginx
```

---

## Step 5: Configure DNS

Go to your domain provider (Namecheap, GoDaddy, Cloudflare, etc.) and add these A records:

```
Type    Name      Value              TTL
A       admin     157.173.124.250    Auto/300
A       api       157.173.124.250    Auto/300
A       tour      157.173.124.250    Auto/300
```

Wait 5-30 minutes for DNS propagation.

---

## Step 6: Deploy the Application

### Option A: Using deploy.sh Script (Recommended)

```bash
# Navigate to application directory
cd /var/www/leetour

# Make deploy script executable
chmod +x deploy.sh

# Run deployment
./deploy.sh
```

### Option B: Manual Deployment

```bash
cd /var/www/leetour

# Pull latest code
git pull origin main

# Install dependencies for each app
cd apps/admin && npm install --production=false
cd ../api && npm install --production=false
cd ../frontend && npm install --production=false
cd ../..

# Build applications
cd apps/admin && NODE_ENV=production npm run build
cd ../api && NODE_ENV=production npm run build
cd ../frontend && NODE_ENV=production npm run build
cd ../..

# Start with PM2
pm2 start ecosystem.config.js --env production
pm2 save
pm2 startup
```

---

## Step 7: Setup SSL Certificates (HTTPS)

```bash
# Install Certbot
sudo apt install -y certbot python3-certbot-nginx

# Obtain SSL certificates for all three domains
sudo certbot --nginx -d admin.goreise.com
sudo certbot --nginx -d api.goreise.com
sudo certbot --nginx -d tour.goreise.com

# Certbot will automatically configure Nginx for HTTPS

# Test auto-renewal
sudo certbot renew --dry-run

# Set up auto-renewal (runs twice daily)
sudo systemctl enable certbot.timer
sudo systemctl start certbot.timer
```

---

## Step 8: Verify Deployment

```bash
# Check PM2 processes
pm2 status

# Check logs
pm2 logs

# Check specific app logs
pm2 logs leetour-admin
pm2 logs leetour-api
pm2 logs leetour-frontend

# Check Nginx status
sudo systemctl status nginx

# Test endpoints
curl http://localhost:3000
curl http://localhost:3001
curl http://localhost:3002
```

---

## Useful PM2 Commands

```bash
# View all processes
pm2 list
pm2 status

# View logs
pm2 logs
pm2 logs leetour-admin --lines 100

# Restart applications
pm2 restart all
pm2 restart leetour-admin

# Stop applications
pm2 stop all
pm2 stop leetour-admin

# Monitor resources
pm2 monit

# Save current process list
pm2 save

# Setup PM2 to start on system boot
pm2 startup

# Delete all processes
pm2 delete all
```

---

## Continuous Deployment

### Method 1: SSH into Server and Deploy

```bash
# From your local machine
ssh deployer@157.173.124.250

# Once connected
cd /var/www/leetour
./deploy.sh
```

### Method 2: Remote Deployment (From Local Machine)

```bash
# SSH and run deploy script
ssh deployer@157.173.124.250 'cd /var/www/leetour && ./deploy.sh'
```

### Method 3: Using PM2 Deploy

```bash
# First time setup
pm2 deploy production setup

# Deploy updates
pm2 deploy production
```

---

## Troubleshooting

### Application not starting

```bash
# Check PM2 logs
pm2 logs leetour-admin --lines 50

# Check if port is already in use
sudo netstat -tlnp | grep 3000

# Restart the application
pm2 restart leetour-admin
```

### Nginx errors

```bash
# Check nginx error logs
sudo tail -f /var/log/nginx/error.log

# Check configuration
sudo nginx -t

# Restart nginx
sudo systemctl restart nginx
```

### Database connection issues

```bash
# Check MongoDB status
sudo systemctl status mongod

# Start MongoDB if not running
sudo systemctl start mongod

# View MongoDB logs
sudo tail -f /var/log/mongodb/mongod.log
```

### DNS not working

```bash
# Check if DNS has propagated
nslookup admin.goreise.com
nslookup api.goreise.com
nslookup tour.goreise.com

# Or use online tools:
# https://dnschecker.org
```

### Port conflicts

```bash
# Check what's using the port
sudo lsof -i :3000
sudo lsof -i :3001
sudo lsof -i :3002

# Kill the process
sudo kill -9 <PID>
```

---

## Security Best Practices

1. **Firewall Configuration**:
```bash
# Enable UFW firewall
sudo ufw enable

# Allow SSH
sudo ufw allow 22/tcp

# Allow HTTP and HTTPS
sudo ufw allow 80/tcp
sudo ufw allow 443/tcp

# Check status
sudo ufw status
```

2. **Keep System Updated**:
```bash
sudo apt update && sudo apt upgrade -y
```

3. **Regular Backups**:
```bash
# The deploy.sh script automatically creates backups
# Backups are stored in /var/www/leetour/backups
```

4. **Monitor Logs**:
```bash
# PM2 logs
pm2 logs

# Nginx logs
sudo tail -f /var/log/nginx/leetour-*-error.log
```

---

## Rollback Procedure

If deployment fails:

```bash
# Stop current processes
pm2 stop all

# Restore from backup
cd /var/www/leetour/backups
# Find the latest backup
ls -lt | head -n 5

# Extract backup
tar -xzf backup-YYYYMMDD-HHMMSS.tar.gz -C /var/www/leetour-rollback

# Switch directories
cd /var/www
mv leetour leetour-failed
mv leetour-rollback leetour

# Restart applications
cd /var/www/leetour
pm2 restart all
```

---

## Support & Resources

- **PM2 Documentation**: https://pm2.keymetrics.io/docs/usage/quick-start/
- **Nginx Documentation**: https://nginx.org/en/docs/
- **Next.js Deployment**: https://nextjs.org/docs/deployment
- **Let's Encrypt**: https://letsencrypt.org/getting-started/

---

## Quick Reference Card

```bash
# Connect to server
ssh deployer@157.173.124.250

# Deploy updates
cd /var/www/leetour && ./deploy.sh

# Check status
pm2 status

# View logs
pm2 logs

# Restart all apps
pm2 restart all

# Nginx reload
sudo systemctl reload nginx
```
