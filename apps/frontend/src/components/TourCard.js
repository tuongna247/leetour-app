'use client';

import {
  Card,
  CardContent,
  CardMedia,
  Typography,
  Box,
  Chip,
  Rating,
  Button,
} from '@mui/material';
import {
  LocationOn,
  AccessTime,
  Group,
  Star,
} from '@mui/icons-material';
import { format } from 'date-fns';

export default function TourCard({ tour }) {
  const {
    title,
    description,
    price,
    duration,
    maxGroupSize,
    difficulty,
    rating,
    ratingsAverage,
    ratingsQuantity,
    imageCover,
    startDates,
    location,
    summary,
  } = tour;

  const formatPrice = (price) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
    }).format(price);
  };

  const getDifficultyColor = (difficulty) => {
    switch (difficulty?.toLowerCase()) {
      case 'easy':
        return 'success';
      case 'medium':
        return 'warning';
      case 'difficult':
        return 'error';
      default:
        return 'default';
    }
  };

  return (
    <Card
      sx={{
        height: '100%',
        display: 'flex',
        flexDirection: 'column',
        transition: 'transform 0.2s, box-shadow 0.2s',
        '&:hover': {
          transform: 'translateY(-4px)',
          boxShadow: 6,
        },
      }}
    >
      <CardMedia
        component="div"
        sx={{
          pt: '56.25%', // 16:9 aspect ratio
          position: 'relative',
          backgroundColor: 'grey.300',
        }}
      >
        {imageCover && (
          <Box
            component="img"
            src={imageCover}
            alt={title}
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              objectFit: 'cover',
            }}
          />
        )}
        <Box
          sx={{
            position: 'absolute',
            top: 16,
            right: 16,
          }}
        >
          {difficulty && (
            <Chip
              label={difficulty}
              color={getDifficultyColor(difficulty)}
              size="small"
              sx={{ fontWeight: 600 }}
            />
          )}
        </Box>
      </CardMedia>

      <CardContent sx={{ flexGrow: 1, display: 'flex', flexDirection: 'column' }}>
        <Typography variant="h5" component="h2" gutterBottom fontWeight={600}>
          {title}
        </Typography>

        <Typography
          variant="body2"
          color="text.secondary"
          sx={{
            mb: 2,
            display: '-webkit-box',
            WebkitLineClamp: 2,
            WebkitBoxOrient: 'vertical',
            overflow: 'hidden',
          }}
        >
          {summary || description}
        </Typography>

        <Box sx={{ mb: 2 }}>
          {location && (
            <Box sx={{ display: 'flex', alignItems: 'center', mb: 1 }}>
              <LocationOn sx={{ fontSize: 18, mr: 0.5, color: 'text.secondary' }} />
              <Typography variant="body2" color="text.secondary">
                {location}
              </Typography>
            </Box>
          )}

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
            {duration && (
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <AccessTime sx={{ fontSize: 18, mr: 0.5, color: 'text.secondary' }} />
                <Typography variant="body2" color="text.secondary">
                  {duration} days
                </Typography>
              </Box>
            )}

            {maxGroupSize && (
              <Box sx={{ display: 'flex', alignItems: 'center' }}>
                <Group sx={{ fontSize: 18, mr: 0.5, color: 'text.secondary' }} />
                <Typography variant="body2" color="text.secondary">
                  Up to {maxGroupSize} people
                </Typography>
              </Box>
            )}
          </Box>
        </Box>

        {(ratingsAverage || rating) && (
          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
            <Rating
              value={ratingsAverage || rating || 0}
              precision={0.1}
              readOnly
              size="small"
              sx={{ mr: 1 }}
            />
            <Typography variant="body2" color="text.secondary">
              {(ratingsAverage || rating)?.toFixed(1)} ({ratingsQuantity || 0} reviews)
            </Typography>
          </Box>
        )}

        {startDates && startDates.length > 0 && (
          <Typography variant="body2" color="text.secondary" sx={{ mb: 2 }}>
            Next tour: {format(new Date(startDates[0]), 'MMM dd, yyyy')}
          </Typography>
        )}

        <Box
          sx={{
            mt: 'auto',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
          }}
        >
          <Box>
            <Typography variant="h5" color="primary" fontWeight={700}>
              {formatPrice(price)}
            </Typography>
            <Typography variant="caption" color="text.secondary">
              per person
            </Typography>
          </Box>

          <Button variant="contained" color="primary">
            View Details
          </Button>
        </Box>
      </CardContent>
    </Card>
  );
}
