'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Grid,
  Card,
  CardContent,
  Typography,
  Button,
  Alert,
  CircularProgress,
  Tabs,
  Tab,
  Divider,
  Chip
} from '@mui/material';
import {
  Save as SaveIcon,
  ArrowBack as ArrowBackIcon,
  MonetizationOn as PriceIcon,
  LocalOffer as PromotionIcon,
  EventBusy as CancellationIcon,
  TrendingUp as SurchargeIcon
} from '@mui/icons-material';
import { useRouter, useParams } from 'next/navigation';
import Breadcrumb from '@/app/(DashboardLayout)/layout/shared/breadcrumb/Breadcrumb';
import PageContainer from '@/app/components/container/PageContainer';
import { useAuth } from '@/contexts/AuthContext';
import TourOptionsSection from '@/components/forms/TourOptionsSection';
import SurchargeSection from '@/components/forms/SurchargeSection';
import PromotionManager from '@/components/forms/PromotionManager';
import CancellationPolicyManager from '@/components/forms/CancellationPolicyManager';

function TabPanel({ children, value, index, ...other }) {
  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`pricing-tabpanel-${index}`}
      aria-labelledby={`pricing-tab-${index}`}
      {...other}
    >
      {value === index && <Box sx={{ py: 3 }}>{children}</Box>}
    </div>
  );
}

export default function TourPricingManagementPage() {
  const { authenticatedFetch } = useAuth();
  const router = useRouter();
  const params = useParams();
  const tourId = params.id;

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [alert, setAlert] = useState({ open: false, message: '', severity: 'success' });
  const [activeTab, setActiveTab] = useState(0);
  const [tour, setTour] = useState(null);
  const [pricingData, setPricingData] = useState({
    tourOptions: [],
    surcharges: [],
    promotions: [],
    cancellationPolicies: []
  });

  const BCrumb = [
    {
      to: '/',
      title: 'Home',
    },
    {
      to: '/admin/tours',
      title: 'Tour Management',
    },
    {
      title: 'Pricing Management',
    },
  ];

  useEffect(() => {
    fetchTourPricing();
  }, [tourId]);

  const fetchTourPricing = async () => {
    try {
      setLoading(true);
      const response = await authenticatedFetch(`/api/tours/${tourId}`);
      const data = await response.json();

      if (data.status === 200) {
        const tourData = data.data;
        setTour(tourData);
        setPricingData({
          tourOptions: tourData.tourOptions || [],
          surcharges: tourData.surcharges || [],
          promotions: tourData.promotions || [],
          cancellationPolicies: tourData.cancellationPolicies || []
        });
      } else {
        showAlert('Failed to fetch tour pricing data', 'error');
      }
    } catch (error) {
      console.error('Error fetching tour pricing:', error);
      showAlert('Error fetching tour pricing data', 'error');
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
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(pricingData),
      });

      const data = await response.json();

      if (data.status === 200) {
        showAlert('Pricing settings saved successfully!');
      } else {
        showAlert(data.msg || 'Failed to save pricing settings', 'error');
      }
    } catch (error) {
      console.error('Error saving pricing:', error);
      showAlert('Error saving pricing settings', 'error');
    } finally {
      setSaving(false);
    }
  };

  const handlePricingChange = (field, value) => {
    setPricingData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  if (loading) {
    return (
      <PageContainer title="Pricing Management" description="Manage tour pricing">
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '400px' }}>
          <CircularProgress />
        </Box>
      </PageContainer>
    );
  }

  if (!tour) {
    return (
      <PageContainer title="Pricing Management" description="Manage tour pricing">
        <Alert severity="error">Tour not found</Alert>
      </PageContainer>
    );
  }

  return (
    <PageContainer title="Pricing Management" description="Manage tour pricing and policies">
      <Breadcrumb title="Pricing Management" items={BCrumb} />

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
                {tour.location?.city}, {tour.location?.country} • Base Price: ${tour.price}
              </Typography>
            </Box>
            <Box sx={{ display: 'flex', gap: 2 }}>
              <Button
                startIcon={<ArrowBackIcon />}
                onClick={() => router.push('/admin/tours')}
              >
                Back to Tours
              </Button>
              <Button
                variant="contained"
                startIcon={<SaveIcon />}
                onClick={handleSave}
                disabled={saving}
              >
                {saving ? 'Saving...' : 'Save All Changes'}
              </Button>
            </Box>
          </Box>
        </CardContent>
      </Card>

      {/* Pricing Tabs */}
      <Card>
        <CardContent>
          <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
            <Tabs
              value={activeTab}
              onChange={(e, newValue) => setActiveTab(newValue)}
              aria-label="pricing management tabs"
            >
              <Tab
                icon={<PriceIcon />}
                label="Pricing Options"
                iconPosition="start"
              />
              <Tab
                icon={<SurchargeIcon />}
                label={
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    Surcharges
                    {pricingData.surcharges.length > 0 && (
                      <Chip
                        label={pricingData.surcharges.length}
                        size="small"
                        color="primary"
                      />
                    )}
                  </Box>
                }
                iconPosition="start"
              />
              <Tab
                icon={<PromotionIcon />}
                label={
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    Promotions
                    {pricingData.promotions.length > 0 && (
                      <Chip
                        label={pricingData.promotions.length}
                        size="small"
                        color="success"
                      />
                    )}
                  </Box>
                }
                iconPosition="start"
              />
              <Tab
                icon={<CancellationIcon />}
                label={
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                    Cancellation Policies
                    {pricingData.cancellationPolicies.length > 0 && (
                      <Chip
                        label={pricingData.cancellationPolicies.length}
                        size="small"
                        color="warning"
                      />
                    )}
                  </Box>
                }
                iconPosition="start"
              />
            </Tabs>
          </Box>

          {/* Tab Panels */}
          <TabPanel value={activeTab} index={0}>
            <TourOptionsSection
              tourOptions={pricingData.tourOptions}
              onChange={(options) => handlePricingChange('tourOptions', options)}
            />
          </TabPanel>

          <TabPanel value={activeTab} index={1}>
            <SurchargeSection
              surcharges={pricingData.surcharges}
              onChange={(surcharges) => handlePricingChange('surcharges', surcharges)}
            />
          </TabPanel>

          <TabPanel value={activeTab} index={2}>
            <PromotionManager
              tourId={tourId}
              initialPromotions={pricingData.promotions}
              onChange={(promotions) => handlePricingChange('promotions', promotions)}
            />
          </TabPanel>

          <TabPanel value={activeTab} index={3}>
            <CancellationPolicyManager
              tourId={tourId}
              initialPolicies={pricingData.cancellationPolicies}
              onChange={(policies) => handlePricingChange('cancellationPolicies', policies)}
            />
          </TabPanel>
        </CardContent>
      </Card>

      {/* Save Button at Bottom */}
      <Box sx={{ display: 'flex', justifyContent: 'flex-end', mt: 3 }}>
        <Button
          variant="contained"
          size="large"
          startIcon={<SaveIcon />}
          onClick={handleSave}
          disabled={saving}
        >
          {saving ? 'Saving...' : 'Save All Changes'}
        </Button>
      </Box>
    </PageContainer>
  );
}
