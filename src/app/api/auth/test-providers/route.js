import { NextResponse } from 'next/server';

// Test endpoint to check NextAuth providers
export async function GET() {
  try {
    // Check if we can access NextAuth
    const { getProviders } = await import('next-auth/react');
    
    console.log('Environment variables check:');
    console.log('GOOGLE_CLIENT_ID:', process.env.GOOGLE_CLIENT_ID ? 'Set' : 'Missing');
    console.log('GOOGLE_CLIENT_SECRET:', process.env.GOOGLE_CLIENT_SECRET ? 'Set' : 'Missing');
    console.log('NEXTAUTH_SECRET:', process.env.NEXTAUTH_SECRET ? 'Set' : 'Missing');
    console.log('NEXTAUTH_URL:', process.env.NEXTAUTH_URL);
    
    // Try to import our NextAuth configuration
    let authOptions;
    try {
      const configModule = await import('../[...nextauth]/route.js');
      authOptions = configModule.authOptions;
      console.log('NextAuth config loaded successfully');
    } catch (error) {
      console.error('Error loading NextAuth config:', error);
    }
    
    return NextResponse.json({
      status: 200,
      message: 'NextAuth test endpoint',
      environment: {
        GOOGLE_CLIENT_ID: process.env.GOOGLE_CLIENT_ID ? 'Set' : 'Missing',
        GOOGLE_CLIENT_SECRET: process.env.GOOGLE_CLIENT_SECRET ? 'Set' : 'Missing',
        NEXTAUTH_SECRET: process.env.NEXTAUTH_SECRET ? 'Set' : 'Missing',
        NEXTAUTH_URL: process.env.NEXTAUTH_URL,
        NEXTAUTH_DEBUG: process.env.NEXTAUTH_DEBUG
      },
      providers: authOptions ? authOptions.providers.length : 'Config not loaded',
      instructions: {
        'Test Google OAuth': 'Visit /api/auth/signin/google',
        'Check providers': 'Visit /api/auth/providers',
        'Check session': 'Visit /api/auth/session'
      }
    });
    
  } catch (error) {
    console.error('Test providers error:', error);
    return NextResponse.json({
      status: 500,
      error: error.message,
      message: 'Failed to test providers'
    }, { status: 500 });
  }
}