import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import User from '@/models/User';
import jwt from 'jsonwebtoken';

// Test login endpoint for development
export async function POST(request) {
  try {
    await connectDB();
    
    const { role = 'admin' } = await request.json();
    
    // Create or find test user
    let testUser = await User.findOne({ email: 'test@admin.com' });
    
    if (!testUser) {
      testUser = new User({
        username: 'testadmin',
        name: 'Test Admin User',
        email: 'test@admin.com',
        password: 'testpassword123', // Will be hashed automatically
        role: role,
        provider: 'local',
        isActive: true,
        isEmailVerified: true
      });
      
      await testUser.save();
      console.log('Created test user:', testUser.email);
    } else {
      // Update role if different
      if (testUser.role !== role) {
        testUser.role = role;
        await testUser.save();
      }
    }
    
    // Update last login
    testUser.lastLogin = new Date();
    await testUser.save();
    
    // Generate JWT token
    const token = jwt.sign(
      { userId: testUser._id },
      process.env.JWT_SECRET || 'your-secret-key',
      { expiresIn: '7d' }
    );
    
    const userResponse = {
      id: testUser._id,
      username: testUser.username,
      name: testUser.name,
      email: testUser.email,
      role: testUser.role,
      provider: testUser.provider,
      isActive: testUser.isActive
    };
    
    return NextResponse.json({
      status: 200,
      data: {
        user: userResponse,
        token: token
      },
      msg: `Test login successful as ${role}`
    });
    
  } catch (error) {
    console.error('Test login error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Test login failed',
      error: error.message
    }, { status: 500 });
  }
}

// GET endpoint to show available test users
export async function GET() {
  return NextResponse.json({
    status: 200,
    msg: 'Test login endpoint',
    availableRoles: ['admin', 'mod', 'customer'],
    usage: 'POST to this endpoint with { "role": "admin|mod|customer" }',
    testCredentials: {
      email: 'test@admin.com',
      password: 'testpassword123'
    }
  });
}