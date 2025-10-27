'use client';

import React from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  IconButton,
  Grid,
  Switch,
  FormControlLabel,
  Divider
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon
} from '@mui/icons-material';

const surchargeTypes = [
  { value: 'holiday', label: 'Holiday' },
  { value: 'weekend', label: 'Weekend' },
  { value: 'peak_season', label: 'Peak Season' },
  { value: 'special_event', label: 'Special Event' },
  { value: 'custom', label: 'Custom' }
];

const amountTypes = [
  { value: 'percentage', label: 'Percentage (%)' },
  { value: 'fixed', label: 'Fixed Amount ($)' }
];

const SurchargeSection = ({ surcharges = [], onChange }) => {
  const handleAddSurcharge = () => {
    const newSurcharge = {
      surchargeName: '',
      surchargeType: 'weekend',
      startDate: '',
      endDate: '',
      amountType: 'percentage',
      amount: 0,
      description: '',
      isActive: true
    };
    onChange([...surcharges, newSurcharge]);
  };

  const handleRemoveSurcharge = (index) => {
    const updated = surcharges.filter((_, i) => i !== index);
    onChange(updated);
  };

  const handleSurchargeChange = (index, field, value) => {
    const updated = surcharges.map((surcharge, i) => {
      if (i === index) {
        return { ...surcharge, [field]: value };
      }
      return surcharge;
    });
    onChange(updated);
  };

  return (
    <Card>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="h6">
            Surcharges (Phụ thu)
          </Typography>
          <Button
            startIcon={<AddIcon />}
            variant="outlined"
            size="small"
            onClick={handleAddSurcharge}
          >
            Add Surcharge
          </Button>
        </Box>

        <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
          Add surcharges for holidays, weekends, or special periods. Surcharges will be automatically applied when bookings fall within the specified dates.
        </Typography>

        {surcharges.length === 0 ? (
          <Box
            sx={{
              textAlign: 'center',
              py: 4,
              border: '2px dashed',
              borderColor: 'divider',
              borderRadius: 1
            }}
          >
            <Typography color="text.secondary">
              No surcharges added yet. Click &quot;Add Surcharge&quot; to create one.
            </Typography>
          </Box>
        ) : (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
            {surcharges.map((surcharge, index) => (
              <Card key={index} variant="outlined" sx={{ position: 'relative' }}>
                <CardContent>
                  <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                    <Typography variant="subtitle1" fontWeight="bold">
                      Surcharge #{index + 1}
                    </Typography>
                    <Box sx={{ display: 'flex', gap: 1, alignItems: 'center' }}>
                      <FormControlLabel
                        control={
                          <Switch
                            checked={surcharge.isActive}
                            onChange={(e) => handleSurchargeChange(index, 'isActive', e.target.checked)}
                            size="small"
                          />
                        }
                        label="Active"
                      />
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => handleRemoveSurcharge(index)}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </Box>
                  </Box>

                  <Grid container spacing={2}>
                    <Grid item xs={12} md={6}>
                      <TextField
                        fullWidth
                        label="Surcharge Name"
                        value={surcharge.surchargeName}
                        onChange={(e) => handleSurchargeChange(index, 'surchargeName', e.target.value)}
                        placeholder="e.g., Lunar New Year Surcharge"
                        required
                      />
                    </Grid>

                    <Grid item xs={12} md={6}>
                      <FormControl fullWidth>
                        <InputLabel>Surcharge Type</InputLabel>
                        <Select
                          value={surcharge.surchargeType}
                          label="Surcharge Type"
                          onChange={(e) => handleSurchargeChange(index, 'surchargeType', e.target.value)}
                        >
                          {surchargeTypes.map((type) => (
                            <MenuItem key={type.value} value={type.value}>
                              {type.label}
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </Grid>

                    <Grid item xs={12} md={4}>
                      <TextField
                        fullWidth
                        label="Start Date"
                        type="date"
                        value={surcharge.startDate}
                        onChange={(e) => handleSurchargeChange(index, 'startDate', e.target.value)}
                        InputLabelProps={{ shrink: true }}
                        required
                      />
                    </Grid>

                    <Grid item xs={12} md={4}>
                      <TextField
                        fullWidth
                        label="End Date"
                        type="date"
                        value={surcharge.endDate}
                        onChange={(e) => handleSurchargeChange(index, 'endDate', e.target.value)}
                        InputLabelProps={{ shrink: true }}
                        required
                      />
                    </Grid>

                    <Grid item xs={12} md={4}>
                      <FormControl fullWidth>
                        <InputLabel>Amount Type</InputLabel>
                        <Select
                          value={surcharge.amountType}
                          label="Amount Type"
                          onChange={(e) => handleSurchargeChange(index, 'amountType', e.target.value)}
                        >
                          {amountTypes.map((type) => (
                            <MenuItem key={type.value} value={type.value}>
                              {type.label}
                            </MenuItem>
                          ))}
                        </Select>
                      </FormControl>
                    </Grid>

                    <Grid item xs={12} md={6}>
                      <TextField
                        fullWidth
                        label={surcharge.amountType === 'percentage' ? 'Percentage (%)' : 'Fixed Amount ($)'}
                        type="number"
                        value={surcharge.amount}
                        onChange={(e) => handleSurchargeChange(index, 'amount', parseFloat(e.target.value) || 0)}
                        inputProps={{
                          min: 0,
                          max: surcharge.amountType === 'percentage' ? 100 : undefined,
                          step: surcharge.amountType === 'percentage' ? 1 : 0.01
                        }}
                        helperText={
                          surcharge.amountType === 'percentage'
                            ? 'Enter percentage (e.g., 20 for 20%)'
                            : 'Enter fixed amount in USD'
                        }
                        required
                      />
                    </Grid>

                    <Grid item xs={12} md={6}>
                      <TextField
                        fullWidth
                        label="Description"
                        value={surcharge.description}
                        onChange={(e) => handleSurchargeChange(index, 'description', e.target.value)}
                        placeholder="e.g., Extra fee during holiday season"
                        multiline
                        rows={2}
                      />
                    </Grid>
                  </Grid>

                  {/* Preview */}
                  <Box sx={{ mt: 2, p: 2, bgcolor: 'primary.light', borderRadius: 1 }}>
                    <Typography variant="caption" color="primary.dark" fontWeight="bold">
                      Preview:
                    </Typography>
                    <Typography variant="caption" color="primary.dark" sx={{ ml: 1 }}>
                      {surcharge.surchargeName || 'Surcharge'} - {' '}
                      {surcharge.amountType === 'percentage'
                        ? `${surcharge.amount}% increase`
                        : `$${surcharge.amount} extra`
                      } from {surcharge.startDate || 'start date'} to {surcharge.endDate || 'end date'}
                    </Typography>
                  </Box>
                </CardContent>
              </Card>
            ))}
          </Box>
        )}

        <Divider sx={{ my: 3 }} />

        <Box sx={{ bgcolor: 'info.light', p: 2, borderRadius: 1 }}>
          <Typography variant="subtitle2" color="info.dark" gutterBottom>
            How Surcharges Work:
          </Typography>
          <Typography variant="body2" color="info.dark" component="ul" sx={{ pl: 2, mb: 0 }}>
            <li>Surcharges are automatically applied when booking dates fall within the specified range</li>
            <li>Percentage surcharges are calculated based on the base tour price</li>
            <li>Fixed amount surcharges add a flat fee to the total price</li>
            <li>Multiple surcharges can apply to the same booking if date ranges overlap</li>
            <li>Only active surcharges will be applied to bookings</li>
          </Typography>
        </Box>
      </CardContent>
    </Card>
  );
};

export default SurchargeSection;
