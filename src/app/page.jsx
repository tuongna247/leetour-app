'use client'

import { useEffect } from 'react'
import { useRouter } from 'next/navigation'
import { useSession } from 'next-auth/react'
import { useAuth } from '@/contexts/AuthContext'
import { Box, CircularProgress, Typography } from '@mui/material'

export default function HomePage() {
  const router = useRouter()
  const { data: session, status } = useSession()
  const { isAuthenticated, isLoading, user } = useAuth()

  useEffect(() => {
    if (status === 'loading' || isLoading) return // Still loading

    if (status === 'unauthenticated' && !isAuthenticated) {
      // User is not logged in, redirect to sign in
      router.replace('/auth/signin')
    }
  }, [status, isAuthenticated, isLoading, router])

  // If user is authenticated, redirect based on role
  if (status === 'authenticated' || isAuthenticated) {
    const userRole = session?.user?.role || user?.role
    
    if (userRole === 'customer') {
      router.replace('/tours')
    } else {
      router.replace('/dashboard-main')
    }
    
    return (
      <Box
        sx={{
          minHeight: '100vh',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          justifyContent: 'center',
          background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
        }}
      >
        <CircularProgress size={60} sx={{ color: 'white', mb: 2 }} />
        <Typography variant="h6" sx={{ color: 'white' }}>
          {userRole === 'customer' ? 'Taking you to tours...' : 'Redirecting to dashboard...'}
        </Typography>
      </Box>
    )
  }

  // Show loading while determining auth status
  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        background: 'linear-gradient(135deg, #667eea 0%, #764ba2 100%)',
      }}
    >
      <CircularProgress size={60} sx={{ color: 'white', mb: 2 }} />
      <Typography variant="h6" sx={{ color: 'white' }}>
        Loading...
      </Typography>
    </Box>
  )
}