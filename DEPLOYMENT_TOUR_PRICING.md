# Tour Pricing System Deployment Guide

## Overview
This guide will help you deploy the new tour pricing system to your test server at **157.173.124.250**.

---

## What's Being Deployed

### New Components
1. **Backend API**: `/api/tours/[id]/pricing` endpoint
2. **Frontend Component**: `TourPricingOptions.jsx`
3. **Updated Tour Detail Page**: Enhanced with pricing integration
4. **Database Schema**: Updated Tour model with new fields

### Files Modified/Created
- `apps/api/src/models/Tour.js` - Enhanced schema
- `apps/api/src/app/api/tours/[id]/pricing/route.js` - New pricing endpoint
- `apps/frontend/src/app/tours/[id]/page.jsx` - Updated with pricing
- `apps/frontend/src/app/components/TourPricingOptions.jsx` - New component

---

## Pre-Deployment Checklist

### 1. Git Status Check

```bash
# Check what files have changed
git status

# Review changes
git diff
```

### 2. Environment Variables

Ensure your server has these environment variables set:

**API Server** (`/var/www/leetour/apps/api/.env`):
```env
MONGODB_URI=mongodb://localhost:27017/leetour
PORT=3001
NODE_ENV=production
```

**Frontend** (`/var/www/leetour/apps/frontend/.env`):
```env
NEXT_PUBLIC_API_URL=https://api.goreise.com
```

### 3. Database Preparation

**Important**: The new Tour schema is backward compatible. No migration required!

However, to get full features, you should add sample data:

```javascript
// Optional: Add sample tour with pricing options
// See SAMPLE_TOUR_DATA.json
```

---

## Deployment Steps

### Step 1: Commit and Push Changes

```bash
# From your local machine (Windows)
cd d:\Projects\GitLap\leetour-app

# Stage all changes
git add .

# Commit with descriptive message
git commit -m "feat: Add tour pricing system with dynamic pricing calculator

- Add /api/tours/[id]/pricing endpoint for pricing calculation
- Add TourPricingOptions component for frontend display
- Update Tour schema with new fields (overview, notes, keywords, etc.)
- Add departure time selection and children pricing (75%)
- Support surcharges, promotions, and 15% tax calculation
- Add comprehensive documentation"

# Push to repository
git push origin main
```

### Step 2: Connect to Server

```bash
# Option A: Using the connect script
.\.sh\connect-server.sh

# Option B: Direct SSH
ssh deployer@157.173.124.250
```

### Step 3: Deploy API and Frontend

Once connected to the server:

```bash
# Navigate to project directory
cd /var/www/leetour

# Pull latest changes
git pull origin main

# Option A: Deploy everything (Recommended)
./.sh/deploy-all.sh

# Option B: Deploy only API and Frontend
./.sh/deploy-api.sh
./.sh/deploy-frontend.sh
```

### Step 4: Verify Deployment

```bash
# Check PM2 status
pm2 status

# Check API logs
pm2 logs leetour-api --lines 50

# Check Frontend logs
pm2 logs leetour-frontend --lines 50

# Test pricing API endpoint
curl "https://api.goreise.com/api/tours/[tourId]/pricing?date=2025-12-25&adults=2&children=1"
```

---

## Testing on Test Site

### 1. Test API Endpoint

```bash
# From your local machine
curl "https://api.goreise.com/api/tours/TOUR_ID/pricing?date=2025-12-25&adults=2&children=0"

# Expected response:
{
  "status": 200,
  "data": {
    "tourId": "...",
    "options": [...]
  }
}
```

### 2. Test Frontend

1. Open browser: `https://tour.goreise.com/tours/[tourId]`
2. Select a departure date
3. Set number of adults and children
4. Click "Check Availability"
5. Pricing options should appear
6. Select a time slot
7. Click "Book Now"
8. Should redirect to booking page

### 3. Browser Console Check

Press F12 and check:
- ✅ No console errors
- ✅ API requests successful (200 status)
- ✅ Pricing data loaded correctly

---

## Post-Deployment Tasks

### 1. Insert Sample Tour Data (Optional)

```bash
# SSH to server
ssh deployer@157.173.124.250

# Connect to MongoDB
mongosh

# Use leetour database
use leetour

# Insert sample tour
db.tours.insertOne({
  // Copy content from SAMPLE_TOUR_DATA.json
  // or use your existing tour and update it
})

# Exit mongosh
exit
```

### 2. Update Existing Tours (Optional)

```bash
# Update existing tours to add departure times
db.tours.updateMany(
  { "tourOptions.departureTimes": { $exists: false } },
  {
    $set: {
      "tourOptions.$[].departureTimes": "08:00 AM;02:00 PM"
    }
  }
)

# Verify update
db.tours.findOne({ _id: ObjectId("YOUR_TOUR_ID") })
```

### 3. Monitor for Issues

```bash
# Monitor PM2 logs in real-time
pm2 logs

# Monitor Nginx error logs
sudo tail -f /var/log/nginx/error.log

# Monitor MongoDB logs
sudo tail -f /var/log/mongodb/mongod.log
```

---

## Rollback Procedure

If something goes wrong:

### Quick Rollback

```bash
# SSH to server
ssh deployer@157.173.124.250

# Navigate to project
cd /var/www/leetour

# Revert to previous commit
git log --oneline -5  # Find previous commit hash
git reset --hard PREVIOUS_COMMIT_HASH

# Rebuild and restart
./.sh/deploy-all.sh
```

### Full Rollback

```bash
# Stop all services
pm2 stop all

# Restore from backup (created by deploy script)
cd /var/www/leetour/backups
ls -lt | head -5  # Find latest backup

# Extract backup
tar -xzf backup-YYYYMMDD-HHMMSS.tar.gz -C /var/www/leetour-rollback

# Switch to backup
cd /var/www
mv leetour leetour-failed
mv leetour-rollback leetour

# Restart services
pm2 restart all
```

---

## Troubleshooting

### Issue: API Endpoint 404

**Symptoms**: `/api/tours/[id]/pricing` returns 404

**Solutions**:
```bash
# Check if file exists
ls -la /var/www/leetour/apps/api/src/app/api/tours/[id]/pricing/route.js

# If missing, pull again
git pull origin main

# Rebuild API
cd /var/www/leetour/apps/api
npm run build
pm2 restart leetour-api
```

### Issue: Component Not Found

**Symptoms**: Error: Cannot find module 'TourPricingOptions'

**Solutions**:
```bash
# Check if component exists
ls -la /var/www/leetour/apps/frontend/src/app/components/TourPricingOptions.jsx

# Rebuild frontend
cd /var/www/leetour/apps/frontend
npm run build
pm2 restart leetour-frontend
```

### Issue: Pricing Not Loading

**Symptoms**: Click "Check Availability" but nothing happens

**Solutions**:
1. Check browser console for errors
2. Verify API URL in frontend .env
3. Check CORS settings in API
4. Verify MongoDB connection
5. Check PM2 logs: `pm2 logs leetour-api`

### Issue: Database Connection Error

**Symptoms**: MongoDB connection refused

**Solutions**:
```bash
# Check MongoDB status
sudo systemctl status mongod

# Start MongoDB
sudo systemctl start mongod

# Check MongoDB logs
sudo tail -f /var/log/mongodb/mongod.log

# Restart API after MongoDB is running
pm2 restart leetour-api
```

---

## Performance Optimization

### 1. Enable Caching

Add to `apps/api/src/app/api/tours/[id]/pricing/route.js`:

```javascript
// Cache pricing responses for 5 minutes
export const revalidate = 300;
```

### 2. Database Indexes

```javascript
// SSH to server and run in mongosh
use leetour

// Add indexes for better performance
db.tours.createIndex({ "seo.slug": 1 })
db.tours.createIndex({ isActive: 1, isFeatured: 1 })
db.tours.createIndex({ "location.city": 1 })
db.tours.createIndex({ price: 1 })
```

### 3. Monitor Performance

```bash
# Check PM2 metrics
pm2 monit

# Check memory usage
free -h

# Check disk usage
df -h
```

---

## Security Checklist

- [ ] Environment variables are set correctly
- [ ] Database connection is secure
- [ ] API endpoints validate input
- [ ] HTTPS is enabled (SSL certificates valid)
- [ ] Firewall rules are configured
- [ ] PM2 is set to restart on reboot: `pm2 startup`
- [ ] Regular backups are enabled

---

## Quick Commands Reference

```bash
# Connect to server
ssh deployer@157.173.124.250

# Full deployment
cd /var/www/leetour && ./.sh/deploy-all.sh

# Check status
pm2 status

# View logs
pm2 logs

# Restart services
pm2 restart all

# Test API
curl https://api.goreise.com/api/tours/TOUR_ID/pricing?date=2025-12-25&adults=2

# MongoDB shell
mongosh

# View backups
ls -lh /var/www/leetour/backups
```

---

## Deployment Timeline

**Estimated Time**: 15-20 minutes

1. **Commit & Push** (2 min)
2. **Connect to Server** (1 min)
3. **Pull & Deploy** (5-10 min)
4. **Verification** (5 min)
5. **Testing** (5 min)

---

## Success Criteria

Deployment is successful when:

✅ No PM2 errors
✅ API endpoint returns 200 with valid JSON
✅ Frontend page loads without console errors
✅ Can check availability and see pricing options
✅ Can select time slots
✅ Can click "Book Now" and redirect works
✅ No MongoDB connection errors
✅ SSL certificates are valid

---

## Support

If you encounter issues:

1. Check this guide's troubleshooting section
2. Review PM2 logs: `pm2 logs`
3. Check `/docs/DEPLOYMENT_GUIDE.md` for general deployment help
4. Review `/TESTING_GUIDE.md` for detailed testing scenarios

---

## Next Steps After Deployment

1. **Test thoroughly** using TESTING_GUIDE.md
2. **Monitor** for 24 hours for any errors
3. **Collect feedback** from test users
4. **Optimize** based on performance metrics
5. **Document** any issues encountered
6. **Plan production** deployment if test is successful

---

## Deployment History

| Date | Version | Changes | Deployed By |
|------|---------|---------|-------------|
| 2025-11-21 | 1.0.0 | Initial tour pricing system | - |

---

**Ready to deploy?** Follow the steps above sequentially. Good luck! 🚀
