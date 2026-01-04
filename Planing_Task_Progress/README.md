# 🏖️ LeeTour - Tourism Management System

A modern, full-stack tourism management application built with Next.js 15 and MongoDB, organized as a monorepo with separate applications.

## 📁 Project Structure

This is a monorepo containing three separate applications:

```
leetour-app/
├── apps/
│   ├── admin/      # Admin Dashboard (Port 3000)
│   ├── api/        # Backend API (Port 3001)
│   └── frontend/   # User Frontend (Port 3002)
├── public/         # Shared static assets
└── rule/           # Business rules
```

### 🎛️ Admin Dashboard (`apps/admin`)
Administrative dashboard for managing tours, users, bookings, and suppliers.
- **Port**: 3000
- **Tech**: Next.js 15, Material-UI, React 19
- [Read more →](apps/admin/README.md)

### 🔌 API Server (`apps/api`)
Standalone backend API service handling all business logic and data operations.
- **Port**: 3001
- **Tech**: Next.js API Routes, MongoDB, Mongoose
- [Read more →](apps/api/README.md)

### 🌐 Frontend (Coming Soon - `apps/frontend`)
User-facing website for browsing and booking tours.
- **Port**: 3002
- **Tech**: Next.js 15, Material-UI

## ✨ Features

- **🎯 Tour Management**: Create, edit, and manage tour packages
- **👥 User Authentication**: Secure login with JWT tokens
- **🔐 Admin Dashboard**: Administrative controls and analytics
- **📱 Responsive Design**: Works on all devices
- **💳 Booking System**: Tour booking and management
- **⭐ Reviews**: Customer review and rating system
- **🏢 Supplier Management**: Multi-supplier support with approval workflow

## 🚀 Quick Start

### Prerequisites
- Node.js 18+ installed
- MongoDB instance running locally or remote

### Running All Services

**Terminal 1 - API Server:**
```bash
cd apps/api
npm install
cp .env.example .env
# Edit .env with your configuration
npm run dev
# Runs on http://localhost:3001
```

**Terminal 2 - Admin Dashboard:**
```bash
cd apps/admin
npm install
cp .env.example .env.local
# Edit .env.local with your configuration
npm run dev
# Runs on http://localhost:3000
```

**Terminal 3 - Frontend (Coming Soon):**
```bash
cd apps/frontend
npm install
npm run dev
# Runs on http://localhost:3002
```

## 📋 Test Accounts

### Admin Access
- **Username**: `admin`
- **Password**: `admin123`
- **Access**: Full administrative controls

### Regular User
- **Username**: `user`
- **Password**: `user123`
- **Access**: Book tours, leave reviews

## 🛠️ Tech Stack

- **Frontend**: Next.js 15, React 19, Material-UI
- **Backend**: Next.js API Routes
- **Database**: MongoDB with Mongoose ODM
- **Authentication**: JWT + NextAuth
- **Styling**: Material-UI + Emotion

## 🔧 Architecture

### Separation of Concerns
- **Admin App**: UI for administrators, connects to API
- **API App**: Handles all business logic, database operations
- **Frontend App**: Public-facing website for end users

### Communication Flow
```
Admin Dashboard (3000) → API Server (3001) → MongoDB
Frontend (3002) → API Server (3001) → MongoDB
```

## 📦 Environment Variables

Each app requires its own environment configuration:

### API App (`apps/api/.env`)
```env
MONGODB_URI=mongodb://localhost:27017/leetour
JWT_SECRET=your_jwt_secret_key
NEXTAUTH_SECRET=your_nextauth_secret
NEXTAUTH_URL=http://localhost:3001
ADMIN_URL=http://localhost:3000
FRONTEND_URL=http://localhost:3002
```

### Admin App (`apps/admin/.env.local`)
```env
MONGODB_URI=mongodb://localhost:27017/leetour
JWT_SECRET=your_jwt_secret_key
NEXTAUTH_SECRET=your_nextauth_secret
NEXTAUTH_URL=http://localhost:3000
NEXT_PUBLIC_API_URL=http://localhost:3001
```

## 📚 Documentation

- [Admin App Setup](apps/admin/README.md)
- [API Documentation](apps/api/README.md)
- [API Integration Guide](apps/admin/API_INTEGRATION.md)
- [Deployment Guide](apps/admin/DEPLOYMENT.md)

## 🔧 API Endpoints

All API endpoints are served from the API app on port 3001:

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `GET /api/auth/me` - Get current user
- `POST /api/auth/logout` - User logout

### Tours
- `GET /api/tours` - List all tours
- `GET /api/tours/[id]` - Get single tour
- `POST /api/tours/[id]/reviews` - Add review

### Admin
- `GET /api/tours` - Admin tour management
- `POST /api/tours` - Create new tour
- `PUT /api/tours/[id]` - Update tour
- `DELETE /api/tours/[id]` - Delete tour
- `GET /api/admin/users` - Manage users
- `GET /api/admin/suppliers` - Manage suppliers

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test all affected apps
5. Submit a pull request
