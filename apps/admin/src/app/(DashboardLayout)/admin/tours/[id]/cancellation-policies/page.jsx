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
import CancellationPolicyManager from '@/components/forms/CancellationPolicyManager';

export default function TourCancellationPoliciesPage() {
  const { authenticatedFetch } = useAuth();
  const router = useRouter();
  const params = useParams();
  const tourId = params.id;

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [alert, setAlert] = useState({ open: false, message: '', severity: 'success' });
  const [tour, setTour] = useState(null);
  const [cancellationPolicies, setCancellationPolicies] = useState([]);

  const BCrumb = [
    { to: '/', title: 'Home' },
    { to: '/admin/tours', title: 'Tour Management' },
    { to: `/admin/tours/${tourId}/edit`, title: 'Edit Tour' },
    { title: 'Cancellation Policies' },
  ];

  useEffect(() => {
    fetchTour();
  }, [tourId]);

  const fetchTour = async () => {
    try {
      setLoading(true);
      const response = await authenticatedFetch(`/api/admin/tours/${tourId}`);
      const data = await response.json();

      if (data.status === 200) {
        setTour(data.data);
        setCancellationPolicies(data.data.cancellationPolicies || []);
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

      const response = await authenticatedFetch(`/api/admin/tours/${tourId}`, {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ cancellationPolicies }),
      });

      const data = await response.json();

      if (data.status === 200) {
        showAlert('Cancellation policies saved successfully!');
      } else {
        showAlert(data.msg || 'Failed to save cancellation policies', 'error');
      }
    } catch (error) {
      console.error('Error saving cancellation policies:', error);
      showAlert('Error saving cancellation policies', 'error');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <PageContainer title="Cancellation Policies" description="Manage tour cancellation policies">
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
          <CircularProgress />
        </Box>
      </PageContainer>
    );
  }

  if (!tour) {
    return (
      <PageContainer title="Cancellation Policies" description="Manage tour cancellation policies">
        <Alert severity="error">Tour not found</Alert>
      </PageContainer>
    );
  }

  return (
    <PageContainer title="Cancellation Policies" description="Manage tour cancellation policies">
      <Breadcrumb title="Cancellation Policies" items={BCrumb} />

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

      {/* Cancellation Policies Section */}
      <CancellationPolicyManager
        tourId={tourId}
        initialPolicies={cancellationPolicies}
        onChange={setCancellationPolicies}
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
