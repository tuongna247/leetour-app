'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Alert,
  CircularProgress
} from '@mui/material';
import {
  Save as SaveIcon,
  ArrowBack as ArrowBackIcon
} from '@mui/icons-material';
import { useRouter, useParams } from 'next/navigation';
import Breadcrumb from '@/app/(DashboardLayout)/layout/shared/breadcrumb/Breadcrumb';
import PageContainer from '@/app/components/container/PageContainer';
import { useAuth } from '@/contexts/AuthContext';
import TourOptionsSection from '@/components/forms/TourOptionsSection';

export default function TourPricingOptionsPage() {
  const { authenticatedFetch } = useAuth();
  const router = useRouter();
  const params = useParams();
  const tourId = params.id;

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [alert, setAlert] = useState({ open: false, message: '', severity: 'success' });
  const [tour, setTour] = useState(null);
  const [tourOptions, setTourOptions] = useState([]);

  const BCrumb = [
    { to: '/', title: 'Home' },
    { to: '/admin/tours', title: 'Tour Management' },
    { to: `/admin/tours/${tourId}/edit`, title: 'Edit Tour' },
    { title: 'Pricing Options' },
  ];

  useEffect(() => {
    fetchTour();
  }, [tourId]);

  const fetchTour = async () => {
    try {
      setLoading(true);
      const response = await authenticatedFetch(`/api/tours/${tourId}`);
      const data = await response.json();

      if (data.status === 200) {
        setTour(data.data);
        setTourOptions(data.data.tourOptions || []);
      } else {
        showAlert('Failed to fetch tour data', 'error');
      }
    } catch (error) {
      console.error('Error fetching tour:', error);
      showAlert('Error fetching tour data', 'error');
    } finally {
      setLoading(false);
    }
  };

  const showAlert = (message, severity = 'success') => {
    setAlert({ open: true, message, severity });
    setTimeout(() => setAlert({ open: false, message: '', severity: 'success' }), 3000);
  };

  const handleSave = async () => {
    try {
      setSaving(true);

      const response = await authenticatedFetch(`/api/tours/${tourId}`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ tourOptions }),
      });

      const data = await response.json();

      if (data.status === 200) {
        showAlert('Pricing options saved successfully!');
      } else {
        showAlert(data.msg || 'Failed to save pricing options', 'error');
      }
    } catch (error) {
      console.error('Error saving pricing options:', error);
      showAlert('Error saving pricing options', 'error');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <PageContainer title="Pricing Options" description="Manage tour pricing options">
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
          <CircularProgress />
        </Box>
      </PageContainer>
    );
  }

  if (!tour) {
    return (
      <PageContainer title="Pricing Options" description="Manage tour pricing options">
        <Alert severity="error">Tour not found</Alert>
      </PageContainer>
    );
  }

  return (
    <PageContainer title="Pricing Options" description="Manage tour pricing options">
      <Breadcrumb title="Pricing Options" items={BCrumb} />

      {alert.open && (
        <Alert severity={alert.severity} sx={{ mb: 2 }}>
          {alert.message}
        </Alert>
      )}

      {/* Tour Info Header */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Box>
              <Typography variant="h5" gutterBottom>
                {tour.title}
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Base Price: ${tour.price} | {tour.location?.city}, {tour.location?.country}
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', gap: 2 }}>
              <Button
                startIcon={<ArrowBackIcon />}
                onClick={() => router.push(`/admin/tours/${tourId}/edit`)}
              >
                Back to Edit
              </Button>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={saving}
              >
                {saving ? 'Saving...' : 'Save Changes'}
              </Button>
            </Box>
          </Box>
        </CardContent>
      </Card>

      {/* Pricing Options Section */}
      <TourOptionsSection
        tourOptions={tourOptions}
        onChange={setTourOptions}
      />

      {/* Save Button at Bottom */}
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
        <Button
          variant="contained"
          size="large"
          startIcon={<SaveIcon />}
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? 'Saving...' : 'Save Changes'}
        </Button>
      </Box>
    </PageContainer>
  );
}
