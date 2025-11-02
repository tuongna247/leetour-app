'use client';

import React, { useState } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  Typography,
  Grid,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  Stack,
  Switch,
  FormControlLabel,
  IconButton,
  Collapse,
  Alert
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  Edit as EditIcon,
  ExpandMore as ExpandMoreIcon
} from '@mui/icons-material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';

const PROMOTION_TYPES = [
  { value: 'early_bird', label: 'Early Bird', description: 'Book X days in advance' },
  { value: 'last_minute', label: 'Last Minute', description: 'Book within X days of departure' },
  { value: 'seasonal', label: 'Seasonal', description: 'Seasonal promotions' },
  { value: 'group_discount', label: 'Group Discount', description: 'Discount for groups' },
  { value: 'custom', label: 'Custom', description: 'Custom promotion' }
];

const DISCOUNT_TYPES = [
  { value: 'percentage', label: 'Percentage (%)' },
  { value: 'fixed', label: 'Fixed Amount ($)' }
];

export default function PromotionManager({ tourId, initialPromotions = [], onChange }) {
  const [promotions, setPromotions] = useState(initialPromotions);
  const [newPromotion, setNewPromotion] = useState({
    promotionName: '',
    promotionType: 'custom',
    discountType: 'percentage',
    discountAmount: 0,
    validFrom: dayjs(),
    validTo: dayjs().add(1, 'month'),
    daysBeforeDeparture: 0,
    minPassengers: null,
    conditions: '',
    isActive: true
  });
  const [showAdvanced, setShowAdvanced] = useState(false);

  const handleAdd = () => {
    const updatedPromotions = [...promotions, { ...newPromotion, _id: Date.now().toString() }];
    setPromotions(updatedPromotions);
    onChange?.(updatedPromotions);

    // Reset form
    setNewPromotion({
      promotionName: '',
      promotionType: 'custom',
      discountType: 'percentage',
      discountAmount: 0,
      validFrom: dayjs(),
      validTo: dayjs().add(1, 'month'),
      daysBeforeDeparture: 0,
      minPassengers: null,
      conditions: '',
      isActive: true
    });
    setShowAdvanced(false);
  };

  const handleDelete = (id) => {
    const updatedPromotions = promotions.filter(p => p._id !== id);
    setPromotions(updatedPromotions);
    onChange?.(updatedPromotions);
  };

  const formatDate = (date) => {
    if (!date) return '';
    return new Date(date).toLocaleDateString();
  };

  const formatDiscount = (promotion) => {
    if (promotion.discountType === 'percentage') {
      return `${promotion.discountAmount}%`;
    }
    return `$${promotion.discountAmount}`;
  };

  const getPromotionDetails = (promotion) => {
    const details = [];

    if (promotion.promotionType === 'early_bird' && promotion.daysBeforeDeparture) {
      details.push(`Book ${promotion.daysBeforeDeparture}+ days in advance`);
    }

    if (promotion.promotionType === 'last_minute' && promotion.daysBeforeDeparture) {
      details.push(`Book within ${promotion.daysBeforeDeparture} days`);
    }

    if (promotion.minPassengers) {
      details.push(`Min ${promotion.minPassengers} passengers`);
    }

    return details.join(' • ');
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Box>
        <Typography variant="h6" gutterBottom>
          Promotions & Discounts
        </Typography>

        {/* Existing Promotions Table */}
        {promotions.length > 0 && (
          <TableContainer component={Paper} sx={{ mb: 3 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Name</TableCell>
                  <TableCell>Type</TableCell>
                  <TableCell>Validity Period</TableCell>
                  <TableCell>Discount</TableCell>
                  <TableCell>Details</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell align="right">Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {promotions.map((promotion) => (
                  <TableRow key={promotion._id}>
                    <TableCell>
                      <Typography variant="body2" fontWeight="medium">
                        {promotion.promotionName}
                      </Typography>
                      {promotion.conditions && (
                        <Typography variant="caption" color="text.secondary">
                          {promotion.conditions}
                        </Typography>
                      )}
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={PROMOTION_TYPES.find(t => t.value === promotion.promotionType)?.label}
                        size="small"
                        color="primary"
                        variant="outlined"
                      />
                    </TableCell>
                    <TableCell>
                      <Typography variant="body2">
                        {formatDate(promotion.validFrom)}
                      </Typography>
                      <Typography variant="body2">
                        to {formatDate(promotion.validTo)}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Chip label={formatDiscount(promotion)} size="small" color="success" />
                    </TableCell>
                    <TableCell>
                      <Typography variant="caption">
                        {getPromotionDetails(promotion)}
                      </Typography>
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={promotion.isActive ? 'Active' : 'Inactive'}
                        size="small"
                        color={promotion.isActive ? 'success' : 'default'}
                      />
                    </TableCell>
                    <TableCell align="right">
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => handleDelete(promotion._id)}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        {/* Add New Promotion Form */}
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              Add New Promotion
            </Typography>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Promotion Name"
                  value={newPromotion.promotionName}
                  onChange={(e) => setNewPromotion({ ...newPromotion, promotionName: e.target.value })}
                  placeholder="e.g., Early Bird 20% Off"
                  required
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Promotion Type</InputLabel>
                  <Select
                    value={newPromotion.promotionType}
                    onChange={(e) => setNewPromotion({ ...newPromotion, promotionType: e.target.value })}
                    label="Promotion Type"
                  >
                    {PROMOTION_TYPES.map(type => (
                      <MenuItem key={type.value} value={type.value}>
                        <Box>
                          <Typography variant="body2">{type.label}</Typography>
                          <Typography variant="caption" color="text.secondary">
                            {type.description}
                          </Typography>
                        </Box>
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Valid From"
                  value={newPromotion.validFrom}
                  onChange={(date) => setNewPromotion({ ...newPromotion, validFrom: date })}
                  slotProps={{ textField: { fullWidth: true, required: true } }}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Valid To"
                  value={newPromotion.validTo}
                  onChange={(date) => setNewPromotion({ ...newPromotion, validTo: date })}
                  slotProps={{ textField: { fullWidth: true, required: true } }}
                  minDate={newPromotion.validFrom}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Discount Type</InputLabel>
                  <Select
                    value={newPromotion.discountType}
                    onChange={(e) => setNewPromotion({ ...newPromotion, discountType: e.target.value })}
                    label="Discount Type"
                  >
                    {DISCOUNT_TYPES.map(type => (
                      <MenuItem key={type.value} value={type.value}>
                        {type.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Discount Amount"
                  type="number"
                  value={newPromotion.discountAmount}
                  onChange={(e) => setNewPromotion({ ...newPromotion, discountAmount: parseFloat(e.target.value) })}
                  InputProps={{
                    inputProps: { min: 0, step: newPromotion.discountType === 'percentage' ? 1 : 0.01 }
                  }}
                  required
                />
              </Grid>

              {/* Advanced Settings */}
              <Grid item xs={12}>
                <Button
                  onClick={() => setShowAdvanced(!showAdvanced)}
                  endIcon={<ExpandMoreIcon sx={{ transform: showAdvanced ? 'rotate(180deg)' : 'none' }} />}
                >
                  Advanced Settings
                </Button>
              </Grid>

              <Grid item xs={12}>
                <Collapse in={showAdvanced}>
                  <Grid container spacing={2}>
                    {(newPromotion.promotionType === 'early_bird' || newPromotion.promotionType === 'last_minute') && (
                      <Grid item xs={12} sm={6}>
                        <TextField
                          fullWidth
                          label="Days Before Departure"
                          type="number"
                          value={newPromotion.daysBeforeDeparture}
                          onChange={(e) => setNewPromotion({ ...newPromotion, daysBeforeDeparture: parseInt(e.target.value) })}
                          helperText={
                            newPromotion.promotionType === 'early_bird'
                              ? 'Must book this many days in advance'
                              : 'Must book within this many days of departure'
                          }
                          InputProps={{ inputProps: { min: 0 } }}
                        />
                      </Grid>
                    )}

                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="Minimum Passengers"
                        type="number"
                        value={newPromotion.minPassengers || ''}
                        onChange={(e) => setNewPromotion({ ...newPromotion, minPassengers: e.target.value ? parseInt(e.target.value) : null })}
                        helperText="Optional: Minimum passengers required for this promotion"
                        InputProps={{ inputProps: { min: 1 } }}
                      />
                    </Grid>

                    <Grid item xs={12}>
                      <TextField
                        fullWidth
                        multiline
                        rows={2}
                        label="Terms & Conditions"
                        value={newPromotion.conditions}
                        onChange={(e) => setNewPromotion({ ...newPromotion, conditions: e.target.value })}
                        placeholder="Optional: Terms and conditions for this promotion..."
                      />
                    </Grid>
                  </Grid>
                </Collapse>
              </Grid>

              <Grid item xs={12}>
                <FormControlLabel
                  control={
                    <Switch
                      checked={newPromotion.isActive}
                      onChange={(e) => setNewPromotion({ ...newPromotion, isActive: e.target.checked })}
                    />
                  }
                  label="Active"
                />
              </Grid>

              {newPromotion.promotionType === 'early_bird' && (
                <Grid item xs={12}>
                  <Alert severity="info">
                    Early Bird: Customers who book {newPromotion.daysBeforeDeparture} or more days before departure will receive {formatDiscount(newPromotion)} off.
                  </Alert>
                </Grid>
              )}

              {newPromotion.promotionType === 'last_minute' && (
                <Grid item xs={12}>
                  <Alert severity="info">
                    Last Minute: Customers who book within {newPromotion.daysBeforeDeparture} days of departure will receive {formatDiscount(newPromotion)} off.
                  </Alert>
                </Grid>
              )}

              <Grid item xs={12}>
                <Button
                  variant="contained"
                  startIcon={<AddIcon />}
                  onClick={handleAdd}
                  disabled={!newPromotion.promotionName || !newPromotion.validFrom || !newPromotion.validTo || !newPromotion.discountAmount}
                >
                  Add Promotion
                </Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Box>
    </LocalizationProvider>
  );
}
