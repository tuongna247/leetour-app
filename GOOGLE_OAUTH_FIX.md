# Google OAuth Configuration Fix Guide

## 🔧 Current Status
Your Google OAuth credentials are correctly set in `.env.local`, but there might be issues with the Google Cloud Console setup or NextAuth configuration.

## 📋 Step-by-Step Fix

### 1. Verify Google Cloud Console Setup

**Visit Google Cloud Console**: https://console.cloud.google.com/

1. **Check Your Project**:
   - Make sure you're in the correct project
   - Your Client ID: `533259289267-7aqi9u4feetnj8tr7dtkme600f76vo1r.apps.googleusercontent.com`

2. **APIs & Services → Credentials**:
   - Find your OAuth 2.0 Client ID
   - Click the pencil icon to edit
   - **Authorized redirect URIs** must include EXACTLY:
     ```
     http://localhost:3000/api/auth/callback/google
     ```
   - ⚠️ **No trailing slash, exact match required**

3. **OAuth Consent Screen**:
   - Go to "OAuth consent screen"
   - Make sure it's configured (can be "Testing" for development)
   - Add your email to "Test users" if in testing mode
   - **Publishing status**: Should be "Testing" or "Published"

4. **Enable Required APIs**:
   - Go to "APIs & Services" → "Library"
   - Search and enable: **"Google+ API"** or **"People API"**

### 2. Test Your Configuration

1. **Start your application**:
   ```bash
   npm run dev
   ```

2. **Test NextAuth endpoints directly**:
   - Visit: http://localhost:3000/api/auth/providers
   - Should show Google provider
   - Visit: http://localhost:3000/api/auth/test-providers
   - Check our custom test endpoint

3. **Test Google OAuth flow**:
   - Visit: http://localhost:3000/api/auth/signin/google
   - Should redirect to Google (not show error page)

### 3. Debug Steps

**If you see "Google login is not configured yet":**

1. **Check server logs** in your terminal running `npm run dev`
2. **Check browser console** (F12 → Console tab)
3. **Verify environment variables** by visiting: `/api/auth/test-providers`

**Common Issues:**

1. **Redirect URI Mismatch**:
   ```
   Error: redirect_uri_mismatch
   ```
   **Fix**: Verify redirect URI in Google Cloud Console matches exactly:
   `http://localhost:3000/api/auth/callback/google`

2. **OAuth Consent Screen Issues**:
   ```
   Error: access_denied
   ```
   **Fix**: 
   - Add your email to test users
   - Make sure consent screen is configured
   - Try with a different Google account

3. **API Not Enabled**:
   ```
   Error: Google+ API has not been enabled
   ```
   **Fix**: Enable Google+ API in Google Cloud Console

### 4. Test with Debug Mode

Debug mode is now enabled in your `.env.local`. Check your server logs for detailed NextAuth debugging information.

### 5. Alternative: Use Different OAuth Approach

If Google Cloud Console setup is too complex, you can:

1. **Use the manual test login** (already working)
2. **Set up a different OAuth provider** later
3. **Use traditional email/password** authentication

## 🚀 Quick Test Commands

Run these in your browser:

1. **Check providers**: `http://localhost:3000/api/auth/providers`
2. **Test endpoint**: `http://localhost:3000/api/auth/test-providers`
3. **Google OAuth**: `http://localhost:3000/api/auth/signin/google`
4. **Your app**: `http://localhost:3000/auth/signin`

## ✅ Success Indicators

**Google OAuth is working when:**
- `/api/auth/providers` shows Google in the list
- Clicking "Sign in with Google" redirects to Google (not error page)
- After Google authorization, you're redirected back to your app
- User is logged in with their Google account information

## 🆘 If Still Not Working

1. **Use the manual test login** buttons on the sign-in page
2. **Double-check Google Cloud Console** redirect URI
3. **Try creating new OAuth credentials** in Google Cloud Console
4. **Contact me** with the specific error messages you're seeing

The manual test login is working perfectly, so you can continue developing and testing all features while we fix the Google OAuth!