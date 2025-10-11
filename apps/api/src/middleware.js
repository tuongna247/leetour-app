import { NextResponse } from 'next/server';

export function middleware(request) {
  const response = NextResponse.next();

  // Allow requests from admin and frontend
  const allowedOrigins = [
    process.env.ADMIN_URL || 'http://localhost:3000',
    process.env.FRONTEND_URL || 'http://localhost:3002',
    'http://localhost:3000',
    'http://localhost:3002'
  ];

  const origin = request.headers.get('origin');

  if (origin && allowedOrigins.includes(origin)) {
    response.headers.set('Access-Control-Allow-Origin', origin);
  }

  response.headers.set('Access-Control-Allow-Credentials', 'true');
  response.headers.set('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, PATCH, OPTIONS');
  response.headers.set('Access-Control-Allow-Headers', 'Content-Type, Authorization, X-Requested-With');

  // Handle preflight requests
  if (request.method === 'OPTIONS') {
    return new NextResponse(null, {
      status: 200,
      headers: response.headers
    });
  }

  return response;
}

export const config = {
  matcher: '/api/:path*',
};
