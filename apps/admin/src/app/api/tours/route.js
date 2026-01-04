import { NextResponse } from 'next/server';

// Proxy all tour requests to the centralized API server
const API_SERVER_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

export async function GET(request) {
  try {
    const { searchParams } = new URL(request.url);
    const queryString = searchParams.toString();

    // Forward request to API server
    const apiUrl = `${API_SERVER_URL}/api/tours${queryString ? `?${queryString}` : ''}`;

    // Get auth token from request headers
    const authHeader = request.headers.get('authorization');

    const headers = {
      'Content-Type': 'application/json',
    };

    if (authHeader) {
      headers['Authorization'] = authHeader;
    }

    const response = await fetch(apiUrl, {
      method: 'GET',
      headers,
    });

    const data = await response.json();

    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error('Proxy error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to fetch tours from API server',
      error: error.message
    }, { status: 500 });
  }
}

export async function POST(request) {
  try {
    const body = await request.json();

    // Forward request to API server
    const apiUrl = `${API_SERVER_URL}/api/tours`;

    // Get auth token from request headers
    const authHeader = request.headers.get('authorization');

    const headers = {
      'Content-Type': 'application/json',
    };

    if (authHeader) {
      headers['Authorization'] = authHeader;
    }

    const response = await fetch(apiUrl, {
      method: 'POST',
      headers,
      body: JSON.stringify(body),
    });

    const data = await response.json();

    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error('Proxy error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to create tour on API server',
      error: error.message
    }, { status: 500 });
  }
}
