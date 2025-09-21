'use client'

import React, { useState } from 'react'
import { signIn, getSession } from 'next-auth/react'
import { useRouter, useSearchParams } from 'next/navigation'
import {
  Box,
  Card,
  CardContent,
  Typography,
  TextField,
  Button,
  Divider,
  Alert,
  IconButton,
  Link
} from '@mui/material'
import { Google as GoogleIcon, Facebook as FacebookIcon, BugReport as TestIcon } from '@mui/icons-material'
import { useAuth } from '@/contexts/AuthContext'

export default function SignInPage() {
  const router = useRouter()
  const searchParams = useSearchParams()
  const { login } = useAuth()
  const [formData, setFormData] = useState({
    email: '',
    password: ''
  })
  
  // Get redirect URL from query parameters
  const redirectUrl = searchParams.get('redirect') || null
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const handleInputChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    })
  }

  const handleCredentialsSignIn = async (e) => {
    e.preventDefault()
    setLoading(true)
    setError('')

    try {
      const result = await signIn('credentials', {
        email: formData.email,
        password: formData.password,
        redirect: false
      })

      if (result?.error) {
        setError(result.error)
      } else if (result?.ok) {
        // Get the session to update our auth context
        const session = await getSession()
        if (session?.user) {
          // Update our custom auth context
          await login(formData.email, formData.password)
          
          // Redirect to intended page or default based on role
          if (redirectUrl) {
            router.push(redirectUrl)
          } else if (session.user.role === 'customer') {
            router.push('/tours')
          } else {
            router.push('/dashboard-main')
          }
        } else {
          router.push('/dashboard-main')
        }
      }
    } catch (err) {
      setError('Sign in failed. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  const handleOAuthSignIn = async (provider) => {
    setLoading(true)
    setError('')

    try {
      const result = await signIn(provider, { 
        callbackUrl: '/auth/oauth-redirect',
        redirect: true 
      })
      
      if (result?.error) {
        setError(`${provider} sign in failed`)
      }
    } catch (err) {
      setError(`${provider} sign in failed. Please try again.`)
    } finally {
      setLoading(false)
    }
  }

  const handleTestLogin = async (role) => {
    setLoading(true)
    setError('')

    try {
      const response = await fetch('/api/auth/test-login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ role })
      })

      const data = await response.json()

      if (data.status === 200) {
        // Store token and user data
        localStorage.setItem('token', data.data.token)
        localStorage.setItem('user', JSON.stringify(data.data.user))
        
        // Update auth context
        await login(data.data.user.email, 'testpassword123')
        
        // Redirect to intended page or default based on role
        if (redirectUrl) {
          router.push(redirectUrl)
        } else if (data.data.user.role === 'customer') {
          router.push('/tours')
        } else {
          router.push('/dashboard-main')
        }
      } else {
        setError(data.msg || 'Test login failed')
      }
    } catch (err) {
      setError('Test login failed. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        p: 2
      }}
    >
      <Card sx={{ maxWidth: 400, width: '100%' }}>
        <CardContent sx={{ p: 4 }}>
          <Typography variant="h4" align="center" gutterBottom>
            Welcome Back
          </Typography>
          <Typography variant="body2" align="center" color="text.secondary" sx={{ mb: 3 }}>
            Sign in to your account
          </Typography>

          {error && (
            <Alert severity="error" sx={{ mb: 2 }}>
              {error}
            </Alert>
          )}

          {/* Test Login Buttons - Development Only */}
          <Box sx={{ mb: 3, p: 2, bgcolor: 'warning.light', borderRadius: 1 }}>
            <Typography variant="body2" sx={{ mb: 2, color: 'warning.dark' }}>
              🧪 Development Test Login (Manual Login)
            </Typography>
            <Box sx={{ display: 'flex', gap: 1, flexDirection: 'column' }}>
              <Button
                size="small"
                variant="contained"
                color="error"
                startIcon={<TestIcon />}
                onClick={() => handleTestLogin('admin')}
                disabled={loading}
              >
                Test as Admin
              </Button>
              <Button
                size="small"
                variant="contained"
                color="warning"
                startIcon={<TestIcon />}
                onClick={() => handleTestLogin('mod')}
                disabled={loading}
              >
                Test as Moderator
              </Button>
              <Button
                size="small"
                variant="contained"
                color="info"
                startIcon={<TestIcon />}
                onClick={() => handleTestLogin('customer')}
                disabled={loading}
              >
                Test as Customer
              </Button>
            </Box>
          </Box>

          {/* OAuth Buttons */}
          <Box sx={{ mb: 3 }}>
            <Button
              fullWidth
              variant="outlined"
              startIcon={<GoogleIcon />}
              onClick={() => handleOAuthSignIn('google')}
              disabled={loading}
              sx={{ mb: 2 }}
            >
              Sign in with Google
            </Button>
            {/* Facebook temporarily disabled
            <Button
              fullWidth
              variant="outlined"
              startIcon={<FacebookIcon />}
              onClick={() => handleOAuthSignIn('facebook')}
              disabled={loading}
              sx={{ mb: 2 }}
            >
              Sign in with Facebook
            </Button>
            */}
          </Box>

          <Divider sx={{ my: 2 }}>
            <Typography variant="body2" color="text.secondary">
              OR
            </Typography>
          </Divider>

          {/* Email/Password Form */}
          <Box component="form" onSubmit={handleCredentialsSignIn}>
            <TextField
              fullWidth
              label="Email"
              name="email"
              type="email"
              value={formData.email}
              onChange={handleInputChange}
              required
              sx={{ mb: 2 }}
            />
            <TextField
              fullWidth
              label="Password"
              name="password"
              type="password"
              value={formData.password}
              onChange={handleInputChange}
              required
              sx={{ mb: 3 }}
            />
            <Button
              type="submit"
              fullWidth
              variant="contained"
              disabled={loading}
              sx={{ mb: 2 }}
            >
              {loading ? 'Signing In...' : 'Sign In'}
            </Button>
          </Box>

          <Typography variant="body2" align="center">
            Don&apos;t have an account?{' '}
            <Link href="/auth/signup" underline="hover">
              Sign up here
            </Link>
          </Typography>
        </CardContent>
      </Card>
    </Box>
  )
}