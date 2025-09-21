#!/usr/bin/env node

// Automated test script for authentication APIs
const http = require('http');

const BASE_URL = 'http://localhost:3000';

console.log('🔧 Automated Authentication API Tests\n');

// Test function for HTTP requests
function testEndpoint(path, expectedStatus = 200, description) {
  return new Promise((resolve) => {
    const url = `${BASE_URL}${path}`;
    console.log(`Testing: ${description}`);
    console.log(`URL: ${url}`);
    
    const req = http.get(url, (res) => {
      const status = res.statusCode;
      const success = status === expectedStatus;
      
      console.log(`Status: ${status} ${success ? '✅' : '❌'}`);
      
      let data = '';
      res.on('data', chunk => data += chunk);
      res.on('end', () => {
        try {
          const json = JSON.parse(data);
          console.log(`Response: ${JSON.stringify(json, null, 2).substring(0, 200)}...`);
        } catch (e) {
          console.log(`Response: ${data.substring(0, 100)}...`);
        }
        console.log('---\n');
        resolve({ success, status, data });
      });
    });
    
    req.on('error', (error) => {
      console.log(`Error: ${error.message} ❌`);
      console.log('---\n');
      resolve({ success: false, error: error.message });
    });
    
    req.setTimeout(5000, () => {
      console.log('Timeout: Request took too long ⏰');
      console.log('---\n');
      req.destroy();
      resolve({ success: false, error: 'Timeout' });
    });
  });
}

// Test admin login
async function testAdminLogin() {
  return new Promise((resolve) => {
    const postData = JSON.stringify({ role: 'admin' });
    
    const options = {
      hostname: 'localhost',
      port: 3000,
      path: '/api/auth/test-login',
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Content-Length': Buffer.byteLength(postData)
      }
    };
    
    console.log('Testing: Admin Test Login API');
    console.log('URL: POST /api/auth/test-login');
    
    const req = http.request(options, (res) => {
      const status = res.statusCode;
      console.log(`Status: ${status} ${status === 200 ? '✅' : '❌'}`);
      
      let data = '';
      res.on('data', chunk => data += chunk);
      res.on('end', () => {
        try {
          const json = JSON.parse(data);
          console.log(`User: ${json.data?.user?.name} (${json.data?.user?.role})`);
          console.log(`Token: ${json.data?.token ? 'Generated ✅' : 'Missing ❌'}`);
        } catch (e) {
          console.log(`Response: ${data.substring(0, 100)}...`);
        }
        console.log('---\n');
        resolve({ success: status === 200, data });
      });
    });
    
    req.on('error', (error) => {
      console.log(`Error: ${error.message} ❌`);
      resolve({ success: false, error: error.message });
    });
    
    req.write(postData);
    req.end();
  });
}

async function runTests() {
  console.log('🚀 Starting automated tests...\n');
  console.log('Make sure your app is running with: npm run dev\n');
  
  const tests = [
    // Test basic endpoints
    { path: '/api/auth/test-providers', status: 200, desc: 'NextAuth Test Providers Endpoint' },
    { path: '/api/auth/providers', status: 200, desc: 'NextAuth Providers Endpoint' },
    { path: '/api/auth/session', status: 200, desc: 'NextAuth Session Endpoint' },
    
    // Test protected routes (should redirect or return 401)
    { path: '/api/admin/tours', status: 401, desc: 'Protected Admin Tours API (should be unauthorized)' },
    
    // Test root page (should redirect)
    { path: '/', status: 200, desc: 'Root Page (should load)' },
  ];
  
  // Run endpoint tests
  for (const test of tests) {
    await testEndpoint(test.path, test.status, test.desc);
    await new Promise(resolve => setTimeout(resolve, 500)); // Brief pause between tests
  }
  
  // Test login API
  await testAdminLogin();
  
  console.log('📋 Manual Tests Still Needed:');
  console.log('1. Visit http://localhost:3000 in browser');
  console.log('2. Click manual test login buttons');
  console.log('3. Test role-based navigation');
  console.log('4. Test logout functionality');
  console.log('5. Test protected routes in browser');
  
  console.log('\n✅ Automated tests complete!');
  console.log('Continue with manual testing in your browser.');
}

// Handle process exit
process.on('unhandledRejection', (error) => {
  console.error('Test error:', error);
  process.exit(1);
});

runTests().catch(console.error);