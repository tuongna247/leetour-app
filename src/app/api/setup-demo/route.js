import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import User from '@/models/User';

export async function POST(request) {
  try {
    await connectDB();
    
    // Create demo users if they don't exist
    const demoUsers = [
      {
        username: 'admin',
        email: 'admin@demo.com',
        password: 'admin123',
        role: 'admin'
      },
      {
        username: 'user',
        email: 'user@demo.com', 
        password: 'user123',
        role: 'user'
      },
      {
        username: 'google_user',
        email: 'google@demo.com',
        password: 'demo123',
        role: 'user'
      },
      {
        username: 'facebook_user',
        email: 'facebook@demo.com',
        password: 'demo123', 
        role: 'user'
      }
    ];

    const results = [];
    
    for (const userData of demoUsers) {
      try {
        // Check if user already exists
        const existingUser = await User.findOne({
          $or: [{ email: userData.email }, { username: userData.username }]
        });

        if (!existingUser) {
          const user = new User(userData);
          await user.save();
          results.push(`✅ Created user: ${userData.username}`);
        } else {
          results.push(`ℹ️ User already exists: ${userData.username}`);
        }
      } catch (error) {
        results.push(`❌ Failed to create ${userData.username}: ${error.message}`);
      }
    }

    return NextResponse.json({
      status: 200,
      msg: 'Demo setup completed',
      results
    });

  } catch (error) {
    console.error('Setup demo error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to setup demo users',
      error: error.message
    }, { status: 500 });
  }
}