'use client';

import React from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  TextField,
  IconButton,
  Grid,
  Divider,
  FormControlLabel,
  Switch,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Chip
} from '@mui/material';
import {
  Add as AddIcon,
  Delete as DeleteIcon,
  ExpandMore as ExpandMoreIcon
} from '@mui/icons-material';

export default function TourOptionsSection({ tourOptions = [], onChange }) {

  const handleAddOption = () => {
    const newOption = {
      optionName: '',
      description: '',
      basePrice: 0,
      pricingTiers: [],
      isActive: true
    };
    onChange([...tourOptions, newOption]);
  };

  const handleRemoveOption = (index) => {
    const updated = tourOptions.filter((_, i) => i !== index);
    onChange(updated);
  };

  const handleOptionChange = (index, field, value) => {
    const updated = tourOptions.map((option, i) =>
      i === index ? { ...option, [field]: value } : option
    );
    onChange(updated);
  };

  const handleAddPricingTier = (optionIndex) => {
    const updated = tourOptions.map((option, i) => {
      if (i === optionIndex) {
        const newTier = {
          minPassengers: 1,
          maxPassengers: 10,
          pricePerPerson: option.basePrice || 0
        };
        return {
          ...option,
          pricingTiers: [...(option.pricingTiers || []), newTier]
        };
      }
      return option;
    });
    onChange(updated);
  };

  const handleRemovePricingTier = (optionIndex, tierIndex) => {
    const updated = tourOptions.map((option, i) => {
      if (i === optionIndex) {
        return {
          ...option,
          pricingTiers: option.pricingTiers.filter((_, ti) => ti !== tierIndex)
        };
      }
      return option;
    });
    onChange(updated);
  };

  const handlePricingTierChange = (optionIndex, tierIndex, field, value) => {
    const updated = tourOptions.map((option, i) => {
      if (i === optionIndex) {
        return {
          ...option,
          pricingTiers: option.pricingTiers.map((tier, ti) =>
            ti === tierIndex ? { ...tier, [field]: Number(value) } : tier
          )
        };
      }
      return option;
    });
    onChange(updated);
  };

  return (
    <Card elevation={3}>
      <CardContent>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Box>
            <Typography variant="h5" gutterBottom>
              Tour Pricing Options
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Create multiple pricing options based on group size and services
            </Typography>
          </Box>
          <Button
            variant="contained"
            startIcon={<AddIcon />}
            onClick={handleAddOption}
            size="small"
          >
            Add Option
          </Button>
        </Box>

        <Divider sx={{ mb: 3 }} />

        {tourOptions.length === 0 ? (
          <Box sx={{ textAlign: 'center', py: 4 }}>
            <Typography variant="body1" color="text.secondary">
              No pricing options yet. Click \"Add Option\" to create your first pricing tier.
            </Typography>
          </Box>
        ) : (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            {tourOptions.map((option, optionIndex) => (
              <Accordion key={optionIndex} defaultExpanded={optionIndex === 0}>
                <AccordionSummary
                  expandIcon={<ExpandMoreIcon />}
                  aria-controls={`option-${optionIndex}-content`}
                  id={`option-${optionIndex}-header`}
                >
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flex: 1 }}>
                    <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
                      {option.optionName || `Option ${optionIndex + 1}`}
                    </Typography>
                    {option.basePrice > 0 && (
                      <Chip
                        label={`Base: $${option.basePrice}`}
                        size="small"
                        color="primary"
                        variant="outlined"
                      />
                    )}
                    {option.pricingTiers?.length > 0 && (
                      <Chip
                        label={`${option.pricingTiers.length} Tiers`}
                        size="small"
                        color="secondary"
                        variant="outlined"
                      />
                    )}
                    <Box sx={{ flex: 1 }} />
                    <IconButton
                      onClick={(e) => {
                        e.stopPropagation();
                        handleRemoveOption(optionIndex);
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
                    {/* Option Name and Base Price on same line */}
                    <Grid size={{ xs: 12, md: 8 }}>
                      <TextField
                        fullWidth
                        label="Option Name"
                        placeholder="e.g., Standard Package, VIP Package"
                        value={option.optionName}
                        onChange={(e) => handleOptionChange(optionIndex, 'optionName', e.target.value)}
                        required
                      />
                    </Grid>

                    <Grid size={{ xs: 12, md: 4 }}>
                      <TextField
                        fullWidth
                        label="Base Price per Person"
                        type="number"
                        value={option.basePrice}
                        onChange={(e) => handleOptionChange(optionIndex, 'basePrice', Number(e.target.value))}
                        required
                        inputProps={{ min: 0, step: 0.01 }}
                      />
                    </Grid>

                    {/* Description - separate row */}
                    <Grid size={{ xs: 12 }}>
                      <TextField
                        fullWidth
                        label="Description"
                        placeholder="Describe what's included in this option"
                        multiline
                        rows={2}
                        value={option.description}
                        onChange={(e) => handleOptionChange(optionIndex, 'description', e.target.value)}
                      />
                    </Grid>

                    {/* Is Active - separate row */}
                    <Grid size={{ xs: 12 }}>
                      <FormControlLabel
                        control={
                          <Switch
                            checked={option.isActive}
                            onChange={(e) => handleOptionChange(optionIndex, 'isActive', e.target.checked)}
                          />
                        }
                        label="Active (visible to customers)"
                      />
                    </Grid>

                    {/* Pricing Tiers Section */}
                    <Grid size={{ xs: 12 }}>
                      <Divider sx={{ my: 2 }} />
                      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
                        <Typography variant="subtitle2" sx={{ fontWeight: 600 }}>
                          Group Size Pricing Tiers
                        </Typography>
                        <Button
                          variant="outlined"
                          size="small"
                          startIcon={<AddIcon />}
                          onClick={() => handleAddPricingTier(optionIndex)}
                        >
                          Add Tier
                        </Button>
                      </Box>

                      <Typography variant="caption" color="text.secondary" sx={{ mb: 2, display: 'block' }}>
                        Define different prices based on the number of passengers (e.g., larger groups get discounts)
                      </Typography>

                      {option.pricingTiers && option.pricingTiers.length > 0 ? (
                        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                          {option.pricingTiers.map((tier, tierIndex) => (
                            <Card key={tierIndex} variant="outlined" sx={{ p: 2 }}>
                              <Grid container spacing={2} alignItems="center">
                                <Grid size={{ xs: 12, sm: 3 }}>
                                  <TextField
                                    fullWidth
                                    label="Min Passengers"
                                    type="number"
                                    size="small"
                                    value={tier.minPassengers}
                                    onChange={(e) => handlePricingTierChange(optionIndex, tierIndex, 'minPassengers', e.target.value)}
                                    inputProps={{ min: 1 }}
                                  />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 3 }}>
                                  <TextField
                                    fullWidth
                                    label="Max Passengers"
                                    type="number"
                                    size="small"
                                    value={tier.maxPassengers}
                                    onChange={(e) => handlePricingTierChange(optionIndex, tierIndex, 'maxPassengers', e.target.value)}
                                    inputProps={{ min: tier.minPassengers || 1 }}
                                  />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 4 }}>
                                  <TextField
                                    fullWidth
                                    label="Price per Person"
                                    type="number"
                                    size="small"
                                    value={tier.pricePerPerson}
                                    onChange={(e) => handlePricingTierChange(optionIndex, tierIndex, 'pricePerPerson', e.target.value)}
                                    inputProps={{ min: 0, step: 0.01 }}
                                  />
                                </Grid>
                                <Grid size={{ xs: 12, sm: 2 }}>
                                  <IconButton
                                    onClick={() => handleRemovePricingTier(optionIndex, tierIndex)}
                                    color="error"
                                    size="small"
                                  >
                                    <DeleteIcon />
                                  </IconButton>
                                </Grid>
                              </Grid>
                            </Card>
                          ))}
                        </Box>
                      ) : (
                        <Box sx={{ textAlign: 'center', py: 2, bgcolor: 'background.default', borderRadius: 1 }}>
                          <Typography variant="body2" color="text.secondary">
                            No pricing tiers. Base price will apply to all group sizes.
                          </Typography>
                        </Box>
                      )}
                    </Grid>
                  </Grid>
                </AccordionDetails>
              </Accordion>
            ))}
          </Box>
        )}
      </CardContent>
    </Card>
  );
}
