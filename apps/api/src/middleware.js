import { NextResponse } from 'next/server';

export function middleware(request) {
  const response = NextResponse.next();

  // Build allowed origins list
  const allowedOrigins = [
    // Development origins
    'http://localhost:3000',
    'http://localhost:3002',
    'http://localhost:3003'
  ];

  // Add production origins from environment variables
  if (process.env.ADMIN_URL) {
    allowedOrigins.push(process.env.ADMIN_URL);
  }
  if (process.env.FRONTEND_URL) {
    allowedOrigins.push(process.env.FRONTEND_URL);
  }

  // Support comma-separated list of additional allowed origins
  if (process.env.ALLOWED_ORIGINS) {
    const additionalOrigins = process.env.ALLOWED_ORIGINS.split(',').map(o => o.trim());
    allowedOrigins.push(...additionalOrigins);
  }

  const origin = request.headers.get('origin');

  // Check if origin is allowed
  if (origin && allowedOrigins.includes(origin)) {
    response.headers.set('Access-Control-Allow-Origin', origin);
  } else if (origin && process.env.NODE_ENV !== 'production') {
    // In development, allow any origin
    response.headers.set('Access-Control-Allow-Origin', origin);
  }

  // CORS headers
  response.headers.set('Access-Control-Allow-Credentials', 'true');
  response.headers.set('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, PATCH, OPTIONS');
  response.headers.set('Access-Control-Allow-Headers', 'Content-Type, Authorization, X-Requested-With, X-CSRF-Token');
  response.headers.set('Access-Control-Max-Age', '86400'); // 24 hours

  // Security headers
  response.headers.set('X-Content-Type-Options', 'nosniff');
  response.headers.set('X-Frame-Options', 'DENY');
  response.headers.set('X-XSS-Protection', '1; mode=block');
  response.headers.set('Referrer-Policy', 'strict-origin-when-cross-origin');

  // Handle preflight requests
  if (request.method === 'OPTIONS') {
    return new NextResponse(null, {
      status: 204,
      headers: response.headers
    });
  }

  return response;
}

export const config = {
  matcher: '/api/:path*',
};
