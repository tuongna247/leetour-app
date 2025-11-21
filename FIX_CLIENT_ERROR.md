# Fix Client-Side Error - tour.goreise.com

## Error: "Application error: a client-side exception has occurred"

This is a Next.js error that happens during rendering. Let's debug and fix it.

---

## Quick Fix Steps

### Step 1: SSH to Server and Check Logs

```bash
# Connect to server
ssh deployer@157.173.124.250

# Check PM2 logs for errors
pm2 logs leetour-frontend --lines 100

# Look for:
# - Module not found errors
# - Import errors
# - Build errors
# - Runtime errors
```

### Step 2: Check Build Status

```bash
# Navigate to frontend
cd /var/www/leetour/apps/frontend

# Check if .next folder exists
ls -la .next

# Check build output
cat .next/BUILD_ID

# If build failed, rebuild
npm run build
```

### Step 3: Common Issues and Fixes

#### Issue A: Module Not Found

**Symptoms**: "Cannot find module '@/app/components/TourPricingOptions'"

**Fix**:
```bash
cd /var/www/leetour/apps/frontend

# Verify file exists
ls -la src/app/components/TourPricingOptions.jsx

# If missing, pull again
cd /var/www/leetour
git pull origin main

# Rebuild
cd apps/frontend
npm run build
pm2 restart leetour-frontend
```

#### Issue B: Import Error in TourPricingOptions

**Symptoms**: Error in the component itself

**Fix**:
```bash
# Check component for syntax errors
cat src/app/components/TourPricingOptions.jsx | head -50

# If there are errors, the component might have been corrupted
# Pull fresh copy
git checkout apps/frontend/src/app/components/TourPricingOptions.jsx
npm run build
pm2 restart leetour-frontend
```

#### Issue C: Environment Variable Missing

**Symptoms**: API calls fail, undefined errors

**Fix**:
```bash
# Check .env file
cat /var/www/leetour/apps/frontend/.env

# Should contain:
# NEXT_PUBLIC_API_URL=https://api.goreise.com

# If missing, add it
echo "NEXT_PUBLIC_API_URL=https://api.goreise.com" >> /var/www/leetour/apps/frontend/.env

# Rebuild
cd /var/www/leetour/apps/frontend
npm run build
pm2 restart leetour-frontend
```

#### Issue D: Node Modules Corruption

**Symptoms**: Various import errors

**Fix**:
```bash
cd /var/www/leetour/apps/frontend

# Remove and reinstall node_modules
rm -rf node_modules
rm -rf .next
npm install

# Rebuild
npm run build
pm2 restart leetour-frontend
```

---

## Complete Rebuild (Nuclear Option)

If nothing else works, do a complete rebuild:

```bash
# SSH to server
ssh deployer@157.173.124.250

# Stop frontend
pm2 stop leetour-frontend

# Navigate to project
cd /var/www/leetour

# Pull latest
git fetch origin
git reset --hard origin/main

# Clean frontend
cd apps/frontend
rm -rf node_modules
rm -rf .next
npm install
npm run build

# Restart
pm2 restart leetour-frontend

# Check logs
pm2 logs leetour-frontend
```

---

## Debugging Commands

### Check Server Logs

```bash
# Real-time logs
pm2 logs leetour-frontend

# Last 100 lines
pm2 logs leetour-frontend --lines 100

# Only errors
pm2 logs leetour-frontend --err

# Show specific time range
pm2 logs leetour-frontend --lines 200 | grep -i error
```

### Check Browser Console

1. Open `https://tour.goreise.com/tours/[tourId]`
2. Press `F12` (Developer Tools)
3. Go to Console tab
4. Look for error messages

Common errors:
- `Module not found`
- `Unexpected token`
- `Cannot read property`
- `undefined is not a function`

### Check Network Tab

1. Open Developer Tools (F12)
2. Go to Network tab
3. Reload page
4. Look for:
   - Failed requests (red)
   - 404 errors
   - 500 errors

---

## Alternative: Use Old Version Temporarily

If you need the site working immediately:

```bash
# SSH to server
ssh deployer@157.173.124.250

cd /var/www/leetour

# Revert to previous commit (before tour pricing)
git log --oneline -5  # Find commit before tour pricing
git reset --hard 5ac2d54  # Previous working version

# Rebuild
cd apps/frontend
npm run build
pm2 restart leetour-frontend
```

---

## Most Likely Fix (Try This First)

The most common issue is that the component wasn't properly built. Try this:

```bash
# SSH to server
ssh deployer@157.173.124.250

# Go to frontend
cd /var/www/leetour/apps/frontend

# Force rebuild
rm -rf .next
npm run build

# Restart
pm2 restart leetour-frontend

# Check logs
pm2 logs leetour-frontend --lines 50

# Test in browser
# Open: https://tour.goreise.com
```

---

## Test Tour Page Without Pricing (Temporary Fix)

If you want the site working while debugging, you can temporarily disable the pricing feature:

```bash
# SSH to server
ssh deployer@157.173.124.250

cd /var/www/leetour/apps/frontend/src/app/tours/[id]

# Backup current page
cp page.jsx page.jsx.backup

# Comment out TourPricingOptions import and usage
nano page.jsx

# Comment these lines:
# import TourPricingOptions from '@/app/components/TourPricingOptions';
# ... and the component usage

# Or use git to get old version
git checkout 5ac2d54~1 -- apps/frontend/src/app/tours/[id]/page.jsx

# Rebuild
cd /var/www/leetour/apps/frontend
npm run build
pm2 restart leetour-frontend
```

---

## Get Exact Error Message

To see the exact error:

### Method 1: PM2 Logs
```bash
pm2 logs leetour-frontend --raw --lines 200 | grep -A 10 -i "error"
```

### Method 2: Check .next/trace
```bash
cat /var/www/leetour/apps/frontend/.next/trace
```

### Method 3: Browser Dev Tools
1. Open site in browser
2. Press F12
3. Console tab
4. Copy the full error message

**Send me the error message** and I can provide a specific fix!

---

## Prevention for Next Time

To avoid this in the future:

1. **Test locally first**:
   ```bash
   npm run build
   npm run start
   ```

2. **Check for errors**:
   ```bash
   npm run lint
   ```

3. **Deploy in stages**:
   - Deploy to test first
   - Verify it works
   - Then deploy to production

4. **Keep backups**:
   ```bash
   # The deploy scripts create backups automatically
   ls /var/www/leetour/backups
   ```

---

## Quick Status Check

Run this to check current status:

```bash
# On server
pm2 status
pm2 logs leetour-frontend --lines 20
curl -I https://tour.goreise.com
ls -la /var/www/leetour/apps/frontend/.next
```

---

## Need More Help?

1. Get the exact error from browser console (F12)
2. Check PM2 logs: `pm2 logs leetour-frontend`
3. Share the error message for specific help

---

**Most Common Solution**: Just rebuild the frontend

```bash
ssh deployer@157.173.124.250
cd /var/www/leetour/apps/frontend
rm -rf .next && npm run build && pm2 restart leetour-frontend
```

Then open `https://tour.goreise.com` and check if it works!
