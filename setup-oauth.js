#!/usr/bin/env node

const fs = require('fs');
const path = require('path');
const crypto = require('crypto');

console.log('🔐 OAuth Setup Wizard for LeetTour App\n');

// Generate secure secret
const nextAuthSecret = crypto.randomBytes(32).toString('hex');

// Template for .env.local
const envTemplate = `# LeetTour OAuth Configuration
# Generated on ${new Date().toISOString()}

# Database Configuration
MONGODB_URI=mongodb+srv://username:password@cluster.mongodb.net/leetour

# NextAuth Configuration
NEXTAUTH_SECRET=${nextAuthSecret}
NEXTAUTH_URL=http://localhost:3000

# Google OAuth - Get these from https://console.cloud.google.com/
# 1. Create project → APIs & Services → Credentials → OAuth 2.0 Client IDs
# 2. Add redirect URI: http://localhost:3000/api/auth/callback/google
GOOGLE_CLIENT_ID=your_google_client_id_here.apps.googleusercontent.com
GOOGLE_CLIENT_SECRET=your_google_client_secret_here

# Facebook OAuth - Get these from https://developers.facebook.com/
# 1. Create App → Add Facebook Login → Settings
# 2. Add redirect URI: http://localhost:3000/api/auth/callback/facebook
FACEBOOK_CLIENT_ID=your_facebook_app_id_here
FACEBOOK_CLIENT_SECRET=your_facebook_app_secret_here

# JWT for local authentication (existing)
JWT_SECRET=your_existing_jwt_secret_or_generate_new_one

# Application Environment
NODE_ENV=development`;

// Write .env.local file
const envPath = path.join(__dirname, '.env.local');
if (!fs.existsSync(envPath)) {
  fs.writeFileSync(envPath, envTemplate);
  console.log('✅ Created .env.local file with template');
} else {
  console.log('⚠️  .env.local already exists, not overwriting');
}

console.log('\n📋 Next Steps:');
console.log('1. 🔗 Set up Google OAuth:');
console.log('   → Go to: https://console.cloud.google.com/');
console.log('   → Create project → APIs & Services → Credentials');
console.log('   → Create OAuth 2.0 Client ID');
console.log('   → Add redirect URI: http://localhost:3000/api/auth/callback/google');
console.log('   → Copy Client ID and Secret to .env.local');

console.log('\n2. 📘 Set up Facebook OAuth:');
console.log('   → Go to: https://developers.facebook.com/');
console.log('   → Create App → Add Facebook Login');
console.log('   → Add redirect URI: http://localhost:3000/api/auth/callback/facebook');
console.log('   → Copy App ID and Secret to .env.local');

console.log('\n3. 🗄️ Update your MongoDB URI in .env.local');

console.log('\n4. 🚀 Start the application:');
console.log('   npm run dev');

console.log('\n5. 🧪 Test authentication:');
console.log('   → Visit: http://localhost:3000');
console.log('   → Try Google and Facebook sign-in buttons');

console.log('\n📚 For detailed instructions, see: OAUTH_SETUP.md');
console.log('🔧 Generated NEXTAUTH_SECRET:', nextAuthSecret);