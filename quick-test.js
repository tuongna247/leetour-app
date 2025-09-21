#!/usr/bin/env node

// Quick test to check if the app is running
const http = require('http');

console.log('🏃 Quick App Status Check\n');

function checkApp() {
  return new Promise((resolve) => {
    const req = http.get('http://localhost:3000', (res) => {
      console.log('✅ App is running on http://localhost:3000');
      console.log(`Status: ${res.statusCode}`);
      resolve(true);
    });
    
    req.on('error', (error) => {
      console.log('❌ App is not running');
      console.log('Please start your app with: npm run dev');
      resolve(false);
    });
    
    req.setTimeout(3000, () => {
      console.log('⏰ App is not responding');
      console.log('Please start your app with: npm run dev');
      req.destroy();
      resolve(false);
    });
  });
}

async function main() {
  const isRunning = await checkApp();
  
  if (isRunning) {
    console.log('\n🎯 Ready for testing!');
    console.log('Next steps:');
    console.log('1. Visit: http://localhost:3000');
    console.log('2. Test manual login buttons');
    console.log('3. Run: node automated-auth-test.js (for API tests)');
    console.log('4. Run: node test-auth-flow.js (for complete guide)');
  } else {
    console.log('\n🚀 Start your app first:');
    console.log('npm run dev');
    console.log('\nThen run this test again.');
  }
}

main().catch(console.error);