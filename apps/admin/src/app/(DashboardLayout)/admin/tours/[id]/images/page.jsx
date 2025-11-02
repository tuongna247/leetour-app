'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  Typography,
  Alert,
  CircularProgress
} from '@mui/material';
import {
  ArrowBack as ArrowBackIcon,
  Save as SaveIcon,
  Image as ImageIcon
} from '@mui/icons-material';
import { useRouter, useParams } from 'next/navigation';
import Breadcrumb from '@/app/(DashboardLayout)/layout/shared/breadcrumb/Breadcrumb';
import PageContainer from '@/app/components/container/PageContainer';
import { useAuth } from '@/contexts/AuthContext';
import dynamic from 'next/dynamic';

const TourGalleryUploader = dynamic(() => import('@/components/forms/TourGalleryUploader'), {
  ssr: false,
  loading: () => <CircularProgress size={20} />
});

export default function TourImagesPage() {
  const { authenticatedFetch } = useAuth();
  const router = useRouter();
  const params = useParams();
  const tourId = params.id;

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [alert, setAlert] = useState({ open: false, message: '', severity: 'success' });
  const [tourTitle, setTourTitle] = useState('');
  const [featuredImage, setFeaturedImage] = useState({ url: '', alt: '' });
  const [galleryImages, setGalleryImages] = useState([]);

  const BCrumb = [
    {
      to: '/',
      title: 'Home',
    },
    {
      to: '/admin',
      title: 'Admin',
    },
    {
      to: '/admin/tours',
      title: 'Tour Management',
    },
    {
      to: `/admin/tours/${tourId}/edit`,
      title: 'Edit Tour',
    },
    {
      title: 'Tour Images',
    },
  ];

  useEffect(() => {
    const fetchTourImages = async () => {
      try {
        setLoading(true);
        const response = await authenticatedFetch(`/api/admin/tours/${tourId}`);
        const data = await response.json();

        if (data.status === 200) {
          const tour = data.data;
          setTourTitle(tour.title);
          setFeaturedImage(tour.featuredImage || { url: '', alt: '' });
          setGalleryImages(tour.galleryImages || []);
        } else {
          showAlert('Failed to fetch tour images', 'error');
        }
      } catch (error) {
        console.error('Error fetching tour images:', error);
        showAlert('Error fetching tour images', 'error');
      } finally {
        setLoading(false);
      }
    };

    if (tourId) {
      fetchTourImages();
    }
  }, [tourId]);

  const showAlert = (message, severity = 'success') => {
    setAlert({ open: true, message, severity });
    setTimeout(() => setAlert({ open: false, message: '', severity: 'success' }), 3000);
  };

  const handleSave = async () => {
    try {
      setSaving(true);

      const response = await fetch(`/api/admin/tours/${tourId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          featuredImage: featuredImage?.url ? featuredImage : null,
          galleryImages: galleryImages || []
        }),
      });

      const data = await response.json();

      if (data.status === 200) {
        showAlert('Images saved successfully!');
      } else {
        showAlert(data.msg || 'Failed to save images', 'error');
      }
    } catch (error) {
      console.error('Error saving images:', error);
      showAlert('Error saving images', 'error');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <PageContainer title="Tour Images" description="Manage tour images">
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: 400 }}>
          <CircularProgress />
        </Box>
      </PageContainer>
    );
  }

  return (
    <PageContainer title="Tour Images" description="Manage tour images">
      <Breadcrumb title="Tour Images" items={BCrumb} />

      {alert.open && (
        <Alert severity={alert.severity} sx={{ mb: 2 }}>
          {alert.message}
        </Alert>
      )}

      {/* Header */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
            <Box>
              <Typography variant="h5" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <ImageIcon /> Tour Images
              </Typography>
              <Typography variant="body2" color="text.secondary">
                {tourTitle}
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', gap: 2 }}>
              <Button
                variant="outlined"
                startIcon={<ArrowBackIcon />}
                onClick={() => router.push(`/admin/tours/${tourId}/edit`)}
              >
                Back to Tour
              </Button>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={saving}
              >
                {saving ? 'Saving...' : 'Save Images'}
              </Button>
            </Box>
          </Box>
        </CardContent>
      </Card>

      {/* Gallery Uploader */}
      <Card>
        <CardContent>
          <TourGalleryUploader
            tourId={tourId}
            featuredImage={featuredImage}
            galleryImages={galleryImages}
            onFeaturedChange={setFeaturedImage}
            onGalleryChange={setGalleryImages}
          />
        </CardContent>
      </Card>

      {/* Bottom Actions */}
      <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end', gap: 2 }}>
        <Button
          variant="outlined"
          startIcon={<ArrowBackIcon />}
          onClick={() => router.push(`/admin/tours/${tourId}/edit`)}
        >
          Back to Tour
        </Button>
        <Button
          variant="contained"
          startIcon={<SaveIcon />}
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? 'Saving...' : 'Save Images'}
        </Button>
      </Box>
    </PageContainer>
  );
}
