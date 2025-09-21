#!/usr/bin/env node

// Google OAuth diagnostic script
require('dotenv').config({ path: '.env.local' });

console.log('🔍 Google OAuth Diagnostic Report\n');

const clientId = process.env.GOOGLE_CLIENT_ID;
const clientSecret = process.env.GOOGLE_CLIENT_SECRET;

console.log('📋 Current Configuration:');
console.log(`Client ID: ${clientId}`);
console.log(`Client Secret: ${clientSecret ? '***' + clientSecret.slice(-4) : 'Missing'}`);
console.log(`NextAuth URL: ${process.env.NEXTAUTH_URL}`);
console.log(`NextAuth Secret: ${process.env.NEXTAUTH_SECRET ? 'Set' : 'Missing'}\n`);

console.log('🚨 Error Analysis:');
console.log('You got redirected to: https://oauth2.example.com/auth?error=access_denied');
console.log('This indicates one of these issues:\n');

console.log('1️⃣ **REDIRECT URI MISMATCH** (Most Likely)');
console.log('   Google Cloud Console redirect URI does not match exactly:');
console.log('   Expected: http://localhost:3000/api/auth/callback/google');
console.log('   Check: https://console.cloud.google.com/apis/credentials\n');

console.log('2️⃣ **OAUTH CONSENT SCREEN NOT CONFIGURED**');
console.log('   Status might be "Not published" or missing test users');
console.log('   Check: https://console.cloud.google.com/apis/credentials/consent\n');

console.log('3️⃣ **REQUIRED APIS NOT ENABLED**');
console.log('   Missing: Google+ API or People API');
console.log('   Check: https://console.cloud.google.com/apis/library\n');

console.log('🔧 **IMMEDIATE FIXES TO TRY**:\n');

console.log('Fix 1: Update Google Cloud Console');
console.log('1. Visit: https://console.cloud.google.com/apis/credentials');
console.log('2. Find your OAuth 2.0 Client ID');
console.log('3. Edit it and set Authorized redirect URIs to:');
console.log('   http://localhost:3000/api/auth/callback/google');
console.log('4. Save changes\n');

console.log('Fix 2: OAuth Consent Screen');
console.log('1. Visit: https://console.cloud.google.com/apis/credentials/consent');
console.log('2. Add your email to "Test users"');
console.log('3. Make sure status is "Testing" or "Published"\n');

console.log('Fix 3: Enable APIs');
console.log('1. Visit: https://console.cloud.google.com/apis/library');
console.log('2. Search and enable "Google+ API"');
console.log('3. Search and enable "People API"\n');

console.log('🧪 **TESTING STEPS**:\n');

console.log('1. After making changes, restart your app:');
console.log('   npm run dev\n');

console.log('2. Test these URLs:');
console.log('   http://localhost:3000/api/auth/providers');
console.log('   http://localhost:3000/api/auth/signin/google\n');

console.log('3. If still not working, use manual login:');
console.log('   Visit: http://localhost:3000/auth/signin');
console.log('   Click "Test as Admin" button\n');

console.log('💡 **ALTERNATIVE SOLUTION**:\n');
console.log('The manual test login works perfectly for development.');
console.log('You can continue using it while Google OAuth is being fixed.');
console.log('All role-based features work identically with manual login.\n');

console.log('🎯 **NEXT STEP**:');
console.log('1. Visit Google Cloud Console and fix redirect URI');
console.log('2. OR use manual test login to continue development');
console.log('3. Report back what you find!');