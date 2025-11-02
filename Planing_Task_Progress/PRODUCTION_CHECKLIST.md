# LeeTour Production Deployment Checklist

Complete this checklist before and after deploying to production.

## Pre-Deployment Checklist

### Security
- [ ] All hardcoded credentials removed from code
- [ ] MongoDB URI moved to environment variables
- [ ] New JWT_SECRET generated (`openssl rand -base64 32`)
- [ ] New NEXTAUTH_SECRET generated (`openssl rand -base64 32`)
- [ ] Same secrets used in both admin and API apps
- [ ] `.env.local` and `.env` files NOT committed to git
- [ ] `.gitignore` properly configured
- [ ] CORS configured for production domains only
- [ ] OAuth redirect URIs updated for production domains
- [ ] Firewall (UFW) enabled and configured
- [ ] Fail2ban configured for SSH protection
- [ ] SSH key authentication enabled
- [ ] Password authentication disabled
- [ ] Root login disabled

### Database
- [ ] Production MongoDB cluster created (or existing verified)
- [ ] Database credentials secured
- [ ] Connection string tested
- [ ] IP whitelist configured in MongoDB Atlas
- [ ] Database indexes created
- [ ] Initial data seeded (if needed)
- [ ] Backup strategy planned

### Environment Configuration
- [ ] Admin `.env.local` created from `.env.production.example`
- [ ] API `.env` created from `.env.production.example`
- [ ] Frontend `.env` created from `.env.production.example`
- [ ] All placeholder values replaced with production values
- [ ] NEXTAUTH_URL updated to production domains
- [ ] ADMIN_URL updated in API
- [ ] FRONTEND_URL updated in API
- [ ] ALLOWED_ORIGINS configured in API
- [ ] Cloudinary credentials verified

### Domain & SSL
- [ ] Domain names purchased/configured
- [ ] DNS A records created and propagated
- [ ] admin.yourdomain.com → Server IP
- [ ] api.yourdomain.com → Server IP
- [ ] yourdomain.com → Server IP
- [ ] www.yourdomain.com → Server IP
- [ ] DNS propagation verified (nslookup, dig)

### OAuth Configuration
- [ ] Google OAuth credentials created for production
- [ ] Google OAuth redirect URIs updated:
  - `https://admin.yourdomain.com/api/auth/callback/google`
- [ ] Facebook OAuth configured (if enabled)
- [ ] OAuth credentials added to admin `.env.local`

### Code Quality
- [ ] All tests passing locally
- [ ] No console.log() or debug code in production
- [ ] Error handling implemented
- [ ] API rate limiting configured
- [ ] Input validation on all endpoints
- [ ] SQL injection protection verified
- [ ] XSS protection enabled

### Server Preparation
- [ ] VPS purchased and accessible
- [ ] Ubuntu 20.04/22.04 installed
- [ ] Root/sudo access verified
- [ ] SSH access configured
- [ ] Server timezone set (UTC recommended)
- [ ] Required ports open (80, 443, 22)

### Application Files
- [ ] `setup-server.sh` uploaded to server
- [ ] `deploy.sh` made executable
- [ ] `ecosystem.config.js` reviewed
- [ ] `nginx-leetour.conf` reviewed
- [ ] Git repository accessible from server

---

## Deployment Process Checklist

### Initial Setup
- [ ] Connected to server via SSH
- [ ] Ran `setup-server.sh` successfully
- [ ] Node.js installed (v20.x)
- [ ] PM2 installed globally
- [ ] Nginx installed and running
- [ ] Deployer user created
- [ ] Application directory created: `/var/www/leetour`

### Code Deployment
- [ ] Repository cloned to `/var/www/leetour`
- [ ] All environment files created and configured
- [ ] Admin dependencies installed
- [ ] API dependencies installed
- [ ] Frontend dependencies installed
- [ ] Admin built successfully
- [ ] API built successfully
- [ ] Frontend built successfully
- [ ] PM2 processes started
- [ ] PM2 startup configured

### Nginx Configuration
- [ ] Nginx config copied to `/etc/nginx/sites-available/leetour`
- [ ] Domain names updated in Nginx config
- [ ] Symlink created in `/etc/nginx/sites-enabled/`
- [ ] Nginx configuration tested (`nginx -t`)
- [ ] Nginx reloaded
- [ ] Applications accessible via IP

### SSL Configuration
- [ ] Certbot installed
- [ ] SSL certificates obtained for all domains
- [ ] HTTPS redirect configured
- [ ] SSL auto-renewal configured
- [ ] Certificate expiry tested (`certbot renew --dry-run`)

---

## Post-Deployment Verification

### Application Health
- [ ] Admin accessible at https://admin.yourdomain.com
- [ ] API accessible at https://api.yourdomain.com
- [ ] Frontend accessible at https://yourdomain.com
- [ ] All apps returning 200 status
- [ ] PM2 status shows all apps online
- [ ] No errors in PM2 logs

### Functionality Testing
- [ ] Admin login works (local auth)
- [ ] Google OAuth login works
- [ ] Database connection verified
- [ ] Can create/edit tours
- [ ] Can upload images (Cloudinary)
- [ ] Can manage pricing options
- [ ] Can manage surcharges
- [ ] Can manage promotions
- [ ] Can manage cancellation policies
- [ ] Can view bookings
- [ ] Can manage users
- [ ] Can manage suppliers
- [ ] Frontend displays tours correctly
- [ ] API endpoints respond correctly

### Security Testing
- [ ] HTTPS working on all domains
- [ ] HTTP redirects to HTTPS
- [ ] Security headers present
- [ ] CORS only allows specified domains
- [ ] JWT authentication working
- [ ] Rate limiting working
- [ ] File upload limits working
- [ ] SQL injection protection verified
- [ ] XSS protection verified

### Performance Testing
- [ ] Page load times acceptable (<3s)
- [ ] API response times acceptable (<500ms)
- [ ] Image optimization working
- [ ] Caching configured
- [ ] Gzip compression enabled
- [ ] No memory leaks detected

### Monitoring & Logging
- [ ] PM2 logs accessible
- [ ] Nginx logs accessible
- [ ] Log rotation configured
- [ ] PM2 monitoring enabled
- [ ] Error tracking setup (optional)
- [ ] Uptime monitoring setup (optional)

### Backup & Recovery
- [ ] Backup script created
- [ ] Cron job for automated backups configured
- [ ] Backup restoration tested
- [ ] Offsite backup configured (recommended)

---

## Production Maintenance Checklist

### Daily
- [ ] Check PM2 status
- [ ] Review error logs
- [ ] Monitor server resources (CPU, RAM, disk)

### Weekly
- [ ] Review application logs
- [ ] Check backup completion
- [ ] Test backup restoration
- [ ] Monitor SSL certificate expiry
- [ ] Review failed login attempts

### Monthly
- [ ] Update system packages (`apt update && apt upgrade`)
- [ ] Update Node.js dependencies (security updates)
- [ ] Review and rotate old logs
- [ ] Review security policies
- [ ] Database optimization/cleanup
- [ ] Performance analysis

### Quarterly
- [ ] Security audit
- [ ] Load testing
- [ ] Disaster recovery drill
- [ ] Review and update documentation

---

## Rollback Plan

If deployment fails:

1. **Stop affected applications**
   ```bash
   pm2 stop all
   ```

2. **Restore from backup**
   ```bash
   cd /var/www/leetour
   tar -xzf backups/backup-TIMESTAMP.tar.gz
   ```

3. **Restore database (if needed)**
   ```bash
   mongorestore --uri="YOUR_MONGODB_URI" backups/db-TIMESTAMP
   ```

4. **Restart applications**
   ```bash
   pm2 restart all
   ```

5. **Verify functionality**
   ```bash
   curl http://localhost:3000
   curl http://localhost:3001
   curl http://localhost:3002
   ```

---

## Emergency Contacts

**Technical Support**:
- Server Provider (Contabo): [support link]
- DNS Provider: [support link]
- Database (MongoDB Atlas): https://support.mongodb.com

**Team Contacts**:
- DevOps Lead: _________________
- Backend Lead: _________________
- Frontend Lead: _________________
- Database Admin: _________________

---

## Deployment Sign-Off

**Deployment Date**: _______________

**Deployed By**: _______________

**Verified By**: _______________

**Production URL**:
- Admin: https://admin.yourdomain.com
- API: https://api.yourdomain.com
- Frontend: https://yourdomain.com

**Issues Encountered**:
_______________________________________________________________
_______________________________________________________________

**Resolution**:
_______________________________________________________________
_______________________________________________________________

**Signature**: _______________

**Date**: _______________
