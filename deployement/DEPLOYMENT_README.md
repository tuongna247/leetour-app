# LeeTour Deployment - Quick Start

Complete deployment package for Contabo VPS (Ubuntu, No Docker)

## 📦 What's Included

This deployment package includes everything needed to deploy LeeTour to a production server:

### Configuration Files
- ✅ `ecosystem.config.js` - PM2 process manager configuration
- ✅ `nginx-leetour.conf` - Nginx reverse proxy configuration
- ✅ `setup-server.sh` - Automated server setup script
- ✅ `deploy.sh` - Automated deployment script

### Environment Templates
- ✅ `apps/admin/.env.production.example` - Admin environment template
- ✅ `apps/api/.env.production.example` - API environment template
- ✅ `apps/frontend/.env.production.example` - Frontend environment template

### Documentation
- ✅ `DEPLOYMENT_GUIDE.md` - Complete step-by-step deployment guide
- ✅ `PRODUCTION_CHECKLIST.md` - Pre/post deployment checklist

### Code Changes
- ✅ Hardcoded MongoDB URI removed from `/apps/admin/src/lib/mongodb.js`
- ✅ Enhanced CORS configuration in `/apps/api/src/middleware.js`
- ✅ Security headers added
- ✅ Production-ready error handling

---

## 🚀 Quick Start (5 Steps)

### 1. Server Setup (15 minutes)
```bash
# SSH to your Contabo VPS
ssh root@YOUR_SERVER_IP

# Upload and run setup script
chmod +x setup-server.sh
sudo ./setup-server.sh
```

### 2. Clone & Configure (10 minutes)
```bash
# Switch to deployer user
su - deployer

# Clone repository
cd /var/www/leetour
git clone YOUR_REPO_URL .

# Create environment files
cd apps/admin
cp .env.production.example .env.local
nano .env.local  # Edit with production values

cd ../api
cp .env.production.example .env
nano .env  # Edit with production values

cd ../frontend
cp .env.production.example .env
nano .env  # Edit with production values
```

### 3. Deploy Applications (20 minutes)
```bash
cd /var/www/leetour
chmod +x deploy.sh
./deploy.sh
```

### 4. Configure Nginx & Domains (10 minutes)
```bash
# Copy Nginx config
sudo cp nginx-leetour.conf /etc/nginx/sites-available/leetour

# Edit with your domains
sudo nano /etc/nginx/sites-available/leetour

# Enable site
sudo ln -s /etc/nginx/sites-available/leetour /etc/nginx/sites-enabled/
sudo rm /etc/nginx/sites-enabled/default

# Test and reload
sudo nginx -t
sudo systemctl reload nginx
```

### 5. Setup SSL (5 minutes)
```bash
# Install SSL certificates
sudo certbot --nginx \
  -d admin.yourdomain.com \
  -d api.yourdomain.com \
  -d yourdomain.com \
  -d www.yourdomain.com
```

**Total Time**: ~60 minutes

---

## 🔐 Security Checklist

Before deploying, ensure you:

- [ ] Generated new JWT_SECRET: `openssl rand -base64 32`
- [ ] Generated new NEXTAUTH_SECRET: `openssl rand -base64 32`
- [ ] Updated MongoDB connection string
- [ ] Configured OAuth redirect URIs
- [ ] Set production domain names
- [ ] Reviewed CORS allowed origins

---

## 📊 Application URLs

After deployment:
- **Admin Dashboard**: https://admin.yourdomain.com
- **API Server**: https://api.yourdomain.com
- **Frontend**: https://yourdomain.com

---

## 🛠️ Common Commands

### PM2 (Process Management)
```bash
pm2 status                    # View all apps status
pm2 logs                      # View all logs
pm2 logs leetour-admin        # View specific app logs
pm2 restart all               # Restart all apps
pm2 monit                     # Monitor resources
```

### Nginx (Web Server)
```bash
sudo nginx -t                 # Test configuration
sudo systemctl reload nginx   # Reload config
sudo systemctl status nginx   # Check status
sudo tail -f /var/log/nginx/error.log  # View errors
```

### Deployment
```bash
cd /var/www/leetour
./deploy.sh                   # Full deployment (build + restart)
git pull && pm2 restart all   # Quick update (no rebuild)
```

---

## 📝 Environment Variables Reference

### Critical Variables (Must Change)

| Variable | Location | Example |
|----------|----------|---------|
| MONGODB_URI | Admin & API | `mongodb+srv://user:pass@cluster.mongodb.net/leetour` |
| JWT_SECRET | Admin & API | `openssl rand -base64 32` |
| NEXTAUTH_SECRET | Admin & API | `openssl rand -base64 32` |
| NEXTAUTH_URL | Admin & API | `https://admin.yourdomain.com` |
| ADMIN_URL | API | `https://admin.yourdomain.com` |
| FRONTEND_URL | API | `https://yourdomain.com` |
| NEXT_PUBLIC_API_URL | Frontend | `https://api.yourdomain.com` |

### OAuth Variables (If Using)

| Variable | Location | Get From |
|----------|----------|----------|
| GOOGLE_CLIENT_ID | Admin | [Google Cloud Console](https://console.cloud.google.com) |
| GOOGLE_CLIENT_SECRET | Admin | [Google Cloud Console](https://console.cloud.google.com) |

### Cloudinary (Image Upload)

| Variable | Location | Value |
|----------|----------|-------|
| NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME | All Apps | `dfmq2saqc` |
| NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET | All Apps | `leetour_preset` |

---

## 🆘 Troubleshooting

### Apps Won't Start
```bash
pm2 logs leetour-admin --lines 50
pm2 restart leetour-admin
```

### Database Connection Failed
```bash
# Check .env files have correct MONGODB_URI
grep MONGODB_URI apps/admin/.env.local
grep MONGODB_URI apps/api/.env
```

### Nginx 502 Bad Gateway
```bash
# Check if apps are running
pm2 status

# Check Nginx logs
sudo tail -f /var/log/nginx/error.log
```

### Can't Access via Domain
```bash
# Check DNS propagation
nslookup admin.yourdomain.com

# Check Nginx is running
sudo systemctl status nginx

# Check firewall
sudo ufw status
```

---

## 📚 Full Documentation

For detailed instructions, see:
- **[DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md)** - Complete deployment guide
- **[PRODUCTION_CHECKLIST.md](PRODUCTION_CHECKLIST.md)** - Deployment checklist

---

## 🔄 Update Process

When you have code changes:

```bash
# SSH to server
ssh deployer@YOUR_SERVER_IP

# Navigate to app directory
cd /var/www/leetour

# Run deployment script
./deploy.sh
```

This will:
1. Create backup
2. Pull latest code
3. Install dependencies
4. Build all apps
5. Restart PM2 processes
6. Run health checks

---

## 💾 Backup & Recovery

### Manual Backup
```bash
cd /var/www/leetour
tar -czf backup-$(date +%Y%m%d).tar.gz \
    --exclude='node_modules' \
    --exclude='.next' \
    --exclude='logs' .
```

### Restore from Backup
```bash
cd /var/www/leetour
tar -xzf backups/backup-YYYYMMDD.tar.gz
pm2 restart all
```

---

## 📞 Support

For deployment issues:
1. Check logs: `pm2 logs`
2. Review [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md)
3. Consult [PRODUCTION_CHECKLIST.md](PRODUCTION_CHECKLIST.md)

---

## ✅ Post-Deployment Verification

After deployment, verify:

- [ ] Can login to admin
- [ ] Can upload images
- [ ] Database connection works
- [ ] OAuth login works (if configured)
- [ ] SSL certificates active
- [ ] All 3 apps showing "online" in PM2

---

## 🎉 You're Done!

Your LeeTour application should now be running in production!

**Access your apps**:
- Admin: https://admin.yourdomain.com
- API: https://api.yourdomain.com
- Frontend: https://yourdomain.com

**Monitor your apps**:
```bash
pm2 monit
```

---

**Deployment Package Version**: 1.0.0
**Last Updated**: 2025-01-11
**Compatible With**: Ubuntu 20.04/22.04, Node.js 20.x, Next.js 15.x
