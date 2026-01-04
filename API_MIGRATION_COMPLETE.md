# API Migration Complete: Admin Routes Consolidated

## Overview

All API routes have been moved from `apps/admin/src/app/api/admin` to `apps/api/src/app/api`. The admin Next.js app now acts as a **frontend-only** application that calls the centralized API server.

## New Architecture

```
apps/
├── api/                    # Centralized API Server (Port 3001)
│   └── src/app/api/
│       ├── tours/          # Tours API (public + admin)
│       ├── users/          # Users API (admin only)
│       ├── suppliers/      # Suppliers API (admin only)
│       ├── bookings/       # Bookings API
│       ├── reviews/        # Reviews API
│       └── ...
│
├── admin/                  # Admin Panel Frontend (Port 3000)
│   └── src/app/
│       └── (DashboardLayout)/  # UI only, calls apps/api
│
└── frontend/               # Public Frontend (Port 3002)
    └── src/app/            # UI only, calls apps/api
```

## API Endpoints

All API endpoints are now served from `http://localhost:3001/api/*`

### Tours API

| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/tours` | Public | List active tours |
| GET | `/api/tours?admin=true` | Admin | List all tours with stats |
| GET | `/api/tours/:id` | Public | Get active tour by ID/slug |
| GET | `/api/tours/:id?admin=true` | Admin | Get any tour (including inactive) |
| POST | `/api/tours` | Admin | Create new tour |
| PUT | `/api/tours/:id` | Admin | Update tour |
| DELETE | `/api/tours/:id` | Admin | Soft delete tour |
| GET/POST | `/api/tours/:id/images` | Admin | Manage tour images |

### Users API

| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/users` | Admin | List users |
| GET | `/api/users/:id` | Admin | Get user by ID |
| PUT | `/api/users/:id` | Admin | Update user |
| DELETE | `/api/users/:id` | Admin | Delete user |

### Suppliers API

| Method | Endpoint | Access | Description |
|--------|----------|--------|-------------|
| GET | `/api/suppliers` | Admin | List suppliers |
| POST | `/api/suppliers` | Admin | Create supplier |
| PUT | `/api/suppliers/:id` | Admin | Update supplier |

## Admin Panel API Calls

The admin panel (`apps/admin`) now makes API calls to `apps/api`:

```javascript
// Example: Admin fetching tours
const response = await authenticatedFetch('/api/tours?admin=true&status=active');

// The authenticatedFetch helper adds:
// - Authorization header with JWT token
// - Calls http://localhost:3001/api/tours
```

### Environment Variables

Make sure `apps/admin/.env.local` has:

```env
NEXT_PUBLIC_API_URL=http://localhost:3001
```

## Migration Benefits

✅ **Single Source of Truth**: All API logic in one place (`apps/api`)
✅ **No Duplication**: Routes not duplicated across apps
✅ **Better Security**: Centralized authentication and authorization
✅ **Easier Testing**: Test one API server, not multiple
✅ **Scalability**: API can be deployed independently
✅ **Clear Separation**: Frontend (UI) vs Backend (API)

## What Changed

### Before (Old Structure)
```
apps/admin/src/app/api/tours/route.js    # Admin tours API
apps/api/src/app/api/tours/route.js            # Public tours API
```

### After (New Structure)
```
apps/api/src/app/api/tours/route.js            # Both public + admin
```

The same file handles both:
- Public requests (no auth, only active tours)
- Admin requests (`?admin=true`, requires auth, all tours)

## Files Copied

The following routes were copied from `apps/admin/src/app/api/admin` to `apps/api/src/app/api`:

- ✅ `suppliers/route.js`
- ✅ `users/route.js`
- ✅ `users/[id]/route.js`
- ✅ `tours/[id]/images/route.js`

## Next Steps

1. **Update Admin Panel**: Change all `/api/admin/*` calls to `/api/*` with `?admin=true` where needed
2. **Test Authentication**: Ensure all admin endpoints require proper auth
3. **Delete Old Routes**: Remove `apps/admin/src/app/api/admin` folder
4. **Delete Old API Routes**: Remove `apps/api/src/app/api/admin` folder (already merged)

## Testing

Start all servers:

```bash
# Terminal 1: API Server
cd apps/api
npm run dev    # Port 3001

# Terminal 2: Admin Panel
cd apps/admin
npm run dev    # Port 3000

# Terminal 3: Frontend
cd apps/frontend
npm run dev    # Port 3002
```

Test endpoints:
```bash
# Public API (no auth)
curl http://localhost:3001/api/tours?limit=5

# Admin API (requires auth token)
curl -H "Authorization: Bearer YOUR_TOKEN" \
  http://localhost:3001/api/tours?admin=true&status=active
```

## Migration Status

- ✅ Tours API migrated
- ✅ Users API copied
- ✅ Suppliers API copied
- ✅ Tour images API copied
- ⏳ Admin panel updates (in progress)
- ⏳ Old folder cleanup (pending)
