'use client';

import React, { useState } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  TextField,
  Typography,
  IconButton,
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
  FormControlLabel
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import dayjs from 'dayjs';

const SURCHARGE_TYPES = [
  { value: 'holiday', label: 'Holiday' },
  { value: 'weekend', label: 'Weekend' },
  { value: 'peak_season', label: 'Peak Season' },
  { value: 'special_event', label: 'Special Event' },
  { value: 'custom', label: 'Custom' }
];

const AMOUNT_TYPES = [
  { value: 'percentage', label: 'Percentage (%)' },
  { value: 'fixed', label: 'Fixed Amount ($)' }
];

export default function SurchargeManager({ tourId, initialSurcharges = [], onChange }) {
  const [surcharges, setSurcharges] = useState(initialSurcharges);
  const [editingId, setEditingId] = useState(null);
  const [newSurcharge, setNewSurcharge] = useState({
    surchargeName: '',
    surchargeType: 'custom',
    startDate: null,
    endDate: null,
    amountType: 'percentage',
    amount: 0,
    description: '',
    isActive: true
  });

  const handleAdd = () => {
    const updatedSurcharges = [...surcharges, { ...newSurcharge, _id: Date.now().toString() }];
    setSurcharges(updatedSurcharges);
    onChange?.(updatedSurcharges);

    // Reset form
    setNewSurcharge({
      surchargeName: '',
      surchargeType: 'custom',
      startDate: null,
      endDate: null,
      amountType: 'percentage',
      amount: 0,
      description: '',
      isActive: true
    });
  };

  const handleUpdate = (id, updatedData) => {
    const updatedSurcharges = surcharges.map(s =>
      s._id === id ? { ...s, ...updatedData } : s
    );
    setSurcharges(updatedSurcharges);
    onChange?.(updatedSurcharges);
    setEditingId(null);
  };

  const handleDelete = (id) => {
    const updatedSurcharges = surcharges.filter(s => s._id !== id);
    setSurcharges(updatedSurcharges);
    onChange?.(updatedSurcharges);
  };

  const formatDate = (date) => {
    if (!date) return '';
    return new Date(date).toLocaleDateString();
  };

  const formatAmount = (surcharge) => {
    if (surcharge.amountType === 'percentage') {
      return `${surcharge.amount}%`;
    }
    return `$${surcharge.amount}`;
  };

  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Box>
        <Typography variant="h6" gutterBottom>
          Surcharges
        </Typography>

        {/* Existing Surcharges Table */}
        {surcharges.length > 0 && (
          <TableContainer component={Paper} sx={{ mb: 3 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Name</TableCell>
                  <TableCell>Type</TableCell>
                  <TableCell>Date Range</TableCell>
                  <TableCell>Amount</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell align="right">Actions</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {surcharges.map((surcharge) => (
                  <TableRow key={surcharge._id}>
                    <TableCell>
                      <Typography variant="body2" fontWeight="medium">
                        {surcharge.surchargeName}
                      </Typography>
                      {surcharge.description && (
                        <Typography variant="caption" color="text.secondary">
                          {surcharge.description}
                        </Typography>
                      )}
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={SURCHARGE_TYPES.find(t => t.value === surcharge.surchargeType)?.label}
                        size="small"
                        color="primary"
                        variant="outlined"
                      />
                    </TableCell>
                    <TableCell>
                      {formatDate(surcharge.startDate)} - {formatDate(surcharge.endDate)}
                    </TableCell>
                    <TableCell>
                      <Chip label={formatAmount(surcharge)} size="small" color="secondary" />
                    </TableCell>
                    <TableCell>
                      <Chip
                        label={surcharge.isActive ? 'Active' : 'Inactive'}
                        size="small"
                        color={surcharge.isActive ? 'success' : 'default'}
                      />
                    </TableCell>
                    <TableCell align="right">
                      <Stack direction="row" spacing={1} justifyContent="flex-end">
                        <IconButton
                          size="small"
                          onClick={() => setEditingId(surcharge._id)}
                        >
                          <EditIcon fontSize="small" />
                        </IconButton>
                        <IconButton
                          size="small"
                          color="error"
                          onClick={() => handleDelete(surcharge._id)}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Stack>
                    </TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        )}

        {/* Add New Surcharge Form */}
        <Card>
          <CardContent>
            <Typography variant="h6" gutterBottom>
              Add New Surcharge
            </Typography>

            <Grid container spacing={2}>
              <Grid item xs={12} sm={6}>
                <TextField
                  fullWidth
                  label="Surcharge Name"
                  value={newSurcharge.surchargeName}
                  onChange={(e) => setNewSurcharge({ ...newSurcharge, surchargeName: e.target.value })}
                  placeholder="e.g., Christmas Holiday Surcharge"
                  required
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Surcharge Type</InputLabel>
                  <Select
                    value={newSurcharge.surchargeType}
                    onChange={(e) => setNewSurcharge({ ...newSurcharge, surchargeType: e.target.value })}
                    label="Surcharge Type"
                  >
                    {SURCHARGE_TYPES.map(type => (
                      <MenuItem key={type.value} value={type.value}>
                        {type.label}
                      </MenuItem>
                    ))}
                  </Select>
                </FormControl>
              </Grid>

              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="Start Date"
                  value={newSurcharge.startDate}
                  onChange={(date) => setNewSurcharge({ ...newSurcharge, startDate: date })}
                  slotProps={{ textField: { fullWidth: true, required: true } }}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <DatePicker
                  label="End Date"
                  value={newSurcharge.endDate}
                  onChange={(date) => setNewSurcharge({ ...newSurcharge, endDate: date })}
                  slotProps={{ textField: { fullWidth: true, required: true } }}
                  minDate={newSurcharge.startDate}
                />
              </Grid>

              <Grid item xs={12} sm={6}>
                <FormControl fullWidth>
                  <InputLabel>Amount Type</InputLabel>
                  <Select
                    value={newSurcharge.amountType}
                    onChange={(e) => setNewSurcharge({ ...newSurcharge, amountType: e.target.value })}
                    label="Amount Type"
                  >
                    {AMOUNT_TYPES.map(type => (
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
                  label="Amount"
                  type="number"
                  value={newSurcharge.amount}
                  onChange={(e) => setNewSurcharge({ ...newSurcharge, amount: parseFloat(e.target.value) })}
                  InputProps={{
                    inputProps: { min: 0, step: newSurcharge.amountType === 'percentage' ? 1 : 0.01 }
                  }}
                  required
                />
              </Grid>

              <Grid item xs={12}>
                <TextField
                  fullWidth
                  multiline
                  rows={2}
                  label="Description"
                  value={newSurcharge.description}
                  onChange={(e) => setNewSurcharge({ ...newSurcharge, description: e.target.value })}
                  placeholder="Optional description..."
                />
              </Grid>

              <Grid item xs={12}>
                <FormControlLabel
                  control={
                    <Switch
                      checked={newSurcharge.isActive}
                      onChange={(e) => setNewSurcharge({ ...newSurcharge, isActive: e.target.checked })}
                    />
                  }
                  label="Active"
                />
              </Grid>

              <Grid item xs={12}>
                <Button
                  variant="contained"
                  startIcon={<AddIcon />}
                  onClick={handleAdd}
                  disabled={!newSurcharge.surchargeName || !newSurcharge.startDate || !newSurcharge.endDate}
                >
                  Add Surcharge
                </Button>
              </Grid>
            </Grid>
          </CardContent>
        </Card>
      </Box>
    </LocalizationProvider>
  );
}
