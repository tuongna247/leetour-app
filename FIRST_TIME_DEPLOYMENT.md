# LeeTour - First Time Deployment Guide

**For users deploying to a fresh Contabo VPS running Ubuntu**

Follow these steps in order. Total time: ~60 minutes.

---

## Prerequisites

Before you start, make sure you have:

- ✅ Contabo VPS running Ubuntu 20.04 or 22.04
- ✅ Root or sudo access to the server
- ✅ Your server's IP address
- ✅ Domain names (optional but recommended):
  - `admin.yourdomain.com`
  - `api.yourdomain.com`
  - `yourdomain.com`
- ✅ MongoDB Atlas connection string (or existing MongoDB)
- ✅ Cloudinary account (already configured: dfmq2saqc)
- ✅ Google OAuth credentials (if using Google login)

---

## Step 1: Connect to Your Server

```bash
# From your local machine
ssh root@YOUR_SERVER_IP

# Enter your password when prompted
```

---

## Step 2: Upload Deployment Files

**Option A: Using Git (Recommended)**

```bash
# On the server
cd /root
git clone https://github.com/YOUR_USERNAME/leetour-app.git
cd leetour-app
```

**Option B: Using SCP from your local machine**

```bash
# From your local machine (in the leetour-app directory)
scp setup-server.sh deploy.sh root@YOUR_SERVER_IP:/root/
scp ecosystem.config.js nginx-leetour.conf root@YOUR_SERVER_IP:/root/
```

---

## Step 3: Run Server Setup

```bash
# Make the script executable
chmod +x setup-server.sh

# Run the setup script (this will take 10-15 minutes)
sudo ./setup-server.sh
```

**What this does:**
- Installs Node.js 20.x
- Installs PM2 (process manager)
- Installs Nginx (web server)
- Configures firewall
- Creates 'deployer' user
- Sets up security (fail2ban)
- Configures system for production

**During setup:**
- You'll be asked to set a password for the 'deployer' user
- Choose a strong password and remember it!

---

## Step 4: Prepare Application Directory

```bash
# Switch to deployer user
su - deployer

# Create and navigate to application directory
sudo mkdir -p /var/www/leetour
sudo chown -R deployer:deployer /var/www/leetour
cd /var/www/leetour

# Clone your repository
git clone https://github.com/YOUR_USERNAME/leetour-app.git .

# If you uploaded files manually, copy them
# cp -r /root/leetour-app/* /var/www/leetour/
```

**Verify the structure:**
```bash
ls -la
# You should see:
# - apps/ (directory with admin, api, frontend)
# - deploy.sh
# - ecosystem.config.js
# - nginx-leetour.conf
# - etc.
```

---

## Step 5: Configure Environment Variables

### 5.1 Generate Secrets

```bash
# Generate JWT_SECRET
openssl rand -base64 32
# Copy this output - you'll need it for BOTH admin and API

# Generate NEXTAUTH_SECRET
openssl rand -base64 32
# Copy this output - you'll need it for BOTH admin and API
```

### 5.2 Admin Environment

```bash
cd /var/www/leetour/apps/admin
cp .env.production.example .env.local
nano .env.local
```

**Edit these values:**
```env
NODE_ENV=production
MONGODB_URI=mongodb+srv://leetour:YOUR_PASSWORD@cluster0.nz7bupo.mongodb.net/leetour
JWT_SECRET=<paste_your_jwt_secret_here>
NEXTAUTH_SECRET=<paste_your_nextauth_secret_here>
NEXTAUTH_URL=https://admin.yourdomain.com
GOOGLE_CLIENT_ID=your_google_client_id
GOOGLE_CLIENT_SECRET=your_google_client_secret
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=dfmq2saqc
NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET=leetour_preset
```

**Save:** Press `Ctrl+X`, then `Y`, then `Enter`

### 5.3 API Environment

```bash
cd /var/www/leetour/apps/api
cp .env.production.example .env
nano .env
```

**Edit these values (use SAME secrets as admin):**
```env
NODE_ENV=production
MONGODB_URI=mongodb+srv://leetour:YOUR_PASSWORD@cluster0.nz7bupo.mongodb.net/leetour
JWT_SECRET=<same_as_admin>
NEXTAUTH_SECRET=<same_as_admin>
NEXTAUTH_URL=https://api.yourdomain.com
ADMIN_URL=https://admin.yourdomain.com
FRONTEND_URL=https://yourdomain.com
ALLOWED_ORIGINS=https://admin.yourdomain.com,https://yourdomain.com
```

**Save:** Press `Ctrl+X`, then `Y`, then `Enter`

### 5.4 Frontend Environment

```bash
cd /var/www/leetour/apps/frontend
cp .env.production.example .env
nano .env
```

**Edit these values:**
```env
NODE_ENV=production
NEXT_PUBLIC_API_URL=https://api.yourdomain.com
NEXT_PUBLIC_ADMIN_URL=https://admin.yourdomain.com
NEXT_PUBLIC_SITE_URL=https://yourdomain.com
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=dfmq2saqc
```

**Save:** Press `Ctrl+X`, then `Y`, then `Enter`

---

## Step 6: Deploy Applications

```bash
# Go back to project root
cd /var/www/leetour

# Make deploy script executable
chmod +x deploy.sh

# Run deployment (this will take 15-20 minutes)
./deploy.sh
```

**What this does:**
- Installs all npm dependencies
- Builds all 3 applications
- Starts PM2 processes
- Runs health checks

**Expected output at the end:**
```
✓ Deployment completed successfully!
┌─────────────────┬────┬─────────┬──────┐
│ App name        │ id │ status  │ port │
├─────────────────┼────┼─────────┼──────┤
│ leetour-admin   │ 0  │ online  │ 3000 │
│ leetour-api     │ 1  │ online  │ 3001 │
│ leetour-frontend│ 2  │ online  │ 3002 │
└─────────────────┴────┴─────────┴──────┘
```

**If you see errors:**
```bash
# Check the logs
pm2 logs

# Common issues:
# - Missing .env file → Go back to Step 5
# - MongoDB connection failed → Check MONGODB_URI
# - Build failed → Check npm install logs
```

---

## Step 7: Configure Nginx (Web Server)

```bash
# Copy nginx configuration
sudo cp /var/www/leetour/nginx-leetour.conf /etc/nginx/sites-available/leetour

# Edit with your actual domain names
sudo nano /etc/nginx/sites-available/leetour
```

**Find and replace (3 locations):**
- `admin.yourdomain.com` → `admin.yourrealdomain.com`
- `api.yourdomain.com` → `api.yourrealdomain.com`
- `yourdomain.com` → `yourrealdomain.com`

**Save:** Press `Ctrl+X`, then `Y`, then `Enter`

```bash
# Remove default site
sudo rm /etc/nginx/sites-enabled/default

# Enable your site
sudo ln -s /etc/nginx/sites-available/leetour /etc/nginx/sites-enabled/

# Test configuration
sudo nginx -t

# If test passes (you should see "syntax is ok")
sudo systemctl reload nginx
```

---

## Step 8: Configure DNS (Domain Settings)

In your domain registrar (GoDaddy, Namecheap, Cloudflare, etc.):

**Create these A records:**

| Type | Name  | Value (Points To)   | TTL  |
|------|-------|---------------------|------|
| A    | admin | YOUR_SERVER_IP      | 3600 |
| A    | api   | YOUR_SERVER_IP      | 3600 |
| A    | @     | YOUR_SERVER_IP      | 3600 |
| A    | www   | YOUR_SERVER_IP      | 3600 |

**Wait 5-15 minutes for DNS to propagate**

**Check DNS propagation:**
```bash
nslookup admin.yourdomain.com
nslookup api.yourdomain.com
nslookup yourdomain.com
```

---

## Step 9: Install SSL Certificates (HTTPS)

**Once DNS has propagated:**

```bash
# Install SSL for all domains at once
sudo certbot --nginx \
  -d admin.yourdomain.com \
  -d api.yourdomain.com \
  -d yourdomain.com \
  -d www.yourdomain.com
```

**Follow the prompts:**
1. Enter your email address
2. Agree to Terms of Service (Y)
3. Share email with EFF (optional - N or Y)
4. Choose option 2: Redirect HTTP to HTTPS

**Expected output:**
```
Successfully received certificate.
Certificate is saved at: /etc/letsencrypt/live/admin.yourdomain.com/fullchain.pem
```

---

## Step 10: Update OAuth Redirect URIs

**If using Google OAuth:**

1. Go to [Google Cloud Console](https://console.cloud.google.com)
2. Select your project
3. Navigate to: **APIs & Services** → **Credentials**
4. Click on your OAuth 2.0 Client ID
5. Under **Authorized redirect URIs**, add:
   ```
   https://admin.yourdomain.com/api/auth/callback/google
   ```
6. Click **Save**

**Restart admin app to apply changes:**
```bash
pm2 restart leetour-admin
```

---

## Step 11: Verify Everything Works

### 11.1 Check Applications

Visit each URL in your browser:

- ✅ **Admin**: https://admin.yourdomain.com
- ✅ **API**: https://api.yourdomain.com (should show "Cannot GET /")
- ✅ **Frontend**: https://yourdomain.com

### 11.2 Test Functionality

**Admin Dashboard:**
- [ ] Can access login page
- [ ] Can login with credentials
- [ ] Google OAuth works (if configured)
- [ ] Can create a tour
- [ ] Can upload images
- [ ] No errors in browser console

**Check Server Status:**
```bash
# SSH to server
ssh deployer@YOUR_SERVER_IP

# Check PM2 status
pm2 status
# All apps should show "online"

# Check logs for errors
pm2 logs --lines 50
# Should not see any red error messages
```

---

## 🎉 Deployment Complete!

Your LeeTour application is now live!

### Access URLs:
- **Admin Dashboard**: https://admin.yourdomain.com
- **API Server**: https://api.yourdomain.com
- **Frontend**: https://yourdomain.com

### Useful Commands:

```bash
# View application status
pm2 status

# View logs
pm2 logs

# Restart all apps
pm2 restart all

# Monitor resources
pm2 monit
```

---

## Troubleshooting

### Apps showing "errored" status

```bash
pm2 logs leetour-admin
# Look for the error message
# Common: Missing environment variable
```

### Can't access via domain

```bash
# Check DNS
nslookup admin.yourdomain.com

# Check Nginx
sudo nginx -t
sudo systemctl status nginx

# Check firewall
sudo ufw status
```

### Database connection error

```bash
# Verify MongoDB URI
cat apps/admin/.env.local | grep MONGODB_URI
cat apps/api/.env | grep MONGODB_URI

# Test connection
mongo "YOUR_MONGODB_URI"
```

### 502 Bad Gateway

```bash
# Check if apps are running
pm2 status

# Restart all apps
pm2 restart all

# Check Nginx logs
sudo tail -f /var/log/nginx/error.log
```

---

## Next Steps

1. **Setup Backups**
   ```bash
   # Daily backup cron job
   crontab -e
   # Add: 0 2 * * * /usr/local/bin/backup-leetour.sh
   ```

2. **Monitor Your Application**
   ```bash
   # Install monitoring
   pm2 install pm2-logrotate
   ```

3. **Secure SSH** (Recommended)
   ```bash
   # Disable password auth, use SSH keys only
   # Edit: sudo nano /etc/ssh/sshd_config
   # Set: PasswordAuthentication no
   ```

---

## Support

For issues, check:
- [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md) - Detailed guide
- [PRODUCTION_CHECKLIST.md](PRODUCTION_CHECKLIST.md) - Checklist
- PM2 Logs: `pm2 logs`
- Nginx Logs: `sudo tail -f /var/log/nginx/error.log`

---

**Congratulations! Your LeeTour application is now running in production!** 🚀
