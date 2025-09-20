const mongoose = require('mongoose');
require('dotenv').config({ path: './.env.local' });

const MONGODB_URI = 'mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0';

console.log('Testing MongoDB connection...');
console.log('MongoDB URI:', MONGODB_URI ? 'Set' : 'Not set');

async function testConnection() {
  try {
    await mongoose.connect(MONGODB_URI, {
      bufferCommands: false,
    });
    console.log('✅ MongoDB connection successful!');
    console.log('Connected to:', mongoose.connection.host);
    console.log('Database:', mongoose.connection.name);
    
    // Test basic operation
    const collections = await mongoose.connection.db.listCollections().toArray();
    console.log('Available collections:', collections.map(c => c.name));
    
  } catch (error) {
    console.log('❌ MongoDB connection failed:');
    console.error('Error:', error.message);
    
    if (error.message.includes('authentication')) {
      console.log('\n🔑 Authentication issue - check credentials');
    } else if (error.message.includes('network')) {
      console.log('\n🌐 Network issue - check internet connection');
    } else if (error.message.includes('timeout')) {
      console.log('\n⏰ Connection timeout - check network or MongoDB Atlas settings');
    }
  } finally {
    await mongoose.disconnect();
    console.log('Connection closed.');
  }
}

testConnection();