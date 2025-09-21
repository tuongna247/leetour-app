import { NextResponse } from 'next/server';
import jwt from 'jsonwebtoken';
import connectDB from '@/lib/mongodb';
import User from '@/models/User';

export async function POST(request) {
  try {
    console.log('=== Login API called ===');
    console.log('Environment:', process.env.NODE_ENV);
    console.log('MongoDB URI exists:', !!process.env.MONGODB_URI);
    console.log('JWT Secret exists:', !!process.env.JWT_SECRET);
    
    await connectDB();
    console.log('=== Database connected ===');
    
    const { username, password } = await request.json();
    console.log('Login attempt for:', username);

    if (!username || !password) {
      console.log('Missing credentials');
      return NextResponse.json({
        status: 400,
        msg: 'Username and password are required'
      }, { status: 400 });
    }

    // Find user by username or email
    console.log('Searching for user...');
    const user = await User.findOne({
      $or: [{ username }, { email: username }],
      isActive: true
    });

    console.log('User found:', !!user);
    if (user) {
      console.log('User details:', {
        id: user._id,
        username: user.username,
        email: user.email,
        role: user.role,
        hasPassword: !!user.password
      });
    }

    if (!user) {
      console.log('User not found or inactive');
      return NextResponse.json({
        status: 401,
        msg: 'Invalid credentials'
      }, { status: 401 });
    }

    // Check password
    console.log('Checking password...');
    const isMatch = await user.comparePassword(password);
    console.log('Password match:', isMatch);
    
    if (!isMatch) {
      console.log('Password mismatch');
      return NextResponse.json({
        status: 401,
        msg: 'Invalid credentials'
      }, { status: 401 });
    }

    // Update last login
    console.log('Updating last login...');
    user.lastLogin = new Date();
    await user.save();
    console.log('Last login updated');

    // Generate JWT token
    const payload = {
      userId: user._id,
      username: user.username,
      role: user.role
    };

    const jwtSecret = process.env.JWT_SECRET || 'your-secret-key';
    console.log('Generating token with secret length:', jwtSecret.length);
    
    const token = jwt.sign(payload, jwtSecret, {
      expiresIn: '24h'
    });

    console.log('Token generated successfully');

    return NextResponse.json({
      status: 200,
      msg: 'Login successful',
      data: {
        token,
        user: {
          id: user._id,
          username: user.username,
          name: user.name,
          email: user.email,
          role: user.role,
          lastLogin: user.lastLogin
        }
      }
    });

  } catch (error) {
    console.error('=== Login error ===');
    console.error('Error message:', error.message);
    console.error('Error stack:', error.stack);
    console.error('Error name:', error.name);
    
    return NextResponse.json({
      status: 500,
      msg: 'Server error',
      error: process.env.NODE_ENV === 'development' ? error.message : 'Internal server error',
      details: process.env.NODE_ENV === 'development' ? {
        name: error.name,
        stack: error.stack
      } : undefined
    }, { status: 500 });
  }
}