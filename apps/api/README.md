# LeeTour API

Backend API service for the LeeTour application.

## Overview

This is a standalone Next.js API application that handles all backend operations including:
- Authentication and authorization
- Tour management
- Booking management
- Payment processing
- Supplier management
- User management

## Getting Started

### Prerequisites

- Node.js 18+ installed
- MongoDB instance running

### Installation

1. Install dependencies:
```bash
npm install
```

2. Create a `.env` file based on `.env.example`:
```bash
cp .env.example .env
```

3. Update the environment variables in `.env`:
```
MONGODB_URI=mongodb://localhost:27017/leetour
JWT_SECRET=your-secret-key-here
NEXTAUTH_SECRET=your-nextauth-secret-here
NEXTAUTH_URL=http://localhost:3001
ADMIN_URL=http://localhost:3000
FRONTEND_URL=http://localhost:3002
```

### Running the Application

Development mode (runs on port 3001):
```bash
npm run dev
```

Production build:
```bash
npm run build
npm start
```

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/logout` - User logout
- `GET /api/auth/me` - Get current user

### Admin
- `GET /api/tours` - List all tours (admin)
- `POST /api/tours` - Create a new tour
- `GET /api/tours/[id]` - Get tour details
- `PUT /api/tours/[id]` - Update tour
- `DELETE /api/tours/[id]` - Delete tour
- `GET /api/admin/users` - List all users
- `GET /api/admin/suppliers` - List all suppliers

### Tours
- `GET /api/tours` - List all tours (public)
- `GET /api/tours/[id]` - Get tour details
- `GET /api/tours/[id]/reviews` - Get tour reviews

### Bookings
- `GET /api/bookings` - List bookings
- `POST /api/bookings` - Create a booking
- `GET /api/bookings/[id]` - Get booking details
- `PUT /api/bookings/[id]` - Update booking

### Payments
- `POST /api/payments` - Process payment

## Project Structure

```
apps/api/
├── src/
│   ├── app/
│   │   ├── api/           # API routes
│   │   ├── layout.js      # Root layout
│   │   └── page.js        # Home page
│   ├── lib/               # Libraries (mongodb, auth)
│   ├── models/            # Mongoose models
│   ├── utils/             # Utility functions
│   └── middleware.js      # CORS middleware
├── .env                   # Environment variables
├── .env.example           # Environment template
├── jsconfig.json          # JavaScript configuration
├── next.config.mjs        # Next.js configuration
├── package.json           # Dependencies
└── README.md              # This file
```

## CORS Configuration

The API is configured to accept requests from:
- Admin app: `http://localhost:3000`
- Frontend app: `http://localhost:3002`

Update the `ADMIN_URL` and `FRONTEND_URL` environment variables for production.

## Development Notes

- The API runs on port 3001 by default
- All API routes are prefixed with `/api`
- Authentication uses JWT tokens
- Database: MongoDB with Mongoose ODM

## Related Apps

- [Admin App](../admin/README.md) - Admin dashboard (port 3000)
- [Frontend App](../frontend/README.md) - User-facing website (port 3002)
