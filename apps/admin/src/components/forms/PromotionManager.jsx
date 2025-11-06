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
  Alert,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Tooltip,
  Divider,
  InputAdornment
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  Edit as EditIcon,
  ExpandMore as ExpandMoreIcon,
  LocalOffer as OfferIcon,
  Info as InfoIcon,
  Close as CloseIcon,
  Cancel as CancelIcon
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
  const [openDialog, setOpenDialog] = useState(false);
  const [editingPromotion, setEditingPromotion] = useState(null);
  const [formData, setFormData] = useState({
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
  const [errors, setErrors] = useState({});

  const validateForm = () => {
    const newErrors = {};

    if (!formData.promotionName?.trim()) {
      newErrors.promotionName = 'Promotion name is required';
    }

    if (!formData.discountAmount || formData.discountAmount <= 0) {
      newErrors.discountAmount = 'Discount amount must be greater than 0';
    }

    if (formData.discountType === 'percentage' && formData.discountAmount > 100) {
      newErrors.discountAmount = 'Percentage cannot exceed 100%';
    }

    if (!formData.validFrom) {
      newErrors.validFrom = 'Valid from date is required';
    }

    if (!formData.validTo) {
      newErrors.validTo = 'Valid to date is required';
    }

    if (formData.validFrom && formData.validTo && dayjs(formData.validTo).isBefore(formData.validFrom)) {
      newErrors.validTo = 'End date must be after start date';
    }

    setErrors(newErrors);
    return Object.keys(newErrors).length === 0;
  };

  const handleOpenDialog = (promotion = null) => {
    if (promotion) {
      setEditingPromotion(promotion);
      setFormData({
        ...promotion,
        validFrom: dayjs(promotion.validFrom),
        validTo: dayjs(promotion.validTo)
      });
    } else {
      setEditingPromotion(null);
      setFormData({
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
    }
    setErrors({});
    setShowAdvanced(false);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setEditingPromotion(null);
    setErrors({});
    setShowAdvanced(false);
  };

  const handleSavePromotion = () => {
    if (!validateForm()) return;

    let updatedPromotions;
    if (editingPromotion) {
      // Update existing promotion
      updatedPromotions = promotions.map(p =>
        p._id === editingPromotion._id ? { ...formData, _id: p._id } : p
      );
    } else {
      // Add new promotion
      updatedPromotions = [...promotions, { ...formData, _id: Date.now().toString() }];
    }

    setPromotions(updatedPromotions);
    onChange?.(updatedPromotions);
    handleCloseDialog();
  };

  const handleDelete = (id) => {
    const updatedPromotions = promotions.filter(p => p._id !== id);
    setPromotions(updatedPromotions);
    onChange?.(updatedPromotions);
  };

  const handleToggleActive = (id) => {
    const updatedPromotions = promotions.map(p =>
      p._id === id ? { ...p, isActive: !p.isActive } : p
    );
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

  const getPromotionColor = (promotionType) => {
    const colors = {
      early_bird: 'primary',
      last_minute: 'warning',
      seasonal: 'info',
      group_discount: 'success',
      custom: 'default'
    };
    return colors[promotionType] || 'default';
  };

  const isPromotionActive = (promotion) => {
    if (!promotion.isActive) return false;
    const now = dayjs();
    return now.isAfter(dayjs(promotion.validFrom)) && now.isBefore(dayjs(promotion.validTo));
  };

  const getPromotionStatus = (promotion) => {
    if (!promotion.isActive) return { label: 'Inactive', color: 'default' };

    const now = dayjs();
    const validFrom = dayjs(promotion.validFrom);
    const validTo = dayjs(promotion.validTo);

    if (now.isBefore(validFrom)) {
      return { label: 'Scheduled', color: 'info' };
    }
    if (now.isAfter(validTo)) {
      return { label: 'Expired', color: 'error' };
    }
    return { label: 'Active', color: 'success' };
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
        {/* Header with Add Button */}
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
          <Box>
            <Typography variant="h5" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <OfferIcon color="primary" />
              Promotions & Discounts
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Manage promotional offers and discounts for this tour
            </Typography>
          </Box>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={() => handleOpenDialog()}
            size="large"
          >
            Add Promotion
          </Button>
        </Box>

        {/* Promotions Summary */}
        {promotions.length > 0 && (
          <Grid container spacing={2} sx={{ mb: 3 }}>
            <Grid item xs={12} sm={6} md={3}>
              <Card sx={{ bgcolor: 'primary.lighter' }}>
                <CardContent>
                  <Typography variant="h4" color="primary">
                    {promotions.length}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Total Promotions
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card sx={{ bgcolor: 'success.lighter' }}>
                <CardContent>
                  <Typography variant="h4" color="success.main">
                    {promotions.filter(p => isPromotionActive(p)).length}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Active Now
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card sx={{ bgcolor: 'info.lighter' }}>
                <CardContent>
                  <Typography variant="h4" color="info.main">
                    {promotions.filter(p => {
                      const status = getPromotionStatus(p);
                      return status.label === 'Scheduled';
                    }).length}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Scheduled
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <Card sx={{ bgcolor: 'warning.lighter' }}>
                <CardContent>
                  <Typography variant="h4" color="warning.main">
                    {promotions.filter(p => {
                      const status = getPromotionStatus(p);
                      return status.label === 'Expired';
                    }).length}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    Expired
                  </Typography>
                </CardContent>
              </Card>
            </Grid>
          </Grid>
        )}

        {/* Existing Promotions Table */}
        {promotions.length > 0 ? (
          <TableContainer component={Paper} elevation={2}>
            <Table>
              <TableHead sx={{ bgcolor: 'grey.100' }}>
                <TableRow>
                  <TableCell><strong>Promotion Name</strong></TableCell>
                  <TableCell><strong>Type</strong></TableCell>
                  <TableCell><strong>Discount</strong></TableCell>
                  <TableCell><strong>Valid Period</strong></TableCell>
                  <TableCell><strong>Status</strong></TableCell>
                  <TableCell align="right"><strong>Actions</strong></TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {promotions.map((promotion) => {
                  const status = getPromotionStatus(promotion);
                  return (
                    <TableRow
                      key={promotion._id}
                      sx={{
                        '&:hover': { bgcolor: 'action.hover' },
                        opacity: promotion.isActive ? 1 : 0.6
                      }}
                    >
                      <TableCell>
                        <Box>
                          <Typography variant="body2" fontWeight="600">
                            {promotion.promotionName}
                          </Typography>
                          {promotion.conditions && (
                            <Typography variant="caption" color="text.secondary" sx={{ display: 'block', mt: 0.5 }}>
                              {promotion.conditions.length > 50
                                ? `${promotion.conditions.substring(0, 50)}...`
                                : promotion.conditions}
                            </Typography>
                          )}
                          {getPromotionDetails(promotion) && (
                            <Typography variant="caption" color="primary" sx={{ display: 'block', mt: 0.5 }}>
                              {getPromotionDetails(promotion)}
                            </Typography>
                          )}
                        </Box>
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={PROMOTION_TYPES.find(t => t.value === promotion.promotionType)?.label}
                          size="small"
                          color={getPromotionColor(promotion.promotionType)}
                          variant="outlined"
                        />
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={formatDiscount(promotion)}
                          size="medium"
                          color="success"
                          sx={{ fontWeight: 'bold', fontSize: '0.9rem' }}
                        />
                      </TableCell>
                      <TableCell>
                        <Typography variant="body2" sx={{ whiteSpace: 'nowrap' }}>
                          {formatDate(promotion.validFrom)}
                        </Typography>
                        <Typography variant="body2" color="text.secondary" sx={{ whiteSpace: 'nowrap' }}>
                          to {formatDate(promotion.validTo)}
                        </Typography>
                      </TableCell>
                      <TableCell>
                        <Stack direction="row" spacing={1} alignItems="center">
                          <Chip
                            label={status.label}
                            size="small"
                            color={status.color}
                          />
                          <Tooltip title={promotion.isActive ? 'Click to deactivate' : 'Click to activate'}>
                            <Switch
                              size="small"
                              checked={promotion.isActive}
                              onChange={() => handleToggleActive(promotion._id)}
                            />
                          </Tooltip>
                        </Stack>
                      </TableCell>
                      <TableCell align="right">
                        <Stack direction="row" spacing={1} justifyContent="flex-end">
                          <Tooltip title="Edit promotion">
                            <IconButton
                              size="small"
                              color="primary"
                              onClick={() => handleOpenDialog(promotion)}
                            >
                              <EditIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                          <Tooltip title="Delete promotion">
                            <IconButton
                              size="small"
                              color="error"
                              onClick={() => handleDelete(promotion._id)}
                            >
                              <DeleteIcon fontSize="small" />
                            </IconButton>
                          </Tooltip>
                        </Stack>
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Card>
            <CardContent sx={{ textAlign: 'center', py: 8 }}>
              <OfferIcon sx={{ fontSize: 80, color: 'text.disabled', mb: 2 }} />
              <Typography variant="h6" color="text.secondary" gutterBottom>
                No promotions yet
              </Typography>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
                Create promotional offers to attract more customers
              </Typography>
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={() => handleOpenDialog()}
              >
                Add Your First Promotion
              </Button>
            </CardContent>
          </Card>
        )}

        {/* Add/Edit Promotion Dialog */}
        <Dialog
          open={openDialog}
          onClose={handleCloseDialog}
          maxWidth="md"
          fullWidth
          PaperProps={{
            sx: { borderRadius: 2 }
          }}
        >
          <DialogTitle sx={{ pb: 1 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <Typography variant="h5" component="span">
                {editingPromotion ? 'Edit Promotion' : 'Add New Promotion'}
              </Typography>
              <IconButton onClick={handleCloseDialog} size="small">
                <CloseIcon />
              </IconButton>
            </Box>
            <Typography variant="body2" color="text.secondary" sx={{ mt: 1 }}>
              {editingPromotion ? 'Update promotion details' : 'Create a new promotional offer for this tour'}
            </Typography>
          </DialogTitle>

          <Divider />

          <DialogContent sx={{ pt: 3 }}>
            <Grid container spacing={3}>
              {/* Promotion Name */}
              <Grid item xs={12}>
                <TextField
                  fullWidth
                  label="Promotion Name"
                  value={formData.promotionName}
                  onChange={(e) => setFormData({ ...formData, promotionName: e.target.value })}
                  placeholder="e.g., Summer Special 25% Off"
                  required
                  error={!!errors.promotionName}
                  helperText={errors.promotionName}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <OfferIcon color="action" />
                      </InputAdornment>
                    )
                  }}
                />
              </Grid>

              {/* Promotion Type */}
              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Promotion Type</InputLabel>
                  <Select
                    value={formData.promotionType}
                    onChange={(e) => setFormData({ ...formData, promotionType: e.target.value })}
                    label="Promotion Type"
                  >
                    {PROMOTION_TYPES.map(type => (
                      <MenuItem key={type.value} value={type.value}>
                        <Box>
                          <Typography variant="body2" fontWeight="500">{type.label}</Typography>
                          <Typography variant="caption" color="text.secondary">
                            {type.description}
                          </Typography>
                        </Box>
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              {/* Discount Type */}
              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Discount Type</InputLabel>
                  <Select
                    value={formData.discountType}
                    onChange={(e) => setFormData({ ...formData, discountType: e.target.value })}
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

              {/* Discount Amount */}
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Discount Amount"
                  type="number"
                  value={formData.discountAmount}
                  onChange={(e) => setFormData({ ...formData, discountAmount: parseFloat(e.target.value) || 0 })}
                  required
                  error={!!errors.discountAmount}
                  helperText={errors.discountAmount || `Enter ${formData.discountType === 'percentage' ? 'percentage (0-100)' : 'fixed amount in dollars'}`}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        {formData.discountType === 'percentage' ? '%' : '$'}
                      </InputAdornment>
                    ),
                    inputProps: {
                      min: 0,
                      max: formData.discountType === 'percentage' ? 100 : undefined,
                      step: formData.discountType === 'percentage' ? 1 : 0.01
                    }
                  }}
                />
              </Grid>

              {/* Active Status */}
              <Grid item xs={12} sm={6}>
                <FormControlLabel
                  control={
                    <Switch
                      checked={formData.isActive}
                      onChange={(e) => setFormData({ ...formData, isActive: e.target.checked })}
                      color="success"
                    />
                  }
                  label={
                    <Box>
                      <Typography variant="body2">
                        {formData.isActive ? 'Active' : 'Inactive'}
                      </Typography>
                      <Typography variant="caption" color="text.secondary">
                        Toggle promotion status
                      </Typography>
                    </Box>
                  }
                />
              </Grid>

              {/* Valid From */}
              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Valid From"
                  value={formData.validFrom}
                  onChange={(date) => setFormData({ ...formData, validFrom: date })}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      required: true,
                      error: !!errors.validFrom,
                      helperText: errors.validFrom
                    }
                  }}
                />
              </Grid>

              {/* Valid To */}
              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Valid To"
                  value={formData.validTo}
                  onChange={(date) => setFormData({ ...formData, validTo: date })}
                  minDate={formData.validFrom}
                  slotProps={{
                    textField: {
                      fullWidth: true,
                      required: true,
                      error: !!errors.validTo,
                      helperText: errors.validTo
                    }
                  }}
                />
              </Grid>

              {/* Advanced Settings Collapse */}
              <Grid item xs={12}>
                <Divider sx={{ my: 1 }} />
                <Button
                  onClick={() => setShowAdvanced(!showAdvanced)}
                  endIcon={
                    <ExpandMoreIcon
                      sx={{
                        transform: showAdvanced ? 'rotate(180deg)' : 'none',
                        transition: 'transform 0.3s'
                      }}
                    />
                  }
                  startIcon={<InfoIcon />}
                >
                  Advanced Settings
                </Button>
              </Grid>

              <Grid item xs={12}>
                <Collapse in={showAdvanced}>
                  <Grid container spacing={2}>
                    {/* Days Before Departure */}
                    {(formData.promotionType === 'early_bird' || formData.promotionType === 'last_minute') && (
                      <Grid item xs={12} sm={6}>
                        <TextField
                          fullWidth
                          label="Days Before Departure"
                          type="number"
                          value={formData.daysBeforeDeparture}
                          onChange={(e) => setFormData({ ...formData, daysBeforeDeparture: parseInt(e.target.value) || 0 })}
                          helperText={
                            formData.promotionType === 'early_bird'
                              ? 'Book this many days in advance'
                              : 'Book within this many days'
                          }
                          InputProps={{ inputProps: { min: 0 } }}
                        />
                      </Grid>
                    )}

                    {/* Minimum Passengers */}
                    <Grid item xs={12} sm={6}>
                      <TextField
                        fullWidth
                        label="Minimum Passengers (Optional)"
                        type="number"
                        value={formData.minPassengers || ''}
                        onChange={(e) => setFormData({ ...formData, minPassengers: e.target.value ? parseInt(e.target.value) : null })}
                        helperText="Leave empty for no minimum"
                        InputProps={{ inputProps: { min: 1 } }}
                      />
                    </Grid>

                    {/* Terms & Conditions */}
                    <Grid item xs={12}>
                      <TextField
                        fullWidth
                        multiline
                        rows={3}
                        label="Terms & Conditions (Optional)"
                        value={formData.conditions}
                        onChange={(e) => setFormData({ ...formData, conditions: e.target.value })}
                        placeholder="Enter any specific terms, conditions, or restrictions..."
                      />
                    </Grid>
                  </Grid>
                </Collapse>
              </Grid>

              {/* Preview Alert */}
              {formData.promotionName && formData.discountAmount > 0 && (
                <Grid item xs={12}>
                  <Alert severity="success" icon={<OfferIcon />}>
                    <Typography variant="body2" fontWeight="600">
                      Preview: {formData.promotionName}
                    </Typography>
                    <Typography variant="body2">
                      Customers will receive {formatDiscount(formData)} discount
                      {formData.promotionType === 'early_bird' && formData.daysBeforeDeparture > 0 &&
                        ` when booking ${formData.daysBeforeDeparture}+ days in advance`}
                      {formData.promotionType === 'last_minute' && formData.daysBeforeDeparture > 0 &&
                        ` when booking within ${formData.daysBeforeDeparture} days`}
                      {formData.minPassengers && ` (min ${formData.minPassengers} passengers)`}
                    </Typography>
                  </Alert>
                </Grid>
              )}
            </Grid>
          </DialogContent>

          <Divider />

          <DialogActions sx={{ px: 3, py: 2 }}>
            <Button
              onClick={handleCloseDialog}
              startIcon={<CancelIcon />}
              color="inherit"
            >
              Cancel
            </Button>
            <Button
              variant="contained"
              onClick={handleSavePromotion}
              startIcon={editingPromotion ? <EditIcon /> : <AddIcon />}
            >
              {editingPromotion ? 'Update Promotion' : 'Add Promotion'}
            </Button>
          </DialogActions>
        </Dialog>
      </Box>
    </LocalizationProvider>
  );
}
