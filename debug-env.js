// Debug script to check environment variables
console.log('=== Environment Variables Debug ===');
console.log('NODE_ENV:', process.env.NODE_ENV);
console.log('MONGODB_URI:', process.env.MONGODB_URI ? 'SET (hidden)' : 'NOT SET');
console.log('JWT_SECRET:', process.env.JWT_SECRET ? 'SET (hidden)' : 'NOT SET');
console.log('NEXT_PUBLIC_APP_URL:', process.env.NEXT_PUBLIC_APP_URL);
console.log('All env vars starting with NEXT_PUBLIC_:');
Object.keys(process.env).filter(key => key.startsWith('NEXT_PUBLIC_')).forEach(key => {
  console.log(`  ${key}:`, process.env[key]);
});
console.log('=== End Debug ===');