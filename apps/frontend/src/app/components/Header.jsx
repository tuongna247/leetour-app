'use client';

import React, { useState } from 'react';
import {
  AppBar,
  Toolbar,
  Container,
  Typography,
  Button,
  Box,
  IconButton,
  InputBase,
  alpha,
} from '@mui/material';
import {
  Search as SearchIcon,
} from '@mui/icons-material';
import Link from 'next/link';
import { useRouter } from 'next/navigation';

const Header = () => {
  const router = useRouter();
  const [searchQuery, setSearchQuery] = useState('');

  const handleSearch = (e) => {
    e.preventDefault();
    if (searchQuery.trim()) {
      router.push(`/tours?search=${encodeURIComponent(searchQuery)}`);
    }
  };

  return (
    <AppBar position="sticky" color="default" elevation={1}>
      <Container maxWidth="lg">
        <Toolbar disableGutters sx={{ py: 1 }}>
          {/* Logo */}
          <Link href="/" style={{ textDecoration: 'none', color: 'inherit' }}>
            <Typography
              variant="h5"
              component="div"
              sx={{
                fontWeight: 700,
                color: 'primary.main',
                mr: 4,
                cursor: 'pointer',
                '&:hover': {
                  opacity: 0.8
                }
              }}
            >
              Goreise Tour
            </Typography>
          </Link>

          {/* Navigation Menu */}
          <Box sx={{ flexGrow: 1, display: 'flex', gap: 2 }}>
            <Button
              component={Link}
              href="/tours"
              color="inherit"
              sx={{ fontWeight: 500 }}
            >
              Tours
            </Button>
            <Button
              component={Link}
              href="/about"
              color="inherit"
              sx={{ fontWeight: 500 }}
            >
              About
            </Button>
          </Box>

          {/* Search */}
          <Box
            component="form"
            onSubmit={handleSearch}
            sx={{
              position: 'relative',
              borderRadius: 1,
              backgroundColor: (theme) => alpha(theme.palette.common.black, 0.05),
              '&:hover': {
                backgroundColor: (theme) => alpha(theme.palette.common.black, 0.08),
              },
              mr: 2,
              width: '200px',
            }}
          >
            <Box sx={{
              position: 'absolute',
              height: '100%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              pl: 1.5,
              pointerEvents: 'none'
            }}>
              <SearchIcon sx={{ color: 'text.secondary', fontSize: 20 }} />
            </Box>
            <InputBase
              placeholder="Search tours..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              sx={{
                width: '100%',
                '& .MuiInputBase-input': {
                  padding: '8px 8px 8px 0',
                  paddingLeft: '40px',
                  fontSize: '0.875rem'
                }
              }}
            />
          </Box>

          {/* Login Button */}
          <Button
            component={Link}
            href="/login"
            variant="outlined"
            size="small"
            sx={{ fontWeight: 500 }}
          >
            Login
          </Button>
        </Toolbar>
      </Container>
    </AppBar>
  );
};

export default Header;
