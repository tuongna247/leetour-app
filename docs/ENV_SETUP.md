# Environment Variables Setup Guide

This guide helps you set up environment variables for production deployment.

## 📋 Required Environment Files

You need to create 3 environment files:

1. `apps/admin/.env.local` - Admin panel configuration
2. `apps/api/.env` - API server configuration
3. `apps/frontend/.env` - Frontend application configuration

---

## 🔐 Admin Panel Environment (.env.local)

**Location:** `apps/admin/.env.local`

```env
# ===============================================
# MongoDB Database Connection
# ===============================================
# Local MongoDB
MONGODB_URI=mongodb://localhost:27017/leetour

# OR MongoDB Atlas (Cloud)
# MONGODB_URI=mongodb+srv://username:password@cluster.mongodb.net/leetour?retryWrites=true&w=majority

# ===============================================
# NextAuth Configuration
# ===============================================
# Your admin domain URL (NO trailing slash)
NEXTAUTH_URL=https://admin.goreise.com

# Generate a secret key (min 32 characters)
# Generate with: openssl rand -base64 32
NEXTAUTH_SECRET=your-secret-key-here-make-it-long-and-random-min-32-chars

# ===============================================
# API Configuration
# ===============================================
# Your API domain URL (NO trailing slash)
NEXT_PUBLIC_API_URL=https://api.goreise.com

# ===============================================
# Cloudinary (Image Upload) - OPTIONAL
# ===============================================
# Get these from: https://cloudinary.com/console
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=your-cloud-name
NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET=your-preset-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret

# ===============================================
# Google OAuth - OPTIONAL
# ===============================================
# Get these from: https://console.cloud.google.com
GOOGLE_CLIENT_ID=your-google-client-id.apps.googleusercontent.com
GOOGLE_CLIENT_SECRET=your-google-client-secret
```

---

## 🔐 API Server Environment (.env)

**Location:** `apps/api/.env`

```env
# ===============================================
# MongoDB Database Connection
# ===============================================
# Should match admin MONGODB_URI
MONGODB_URI=mongodb://localhost:27017/leetour

# OR MongoDB Atlas (Cloud)
# MONGODB_URI=mongodb+srv://username:password@cluster.mongodb.net/leetour?retryWrites=true&w=majority

# ===============================================
# Server Configuration
# ===============================================
PORT=3001
NODE_ENV=production

# ===============================================
# CORS Configuration
# ===============================================
# Comma-separated list of allowed origins
ALLOWED_ORIGINS=https://admin.goreise.com,https://tour.goreise.com

# ===============================================
# JWT Secret
# ===============================================
# Generate with: openssl rand -base64 32
JWT_SECRET=your-jwt-secret-key-here

# ===============================================
# Cloudinary - OPTIONAL (Same as admin)
# ===============================================
CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret
```

---

## 🔐 Frontend Environment (.env)

**Location:** `apps/frontend/.env`

```env
# ===============================================
# API Configuration
# ===============================================
# Your API domain URL (NO trailing slash)
NEXT_PUBLIC_API_URL=https://api.goreise.com

# ===============================================
# Application Configuration
# ===============================================
NEXT_PUBLIC_APP_NAME=LeeTour
NEXT_PUBLIC_APP_URL=https://tour.goreise.com

# ===============================================
# Google Maps API - OPTIONAL
# ===============================================
# Get this from: https://console.cloud.google.com
NEXT_PUBLIC_GOOGLE_MAPS_API_KEY=your-google-maps-api-key

# ===============================================
# Analytics - OPTIONAL
# ===============================================
# Google Analytics
# NEXT_PUBLIC_GA_TRACKING_ID=G-XXXXXXXXXX

# Facebook Pixel
# NEXT_PUBLIC_FB_PIXEL_ID=your-pixel-id
```

---

## 🔑 How to Generate Secret Keys

### Method 1: Using OpenSSL (Recommended)
```bash
# Generate a random 32-character secret
openssl rand -base64 32
```

### Method 2: Using Node.js
```bash
node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"
```

### Method 3: Using Online Generator
Visit: https://generate-secret.vercel.app/32

**⚠️ IMPORTANT:**
- Use DIFFERENT secrets for NEXTAUTH_SECRET and JWT_SECRET
- Never commit these files to Git
- Keep them secure

---

## 📝 Quick Setup Steps

### Step 1: Create Files

```bash
# On your local machine
cd /path/to/leetour-app

# Create admin env
nano apps/admin/.env.local
# Paste admin configuration, update values, save (Ctrl+X, Y, Enter)

# Create API env
nano apps/api/.env
# Paste API configuration, update values, save

# Create frontend env
nano apps/frontend/.env
# Paste frontend configuration, update values, save
```

### Step 2: Update Values

Replace these placeholders with your actual values:

- `your-secret-key-here-make-it-long-and-random-min-32-chars`
- `your-jwt-secret-key-here`
- `your-cloud-name`
- `your-preset-name`
- `your-api-key`
- `your-api-secret`
- `your-google-client-id`
- `your-google-client-secret`
- `your-google-maps-api-key`

### Step 3: Copy to Server

Use the helper script:
```bash
# Windows
connect-server.bat
# Choose option 6 (Copy environment files to server)

# Mac/Linux
./connect-server.sh
# Choose option 6
```

Or manually:
```bash
# Copy admin env
scp apps/admin/.env.local deployer@157.173.124.250:/var/www/leetour/apps/admin/

# Copy API env
scp apps/api/.env deployer@157.173.124.250:/var/www/leetour/apps/api/

# Copy frontend env
scp apps/frontend/.env deployer@157.173.124.250:/var/www/leetour/apps/frontend/
```

---

## 🗄️ MongoDB Setup Options

### Option 1: Local MongoDB (On Server)

**Install MongoDB on server:**
```bash
ssh deployer@157.173.124.250

# Install MongoDB
wget -qO - https://www.mongodb.org/static/pgp/server-6.0.asc | sudo apt-key add -
echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu $(lsb_release -cs)/mongodb-org/6.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-6.0.list
sudo apt update
sudo apt install -y mongodb-org

# Start MongoDB
sudo systemctl start mongod
sudo systemctl enable mongod

# Check status
sudo systemctl status mongod
```

**Use in .env:**
```env
MONGODB_URI=mongodb://localhost:27017/leetour
```

### Option 2: MongoDB Atlas (Cloud - Recommended)

1. Go to https://www.mongodb.com/cloud/atlas
2. Create a free account
3. Create a new cluster (M0 Free tier)
4. Add database user (Database Access)
5. Add IP address (Network Access) - Add `0.0.0.0/0` for all IPs or your server IP
6. Get connection string:
   - Click "Connect" → "Connect your application"
   - Copy the connection string
   - Replace `<password>` with your database user password

**Use in .env:**
```env
MONGODB_URI=mongodb+srv://username:password@cluster.mongodb.net/leetour?retryWrites=true&w=majority
```

---

## ☁️ Cloudinary Setup (Optional - For Image Uploads)

1. Go to https://cloudinary.com
2. Create a free account
3. Go to Dashboard
4. Find your:
   - Cloud Name
   - API Key
   - API Secret
5. Create upload preset:
   - Settings → Upload → Upload presets
   - Add upload preset
   - Set signing mode to "Unsigned"
   - Save

---

## 🗺️ Google Maps API (Optional)

1. Go to https://console.cloud.google.com
2. Create a new project
3. Enable "Maps JavaScript API"
4. Create credentials (API Key)
5. Restrict the key to your domain (tour.goreise.com)
6. Copy the API key

---

## ✅ Verification Checklist

After setting up all environment files:

- [ ] All three .env files created
- [ ] Secret keys generated (NEXTAUTH_SECRET, JWT_SECRET)
- [ ] MongoDB connection string configured
- [ ] URLs updated to use your domains (admin.goreise.com, api.goreise.com, tour.goreise.com)
- [ ] Cloudinary credentials added (if using image uploads)
- [ ] Files copied to server
- [ ] Files are NOT committed to Git (.gitignore should exclude them)

---

## 🔒 Security Best Practices

1. **Never commit .env files to Git**
   - They should be in `.gitignore`
   - Keep them only on server and local dev machine

2. **Use strong, unique secrets**
   - Generate random keys for each secret
   - Don't reuse secrets across environments

3. **Restrict database access**
   - If using MongoDB Atlas, whitelist only your server IP
   - Use strong database passwords

4. **Backup your .env files**
   - Keep a secure backup (encrypted USB, password manager)
   - You'll need them if you ever rebuild the server

5. **Rotate secrets periodically**
   - Change secrets every 6-12 months
   - Especially if team members leave

---

## 🆘 Troubleshooting

### Issue: MongoDB connection failed

**Check connection string:**
```bash
# On server
ssh deployer@157.173.124.250
node -e "const mongoose = require('mongoose'); mongoose.connect('YOUR_MONGODB_URI').then(() => console.log('Connected!')).catch(err => console.log('Error:', err.message))"
```

### Issue: Cloudinary upload not working

**Verify credentials:**
- Check Cloud Name matches exactly (case-sensitive)
- Verify API Key and Secret
- Ensure upload preset exists and is unsigned

### Issue: CORS errors

**Check ALLOWED_ORIGINS:**
```env
# Must include both admin and frontend domains
ALLOWED_ORIGINS=https://admin.goreise.com,https://tour.goreise.com
```

### Issue: NextAuth not working

**Verify:**
- NEXTAUTH_URL matches your admin domain exactly
- NEXTAUTH_SECRET is at least 32 characters
- No trailing slashes in URLs

---

## 📞 Need Help?

1. Check [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md) for deployment issues
2. Check [QUICK_START.md](./QUICK_START.md) for quick commands
3. Review application logs: `pm2 logs`

---

## 📚 External Resources

- **MongoDB Atlas**: https://docs.atlas.mongodb.com/getting-started/
- **Cloudinary**: https://cloudinary.com/documentation
- **NextAuth**: https://next-auth.js.org/configuration/options
- **Google Cloud Console**: https://console.cloud.google.com
