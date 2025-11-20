# Admin Tour Add/Delete Fix Guide

## 🔍 Issues Found & Fixed

### 1. Missing `categories` field in API stats
**Problem:** The frontend expects `stats.categories` array but API wasn't returning it
**Fixed:** Added `categories: await Tour.distinct('category', baseStatsFilter)` to stats object

### 2. Insufficient error logging
**Problem:** Hard to diagnose what's failing
**Fixed:** Added detailed console logging to:
- Add tour function
- Delete tour function
- AuthContext authenticatedFetch

## 📋 Steps to Test Locally

### Step 1: Start Local Admin Server
The admin server is already running at: **http://localhost:3000**

### Step 2: Login to Admin Panel
1. Open: http://localhost:3000/auth/auth1/login
2. Login with your admin credentials

### Step 3: Test & Check Console

#### To Test ADD Tour:
1. Go to: http://localhost:3000/admin/tours
2. Click "Add New Tour" button
3. Fill in the form:
   - Tour Title (required)
   - Description
   - Location info
   - Price
4. Click "Create Tour"
5. **Open Browser Console (F12)** and look for:
   ```
   🔐 AuthenticatedFetch called: {url: '/api/admin/tours', method: 'POST'}
   🔐 Current auth state: {...}
   🔐 Response status: XXX
   Create tour response status: XXX
   Create tour response data: {...}
   ```

#### To Test DELETE Tour:
1. Go to: http://localhost:3000/admin/tours
2. Click delete icon on any tour
3. Confirm deletion
4. **Open Browser Console (F12)** and look for:
   ```
   🔐 AuthenticatedFetch called: {url: '/api/admin/tours/[id]', method: 'DELETE'}
   Attempting to delete tour: [id]
   Delete response status: XXX
   Delete response data: {...}
   ```

## 🐛 What to Look For in Console

### If You See 401 Unauthorized:
```
🔐 Authentication failed - 401 Unauthorized
Error: Authentication failed
```
**Problem:** User not authenticated or token expired
**Solution:**
- Check if you're logged in
- Try logging out and back in
- Check localStorage for token: `localStorage.getItem('token')`

### If You See 403 Forbidden:
```
Create tour failed: Access denied. Only admins and moderators can create tours
```
**Problem:** User doesn't have admin/mod role
**Solution:** Check user role in database

### If You See "No authentication token available":
```
🔐 No authentication token available
```
**Problem:** Not logged in or session expired
**Solution:** Login again

### If You See Validation Errors:
```
Create tour failed: Validation error
errors: [{field: 'title', message: '...'}]
```
**Problem:** Missing required fields
**Solution:** Fill in all required fields

## ✅ Success Indicators

### Successful ADD:
```
Create tour response status: 200
Create tour response data: {status: 201, msg: "Tour created successfully"}
✅ Alert: "Tour created successfully!"
→ Redirects to /admin/tours
```

### Successful DELETE:
```
Delete response status: 200
Delete response data: {status: 200, msg: "Tour deleted successfully"}
✅ Alert: "Tour deleted successfully"
→ Tour removed from list
```

## 🚀 Deploy to Production

Once local testing works, commit and deploy:

```bash
# Commit changes
git add .
git commit -m "Fix admin tour add/delete with better error logging and stats"
git push

# Deploy to server
ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/admin && npm run build && pm2 restart leetour-admin"
```

## 📝 After Deployment

1. Go to: https://admin.goreise.com/admin/tours
2. Try adding a tour
3. Try deleting a tour
4. If issues persist, check PM2 logs:
   ```bash
   ssh deployer@157.173.124.250 "pm2 logs leetour-admin --lines 100"
   ```

## 🔧 Common Issues & Solutions

### Issue: "Invalid Server Actions request"
**Seen in logs:** `x-forwarded-host header mismatch`
**Solution:** This is a Next.js proxy header issue - usually safe to ignore if main functionality works

### Issue: Categories dropdown empty
**Was:** API didn't return categories array
**Fixed:** Now returns `stats.categories` from distinct Tour categories

### Issue: Deleted tours still showing
**Was:** Default filter showed all tours (active + inactive)
**Fixed:** Default filter now shows only active tours. Use status dropdown to see inactive/all tours

### Issue: Can't see delete/add buttons
**Check:** User role must be 'admin' or 'mod'
**Query MongoDB:**
```javascript
db.users.find({ email: "your-email@example.com" }, { role: 1 })
```

## 📞 Need More Help?

1. Check browser console (F12) for detailed logs
2. Check server logs: `pm2 logs leetour-admin`
3. Share console output or error messages
