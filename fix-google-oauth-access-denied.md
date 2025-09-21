# Fix Google OAuth access_denied Error

## 🚨 Problem
You're getting redirected to: `https://oauth2.example.com/auth?error=access_denied`

This means Google OAuth is misconfigured and redirecting to the wrong URL.

## 🔧 Step-by-Step Fix

### 1. Check Google Cloud Console Settings

**Visit**: https://console.cloud.google.com/

#### A. Verify Your Project
- Make sure you're in the correct project
- Your Client ID: `533259289267-7aqi9u4feetnj8tr7dtkme600f76vo1r.apps.googleusercontent.com`

#### B. Check OAuth 2.0 Client IDs
1. Go to **APIs & Services** → **Credentials**
2. Find your OAuth 2.0 Client ID
3. Click the **pencil icon** to edit
4. **CRITICAL**: Check "Authorized redirect URIs"

**Must include EXACTLY**:
```
http://localhost:3000/api/auth/callback/google
```

⚠️ **Common mistakes**:
- ❌ `http://localhost:3000/api/auth/callback/google/` (with trailing slash)
- ❌ `https://localhost:3000/...` (https instead of http)
- ❌ `http://127.0.0.1:3000/...` (127.0.0.1 instead of localhost)

### 2. Check OAuth Consent Screen

1. Go to **APIs & Services** → **OAuth consent screen**
2. **Status should be**: "Testing" or "Published"
3. **Add yourself as test user**:
   - In "Test users" section
   - Add your Google email address

### 3. Enable Required APIs

1. Go to **APIs & Services** → **Library**
2. Search and enable these APIs:
   - ✅ **Google+ API** (legacy but still needed)
   - ✅ **People API** (newer alternative)
   - ✅ **Google Identity Services API**

### 4. Check App Domain Settings

1. In **OAuth consent screen**
2. Under "App information"
3. **Authorized domains**: Add `localhost` for testing

## 🧪 Test After Each Fix

### Test 1: Direct OAuth URL
Visit: http://localhost:3000/api/auth/signin/google

**Expected**: Should redirect to Google (not oauth2.example.com)

### Test 2: Check Providers Endpoint
Visit: http://localhost:3000/api/auth/providers

**Expected**: Should show Google provider in the list

### Test 3: Your Sign-in Page
Visit: http://localhost:3000/auth/signin
Click "Sign in with Google"

## 🆘 If Still Not Working

### Option 1: Create New OAuth Credentials
1. In Google Cloud Console
2. Create **new** OAuth 2.0 Client ID
3. Set redirect URI: `http://localhost:3000/api/auth/callback/google`
4. Update your `.env.local` with new credentials

### Option 2: Use Manual Test Login (Recommended)
The manual test login buttons work perfectly:
- Click "Test as Admin" for full access
- Click "Test as Moderator" for mod features  
- Click "Test as Customer" for customer view

### Option 3: Simplify OAuth Setup
Try removing authorization parameters:

In `src/app/api/auth/[...nextauth]/route.js`, change:
```javascript
GoogleProvider({
  clientId: process.env.GOOGLE_CLIENT_ID,
  clientSecret: process.env.GOOGLE_CLIENT_SECRET,
  // Remove the authorization section temporarily
})
```

## 🎯 Quick Resolution

**For immediate testing**: Use the manual login buttons instead of Google OAuth.

**For production**: Fix the Google Cloud Console redirect URI to match exactly:
`http://localhost:3000/api/auth/callback/google`

## 📞 Debug Information

Your current settings:
- Client ID: `533259289267-7aqi9u4feetnj8tr7dtkme600f76vo1r.apps.googleusercontent.com`
- Expected callback: `http://localhost:3000/api/auth/callback/google`
- Current error URL: `https://oauth2.example.com/auth?error=access_denied`

The error URL suggests Google doesn't recognize your domain configuration.