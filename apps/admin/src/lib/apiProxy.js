import { NextResponse } from 'next/server';

// Centralized API server URL
const API_SERVER_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

/**
 * Generic proxy function to forward requests to the centralized API server
 * @param {Request} request - The incoming request
 * @param {string} path - The API path to forward to (e.g., '/api/users')
 * @param {Object} options - Additional options
 * @param {string} options.method - HTTP method (GET, POST, PUT, DELETE, etc.)
 * @param {Object} options.body - Request body (for POST, PUT, PATCH)
 * @returns {Promise<NextResponse>} The API response
 */
export async function proxyToAPI(request, path, options = {}) {
  try {
    const method = options.method || request.method;
    const apiUrl = `${API_SERVER_URL}${path}`;

    // Get auth token from request headers
    const authHeader = request.headers.get('authorization');

    const headers = {
      'Content-Type': 'application/json',
    };

    if (authHeader) {
      headers['Authorization'] = authHeader;
    }

    const fetchOptions = {
      method,
      headers,
    };

    // Add body for non-GET requests
    if (options.body) {
      fetchOptions.body = JSON.stringify(options.body);
    } else if (method !== 'GET' && method !== 'DELETE') {
      // Try to read body from request
      try {
        const body = await request.json();
        if (body) {
          fetchOptions.body = JSON.stringify(body);
        }
      } catch (e) {
        // No body or invalid JSON
      }
    }

    const response = await fetch(apiUrl, fetchOptions);
    const data = await response.json();

    return NextResponse.json(data, { status: response.status });
  } catch (error) {
    console.error('API Proxy error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to proxy request to API server',
      error: error.message
    }, { status: 500 });
  }
}

/**
 * Create a simple proxy route handler
 * @param {string} basePath - The base API path (e.g., '/api/users')
 * @returns {Object} Object with GET, POST, PUT, DELETE handlers
 */
export function createProxyHandlers(basePath) {
  return {
    async GET(request, context) {
      const { params } = context || {};
      const { searchParams } = new URL(request.url);
      const queryString = searchParams.toString();

      // Build path with params if available
      let path = basePath;
      if (params?.id) {
        path = `${basePath}/${params.id}`;
      }
      if (queryString) {
        path = `${path}?${queryString}`;
      }

      return proxyToAPI(request, path);
    },

    async POST(request, context) {
      const { params } = context || {};
      let path = basePath;
      if (params?.id) {
        path = `${basePath}/${params.id}`;
      }

      return proxyToAPI(request, path, { method: 'POST' });
    },

    async PUT(request, context) {
      const { params } = context || {};
      let path = basePath;
      if (params?.id) {
        path = `${basePath}/${params.id}`;
      }

      return proxyToAPI(request, path, { method: 'PUT' });
    },

    async PATCH(request, context) {
      const { params } = context || {};
      let path = basePath;
      if (params?.id) {
        path = `${basePath}/${params.id}`;
      }

      return proxyToAPI(request, path, { method: 'PATCH' });
    },

    async DELETE(request, context) {
      const { params } = context || {};
      let path = basePath;
      if (params?.id) {
        path = `${basePath}/${params.id}`;
      }

      return proxyToAPI(request, path, { method: 'DELETE' });
    }
  };
}
