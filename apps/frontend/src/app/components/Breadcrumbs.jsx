'use client';

import React from 'react';
import { Breadcrumbs as MuiBreadcrumbs, Typography, Container } from '@mui/material';
import Link from 'next/link';
import { NavigateNext as NavigateNextIcon } from '@mui/icons-material';

const Breadcrumbs = ({ items }) => {
  return (
    <Container maxWidth="lg" sx={{ py: 2 }}>
      <MuiBreadcrumbs
        separator={<NavigateNextIcon fontSize="small" />}
        aria-label="breadcrumb"
      >
        <Link
          href="/"
          style={{
            textDecoration: 'none',
            color: 'inherit',
            '&:hover': {
              textDecoration: 'underline'
            }
          }}
        >
          <Typography color="text.secondary" sx={{ '&:hover': { textDecoration: 'underline' } }}>
            Home
          </Typography>
        </Link>
        {items.map((item, index) => {
          const isLast = index === items.length - 1;

          if (isLast) {
            return (
              <Typography key={index} color="text.primary" fontWeight={500}>
                {item.label}
              </Typography>
            );
          }

          return (
            <Link
              key={index}
              href={item.href}
              style={{ textDecoration: 'none', color: 'inherit' }}
            >
              <Typography color="text.secondary" sx={{ '&:hover': { textDecoration: 'underline' } }}>
                {item.label}
              </Typography>
            </Link>
          );
        })}
      </MuiBreadcrumbs>
    </Container>
  );
};

export default Breadcrumbs;
