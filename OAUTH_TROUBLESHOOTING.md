# OAuth Troubleshooting Guide

## 🔧 Common Issues and Solutions

### Google OAuth Issues

#### Issue: "redirect_uri_mismatch" Error
```
Error: redirect_uri_mismatch
The redirect URI in the request: http://localhost:3000/api/auth/callback/google does not match
```

**Solution:**
1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Navigate to APIs & Services → Credentials
3. Click on your OAuth 2.0 Client ID
4. Under "Authorized redirect URIs", add exactly:
   - `http://localhost:3000/api/auth/callback/google`
   - `https://yourdomain.com/api/auth/callback/google` (for production)

#### Issue: "access_denied" Error
**Solution:**
1. Check if Google+ API is enabled
2. Verify OAuth consent screen is configured
3. Add your email as a test user during development

### Facebook OAuth Issues

#### Issue: "URL Blocked" Error
```
Can't Load URL: The domain of this URL isn't included in the app's domains
```

**Solution:**
1. Go to [Facebook Developers](https://developers.facebook.com/)
2. Select your app → Settings → Basic
3. Add `localhost` to "App Domains"
4. In Facebook Login → Settings, add:
   - `http://localhost:3000/api/auth/callback/facebook`

#### Issue: "Invalid OAuth access token"
**Solution:**
1. Verify App ID and App Secret are correct
2. Check if Facebook Login product is added to your app
3. Ensure app is not in "Development Mode" for production

### Environment Variables

#### Issue: "NEXTAUTH_SECRET not set" Warning
**Solution:**
Add to your `.env.local`:
```bash
NEXTAUTH_SECRET=your_32_character_random_string_here
```

Generate one with:
```bash
node -e "console.log(require('crypto').randomBytes(32).toString('hex'))"
```

#### Issue: Environment variables not loading
**Solution:**
1. Ensure file is named exactly `.env.local` (not `.env.local.txt`)
2. Restart your development server after changes
3. Verify variables are not cached

### Database Issues

#### Issue: "User not found" after OAuth sign-in
**Solution:**
1. Check MongoDB connection string
2. Verify User model is properly imported
3. Check database permissions

#### Issue: Duplicate user entries
**Solution:**
The system handles this automatically by:
1. Checking if user exists with OAuth ID
2. Linking OAuth account to existing email if found
3. Creating new user only if email doesn't exist

### Development Testing

#### Issue: OAuth works in development but not production
**Solution:**
1. Update redirect URIs to use production domain
2. Set correct `NEXTAUTH_URL` for production
3. Verify SSL certificate for HTTPS

#### Issue: "This app is blocked" Google warning
**Solution:**
1. This appears for unverified apps
2. Click "Advanced" → "Go to app (unsafe)" for testing
3. For production, complete Google's app verification process

## 🧪 Testing Your Setup

### 1. Check Environment Variables
```bash
# In your terminal
node -e "
const fs = require('fs');
const env = fs.readFileSync('.env.local', 'utf8');
console.log('Environment variables loaded:');
console.log('GOOGLE_CLIENT_ID:', env.includes('GOOGLE_CLIENT_ID'));
console.log('FACEBOOK_CLIENT_ID:', env.includes('FACEBOOK_CLIENT_ID'));
console.log('NEXTAUTH_SECRET:', env.includes('NEXTAUTH_SECRET'));
"
```

### 2. Test OAuth Endpoints
Open these URLs in your browser:
- Google: `http://localhost:3000/api/auth/signin/google`
- Facebook: `http://localhost:3000/api/auth/signin/facebook`

### 3. Check Database Connection
1. Sign in with OAuth
2. Check your MongoDB collection for new user
3. Verify user has correct role and OAuth ID

## 🔍 Debug Mode

Enable debug logging by adding to `.env.local`:
```bash
NEXTAUTH_DEBUG=true
```

This will show detailed logs in your console for troubleshooting.

## 📞 Getting Help

If you're still having issues:

1. **Check Browser Console**: Look for JavaScript errors
2. **Check Server Logs**: Look for API route errors
3. **Verify Credentials**: Double-check all IDs and secrets
4. **Test Redirect URIs**: Ensure they match exactly

## 🚀 Production Deployment

Before going live:

1. **Update Redirect URIs** with your production domain
2. **Set NEXTAUTH_URL** to your production URL
3. **Verify OAuth Apps** are approved for public use
4. **Enable SSL/HTTPS** for secure authentication
5. **Remove debug flags** from environment variables

## 🔐 Security Best Practices

1. **Never commit** `.env.local` to version control
2. **Rotate secrets** regularly
3. **Use HTTPS** in production
4. **Review OAuth scopes** - request only what you need
5. **Monitor authentication logs** for suspicious activity