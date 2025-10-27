'use client'

import { useRouter } from 'next/navigation'
import { useEffect } from 'react'
import { Box, CircularProgress } from '@mui/material'
import { useAuth } from '@/contexts/AuthContext'

export default function HomePage() {
  const { isAuthenticated, isLoading } = useAuth()
  const router = useRouter()

  useEffect(() => {
    if (isLoading) return

    if (!isAuthenticated) {
      // Redirect to login if not authenticated
      router.replace('/auth/auth1/login')
      return
    }

    if (isAuthenticated) {
      // Redirect to dashboard if authenticated
      router.replace('/dashboard-main')
    }
  }, [isAuthenticated, isLoading, router])

  return (
    <Box
      sx={{
        minHeight: '100vh',
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'center'
      }}
    >
      <CircularProgress />
    </Box>
  )
}
