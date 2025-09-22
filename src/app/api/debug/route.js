import { NextResponse } from 'next/server';

export async function GET() {
  // Only allow in development or with a debug key
  const isDev = process.env.NODE_ENV === 'development';

  if (!isDev) {
    return NextResponse.json({ error: 'Debug endpoint disabled in production' }, { status: 403 });
  }

  const envCheck = {
    NODE_ENV: process.env.NODE_ENV,
    MONGODB_URI: process.env.MONGODB_URI ? 'SET' : 'NOT SET',
    JWT_SECRET: process.env.JWT_SECRET ? 'SET' : 'NOT SET',
    NEXT_PUBLIC_APP_URL: process.env.NEXT_PUBLIC_APP_URL,
    timestamp: new Date().toISOString(),
    platform: process.platform,
    nodeVersion: process.version
  };

  return NextResponse.json({
    message: 'Environment check',
    environment: envCheck,
    publicEnvVars: Object.keys(process.env)
      .filter(key => key.startsWith('NEXT_PUBLIC_'))
      .reduce((acc, key) => {
        acc[key] = process.env[key];
        return acc;
      }, {})
  });
}