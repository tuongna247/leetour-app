#!/usr/bin/env node

// Verification script for Google OAuth setup
require('dotenv').config({ path: '.env.local' });

console.log('🔍 Google OAuth Configuration Verification\n');

// Check environment variables
const requiredVars = {
  'GOOGLE_CLIENT_ID': process.env.GOOGLE_CLIENT_ID,
  'GOOGLE_CLIENT_SECRET': process.env.GOOGLE_CLIENT_SECRET,
  'NEXTAUTH_SECRET': process.env.NEXTAUTH_SECRET,
  'NEXTAUTH_URL': process.env.NEXTAUTH_URL,
  'MONGODB_URI': process.env.MONGODB_URI
};

let configValid = true;

console.log('📋 Environment Variables Check:');
for (const [name, value] of Object.entries(requiredVars)) {
  if (!value) {
    console.log(`❌ ${name}: Missing`);
    configValid = false;
  } else if (value.includes('your_') || value.includes('here')) {
    console.log(`⚠️  ${name}: Contains placeholder text`);
    configValid = false;
  } else {
    console.log(`✅ ${name}: ${name.includes('SECRET') ? '***' + value.slice(-4) : value}`);
  }
}

console.log('\n🔗 Google OAuth URLs:');
const baseUrl = process.env.NEXTAUTH_URL || 'http://localhost:3000';
console.log(`Callback URL: ${baseUrl}/api/auth/callback/google`);
console.log(`Sign-in URL: ${baseUrl}/api/auth/signin/google`);
console.log(`NextAuth Session URL: ${baseUrl}/api/auth/session`);

console.log('\n📝 Google Cloud Console Checklist:');
console.log('1. ✓ Go to https://console.cloud.google.com/');
console.log('2. ✓ Create/Select project');
console.log('3. ✓ Enable Google+ API');
console.log('4. ✓ Create OAuth 2.0 Client ID credentials');
console.log('5. ✓ Add authorized redirect URI:');
console.log(`   → ${baseUrl}/api/auth/callback/google`);
console.log('6. ✓ Configure OAuth consent screen');

console.log('\n🧪 Test Steps:');
console.log('1. Start your app: npm run dev');
console.log('2. Visit: http://localhost:3000/auth/signin');
console.log('3. Click "Sign in with Google" button');
console.log('4. Check browser console and server logs for errors');

if (configValid) {
  console.log('\n✅ Configuration appears valid!');
  console.log('\nIf Google OAuth still shows "not configured":');
  console.log('- Restart your development server');
  console.log('- Check Google Cloud Console redirect URI exactly matches');
  console.log('- Verify OAuth consent screen is published');
  console.log('- Try visiting the sign-in URL directly');
} else {
  console.log('\n❌ Configuration issues found. Please fix the above problems.');
}

// Test NextAuth endpoints
console.log('\n🔧 Testing NextAuth endpoints...');
console.log('You can test these URLs manually:');
console.log(`- GET ${baseUrl}/api/auth/providers (shows available providers)`);
console.log(`- GET ${baseUrl}/api/auth/session (shows current session)`);
console.log(`- GET ${baseUrl}/api/auth/csrf (shows CSRF token)`);

console.log('\n💡 Debug Tips:');
console.log('1. Enable NextAuth debug mode by adding NEXTAUTH_DEBUG=true to .env.local');
console.log('2. Check browser Network tab for failed requests');
console.log('3. Look for 500 errors in server console');
console.log('4. Verify Google credentials haven\'t expired');