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
  Divider,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Chip
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  ExpandMore as ExpandMoreIcon,
  Event as EventIcon,
  AttachMoney as MoneyIcon
} from '@mui/icons-material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';

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
      startDate: dayjs(),
      endDate: dayjs().add(1, 'month'),
      amountType: 'percentage',
      amount: 10,
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
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Card elevation={3}>
        <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Box>
            <Typography variant="h5" gutterBottom>
              Surcharges (Phụ thu)
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Add surcharges for holidays, weekends, or special periods
            </Typography>
          </Box>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleAddSurcharge}
            size="small"
          >
            Add Surcharge
          </Button>
        </Box>

        <Divider sx={{ mb: 3 }} />

        {surcharges.length === 0 ? (
          <Box sx={{ textAlign: 'center', py: 4 }}>
            <Typography variant="body1" color="text.secondary">
              No surcharges yet. Click "Add Surcharge" to create your first surcharge.
            </Typography>
          </Box>
        ) : (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            {surcharges.map((surcharge, index) => (
              <Accordion key={index} defaultExpanded={index === 0}>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls={`surcharge-${index}-content`}
                  id={`surcharge-${index}-header`}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flex: 1 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
                      {surcharge.surchargeName || `Surcharge ${index + 1}`}
                    </Typography>
                    <Chip
                      icon={<EventIcon />}
                      label={surchargeTypes.find(t => t.value === surcharge.surchargeType)?.label || 'Custom'}
                      size="small"
                      color="primary"
                      variant="outlined"
                    />
                    <Chip
                      icon={<MoneyIcon />}
                      label={surcharge.amountType === 'percentage' ? `${surcharge.amount}%` : `$${surcharge.amount}`}
                      size="small"
                      color="secondary"
                      variant="outlined"
                    />
                    {!surcharge.isActive && (
                      <Chip label="Inactive" size="small" color="default" variant="outlined" />
                    )}
                    <Box sx={{ flex: 1 }} />
                    <IconButton
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveSurcharge(index);
                      }}
                      color="error"
                      size="small"
                    >
                      <DeleteIcon />
                    </IconButton>
                  </Box>
                </AccordionSummary>
                <AccordionDetails>
                  <Grid container spacing={2}>
                    {/* Row 1: Name and Type */}
                    <Grid size={{ xs: 12, md: 6 }}>
                      <TextField
                        fullWidth
                        label="Surcharge Name"
                        value={surcharge.surchargeName}
                        onChange={(e) => handleSurchargeChange(index, 'surchargeName', e.target.value)}
                        placeholder="e.g., Lunar New Year Surcharge"
                        required
                      />
                    </Grid>

                    <Grid size={{ xs: 12, md: 6 }}>
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

                    {/* Row 2: Dates */}
                    <Grid size={{ xs: 12, md: 6 }}>
                      <DatePicker
                        label="Start Date"
                        value={dayjs(surcharge.startDate)}
                        onChange={(date) => handleSurchargeChange(index, 'startDate', date)}
                        slotProps={{
                          textField: {
                            fullWidth: true,
                            required: true
                          }
                        }}
                      />
                    </Grid>

                    <Grid size={{ xs: 12, md: 6 }}>
                      <DatePicker
                        label="End Date"
                        value={dayjs(surcharge.endDate)}
                        onChange={(date) => handleSurchargeChange(index, 'endDate', date)}
                        minDate={dayjs(surcharge.startDate)}
                        slotProps={{
                          textField: {
                            fullWidth: true,
                            required: true
                          }
                        }}
                      />
                    </Grid>

                    {/* Row 3: Amount Type and Amount on same line */}
                    <Grid size={{ xs: 12, md: 6 }}>
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

                    <Grid size={{ xs: 12, md: 6 }}>
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

                    {/* Row 4: Description - separate row */}
                    <Grid size={{ xs: 12 }}>
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

                    {/* Row 5: Active Toggle - separate row */}
                    <Grid size={{ xs: 12 }}>
                      <FormControlLabel
                        control={
                          <Switch
                            checked={surcharge.isActive}
                            onChange={(e) => handleSurchargeChange(index, 'isActive', e.target.checked)}
                          />
                        }
                        label="Active (visible to customers)"
                      />
                    </Grid>

                    {/* Preview */}
                    <Grid size={{ xs: 12 }}>
                      <Box sx={{ mt: 1, p: 2, bgcolor: 'success.lighter', borderRadius: 1, border: 1, borderColor: 'success.light' }}>
                        <Typography variant="caption" color="success.dark" fontWeight="bold" display="block" gutterBottom>
                          Preview:
                        </Typography>
                        <Typography variant="body2" color="success.dark">
                          {surcharge.surchargeName || 'Surcharge'} - {' '}
                          {surcharge.amountType === 'percentage'
                            ? `${surcharge.amount}% increase`
                            : `$${surcharge.amount} extra`
                          } from {surcharge.startDate ? dayjs(surcharge.startDate).format('MMM DD, YYYY') : 'start date'} to {surcharge.endDate ? dayjs(surcharge.endDate).format('MMM DD, YYYY') : 'end date'}
                        </Typography>
                      </Box>
                    </Grid>
                  </Grid>
                </AccordionDetails>
              </Accordion>
            ))}
          </Box>
        )}

        <Divider sx={{ my: 3 }} />

        <Box sx={{ bgcolor: 'info.lighter', p: 2, borderRadius: 1, border: 1, borderColor: 'info.light' }}>
          <Typography variant="subtitle2" color="info.dark" gutterBottom sx={{ fontWeight: 600 }}>
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
    </LocalizationProvider>
  );
};

export default SurchargeSection;
