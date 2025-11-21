# Pre-Deployment Checklist for Tour Pricing System

## ✅ Before You Deploy

Go through this checklist **before** running the deployment:

---

### 1. Local Testing ✓

- [ ] Tour detail page loads without errors
- [ ] Can select date and passengers
- [ ] "Check Availability" button works
- [ ] Pricing API returns valid response
- [ ] TourPricingOptions component renders
- [ ] Time slots are selectable
- [ ] "Book Now" redirects properly
- [ ] No console errors in browser
- [ ] Mobile view works correctly

**Test Command**:
```bash
# Start local servers
cd apps/api && npm run dev
cd apps/frontend && npm run dev

# Open browser
http://localhost:3002/tours/[tourId]
```

---

### 2. Code Review ✓

- [ ] All new files are created
- [ ] No syntax errors
- [ ] Import paths are correct
- [ ] Environment variables used correctly
- [ ] No hardcoded URLs or secrets
- [ ] Comments are clear and helpful
- [ ] No `console.log` statements left in production code
- [ ] Error handling is implemented

**Files to verify**:
```
✓ apps/api/src/models/Tour.js
✓ apps/api/src/app/api/tours/[id]/pricing/route.js
✓ apps/frontend/src/app/tours/[id]/page.jsx
✓ apps/frontend/src/app/components/TourPricingOptions.jsx
✓ apps/api/src/utils/pricingCalculator.js (already exists)
```

---

### 3. Git Status ✓

- [ ] All changes are committed
- [ ] Commit message is descriptive
- [ ] No untracked files that should be committed
- [ ] No sensitive data in commits (.env files excluded)
- [ ] Branch is up to date

**Commands**:
```bash
# Check status
git status

# Check for uncommitted changes
git diff

# View last commit
git log -1

# Push to remote
git push origin main
```

---

### 4. Environment Variables ✓

**API Server** (`apps/api/.env`):
- [ ] `MONGODB_URI` is set
- [ ] `PORT=3001` is set
- [ ] `NODE_ENV=production` on server

**Frontend** (`apps/frontend/.env`):
- [ ] `NEXT_PUBLIC_API_URL` points to API server
- [ ] URL format is correct (https://api.goreise.com)

**Verify on server**:
```bash
# SSH to server
ssh deployer@157.173.124.250

# Check API env
cat /var/www/leetour/apps/api/.env | grep -E "MONGODB_URI|PORT"

# Check Frontend env
cat /var/www/leetour/apps/frontend/.env | grep API_URL
```

---

### 5. Server Health ✓

- [ ] Server is accessible via SSH
- [ ] MongoDB is running
- [ ] Disk space is sufficient (>10% free)
- [ ] PM2 processes are healthy
- [ ] No memory issues
- [ ] Nginx is running

**Health Check Commands**:
```bash
# SSH to server
ssh deployer@157.173.124.250

# Check MongoDB
sudo systemctl status mongod

# Check disk space
df -h

# Check PM2
pm2 status

# Check memory
free -h

# Check Nginx
sudo systemctl status nginx
```

---

### 6. Backup Verification ✓

- [ ] Backup directory exists: `/var/www/leetour/backups`
- [ ] Recent backup is available
- [ ] Know rollback procedure

**Commands**:
```bash
# List recent backups
ls -lth /var/www/leetour/backups | head -5

# Create manual backup before deployment (optional)
cd /var/www/leetour
tar -czf backups/manual-backup-$(date +%Y%m%d-%H%M%S).tar.gz --exclude=node_modules --exclude=.next --exclude=backups .
```

---

### 7. Database Preparation ✓

- [ ] MongoDB is accessible
- [ ] Test database has sample tour data
- [ ] Know how to query tours
- [ ] Backup exists (if making schema changes)

**Commands**:
```bash
# Connect to MongoDB
mongosh

# Switch to database
use leetour

# Check tours count
db.tours.countDocuments()

# Get a tour ID for testing
db.tours.findOne({}, {_id: 1, title: 1})

# Exit
exit
```

---

### 8. DNS & SSL ✓

- [ ] Domain DNS is configured
  - admin.goreise.com → 157.173.124.250
  - api.goreise.com → 157.173.124.250
  - tour.goreise.com → 157.173.124.250
- [ ] SSL certificates are valid
- [ ] HTTPS works for all domains

**Verification**:
```bash
# Check DNS
nslookup api.goreise.com
nslookup tour.goreise.com

# Check SSL certificates
curl -I https://api.goreise.com
curl -I https://tour.goreise.com

# Or check expiry
echo | openssl s_client -servername api.goreise.com -connect api.goreise.com:443 2>/dev/null | openssl x509 -noout -dates
```

---

### 9. Rollback Plan ✓

- [ ] Know how to revert git commits
- [ ] Know where backups are stored
- [ ] Know how to restore from backup
- [ ] Have tested rollback procedure before

**Quick Rollback**:
```bash
# If just deployed and something is wrong
cd /var/www/leetour
git log --oneline -5  # Get previous commit hash
git reset --hard PREVIOUS_COMMIT_HASH
pm2 restart all
```

---

### 10. Communication ✓

- [ ] Team knows deployment is happening
- [ ] Maintenance window scheduled (if needed)
- [ ] Have way to contact server admin
- [ ] Documentation is updated

---

## Quick Pre-Deploy Test

Run this quick test before deploying:

```bash
# On your local machine

# 1. Check git status
git status

# 2. Run local tests
cd apps/frontend
npm run dev

# In another terminal
cd apps/api
npm run dev

# 3. Open browser and test
# http://localhost:3002/tours/[tourId]
# - Select date
# - Click "Check Availability"
# - Verify pricing appears

# 4. If all good, commit and push
git add .
git commit -m "feat: Add tour pricing system"
git push origin main
```

---

## Deployment Command

Once all checks pass:

```bash
# Option 1: SSH and deploy manually
ssh deployer@157.173.124.250
cd /var/www/leetour
./.sh/deploy-tour-pricing.sh

# Option 2: Remote deploy (one command from local)
ssh deployer@157.173.124.250 'cd /var/www/leetour && bash .sh/deploy-tour-pricing.sh'
```

---

## Post-Deployment Verification

After deployment, immediately check:

```bash
# 1. PM2 status
pm2 status

# 2. Check logs for errors
pm2 logs leetour-api --lines 20
pm2 logs leetour-frontend --lines 20

# 3. Test API endpoint
curl "https://api.goreise.com/api/tours/TOUR_ID/pricing?date=2025-12-25&adults=2"

# 4. Open in browser
# https://tour.goreise.com/tours/TOUR_ID
```

---

## Checklist Summary

Total items: **40+ checks**

**Critical checks** (Must pass):
- ✅ Local testing works
- ✅ Git is up to date
- ✅ Environment variables set
- ✅ Server is healthy
- ✅ Backup exists

**Important checks** (Should pass):
- Code review complete
- Database prepared
- DNS/SSL working
- Rollback plan ready

**Nice to have**:
- Team notified
- Documentation complete

---

## Ready to Deploy?

If you've checked all items above:

1. Take a deep breath 😊
2. Run the deployment command
3. Monitor logs carefully
4. Test the live site immediately
5. Keep this checklist open for reference

**Good luck with your deployment!** 🚀

---

## Emergency Contacts

- **Server IP**: 157.173.124.250
- **SSH User**: deployer
- **MongoDB**: localhost:27017
- **Backup Location**: /var/www/leetour/backups

---

**Last Updated**: 2025-11-21
