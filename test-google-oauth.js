#!/usr/bin/env node

// Test script to verify Google OAuth configuration
require('dotenv').config({ path: '.env.local' });

console.log('🔧 Testing Google OAuth Configuration\n');

// Check environment variables
const requiredVars = ['GOOGLE_CLIENT_ID', 'GOOGLE_CLIENT_SECRET', 'NEXTAUTH_SECRET', 'MONGODB_URI'];
let allGood = true;

for (const varName of requiredVars) {
  const value = process.env[varName];
  if (!value) {
    console.log(`❌ ${varName}: Missing`);
    allGood = false;
  } else if (value.includes('your_') || value.includes('here')) {
    console.log(`⚠️  ${varName}: Not configured (contains placeholder)`);
    allGood = false;
  } else {
    console.log(`✅ ${varName}: Configured`);
  }
}

console.log('\n📋 Google OAuth Details:');
console.log(`Client ID: ${process.env.GOOGLE_CLIENT_ID || 'Not set'}`);
console.log(`Client Secret: ${process.env.GOOGLE_CLIENT_SECRET ? '***' + process.env.GOOGLE_CLIENT_SECRET.slice(-4) : 'Not set'}`);

console.log('\n🔗 OAuth URLs to test:');
console.log(`Google OAuth: http://localhost:3000/api/auth/signin/google`);
console.log(`NextAuth Status: http://localhost:3000/api/auth/session`);

if (allGood) {
  console.log('\n✅ Configuration looks good! Try testing the Google sign-in button.');
} else {
  console.log('\n❌ Configuration issues found. Please fix the above issues and try again.');
}

console.log('\n🛠️  If you\'re still getting "Server error":');
console.log('1. Check browser console for JavaScript errors');
console.log('2. Check server logs in your terminal');
console.log('3. Verify Google Cloud Console redirect URI: http://localhost:3000/api/auth/callback/google');
console.log('4. Make sure Google+ API is enabled in Google Cloud Console');