# Local Testing Guide - Tour Pricing System

## Quick Start

### Step 1: Start API Server

```bash
# Terminal 1 - API Server
cd d:\Projects\GitLap\leetour-app\apps\api
npm run dev
```

**Wait for**: "✓ Ready on http://localhost:3001"

### Step 2: Start Frontend Server

```bash
# Terminal 2 - Frontend Server
cd d:\Projects\GitLap\leetour-app\apps\frontend
npm run dev
```

**Wait for**: "✓ Ready on http://localhost:3002"

---

## Test the Pricing API

### Test 1: With Slug

```bash
# Test with tour slug (like your example)
curl "http://localhost:3001/api/tours/indian-golden-triangle-tour/pricing?date=2025-11-22&adults=1&children=0"
```

**Expected**: JSON response with pricing data or "Tour not found" if tour doesn't exist

### Test 2: Check Available Tours

```bash
# Get list of tours to find a valid ID/slug
curl http://localhost:3001/api/tours
```

### Test 3: Get Specific Tour Details

```bash
# Get tour by slug
curl http://localhost:3001/api/tours/indian-golden-triangle-tour
```

---

## Test in Browser

### Step 1: Find a Tour

1. Open: http://localhost:3002
2. Browse tours or go to: http://localhost:3002/tours

### Step 2: Open Tour Detail Page

1. Click on a tour
2. URL should be: http://localhost:3002/tours/[slug-or-id]
3. Page should load without errors

### Step 3: Test Pricing

1. **Select a date** (future date)
2. **Set adults**: 1-10
3. **Set children**: 0-5
4. **Click "Check Availability"**

**Expected Results:**
- Button shows "Checking..." with spinner
- After 1-2 seconds, pricing options appear below
- No console errors (press F12 to check)

### Step 4: Test Time Selection

1. Pricing options should show
2. Click on a time slot (e.g., "08:00 AM")
3. Time slot should highlight
4. Click "Book Now"

**Expected**: Redirect to booking page with query parameters

---

## Check for Errors

### Browser Console (F12)

**Good signs** ✅:
- No red errors
- API calls return 200 status
- Pricing data loads

**Bad signs** ❌:
- Module not found errors
- 404 errors on API calls
- Cannot read property errors
- White screen

### API Terminal

**Good signs** ✅:
```
GET /api/tours/indian-golden-triangle-tour 200
GET /api/tours/indian-golden-triangle-tour/pricing 200
```

**Bad signs** ❌:
```
GET /api/tours/indian-golden-triangle-tour/pricing 404
Error: Cannot find module
TypeError: ...
```

---

## Common Local Issues

### Issue 1: Tour Not Found (404)

**Cause**: Tour doesn't exist in database

**Fix**:
```bash
# Check MongoDB
mongosh
use leetour
db.tours.find({}, {title: 1, "seo.slug": 1})
```

If no tours exist, insert sample data from `SAMPLE_TOUR_DATA.json`

### Issue 2: MongoDB Connection Error

**Cause**: MongoDB not running

**Fix**:
```bash
# Windows - Start MongoDB
net start MongoDB

# Or check if running
tasklist | findstr mongod
```

### Issue 3: Port Already in Use

**Cause**: Another process using port 3001 or 3002

**Fix**:
```bash
# Find and kill process
netstat -ano | findstr :3001
taskkill /PID [PID_NUMBER] /F
```

### Issue 4: Environment Variables

**Cause**: Missing .env files

**Fix**:
```bash
# API - Create .env
cd apps/api
echo MONGODB_URI=mongodb://localhost:27017/leetour > .env

# Frontend - Create .env.local
cd apps/frontend
echo NEXT_PUBLIC_API_URL=http://localhost:3001 > .env.local
```

### Issue 5: Module Not Found

**Cause**: Dependencies not installed

**Fix**:
```bash
# Install API dependencies
cd apps/api
npm install

# Install Frontend dependencies
cd apps/frontend
npm install
```

---

## Testing Checklist

Before deploying to server, verify:

- [ ] API server starts without errors
- [ ] Frontend server starts without errors
- [ ] Can access http://localhost:3002
- [ ] Tour list page loads
- [ ] Tour detail page loads
- [ ] Can select date and passengers
- [ ] "Check Availability" works
- [ ] Pricing options display
- [ ] Can select time slots
- [ ] "Book Now" redirects properly
- [ ] No console errors
- [ ] API returns 200 status

---

## API Test Commands

### Get All Tours
```bash
curl http://localhost:3001/api/tours
```

### Get Tour by Slug
```bash
curl http://localhost:3001/api/tours/indian-golden-triangle-tour
```

### Get Tour Pricing
```bash
curl "http://localhost:3001/api/tours/indian-golden-triangle-tour/pricing?date=2025-12-25&adults=2&children=1"
```

### Test with MongoDB ObjectId (if you have one)
```bash
curl "http://localhost:3001/api/tours/507f1f77bcf86cd799439011/pricing?date=2025-12-25&adults=2"
```

---

## Database Check

### Check if Tours Exist

```bash
mongosh
use leetour
db.tours.countDocuments()
db.tours.findOne({}, {title: 1, "seo.slug": 1, tourOptions: 1})
```

### Insert Sample Tour (if none exist)

```javascript
// In mongosh
use leetour

db.tours.insertOne({
  title: "Ha Long Bay Day Cruise",
  seo: {
    slug: "ha-long-bay-day-cruise"
  },
  price: 1800000,
  currency: "VND",
  isActive: true,
  tourOptions: [{
    optionName: "Standard Tour",
    basePrice: 1800000,
    departureTimes: "08:00 AM;02:00 PM",
    isActive: true
  }]
})
```

---

## When Everything Works

Once all tests pass locally:

1. **Commit any fixes**:
   ```bash
   git add .
   git commit -m "fix: Local testing fixes"
   git push origin main
   ```

2. **Deploy to server**:
   ```bash
   ssh deployer@157.173.124.250
   cd /var/www/leetour
   git pull origin main
   bash .sh/deploy-tour-pricing.sh
   ```

3. **Test on server**:
   ```bash
   # Test API
   curl "https://api.goreise.com/api/tours/indian-golden-triangle-tour/pricing?date=2025-11-22&adults=1"

   # Test frontend
   # Open: https://tour.goreise.com
   ```

---

## Debug Mode

For detailed debugging:

### Enable Verbose Logs

**API** (`apps/api/.env`):
```env
NODE_ENV=development
DEBUG=*
```

**Frontend** (`apps/frontend/.env.local`):
```env
NEXT_PUBLIC_API_URL=http://localhost:3001
NODE_ENV=development
```

### Watch for Changes

Both servers support hot reload - just save files and they'll rebuild automatically!

---

## Quick Test Script

Save this as `test-local.sh`:

```bash
#!/bin/bash

echo "Testing Tour Pricing System Locally..."
echo ""

# Test API
echo "1. Testing API health..."
curl -s http://localhost:3001/api/tours | head -20

echo ""
echo "2. Testing tour detail..."
curl -s http://localhost:3001/api/tours/indian-golden-triangle-tour | head -20

echo ""
echo "3. Testing pricing endpoint..."
curl -s "http://localhost:3001/api/tours/indian-golden-triangle-tour/pricing?date=2025-11-22&adults=1" | head -50

echo ""
echo "4. Opening frontend in browser..."
start http://localhost:3002

echo ""
echo "✓ Tests complete! Check results above."
```

Run with: `bash test-local.sh`

---

## Success Indicators

**You're ready to deploy when:**

✅ No errors in terminal
✅ Both servers running
✅ API returns valid JSON
✅ Frontend loads in browser
✅ Pricing options appear
✅ No console errors
✅ Can complete booking flow

---

**Happy Testing!** 🧪

When everything works locally, deploy to server with confidence! 🚀
