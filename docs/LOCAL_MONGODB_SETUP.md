# Connect Local Development to Remote MongoDB

## Overview
This guide helps you connect your local development environment to the MongoDB server running on your Ubuntu server (157.173.124.250).

---

## Method 1: Direct Connection (Simple - For Development)

### Step 1: Configure MongoDB on Server

**Upload and run the setup script:**
```bash
# Upload script to server
scp setup-mongodb-remote.sh deployer@157.173.124.250:~/

# Run the script on server
ssh deployer@157.173.124.250 "bash ~/setup-mongodb-remote.sh"
```

### Step 2: Create MongoDB Users

**Connect to server and create users:**
```bash
# SSH to server
ssh deployer@157.173.124.250

# Connect to MongoDB
mongosh

# Create admin user
use admin
db.createUser({
  user: "admin",
  pwd: "YourSecureAdminPassword123!",
  roles: [
    { role: "userAdminAnyDatabase", db: "admin" },
    "readWriteAnyDatabase"
  ]
})

# Create leetour database user
use leetour
db.createUser({
  user: "leetour",
  pwd: "YourLeetourPassword123!",
  roles: [
    { role: "readWrite", db: "leetour" }
  ]
})

# Exit mongosh
exit

# Exit SSH
exit
```

### Step 3: Update Local Environment Files

**Create `.env.local` files in your local project:**

**File: `apps/api/.env.local`**
```env
MONGODB_URI=mongodb://leetour:YourLeetourPassword123!@157.173.124.250:27017/leetour?authSource=leetour
```

**File: `apps/admin/.env.local`**
```env
MONGODB_URI=mongodb://leetour:YourLeetourPassword123!@157.173.124.250:27017/leetour?authSource=leetour
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-nextauth-secret-here
```

**File: `apps/frontend/.env.local`**
```env
NEXT_PUBLIC_API_URL=http://localhost:3001
```

### Step 4: Test Connection

```bash
# From your local machine
cd d:\Projects\GitLap\leetour-app\apps\api

# Start the API
npm run dev

# You should see: "✅ MongoDB connected successfully"
```

---

## Method 2: SSH Tunnel (Secure - Recommended)

This method is more secure as MongoDB remains only accessible via localhost on the server.

### Step 1: Create SSH Tunnel

**Open a new terminal and run:**
```bash
ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
```

Keep this terminal window open while developing. Press `Ctrl+C` to close the tunnel.

### Step 2: Update Local Environment Files

**Create `.env.local` files:**

**File: `apps/api/.env.local`**
```env
MONGODB_URI=mongodb://localhost:27017/leetour
```

**File: `apps/admin/.env.local`**
```env
MONGODB_URI=mongodb://localhost:27017/leetour
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-nextauth-secret-here
```

**File: `apps/frontend/.env.local`**
```env
NEXT_PUBLIC_API_URL=http://localhost:3001
```

### Step 3: Start Development Servers

**Terminal 1 - SSH Tunnel (keep running):**
```bash
ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
```

**Terminal 2 - API:**
```bash
cd d:\Projects\GitLap\leetour-app\apps\api
npm run dev
```

**Terminal 3 - Admin:**
```bash
cd d:\Projects\GitLap\leetour-app\apps\admin
npm run dev
```

**Terminal 4 - Frontend:**
```bash
cd d:\Projects\GitLap\leetour-app\apps\frontend
npm run dev
```

---

## Method 3: PowerShell Tunnel Script (Windows)

Create a PowerShell script to manage the tunnel:

**Create file: `start-tunnel.ps1`**
```powershell
# MongoDB SSH Tunnel for Local Development
Write-Host "Starting MongoDB SSH Tunnel..." -ForegroundColor Green
Write-Host "MongoDB on server will be available at localhost:27017" -ForegroundColor Yellow
Write-Host "Press Ctrl+C to stop the tunnel" -ForegroundColor Yellow
Write-Host ""

ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
```

**Run the script:**
```powershell
powershell -ExecutionPolicy Bypass -File start-tunnel.ps1
```

---

## Verify Connection

### Test MongoDB Connection from Local

**Create a test file: `test-mongodb-connection.js`**
```javascript
const mongoose = require('mongoose');

const MONGODB_URI = process.env.MONGODB_URI || 'mongodb://localhost:27017/leetour';

async function testConnection() {
  try {
    console.log('Connecting to MongoDB...');
    console.log('URI:', MONGODB_URI);

    await mongoose.connect(MONGODB_URI);

    console.log('✅ Successfully connected to MongoDB!');

    // List all collections
    const collections = await mongoose.connection.db.listCollections().toArray();
    console.log('📚 Collections:', collections.map(c => c.name));

    // Count tours
    const Tour = mongoose.model('Tour', new mongoose.Schema({}, { strict: false }));
    const tourCount = await Tour.countDocuments();
    console.log('🎯 Total tours:', tourCount);

    await mongoose.connection.close();
    console.log('Connection closed.');

  } catch (error) {
    console.error('❌ Connection failed:', error.message);
    process.exit(1);
  }
}

testConnection();
```

**Run the test:**
```bash
cd d:\Projects\GitLap\leetour-app\apps\api
node test-mongodb-connection.js
```

---

## Current MongoDB Status on Server

To check if MongoDB is currently installed and running:

```bash
# Check MongoDB status
ssh deployer@157.173.124.250 "sudo systemctl status mongod"

# Check MongoDB version
ssh deployer@157.173.124.250 "mongod --version"

# Check if MongoDB is listening
ssh deployer@157.173.124.250 "sudo netstat -tlnp | grep 27017"

# View MongoDB logs
ssh deployer@157.173.124.250 "sudo tail -50 /var/log/mongodb/mongod.log"
```

---

## Security Considerations

### For Development (Method 1):
- ✅ Easy to set up
- ✅ Works from anywhere
- ⚠️ Exposes MongoDB to internet
- ⚠️ Requires strong passwords
- ⚠️ Firewall rules needed

### For Production (Method 2):
- ✅ Very secure (no internet exposure)
- ✅ Uses SSH encryption
- ✅ No password needed if using SSH keys
- ⚠️ Requires SSH tunnel to be running

**Recommendation:** Use Method 2 (SSH Tunnel) for development to keep MongoDB secure.

---

## Troubleshooting

### Connection Timeout
```bash
# Check if MongoDB is running
ssh deployer@157.173.124.250 "sudo systemctl status mongod"

# Check firewall
ssh deployer@157.173.124.250 "sudo ufw status"

# Check MongoDB logs
ssh deployer@157.173.124.250 "sudo tail -100 /var/log/mongodb/mongod.log"
```

### Authentication Failed
```bash
# Verify user exists
ssh deployer@157.173.124.250 "mongosh --eval 'use leetour; db.getUsers()'"
```

### Port Already in Use
```bash
# Check what's using port 27017
netstat -ano | findstr :27017

# Kill the process if needed (Windows)
taskkill /PID <PID> /F
```

### SSH Tunnel Disconnects
Add these flags for auto-reconnect:
```bash
ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N -o ServerAliveInterval=60 -o ServerAliveCountMax=3
```

---

## Quick Start Checklist

- [ ] MongoDB is running on Ubuntu server
- [ ] MongoDB users are created (if using Method 1)
- [ ] Firewall allows port 27017 (if using Method 1)
- [ ] SSH tunnel is running (if using Method 2)
- [ ] `.env.local` files are created locally
- [ ] Connection test passes
- [ ] Development servers start successfully

---

## Development Workflow

1. **Start SSH Tunnel** (Method 2):
   ```bash
   ssh -L 27017:localhost:27017 deployer@157.173.124.250 -N
   ```

2. **Start API**:
   ```bash
   cd apps/api
   npm run dev
   ```

3. **Start Admin**:
   ```bash
   cd apps/admin
   npm run dev
   ```

4. **Start Frontend**:
   ```bash
   cd apps/frontend
   npm run dev
   ```

5. **Access applications**:
   - Frontend: http://localhost:3002
   - API: http://localhost:3001
   - Admin: http://localhost:3000

6. **When done**, press `Ctrl+C` in the SSH tunnel terminal to close the connection.
