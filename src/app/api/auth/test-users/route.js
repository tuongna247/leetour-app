import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import User from '@/models/User';

export async function GET(request) {
  try {
    console.log('=== Test Users API called ===');
    console.log('Environment:', process.env.NODE_ENV);
    console.log('MongoDB URI exists:', !!process.env.MONGODB_URI);
    
    await connectDB();
    console.log('=== Database connected ===');
    
    // Get all users (without passwords)
    const users = await User.find({}, '-password').limit(10);
    console.log('Users found:', users.length);
    
    const userSummary = users.map(user => ({
      id: user._id,
      username: user.username,
      email: user.email,
      role: user.role,
      isActive: user.isActive,
      createdAt: user.createdAt
    }));

    return NextResponse.json({
      status: 200,
      msg: 'Users retrieved successfully',
      data: {
        userCount: users.length,
        users: userSummary
      }
    });

  } catch (error) {
    console.error('=== Test Users error ===');
    console.error('Error message:', error.message);
    console.error('Error stack:', error.stack);
    
    return NextResponse.json({
      status: 500,
      msg: 'Server error',
      error: error.message,
      details: {
        name: error.name,
        stack: error.stack
      }
    }, { status: 500 });
  }
}

export async function POST(request) {
  try {
    console.log('=== Test Login API called ===');
    
    await connectDB();
    console.log('=== Database connected ===');
    
    const { email } = await request.json();
    console.log('Testing login for email:', email);
    
    if (!email) {
      return NextResponse.json({
        status: 400,
        msg: 'Email is required for testing'
      }, { status: 400 });
    }

    // Find user by email
    const user = await User.findOne({ email, isActive: true });
    console.log('User found:', !!user);
    
    if (user) {
      console.log('User details:', {
        id: user._id,
        username: user.username,
        email: user.email,
        role: user.role,
        hasPassword: !!user.password,
        passwordLength: user.password?.length || 0
      });
    }

    return NextResponse.json({
      status: 200,
      msg: 'Test completed',
      data: {
        userExists: !!user,
        userDetails: user ? {
          id: user._id,
          username: user.username,
          email: user.email,
          role: user.role,
          hasPassword: !!user.password
        } : null
      }
    });

  } catch (error) {
    console.error('=== Test Login error ===');
    console.error('Error message:', error.message);
    
    return NextResponse.json({
      status: 500,
      msg: 'Server error',
      error: error.message
    }, { status: 500 });
  }
}