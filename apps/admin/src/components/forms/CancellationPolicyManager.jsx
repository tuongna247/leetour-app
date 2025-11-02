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
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Chip,
  Stack,
  IconButton,
  Alert,
  Slider
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  Edit as EditIcon,
  Save as SaveIcon,
  Cancel as CancelIcon
} from '@mui/icons-material';

export default function CancellationPolicyManager({ tourId, initialPolicies = [], onChange }) {
  const [policies, setPolicies] = useState(initialPolicies.sort((a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture));
  const [editingId, setEditingId] = useState(null);
  const [newPolicy, setNewPolicy] = useState({
    daysBeforeDeparture: 7,
    refundPercentage: 100,
    description: ''
  });

  const handleAdd = () => {
    // Check for duplicate days
    const duplicate = policies.find(p => p.daysBeforeDeparture === newPolicy.daysBeforeDeparture);
    if (duplicate) {
      alert(`Policy for ${newPolicy.daysBeforeDeparture} days before departure already exists`);
      return;
    }

    const updatedPolicies = [...policies, { ...newPolicy, _id: Date.now().toString() }]
      .sort((a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture);

    setPolicies(updatedPolicies);
    onChange?.(updatedPolicies);

    // Reset form
    setNewPolicy({
      daysBeforeDeparture: 7,
      refundPercentage: 100,
      description: ''
    });
  };

  const handleUpdate = (id, updatedData) => {
    const updatedPolicies = policies.map(p =>
      p._id === id ? { ...p, ...updatedData } : p
    ).sort((a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture);

    setPolicies(updatedPolicies);
    onChange?.(updatedPolicies);
    setEditingId(null);
  };

  const handleDelete = (id) => {
    const updatedPolicies = policies.filter(p => p._id !== id);
    setPolicies(updatedPolicies);
    onChange?.(updatedPolicies);
  };

  const generateDescription = (days, percentage) => {
    if (percentage === 100) {
      return `Full refund if cancelled ${days}+ days before departure`;
    } else if (percentage === 0) {
      return `No refund if cancelled less than ${days} days before departure`;
    } else {
      return `${percentage}% refund if cancelled ${days}+ days before departure`;
    }
  };

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Cancellation Policy
      </Typography>

      <Alert severity="info" sx={{ mb: 3 }}>
        Define tiered refund policies based on how many days before departure the cancellation occurs.
        Policies are applied in order from highest to lowest days.
      </Alert>

      {/* Existing Policies Table */}
      {policies.length > 0 && (
        <TableContainer component={Paper} sx={{ mb: 3 }}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Days Before Departure</TableCell>
                <TableCell>Refund Percentage</TableCell>
                <TableCell>Description</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {policies.map((policy, index) => (
                <TableRow key={policy._id}>
                  <TableCell>
                    <Chip
                      label={`${policy.daysBeforeDeparture}+ days`}
                      color="primary"
                      variant="outlined"
                    />
                  </TableCell>
                  <TableCell>
                    <Chip
                      label={`${policy.refundPercentage}%`}
                      color={
                        policy.refundPercentage === 100 ? 'success' :
                        policy.refundPercentage === 0 ? 'error' :
                        'warning'
                      }
                    />
                  </TableCell>
                  <TableCell>
                    <Typography variant="body2">
                      {policy.description || generateDescription(policy.daysBeforeDeparture, policy.refundPercentage)}
                    </Typography>
                  </TableCell>
                  <TableCell align="right">
                    <Stack direction="row" spacing={1} justifyContent="flex-end">
                      <IconButton
                        size="small"
                        color="error"
                        onClick={() => handleDelete(policy._id)}
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

      {/* Add New Policy Form */}
      <Card>
        <CardContent>
          <Typography variant="h6" gutterBottom>
            Add New Policy Tier
          </Typography>

          <Grid container spacing={3}>
            <Grid item xs={12} sm={6}>
              <TextField
                fullWidth
                label="Days Before Departure"
                type="number"
                value={newPolicy.daysBeforeDeparture}
                onChange={(e) => setNewPolicy({ ...newPolicy, daysBeforeDeparture: parseInt(e.target.value) })}
                InputProps={{ inputProps: { min: 0 } }}
                helperText="Number of days before tour starts"
                required
              />
            </Grid>

            <Grid item xs={12} sm={6}>
              <Typography gutterBottom>
                Refund Percentage: {newPolicy.refundPercentage}%
              </Typography>
              <Slider
                value={newPolicy.refundPercentage}
                onChange={(e, value) => setNewPolicy({ ...newPolicy, refundPercentage: value })}
                min={0}
                max={100}
                step={5}
                marks={[
                  { value: 0, label: '0%' },
                  { value: 50, label: '50%' },
                  { value: 100, label: '100%' }
                ]}
                valueLabelDisplay="auto"
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                fullWidth
                multiline
                rows={2}
                label="Description (Optional)"
                value={newPolicy.description}
                onChange={(e) => setNewPolicy({ ...newPolicy, description: e.target.value })}
                placeholder={generateDescription(newPolicy.daysBeforeDeparture, newPolicy.refundPercentage)}
              />
            </Grid>

            <Grid item xs={12}>
              <Alert severity={newPolicy.refundPercentage === 100 ? 'success' : newPolicy.refundPercentage === 0 ? 'error' : 'warning'}>
                {generateDescription(newPolicy.daysBeforeDeparture, newPolicy.refundPercentage)}
              </Alert>
            </Grid>

            <Grid item xs={12}>
              <Button
                variant="contained"
                startIcon={<AddIcon />}
                onClick={handleAdd}
              >
                Add Policy Tier
              </Button>
            </Grid>
          </Grid>
        </CardContent>
      </Card>

      {/* Common Policy Templates */}
      <Card sx={{ mt: 3 }}>
        <CardContent>
          <Typography variant="h6" gutterBottom>
            Common Policy Templates
          </Typography>
          <Typography variant="body2" color="text.secondary" paragraph>
            Click to add a standard cancellation policy tier
          </Typography>

          <Stack direction="row" spacing={1} flexWrap="wrap" gap={1}>
            <Button
              variant="outlined"
              size="small"
              onClick={() => setNewPolicy({ daysBeforeDeparture: 30, refundPercentage: 100, description: '' })}
            >
              30 days - 100% refund
            </Button>
            <Button
              variant="outlined"
              size="small"
              onClick={() => setNewPolicy({ daysBeforeDeparture: 14, refundPercentage: 50, description: '' })}
            >
              14 days - 50% refund
            </Button>
            <Button
              variant="outlined"
              size="small"
              onClick={() => setNewPolicy({ daysBeforeDeparture: 7, refundPercentage: 25, description: '' })}
            >
              7 days - 25% refund
            </Button>
            <Button
              variant="outlined"
              size="small"
              onClick={() => setNewPolicy({ daysBeforeDeparture: 0, refundPercentage: 0, description: '' })}
            >
              Less than 7 days - No refund
            </Button>
          </Stack>
        </CardContent>
      </Card>
    </Box>
  );
}
