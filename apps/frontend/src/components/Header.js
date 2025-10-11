'use client';

import {
  AppBar,
  Toolbar,
  Typography,
  Button,
  Container,
  Box,
} from '@mui/material';
import { Explore, Person } from '@mui/icons-material';

export default function Header() {
  return (
    <AppBar position="sticky" elevation={1} sx={{ backgroundColor: 'white', color: 'text.primary' }}>
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <Explore sx={{ fontSize: 32, mr: 1, color: 'primary.main' }} />
          <Typography
            variant="h5"
            component="a"
            href="/"
            sx={{
              mr: 2,
              fontWeight: 700,
              color: 'primary.main',
              textDecoration: 'none',
              flexGrow: { xs: 1, md: 0 },
            }}
          >
            LeeTour
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: 'none', md: 'flex' }, ml: 4 }}>
            <Button sx={{ my: 2, color: 'text.primary' }} href="/tours">
              Tours
            </Button>
            <Button sx={{ my: 2, color: 'text.primary' }} href="/about">
              About
            </Button>
            <Button sx={{ my: 2, color: 'text.primary' }} href="/contact">
              Contact
            </Button>
          </Box>

          <Box sx={{ display: 'flex', gap: 1 }}>
            <Button
              variant="outlined"
              startIcon={<Person />}
              href="/auth/login"
            >
              Login
            </Button>
            <Button
              variant="contained"
              href="/auth/register"
            >
              Sign Up
            </Button>
          </Box>
        </Toolbar>
      </Container>
    </AppBar>
  );
}
