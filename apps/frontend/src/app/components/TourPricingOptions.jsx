'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  Radio,
  RadioGroup,
  FormControlLabel,
  FormControl,
  Chip,
  Alert,
  CircularProgress,
  Divider
} from '@mui/material';
import { styled } from '@mui/material/styles';
import { useRouter } from 'next/navigation';

// Styled Components
const OptionCard = styled(Card)(({ theme, selected }) => ({
  marginBottom: theme.spacing(2),
  border: selected ? `2px solid ${theme.palette.primary.main}` : '1px solid #d9d9d9',
  backgroundColor: selected ? '#fff' : 'transparent',
  cursor: 'pointer',
  transition: 'all 0.3s ease',
  '&:hover': {
    boxShadow: theme.shadows[4],
  }
}));

const PopularBadge = styled(Box)(({ theme }) => ({
  backgroundColor: '#eb7200',
  color: '#fff',
  fontSize: '0.7rem',
  padding: '4px 7px',
  display: 'inline-block',
  marginBottom: theme.spacing(0.5),
}));

const TimeSlot = styled(Box)(({ theme, selected, disabled }) => ({
  border: disabled ? '1px solid #ccc' : '1px solid #007d9e',
  borderRadius: theme.spacing(0.5),
  padding: '8px 16px',
  margin: theme.spacing(0.5),
  cursor: disabled ? 'not-allowed' : 'pointer',
  fontWeight: 500,
  textAlign: 'center',
  whiteSpace: 'nowrap',
  minWidth: '105px',
  backgroundColor: selected ? '#007d9e' : 'transparent',
  color: selected ? '#fff' : disabled ? '#999' : '#007d9e',
  opacity: disabled ? 0.5 : 1,
  '&:hover': {
    backgroundColor: disabled ? 'transparent' : selected ? '#007d9e' : '#e0f7fa',
  }
}));

const BookButton = styled(Button)(({ theme }) => ({
  backgroundColor: '#ef4904',
  color: '#fff',
  fontSize: '16px',
  padding: '10px 24px',
  '&:hover': {
    backgroundColor: '#d33d03',
  },
  '&:disabled': {
    backgroundColor: '#ccc',
  }
}));

/**
 * TourPricingOptions Component
 * Displays tour pricing options with time selection and booking
 * Equivalent to _TourRateOptions.cshtml
 */
const TourPricingOptions = ({
  tourId,
  bookingDate,
  adults,
  children,
  pricingData,
  onBook
}) => {
  const router = useRouter();
  const [selectedOption, setSelectedOption] = useState(null);
  const [selectedTime, setSelectedTime] = useState({});
  const [showTimeError, setShowTimeError] = useState({});
  const [loading, setLoading] = useState(false);

  // Auto-select first option if available
  useEffect(() => {
    if (pricingData?.options && pricingData.options.length > 0) {
      setSelectedOption(pricingData.options[0].id);
    }
  }, [pricingData]);

  if (!pricingData || !pricingData.options || pricingData.options.length === 0) {
    return (
      <Alert severity="warning">
        No pricing options available for the selected date and passenger count.
      </Alert>
    );
  }

  const handleOptionSelect = (optionId) => {
    setSelectedOption(optionId);
    setShowTimeError({});
  };

  const handleTimeSelect = (optionId, time) => {
    setSelectedTime({
      ...selectedTime,
      [optionId]: time
    });
    setShowTimeError({
      ...showTimeError,
      [optionId]: false
    });
  };

  const handleBookNow = async (option) => {
    // Validate time selection
    const time = selectedTime[option.id];
    if (!time) {
      setShowTimeError({
        ...showTimeError,
        [option.id]: true
      });
      return;
    }

    setLoading(true);

    try {
      const bookingData = {
        tourId,
        optionId: option.id,
        optionName: option.name,
        departureDate: bookingDate,
        departureTime: time,
        adults,
        children,
        pricing: option.pricing,
        retailRate: option.pricing.basePrice,
        totalPrice: option.pricing.grandTotal,
        currency: pricingData.currency
      };

      if (onBook) {
        await onBook(bookingData);
      } else {
        // Default: Navigate to booking page
        const queryParams = new URLSearchParams({
          date: bookingDate,
          time: time,
          adults: adults.toString(),
          children: children.toString(),
          option: option.id
        });
        router.push(`/tours/${tourId}/booking?${queryParams.toString()}`);
      }
    } catch (error) {
      console.error('Booking error:', error);
      alert('Failed to proceed with booking. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const formatPrice = (amount) => {
    return new Intl.NumberFormat('vi-VN').format(Math.round(amount));
  };

  const formatDate = (dateString) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      weekday: 'long',
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  // Check if a time slot is in the past
  const isTimePast = (timeString) => {
    const now = new Date();
    const selected = new Date(bookingDate);

    // If not today, all times are valid
    if (selected.toDateString() !== now.toDateString()) {
      return false;
    }

    // Parse time string (e.g., "08:00 AM" or "02:30 PM")
    const timeParts = timeString.match(/(\d+):(\d+)\s*(AM|PM)?/i);
    if (!timeParts) return false;

    let hour = parseInt(timeParts[1]);
    const minute = parseInt(timeParts[2]);
    const meridiem = timeParts[3]?.toUpperCase();

    if (meridiem === 'PM' && hour !== 12) {
      hour += 12;
    } else if (meridiem === 'AM' && hour === 12) {
      hour = 0;
    }

    return hour < now.getHours() || (hour === now.getHours() && minute < now.getMinutes());
  };

  // Parse departure times from semicolon-separated string
  const parseDepartureTimes = (timesString) => {
    if (!timesString) return ['08:00 AM'];
    return timesString.split(';').filter(t => t.trim());
  };

  return (
    <Box sx={{ backgroundColor: '#fff', borderRadius: 2, p: 3, mb: 3 }}>
      {/* Header */}
      <Typography
        variant="h6"
        sx={{
          fontWeight: 600,
          color: '#404040',
          mb: 2,
          fontSize: '18px'
        }}
      >
        <strong>Select an option for</strong> {formatDate(bookingDate)}
      </Typography>

      <input type="hidden" id="hdnBookingDate" value={bookingDate} />

      {/* Options List */}
      {pricingData.options.map((option, index) => {
        const isSelected = selectedOption === option.id;
        const isMostPopular = index === 0;
        const times = parseDepartureTimes(option.departureTimes);

        return (
          <OptionCard
            key={option.id}
            selected={isSelected}
            onClick={() => handleOptionSelect(option.id)}
          >
            <CardContent>
              {/* Option Header */}
              <Box sx={{ display: 'flex', px: 1 }}>
                <FormControl>
                  <Radio
                    checked={isSelected}
                    onChange={() => handleOptionSelect(option.id)}
                    value={option.id}
                    name="tour-option"
                  />
                </FormControl>

                <Box sx={{ flex: 1, ml: 2 }}>
                  {/* Popular Badge */}
                  {isMostPopular && (
                    <PopularBadge>MOST POPULAR</PopularBadge>
                  )}

                  {/* Option Title */}
                  <Typography
                    variant="h6"
                    sx={{
                      fontSize: '18px',
                      fontWeight: 'bold',
                      mb: 1
                    }}
                  >
                    Option {index + 1}: {option.name}
                  </Typography>

                  {/* Pricing */}
                  <Box sx={{ mb: 2 }}>
                    <Typography
                      variant="h5"
                      component="span"
                      sx={{ fontWeight: 500 }}
                    >
                      {formatPrice(option.pricing.basePrice)} VND × {adults} Adult
                      {children > 0 && (
                        <Typography component="span">
                          {' '}+ {formatPrice(option.pricing.children.pricePerChild)} VND × {children} Children
                        </Typography>
                      )}
                    </Typography>
                    <Typography
                      variant="h6"
                      component="span"
                      sx={{ ml: 2, color: 'text.secondary' }}
                    >
                      = {formatPrice(option.pricing.grandTotal)} VND
                    </Typography>
                  </Box>

                  {/* What's Included */}
                  {option.description && (
                    <Box sx={{ mb: 2 }}>
                      <Typography variant="body2" color="text.secondary">
                        <strong>Included:</strong>{' '}
                        <span dangerouslySetInnerHTML={{ __html: option.description }} />
                      </Typography>
                    </Box>
                  )}
                </Box>
              </Box>

              {/* Expanded Details (when selected) */}
              {isSelected && (
                <Box sx={{ mt: 3, pl: { xs: 1, md: 7 }, pr: 3 }}>
                  <Divider sx={{ mb: 2 }} />

                  {/* Time Selection */}
                  <Typography
                    variant="body1"
                    sx={{
                      fontWeight: 500,
                      color: '#404040',
                      mb: 2
                    }}
                  >
                    Select a departure time:
                  </Typography>

                  <Box sx={{ display: 'flex', flexWrap: 'wrap', mb: 2 }}>
                    {times.map((time, idx) => {
                      const isPast = isTimePast(time);
                      const isTimeSelected = selectedTime[option.id] === time;

                      return (
                        <TimeSlot
                          key={idx}
                          selected={isTimeSelected}
                          disabled={isPast}
                          onClick={() => !isPast && handleTimeSelect(option.id, time)}
                        >
                          {time}
                        </TimeSlot>
                      );
                    })}
                  </Box>

                  {showTimeError[option.id] && (
                    <Alert severity="error" sx={{ mb: 2 }}>
                      Please select a time to proceed.
                    </Alert>
                  )}

                  {/* Book Button */}
                  <Box
                    sx={{
                      display: 'flex',
                      justifyContent: 'flex-end',
                      borderTop: '1px solid #e0e0e0',
                      pt: 2
                    }}
                  >
                    <BookButton
                      variant="contained"
                      size="large"
                      onClick={() => handleBookNow(option)}
                      disabled={loading}
                      startIcon={loading && <CircularProgress size={20} color="inherit" />}
                    >
                      {loading ? 'Processing...' : 'Book Now'}
                    </BookButton>
                  </Box>
                </Box>
              )}
            </CardContent>
          </OptionCard>
        );
      })}
    </Box>
  );
};

export default TourPricingOptions;
