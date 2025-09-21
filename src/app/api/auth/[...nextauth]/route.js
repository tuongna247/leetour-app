import NextAuth from 'next-auth'
import GoogleProvider from 'next-auth/providers/google'
import FacebookProvider from 'next-auth/providers/facebook'
import CredentialsProvider from 'next-auth/providers/credentials'
import { MongoDBAdapter } from "@next-auth/mongodb-adapter"
import { MongoClient } from "mongodb"
import connectDB from '@/lib/mongodb'
import User from '@/models/User'
import bcrypt from 'bcryptjs'

const client = new MongoClient(process.env.MONGODB_URI)
const clientPromise = client.connect()

const authOptions = {
  // Temporarily disable adapter to test OAuth flow
  // adapter: MongoDBAdapter(clientPromise),
  providers: [
    GoogleProvider({
      clientId: process.env.GOOGLE_CLIENT_ID,
      clientSecret: process.env.GOOGLE_CLIENT_SECRET,
      profile(profile) {
        console.log('Google OAuth profile:', profile);
        return {
          id: profile.sub,
          name: profile.name,
          email: profile.email,
          image: profile.picture,
          role: 'customer',
          provider: 'google',
          googleId: profile.sub,
          isEmailVerified: profile.email_verified || false
        }
      }
    }),
    // Facebook temporarily disabled
    // FacebookProvider({
    //   clientId: process.env.FACEBOOK_CLIENT_ID,
    //   clientSecret: process.env.FACEBOOK_CLIENT_SECRET,
    //   profile(profile) {
    //     return {
    //       id: profile.id,
    //       name: profile.name,
    //       email: profile.email,
    //       image: profile.picture?.data?.url,
    //       role: 'customer', // Default role for new users
    //       provider: 'facebook',
    //       facebookId: profile.id,
    //       isEmailVerified: false
    //     }
    //   }
    // }),
    CredentialsProvider({
      name: 'credentials',
      credentials: {
        email: { label: 'Email', type: 'email' },
        password: { label: 'Password', type: 'password' }
      },
      async authorize(credentials) {
        try {
          await connectDB()
          
          if (!credentials?.email || !credentials?.password) {
            throw new Error('Email and password required')
          }

          const user = await User.findOne({ email: credentials.email.toLowerCase() })
          
          if (!user || !user.password) {
            throw new Error('Invalid email or password')
          }

          const isPasswordCorrect = await user.comparePassword(credentials.password)
          
          if (!isPasswordCorrect) {
            throw new Error('Invalid email or password')
          }

          if (!user.isActive) {
            throw new Error('Account is deactivated')
          }

          // Update last login
          user.lastLogin = new Date()
          await user.save()

          return {
            id: user._id.toString(),
            name: user.name,
            email: user.email,
            role: user.role,
            provider: user.provider || 'local',
            image: user.profilePicture || null
          }
        } catch (error) {
          console.error('Auth error:', error)
          throw new Error(error.message || 'Authentication failed')
        }
      }
    })
  ],
  callbacks: {
    async signIn({ user, account, profile }) {
      try {
        await connectDB()
        
        if (account.provider === 'google') {
          const providerId = profile.sub
          const providerField = 'googleId'
          
          console.log('Google OAuth sign in attempt:', { 
            email: user.email, 
            providerId,
            profileData: profile 
          })
          
          // Check if user already exists with this provider ID
          let existingUser = await User.findOne({ [providerField]: providerId })
          
          if (!existingUser) {
            // Check if user exists with same email
            existingUser = await User.findOne({ email: user.email })
            
            if (existingUser) {
              // Link OAuth account to existing user
              existingUser[providerField] = providerId
              existingUser.provider = account.provider
              existingUser.isEmailVerified = user.isEmailVerified || existingUser.isEmailVerified
              if (user.image) existingUser.profilePicture = user.image
              await existingUser.save()
              console.log('Linked Google account to existing user:', existingUser.email)
            } else {
              // Create new user
              const newUser = new User({
                name: user.name,
                email: user.email,
                [providerField]: providerId,
                provider: account.provider,
                role: 'customer',
                isEmailVerified: user.isEmailVerified || false,
                profilePicture: user.image || '',
                username: user.email.split('@')[0] + Math.random().toString(36).substr(2, 5),
                isActive: true
              })
              
              await newUser.save()
              existingUser = newUser
              console.log('Created new Google user:', existingUser.email)
            }
          } else {
            console.log('Existing Google user found:', existingUser.email)
          }
          
          // Update last login
          existingUser.lastLogin = new Date()
          await existingUser.save()
          
          // Update user object with database info
          user.id = existingUser._id.toString()
          user.role = existingUser.role
          user.provider = existingUser.provider
        }
        
        return true
      } catch (error) {
        console.error('Sign in callback error:', error)
        return false
      }
    },
    
    async session({ session, token }) {
      if (token) {
        session.user.id = token.id
        session.user.role = token.role
        session.user.provider = token.provider
      }
      return session
    },
    
    async redirect({ url, baseUrl }) {
      // Custom redirect logic based on user role
      console.log('NextAuth redirect called with url:', url, 'baseUrl:', baseUrl)
      
      // If it's a callback URL, check if it should redirect based on role
      if (url.startsWith(baseUrl)) {
        return url
      }
      
      // Default redirect to dashboard-main for admins/mods, tours for customers
      // Note: We can't easily access user role here, so we'll handle this in the client
      return `${baseUrl}/dashboard-main`
    },
    
    async jwt({ token, user }) {
      if (user) {
        token.id = user.id
        token.role = user.role
        token.provider = user.provider
      }
      return token
    }
  },
  pages: {
    signIn: '/auth/signin',
    signUp: '/auth/signup',
    error: '/auth/error',
  },
  debug: true,
  session: {
    strategy: 'jwt',
    maxAge: 30 * 24 * 60 * 60, // 30 days
  },
  secret: process.env.NEXTAUTH_SECRET,
}

const handler = NextAuth(authOptions)
export { handler as GET, handler as POST }