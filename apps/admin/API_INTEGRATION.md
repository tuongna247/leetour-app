# API Integration Guide

The admin application now uses a separate API service.

## Configuration

### Environment Variables

Update your `.env` or `.env.local` file with the API URL:

```env
NEXT_PUBLIC_API_URL=http://localhost:3001
```

For production:
```env
NEXT_PUBLIC_API_URL=https://api.yourdomain.com
```

## Making API Requests

### Example: Fetching Tours

```javascript
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

async function getTours() {
  const response = await fetch(`${API_URL}/api/admin/tours`, {
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    }
  });

  return await response.json();
}
```

### Example: Creating a Tour

```javascript
async function createTour(tourData) {
  const response = await fetch(`${API_URL}/api/admin/tours`, {
    method: 'POST',
    headers: {
      'Authorization': `Bearer ${token}`,
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(tourData)
  });

  return await response.json();
}
```

## Authentication

The API uses JWT tokens for authentication. After login, store the token and include it in all subsequent requests:

```javascript
// Login
const loginResponse = await fetch(`${API_URL}/api/auth/login`, {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({ username, password })
});

const { data } = await loginResponse.json();
const token = data.token;

// Store token (localStorage, sessionStorage, or cookies)
localStorage.setItem('token', token);

// Use token in requests
const response = await fetch(`${API_URL}/api/admin/tours`, {
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

## API Helper Utility

Create a utility file for API calls:

```javascript
// src/lib/api.js
const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';

export async function apiRequest(endpoint, options = {}) {
  const token = localStorage.getItem('token');

  const defaultHeaders = {
    'Content-Type': 'application/json',
    ...(token && { 'Authorization': `Bearer ${token}` })
  };

  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      ...defaultHeaders,
      ...options.headers
    }
  });

  if (!response.ok) {
    throw new Error(`API Error: ${response.statusText}`);
  }

  return await response.json();
}

// Usage
import { apiRequest } from '@/lib/api';

const tours = await apiRequest('/api/admin/tours');
const newTour = await apiRequest('/api/admin/tours', {
  method: 'POST',
  body: JSON.stringify(tourData)
});
```

## Running Both Apps

You need to run both the admin app and the API app:

**Terminal 1 - API Server:**
```bash
cd apps/api
npm run dev
# Runs on http://localhost:3001
```

**Terminal 2 - Admin App:**
```bash
cd apps/admin
npm run dev
# Runs on http://localhost:3000
```

## Troubleshooting

### CORS Errors

If you see CORS errors, ensure:
1. The API server is running on port 3001
2. The `ADMIN_URL` environment variable in the API app is set correctly
3. The middleware is configured properly in `apps/api/src/middleware.js`

### Authentication Issues

If authentication fails:
1. Check that the JWT_SECRET matches between apps
2. Verify the token is being sent in the Authorization header
3. Check that the token hasn't expired (default: 24 hours)

### Connection Refused

If you get "Connection refused" errors:
1. Ensure the API server is running
2. Check the `NEXT_PUBLIC_API_URL` is set correctly
3. Verify the port 3001 is not blocked by firewall
