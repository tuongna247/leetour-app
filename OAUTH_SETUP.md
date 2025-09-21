# OAuth Authentication Setup Guide

This application now supports real Google and Facebook authentication alongside traditional email/password login.

## 🚀 Features Implemented

### User Roles
- **Admin**: Full access to all features (dashboard, tours, bookings, user management)
- **Moderator (Tour Guide)**: Can create and manage their own tours, book tours as customer
- **Customer**: Can view and book tours

### Authentication Methods
- **Google OAuth**: Sign in with Google account
- **Facebook OAuth**: Sign in with Facebook account  
- **Email/Password**: Traditional registration and login

## 📋 Setup Instructions

### 1. Environment Variables

Copy `.env.example` to `.env.local` and fill in the required values:

```bash
# Database Configuration
MONGODB_URI=your_mongodb_connection_string

# Authentication
JWT_SECRET=your_super_secret_jwt_key_change_this_in_production
NEXTAUTH_SECRET=your_nextauth_secret_key_change_this_in_production
NEXTAUTH_URL=http://localhost:3000

# Google OAuth
GOOGLE_CLIENT_ID=your_google_client_id_here
GOOGLE_CLIENT_SECRET=your_google_client_secret_here

# Facebook OAuth
FACEBOOK_CLIENT_ID=your_facebook_app_id_here
FACEBOOK_CLIENT_SECRET=your_facebook_app_secret_here
```

### 2. Google OAuth Setup

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing one
3. Enable Google+ API
4. Go to "Credentials" → "Create Credentials" → "OAuth 2.0 Client IDs"
5. Configure OAuth consent screen
6. Add authorized redirect URIs:
   - `http://localhost:3000/api/auth/callback/google` (development)
   - `https://yourdomain.com/api/auth/callback/google` (production)
7. Copy Client ID and Client Secret to your `.env.local`

### 3. Facebook OAuth Setup

1. Go to [Facebook Developers](https://developers.facebook.com/)
2. Create a new app
3. Add "Facebook Login" product
4. Configure Valid OAuth Redirect URIs:
   - `http://localhost:3000/api/auth/callback/facebook` (development)
   - `https://yourdomain.com/api/auth/callback/facebook` (production)
5. Copy App ID and App Secret to your `.env.local`

### 4. MongoDB User Schema

The User model has been updated to support OAuth:

```javascript
{
  username: String,           // Required for local auth
  name: String,              // User's full name
  email: String,             // User's email (unique)
  password: String,          // Required only for local auth
  role: String,              // 'admin', 'mod', 'customer'
  
  // OAuth fields
  googleId: String,          // Google user ID
  facebookId: String,        // Facebook user ID
  provider: String,          // 'local', 'google', 'facebook'
  profilePicture: String,    // Profile image URL
  isEmailVerified: Boolean,  // Email verification status
  
  isActive: Boolean,         // Account status
  lastLogin: Date,           // Last login timestamp
  createdAt: Date,           // Account creation
  updatedAt: Date            // Last update
}
```

## 🔐 Authentication Flow

### For New Users (OAuth)
1. User clicks "Sign in with Google/Facebook"
2. Redirected to OAuth provider
3. User authorizes the application
4. System creates new user account with 'customer' role
5. User is logged in and redirected to dashboard

### For Existing Users (OAuth)
1. If email matches existing account, OAuth account is linked
2. User can sign in with either OAuth or email/password
3. All authentication methods share the same user account

### Local Registration
1. User fills registration form
2. Can choose between 'customer' and 'mod' roles
3. Account created with local provider
4. Email verification recommended (not implemented yet)

## 🛡️ Security Features

- **Session Management**: NextAuth handles secure session management
- **CSRF Protection**: Built-in CSRF protection
- **Role-Based Access Control**: Different UI and API access based on user role
- **Password Hashing**: bcrypt with salt rounds for local passwords
- **Token Validation**: JWT tokens for API authentication
- **Account Linking**: Multiple auth methods can be linked to same account

## 🔄 Migration from Old System

The new system is backward compatible:

1. **Existing local accounts**: Continue to work with email/password
2. **OAuth linking**: Users can link Google/Facebook to existing accounts
3. **Admin accounts**: Existing admin accounts maintain full access
4. **API authentication**: Supports both JWT tokens and NextAuth sessions

## 📱 Usage

### Sign In Page: `/auth/signin`
- Google OAuth button
- Facebook OAuth button  
- Email/password form
- Link to registration

### Sign Up Page: `/auth/signup`
- Google OAuth button
- Facebook OAuth button
- Registration form with role selection
- Account type selection (Customer/Tour Guide)

### Authentication States
```javascript
const { user, isAuthenticated, isLoading } = useAuth();

// User object includes:
{
  id: "user_id",
  name: "Full Name", 
  email: "email@example.com",
  role: "customer|mod|admin",
  provider: "local|google|facebook",
  profilePicture: "image_url"
}
```

## 🐛 Troubleshooting

### Common Issues

1. **OAuth redirect mismatch**: Check authorized redirect URIs match exactly
2. **Environment variables**: Ensure all required env vars are set
3. **MongoDB connection**: Verify database connection string
4. **Port conflicts**: Ensure NextAuth URL matches your development port

### Debug Mode

Enable debug logging in `.env.local`:

```bash
NEXTAUTH_DEBUG=true
```

## 🚀 Next Steps

1. **Email Verification**: Implement email verification for local accounts
2. **Password Reset**: Add forgot password functionality  
3. **Social Account Management**: Allow users to link/unlink social accounts
4. **Admin User Management**: Admin interface to manage user roles
5. **Audit Logging**: Track authentication events

## 📞 Support

If you encounter issues:

1. Check the browser console for errors
2. Verify environment variables are set correctly
3. Ensure OAuth redirect URIs are configured properly
4. Check MongoDB connection and user permissions