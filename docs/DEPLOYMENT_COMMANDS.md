# Deployment Commands Reference

## Quick Deployment Scripts

### 1. Deploy Frontend Only
```bash
ssh deployer@157.173.124.250 "bash /var/www/leetour/deploy-frontend.sh"
```

### 2. Deploy API Only
```bash
ssh deployer@157.173.124.250 "bash /var/www/leetour/deploy-api.sh"
```

### 3. Deploy Everything (Admin + API + Frontend)
```bash
ssh deployer@157.173.124.250 "bash /var/www/leetour/deploy-all.sh"
```

---

## Manual Step-by-Step Commands

### Frontend Deployment
```bash
# Connect to server
ssh deployer@157.173.124.250

# Navigate and pull
cd /var/www/leetour && git pull

# Build frontend
cd apps/frontend && npm run build

# Restart frontend
pm2 restart leetour-frontend

# Check status
pm2 status
```

### API Deployment
```bash
# Connect to server
ssh deployer@157.173.124.250

# Navigate and pull
cd /var/www/leetour && git pull

# Build API
cd apps/api && npm run build

# Restart API
pm2 restart leetour-api

# Check status
pm2 status
```

### Admin Deployment
```bash
# Connect to server
ssh deployer@157.173.124.250

# Navigate and pull
cd /var/www/leetour && git pull

# Build admin
cd apps/admin && npm run build

# Restart admin
pm2 restart leetour-admin

# Check status
pm2 status
```

---

## One-Line Commands (From Local Machine)

### Deploy Frontend
```bash
ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/frontend && npm run build && pm2 restart leetour-frontend && pm2 status"
```

### Deploy API
```bash
ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/api && npm run build && pm2 restart leetour-api && pm2 status"
```

### Deploy Admin
```bash
ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/admin && npm run build && pm2 restart leetour-admin && pm2 status"
```

### Deploy All Applications
```bash
ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/api && npm run build && cd ../admin && npm run build && cd ../frontend && npm run build && cd ../.. && pm2 restart all && pm2 status"
```

---

## Useful PM2 Commands

### Check Application Status
```bash
ssh deployer@157.173.124.250 "pm2 status"
```

### View Logs
```bash
# Frontend logs
ssh deployer@157.173.124.250 "pm2 logs leetour-frontend --lines 50"

# API logs
ssh deployer@157.173.124.250 "pm2 logs leetour-api --lines 50"

# Admin logs
ssh deployer@157.173.124.250 "pm2 logs leetour-admin --lines 50"

# All logs
ssh deployer@157.173.124.250 "pm2 logs --lines 50"
```

### Restart Individual Applications
```bash
# Restart frontend
ssh deployer@157.173.124.250 "pm2 restart leetour-frontend"

# Restart API
ssh deployer@157.173.124.250 "pm2 restart leetour-api"

# Restart admin
ssh deployer@157.173.124.250 "pm2 restart leetour-admin"

# Restart all
ssh deployer@157.173.124.250 "pm2 restart all"
```

### Stop Applications
```bash
# Stop frontend
ssh deployer@157.173.124.250 "pm2 stop leetour-frontend"

# Stop API
ssh deployer@157.173.124.250 "pm2 stop leetour-api"

# Stop admin
ssh deployer@157.173.124.250 "pm2 stop leetour-admin"

# Stop all
ssh deployer@157.173.124.250 "pm2 stop all"
```

---

## Git Commands (From Local)

### Commit and Push Changes
```bash
# Stage all changes
git add -A

# Commit with message
git commit -m "Your commit message"

# Push to remote
git push
```

### Quick Commit (All in One)
```bash
git add -A && git commit -m "Your message" && git push
```

---

## Complete Workflow

### After making code changes locally:

1. **Commit and push changes:**
   ```bash
   cd d:\Projects\GitLap\leetour-app
   git add -A
   git commit -m "Your changes description"
   git push
   ```

2. **Deploy to server** (choose one):

   **Option A - Using deployment scripts (Recommended):**
   ```bash
   # Upload scripts first (only needed once)
   scp deploy-*.sh deployer@157.173.124.250:/var/www/leetour/

   # Then deploy
   ssh deployer@157.173.124.250 "bash /var/www/leetour/deploy-frontend.sh"
   ```

   **Option B - One-line command:**
   ```bash
   ssh deployer@157.173.124.250 "cd /var/www/leetour && git pull && cd apps/frontend && npm run build && pm2 restart leetour-frontend"
   ```

3. **Verify deployment:**
   ```bash
   ssh deployer@157.173.124.250 "pm2 status"
   ```

---

## Quick Reference URLs

- **Frontend**: https://tour.goreise.com
- **API**: https://api.goreise.com
- **Admin**: https://admin.goreise.com

---

## Troubleshooting

### If build fails:
```bash
# Check logs
ssh deployer@157.173.124.250 "pm2 logs leetour-frontend --lines 100"

# Try rebuilding
ssh deployer@157.173.124.250 "cd /var/www/leetour/apps/frontend && npm install && npm run build"
```

### If application won't start:
```bash
# Check PM2 logs
ssh deployer@157.173.124.250 "pm2 logs --err --lines 50"

# Restart with fresh start
ssh deployer@157.173.124.250 "pm2 delete leetour-frontend && pm2 start /var/www/leetour/apps/frontend/ecosystem.config.js"
```

### Clear PM2 logs:
```bash
ssh deployer@157.173.124.250 "pm2 flush"
```
