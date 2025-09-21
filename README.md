# 🏖️ LeeTour - Tourism Management System

A modern, full-stack tourism management application built with Next.js 15 and MongoDB.

## ✨ Features

- **🎯 Tour Management**: Create, edit, and manage tour packages
- **👥 User Authentication**: Secure login with JWT tokens
- **🔐 Admin Dashboard**: Administrative controls and analytics
- **📱 Responsive Design**: Works on all devices
- **🌐 Social Login**: Google and Facebook authentication (demo)
- **💳 Booking System**: Tour booking and management
- **⭐ Reviews**: Customer review and rating system

## 🚀 Live Demo

**Coming Soon** - Deploy to see your live application!

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
- **Database**: MongoDB Atlas
- **Authentication**: JWT + Custom Auth
- **Hosting**: Vercel (recommended)
- **Styling**: Material-UI + Custom CSS

## ⚡ Quick Start

### Local Development
```bash
# Install dependencies
npm install

# Set up environment variables
cp .env.example .env.local

# Start development server
npm run dev
```

### Environment Variables
```env
MONGODB_URI=mongodb://localhost:27017/leetour
JWT_SECRET=your_jwt_secret_key
NODE_ENV=development
```

## 📦 Deployment

See [DEPLOYMENT.md](./DEPLOYMENT.md) for detailed deployment instructions.

### Quick Deploy to Vercel
1. Push code to GitHub
2. Connect to Vercel
3. Add environment variables
4. Deploy!

## 🏗️ Project Structure

```
frontend/
├── src/
│   ├── app/
│   │   ├── api/              # API routes
│   │   ├── auth/             # Authentication pages
│   │   └── (DashboardLayout)/ # Main application
│   ├── components/           # Reusable components
│   ├── contexts/            # React contexts
│   ├── lib/                 # Utilities
│   └── models/              # Database models
├── public/                  # Static assets
└── package.json
```

## 🔧 API Endpoints

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
- `GET /api/admin/tours` - Admin tour management
- `POST /api/admin/tours` - Create new tour
- `PUT /api/admin/tours/[id]` - Update tour
- `DELETE /api/admin/tours/[id]` - Delete tour


taskkill //F //IM node.exe
taskkill /f /im node.exe