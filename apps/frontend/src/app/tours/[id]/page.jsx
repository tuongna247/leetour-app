'use client';

import React, { useState, useEffect } from 'react';
import {
  Box,
  Container,
  Grid,
  Card,
  CardContent,
  Typography,
  Button,
  Chip,
  Rating,
  Avatar,
  Divider,
  List,
  ListItem,
  ListItemIcon,
  ListItemText,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  Skeleton,
  IconButton,
  LinearProgress,
  TextField,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Breadcrumbs as MuiBreadcrumbs,
  Link,
  Alert,
  CircularProgress
} from '@mui/material';
import {
  LocationOn as LocationIcon,
  AccessTime as TimeIcon,
  Phone as PhoneIcon,
  Group as GroupIcon,
  CheckCircle as CheckIcon,
  Cancel as CancelIcon,
  Star as StarIcon,
  Share as ShareIcon,
  Print as PrintIcon,
  ChevronLeft as ChevronLeftIcon,
  ChevronRight as ChevronRightIcon,
  NavigateNext as NavigateNextIcon
} from '@mui/icons-material';
import { useRouter, useParams } from 'next/navigation';
import { styled } from '@mui/material/styles';
import TourPricingOptions from '@/app/components/TourPricingOptions';

// Custom styled components
const StyledCard = styled(Card)(({ theme }) => ({
  marginBottom: theme.spacing(2),
}));

const ImageSliderContainer = styled(Box)(({ theme }) => ({
  position: 'relative',
  width: '100%',
  height: 470,
  overflow: 'hidden',
  borderRadius: theme.spacing(1),
  [theme.breakpoints.down('md')]: {
    height: 300,
  },
}));

const BookingSidebar = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  backgroundColor: '#f5f5f5',
  border: '1px solid #d9d9d9',
  position: 'sticky',
  top: 20,
}));

const ReviewRatingBar = styled(Box)(({ theme }) => ({
  display: 'flex',
  alignItems: 'center',
  marginBottom: theme.spacing(1.5),
}));

const TourDetailPage = () => {
  const router = useRouter();
  const params = useParams();
  const { id } = params;

  const [tour, setTour] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [currentImageIndex, setCurrentImageIndex] = useState(0);
  const [selectedDate, setSelectedDate] = useState('');
  const [selectedPeople, setSelectedPeople] = useState(2);
  const [childrenCount, setChildrenCount] = useState(0);
  const [similarTours, setSimilarTours] = useState([]);

  // Pricing state
  const [pricingData, setPricingData] = useState(null);
  const [pricingLoading, setPricingLoading] = useState(false);
  const [pricingError, setPricingError] = useState(null);
  const [showPricing, setShowPricing] = useState(false);

  useEffect(() => {
    if (id) {
      fetchTour();
    }
  }, [id]);

  const fetchTour = async () => {
    setLoading(true);
    try {
      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';
      const response = await fetch(`${apiUrl}/api/tours/${id}`);
      const data = await response.json();

      if (data.status === 200) {
        setTour(data.data);
        // Fetch similar tours if needed
        fetchSimilarTours(data.data.category);
      } else {
        setError(data.msg);
      }
    } catch (err) {
      setError('Failed to fetch tour details');
      console.error('Error fetching tour:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchSimilarTours = async (category) => {
    try {
      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';
      const response = await fetch(`${apiUrl}/api/tours?category=${category}&limit=4`);
      const data = await response.json();
      if (data.status === 200) {
        setSimilarTours(data.data.filter(t => t._id !== id).slice(0, 4));
      }
    } catch (err) {
      console.error('Error fetching similar tours:', err);
    }
  };

  const handlePrint = () => {
    window.print();
  };

  const handleCheckAvailability = async () => {
    // Validate date selection
    if (!selectedDate) {
      setPricingError('Please select a departure date');
      return;
    }

    setPricingLoading(true);
    setPricingError(null);
    setShowPricing(false);

    try {
      const apiUrl = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:3001';
      const queryParams = new URLSearchParams({
        date: selectedDate,
        adults: selectedPeople.toString(),
        children: childrenCount.toString()
      });

      const response = await fetch(`${apiUrl}/api/tours/${id}/pricing?${queryParams.toString()}`);
      const data = await response.json();

      if (data.status === 200) {
        setPricingData(data.data);
        setShowPricing(true);
        // Scroll to pricing section
        setTimeout(() => {
          const pricingSection = document.getElementById('tour-pricing-section');
          if (pricingSection) {
            pricingSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
          }
        }, 100);
      } else {
        setPricingError(data.msg || 'Failed to fetch pricing information');
      }
    } catch (err) {
      console.error('Error fetching pricing:', err);
      setPricingError('Failed to fetch pricing information. Please try again.');
    } finally {
      setPricingLoading(false);
    }
  };

  const handleBooking = async (bookingData) => {
    try {
      console.log('Booking data:', bookingData);
      // Navigate to booking page with all the data
      const queryParams = new URLSearchParams({
        date: bookingData.departureDate,
        time: bookingData.departureTime,
        adults: bookingData.adults.toString(),
        children: bookingData.children.toString(),
        optionId: bookingData.optionId,
        optionName: bookingData.optionName,
        totalPrice: bookingData.totalPrice.toString()
      });
      router.push(`/tours/${id}/booking?${queryParams.toString()}`);
    } catch (error) {
      console.error('Booking error:', error);
      alert('Failed to proceed with booking. Please try again.');
    }
  };

  const handlePrevImage = () => {
    setCurrentImageIndex((prev) =>
      prev === 0 ? (tour?.images?.length || 1) - 1 : prev - 1
    );
  };

  const handleNextImage = () => {
    setCurrentImageIndex((prev) =>
      prev === (tour?.images?.length || 1) - 1 ? 0 : prev + 1
    );
  };

  const calculateStarPercentages = (reviews) => {
    if (!reviews || reviews.length === 0) {
      return { star1: 0, star2: 0, star3: 0, star4: 0, star5: 0 };
    }

    const total = reviews.length;
    return {
      star1: ((reviews.filter(r => r.rating === 1).length / total) * 100).toFixed(0),
      star2: ((reviews.filter(r => r.rating === 2).length / total) * 100).toFixed(0),
      star3: ((reviews.filter(r => r.rating === 3).length / total) * 100).toFixed(0),
      star4: ((reviews.filter(r => r.rating === 4).length / total) * 100).toFixed(0),
      star5: ((reviews.filter(r => r.rating === 5).length / total) * 100).toFixed(0),
    };
  };

  if (loading) {
    return (
      <Container maxWidth="lg" sx={{ py: 4 }}>
        <Skeleton variant="rectangular" height={60} sx={{ mb: 2 }} />
        <Skeleton variant="rectangular" height={470} sx={{ mb: 3 }} />
        <Grid container spacing={3}>
          <Grid item xs={12} md={8}>
            <Skeleton variant="rectangular" height={500} />
          </Grid>
          <Grid item xs={12} md={4}>
            <Skeleton variant="rectangular" height={400} />
          </Grid>
        </Grid>
      </Container>
    );
  }

  if (error || !tour) {
    return (
      <Container maxWidth="lg" sx={{ py: 4, textAlign: 'center' }}>
        <Typography variant="h4" gutterBottom>
          Tour Not Found
        </Typography>
        <Typography variant="body1" color="text.secondary" sx={{ mb: 3 }}>
          {error || 'No content for this page. Please contact support.'}
        </Typography>
        <Button variant="contained" onClick={() => router.push('/tours')}>
          Browse All Tours
        </Button>
      </Container>
    );
  }

  const tourImages = tour.images || [];
  const currentImage = tourImages[currentImageIndex] || {
    url: '/images/tours/default-tour.jpg',
    alt: tour.title
  };
  const reviews = tour.reviews || [];
  const averageRating = reviews.length > 0
    ? (reviews.reduce((sum, r) => sum + r.rating, 0) / reviews.length).toFixed(1)
    : 0;
  const starPercentages = calculateStarPercentages(reviews);

  return (
    <Box sx={{ backgroundColor: 'white', minHeight: '100vh' }}>
      <Container maxWidth="lg" sx={{ py: 2 }}>
        {/* Breadcrumbs */}
        <MuiBreadcrumbs
          separator={<NavigateNextIcon fontSize="small" />}
          aria-label="breadcrumb"
          sx={{ mb: 2 }}
        >
          <Link underline="hover" color="inherit" href="/">
            Home
          </Link>
          <Link underline="hover" color="inherit" href="/tours">
            Tours &amp; Day Trips
          </Link>
          <Typography color="text.primary">{tour.title}</Typography>
        </MuiBreadcrumbs>

        {/* Tour Title and Info */}
        <Box sx={{ mb: 3 }}>
          <Typography
            variant="h4"
            component="h2"
            sx={{
              fontSize: { xs: '24px', md: '30px' },
              fontWeight: 600,
              mb: 1
            }}
          >
            {tour.title}
          </Typography>

          <Box sx={{ display: 'flex', alignItems: 'center', mb: 2 }}>
            <Rating value={parseFloat(averageRating)} precision={0.1} readOnly size="small" />
            <Typography variant="body2" color="text.secondary" sx={{ ml: 1 }}>
              - {reviews.length} Reviews -
            </Typography>
          </Box>

          <Box sx={{ display: 'flex', alignItems: 'center', gap: 2, flexWrap: 'wrap' }}>
            <Box sx={{ display: 'flex', alignItems: 'center' }}>
              <LocationIcon fontSize="small" sx={{ mr: 0.5 }} />
              <Typography variant="body1">
                <strong>Location:</strong> {tour.location?.city || tour.location}, {tour.location?.country || ''}
              </Typography>
            </Box>
            <Box
              sx={{ display: 'flex', alignItems: 'center', cursor: 'pointer' }}
              onClick={handlePrint}
              className="hidden-print"
            >
              <PrintIcon fontSize="small" sx={{ mr: 0.5 }} />
              <Typography variant="body2">
                Print e-brochure here
              </Typography>
            </Box>
          </Box>
        </Box>

        {/* Main Content Grid */}
        <Grid container spacing={3}>
          {/* Image Slider */}
          <Grid item xs={12} md={8}>
            {tourImages.length > 0 && (
              <ImageSliderContainer>
                <Box
                  component="img"
                  sx={{
                    width: '100%',
                    height: '100%',
                    objectFit: 'cover',
                  }}
                  alt={currentImage.alt || tour.title}
                  src={currentImage.url}
                />

                {tourImages.length > 1 && (
                  <>
                    <IconButton
                      onClick={handlePrevImage}
                      sx={{
                        position: 'absolute',
                        left: 16,
                        top: '50%',
                        transform: 'translateY(-50%)',
                        backgroundColor: 'rgba(255, 255, 255, 0.9)',
                        '&:hover': {
                          backgroundColor: 'rgba(255, 255, 255, 1)',
                        },
                      }}
                    >
                      <ChevronLeftIcon />
                    </IconButton>
                    <IconButton
                      onClick={handleNextImage}
                      sx={{
                        position: 'absolute',
                        right: 16,
                        top: '50%',
                        transform: 'translateY(-50%)',
                        backgroundColor: 'rgba(255, 255, 255, 0.9)',
                        '&:hover': {
                          backgroundColor: 'rgba(255, 255, 255, 1)',
                        },
                      }}
                    >
                      <ChevronRightIcon />
                    </IconButton>

                    {/* Thumbnail Dots */}
                    <Box
                      sx={{
                        position: 'absolute',
                        bottom: 16,
                        left: '50%',
                        transform: 'translateX(-50%)',
                        display: 'flex',
                        gap: 1,
                      }}
                    >
                      {tourImages.map((_, index) => (
                        <Box
                          key={index}
                          onClick={() => setCurrentImageIndex(index)}
                          sx={{
                            width: 10,
                            height: 10,
                            borderRadius: '50%',
                            backgroundColor: index === currentImageIndex
                              ? 'white'
                              : 'rgba(255, 255, 255, 0.5)',
                            cursor: 'pointer',
                          }}
                        />
                      ))}
                    </Box>
                  </>
                )}
              </ImageSliderContainer>
            )}
          </Grid>

          {/* Booking Sidebar */}
          <Grid item xs={12} md={4} className="hidden-print">
            <BookingSidebar>
              <Typography
                variant="h6"
                sx={{
                  fontSize: '17px',
                  fontWeight: 'bold',
                  textAlign: 'center',
                  mb: 2
                }}
              >
                Select Date and Travelers
              </Typography>

              <TextField
                fullWidth
                type="date"
                label="Departure Date"
                value={selectedDate}
                onChange={(e) => setSelectedDate(e.target.value)}
                InputLabelProps={{ shrink: true }}
                sx={{ mb: 2, backgroundColor: 'white' }}
              />

              <FormControl fullWidth sx={{ mb: 2 }}>
                <InputLabel>Number of People</InputLabel>
                <Select
                  value={selectedPeople}
                  onChange={(e) => setSelectedPeople(e.target.value)}
                  label="Number of People"
                  sx={{ backgroundColor: 'white' }}
                >
                  {[1, 2, 3, 4, 5, 6, 7, 8, 9, 10].map(num => (
                    <MenuItem key={num} value={num}>{num} {num === 1 ? 'Person' : 'People'}</MenuItem>
                  ))}
                </Select>
              </FormControl>

              <Typography variant="body2" sx={{ mb: 1 }}>
                Children: (from 4 to 8 years old)
              </Typography>
              <TextField
                fullWidth
                type="number"
                value={childrenCount}
                onChange={(e) => setChildrenCount(Math.max(0, parseInt(e.target.value) || 0))}
                inputProps={{ min: 0 }}
                sx={{ mb: 2, backgroundColor: 'white' }}
              />

              <Button
                variant="contained"
                fullWidth
                onClick={handleCheckAvailability}
                disabled={pricingLoading || !selectedDate}
                sx={{
                  mb: 1,
                  backgroundColor: '#d32f2f',
                  '&:hover': {
                    backgroundColor: '#b71c1c',
                  }
                }}
              >
                {pricingLoading ? (
                  <>
                    <CircularProgress size={20} color="inherit" sx={{ mr: 1 }} />
                    Checking...
                  </>
                ) : (
                  'Check Availability'
                )}
              </Button>

              {pricingError && (
                <Alert severity="error" sx={{ mb: 2, fontSize: '0.875rem' }}>
                  {pricingError}
                </Alert>
              )}

              <Typography variant="body2" sx={{ textAlign: 'center', my: 1 }}>
                or
              </Typography>

              <Button
                variant="contained"
                fullWidth
                onClick={() => router.push('/inquiry')}
              >
                Customise your Itinerary
              </Button>
            </BookingSidebar>
          </Grid>
        </Grid>

        {/* Tour Duration and Contact Info */}
        <Box sx={{ mt: 3, border: '1px solid #e0e0e0', borderRadius: 1 }}>
          <Divider />
          <Box sx={{ display: 'flex', flexWrap: 'wrap', p: 2, fontSize: '16px' }}>
            <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 33%' }, display: 'flex', alignItems: 'center', mb: { xs: 1, md: 0 } }}>
              <TimeIcon sx={{ mr: 1 }} />
              <Typography>
                {tour.duration} - {tour.type === 'daytrip' ? 'Day Trip' : 'Tour'}
              </Typography>
            </Box>
            <Box sx={{ flex: { xs: '1 1 100%', md: '1 1 67%' }, display: 'flex', alignItems: 'center' }}>
              <PhoneIcon sx={{ mr: 1 }} />
              <Typography>
                Book online or call: +84 916 388 382
              </Typography>
            </Box>
          </Box>
          <Divider />
        </Box>

        {/* Tour Pricing Options Section */}
        {showPricing && pricingData && (
          <Box id="tour-pricing-section" sx={{ mt: 4 }}>
            <TourPricingOptions
              tourId={id}
              bookingDate={selectedDate}
              adults={selectedPeople}
              children={childrenCount}
              pricingData={pricingData}
              onBook={handleBooking}
            />
          </Box>
        )}

        {/* Main Tour Content */}
        <Box sx={{ mt: 4 }}>
          {/* Why You'll Love This Trip */}
          <Typography variant="h5" sx={{ mt: 4, mb: 2, fontWeight: 600 }}>
            Why you&apos;ll love this trip
          </Typography>
          <Typography
            variant="body1"
            sx={{ textAlign: 'justify', mb: 3 }}
            dangerouslySetInnerHTML={{ __html: tour.overview || tour.description }}
          />

          {/* Brief Itinerary Table */}
          {tour.itinerary && tour.itinerary.length > 1 && (
            <>
              <Typography variant="h5" sx={{ mb: 2, fontWeight: 600 }}>
                Brief Itinerary
              </Typography>
              <TableContainer component={Paper} sx={{ mb: 3 }}>
                <Table>
                  <TableHead>
                    <TableRow>
                      <TableCell><strong>Day</strong></TableCell>
                      <TableCell><strong>Highlights</strong></TableCell>
                      <TableCell><strong>Overnight</strong></TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {tour.itinerary.map((day, index) => (
                      <TableRow key={index}>
                        <TableCell>{index + 1}</TableCell>
                        <TableCell>{day.activity || day.title}</TableCell>
                        <TableCell>{day.overnight || '-'}</TableCell>
                      </TableRow>
                    ))}
                  </TableBody>
                </Table>
              </TableContainer>
            </>
          )}

          {/* Detailed Itinerary */}
          <Typography variant="h5" sx={{ mb: 2, fontWeight: 600 }}>
            Detailed Itinerary of {tour.title}
          </Typography>
          {tour.itinerary && tour.itinerary.map((day, index) => (
            <Box key={index} sx={{ mb: 3 }}>
              {tour.itinerary.length > 1 && (
                <Typography
                  variant="h6"
                  sx={{
                    color: '#000',
                    textDecoration: 'underline',
                    mb: 1
                  }}
                >
                  Day {index + 1} - {day.activity || day.title}
                </Typography>
              )}

              {day.image && (
                <Box
                  component="img"
                  src={day.image}
                  alt={`Day ${index + 1}`}
                  sx={{ width: '100%', mb: 2, borderRadius: 1 }}
                />
              )}

              <Typography
                variant="body1"
                sx={{ mb: 2 }}
                dangerouslySetInnerHTML={{ __html: day.description }}
              />

              <Box sx={{ display: 'flex', gap: 4, flexWrap: 'wrap' }}>
                {day.meal && (
                  <Typography variant="body1">
                    <strong>Meal:</strong> {day.meal}
                  </Typography>
                )}
                {day.transport && (
                  <Typography variant="body1">
                    <strong>Transport:</strong> {day.transport}
                  </Typography>
                )}
              </Box>
            </Box>
          ))}

          {/* Price Check CTA */}
          <Box sx={{ mb: 3, p: 2, backgroundColor: '#f5f5f5', borderRadius: 1 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap' }}>
              <Typography variant="h6">Prices:</Typography>
              <Button
                variant="contained"
                color="error"
                href="#book-tours"
                sx={{ ml: 2 }}
              >
                Check Price Now
              </Button>
            </Box>
          </Box>

          {/* Included */}
          <Box sx={{ mb: 3 }}>
            <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
              Included:
            </Typography>
            <Box dangerouslySetInnerHTML={{ __html: tour.includeActivity || '' }} />
            {tour.included && (
              <List dense>
                {tour.included.map((item, index) => (
                  <ListItem key={index}>
                    <ListItemIcon>
                      <CheckIcon color="success" fontSize="small" />
                    </ListItemIcon>
                    <ListItemText primary={item} />
                  </ListItem>
                ))}
              </List>
            )}
          </Box>

          {/* Excluded */}
          <Box sx={{ mb: 3 }}>
            <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
              Excluded:
            </Typography>
            <Box dangerouslySetInnerHTML={{ __html: tour.excludeActivity || '' }} />
            {tour.notIncluded && (
              <List dense>
                {tour.notIncluded.map((item, index) => (
                  <ListItem key={index}>
                    <ListItemIcon>
                      <CancelIcon color="error" fontSize="small" />
                    </ListItemIcon>
                    <ListItemText primary={item} />
                  </ListItem>
                ))}
              </List>
            )}
          </Box>

          {/* Notes */}
          {tour.notes && (
            <Box sx={{ mb: 3 }}>
              <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
                Notes:
              </Typography>
              <Box dangerouslySetInnerHTML={{ __html: tour.notes }} />
            </Box>
          )}

          {/* Keywords */}
          {tour.keywords && tour.keywords.length > 0 && (
            <Box sx={{ mb: 3 }} className="hidden-print">
              <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
                Keywords:
              </Typography>
              <Box sx={{ display: 'flex', gap: 1, flexWrap: 'wrap' }}>
                {tour.keywords.map((keyword, index) => (
                  <Chip
                    key={index}
                    label={keyword}
                    component="a"
                    href={`/search?keyword=${keyword}`}
                    clickable
                    size="small"
                  />
                ))}
              </Box>
            </Box>
          )}
        </Box>

        {/* Reviews Section */}
        <Box sx={{ mt: 5 }}>
          <Typography variant="h5" sx={{ mb: 3, fontWeight: 600 }}>
            Reviews
          </Typography>

          <Grid container spacing={3} sx={{ mb: 4 }}>
            {/* Overall Rating */}
            <Grid item xs={12} md={4}>
              <Box sx={{ textAlign: 'center', borderRight: { md: '1px solid #f7f7f7' }, pr: 2 }}>
                <Rating
                  value={parseFloat(averageRating)}
                  precision={0.1}
                  readOnly
                  size="large"
                  sx={{ mb: 1 }}
                />
                <Typography variant="h3" color="error" sx={{ fontWeight: 500 }}>
                  {averageRating}/5
                </Typography>
                <Typography variant="body2" color="text.secondary">
                  Based on {reviews.length} reviews
                </Typography>
              </Box>
            </Grid>

            {/* Rating Breakdown */}
            <Grid item xs={12} md={4}>
              <Box sx={{ borderRight: { md: '1px solid #f7f7f7' }, pr: 2 }}>
                {[5, 4, 3, 2, 1].map((star) => (
                  <ReviewRatingBar key={star}>
                    <Typography variant="body2" sx={{ width: 50 }}>
                      {star} <StarIcon fontSize="small" sx={{ verticalAlign: 'middle' }} />
                    </Typography>
                    <LinearProgress
                      variant="determinate"
                      value={parseFloat(starPercentages[`star${star}`]) || 0}
                      sx={{
                        flex: 1,
                        mx: 1,
                        height: 10,
                        borderRadius: 1,
                        backgroundColor: '#efefef',
                        '& .MuiLinearProgress-bar': {
                          backgroundColor: '#4caf50',
                        }
                      }}
                    />
                    <Typography variant="body2" sx={{ width: 50, textAlign: 'right' }}>
                      {starPercentages[`star${star}`]}%
                    </Typography>
                  </ReviewRatingBar>
                ))}
              </Box>
            </Grid>

            {/* Write Review Button */}
            <Grid item xs={12} md={4}>
              <Box sx={{ textAlign: 'center', display: 'flex', alignItems: 'center', justifyContent: 'center', height: '100%' }}>
                <Button
                  variant="contained"
                  onClick={() => router.push(`/tours/${id}/review`)}
                >
                  Write a Review
                </Button>
              </Box>
            </Grid>
          </Grid>

          {/* Individual Reviews */}
          {reviews.map((review, index) => (
            <Card key={index} sx={{ mb: 2, borderTop: '1px solid #f4f4f4' }}>
              <CardContent>
                <Grid container spacing={2}>
                  <Grid item xs={12} sm={2}>
                    <Box sx={{ textAlign: 'center' }}>
                      <Avatar
                        sx={{
                          width: 65,
                          height: 65,
                          margin: '0 auto',
                          backgroundColor: '#d3d2d3',
                          color: '#919090'
                        }}
                      >
                        {review.user?.name?.charAt(0) || review.guestName?.charAt(0) || 'G'}
                      </Avatar>
                      <Typography variant="body2" sx={{ mt: 1, fontWeight: 500 }}>
                        {review.user?.name || review.guestName || 'Guest'}
                      </Typography>
                    </Box>
                  </Grid>
                  <Grid item xs={12} sm={10}>
                    <Rating value={review.rating} readOnly size="small" sx={{ mb: 1 }} />
                    <Typography variant="subtitle1" sx={{ fontWeight: 500, mb: 1 }}>
                      {review.title}
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                      {review.comment || review.reviewContent}
                    </Typography>
                  </Grid>
                </Grid>
              </CardContent>
            </Card>
          ))}
        </Box>

        {/* Similar Tours Section */}
        {similarTours.length > 0 && (
          <Box sx={{ mt: 5, mb: 5 }}>
            <Typography variant="h5" sx={{ mb: 3, fontWeight: 600 }}>
              You might also like...
            </Typography>
            <Grid container spacing={3}>
              {similarTours.map((similarTour) => (
                <Grid item xs={12} sm={6} md={3} key={similarTour._id}>
                  <Card
                    sx={{
                      height: '100%',
                      cursor: 'pointer',
                      '&:hover': {
                        boxShadow: 4,
                      }
                    }}
                    onClick={() => router.push(`/tours/${similarTour._id}`)}
                  >
                    <Box
                      component="img"
                      src={similarTour.images?.[0]?.url || '/images/tours/default-tour.jpg'}
                      alt={similarTour.title}
                      sx={{
                        width: '100%',
                        height: 150,
                        objectFit: 'cover',
                      }}
                    />
                    <CardContent>
                      <Typography variant="body1" sx={{ fontSize: '14px', mb: 1, minHeight: 40 }}>
                        {similarTour.title}
                      </Typography>
                      <Typography variant="h6" color="primary" sx={{ mb: 1 }}>
                        {similarTour.price ? (
                          <>
                            <Typography variant="caption" component="span">From USD </Typography>
                            <strong>${similarTour.price}</strong>
                          </>
                        ) : (
                          'On request'
                        )}
                      </Typography>
                      <Link
                        href={`/tours/${similarTour._id}`}
                        underline="hover"
                        sx={{ fontSize: '14px' }}
                      >
                        {similarTour.price ? 'Book now »' : 'Detail »'}
                      </Link>
                    </CardContent>
                  </Card>
                </Grid>
              ))}
            </Grid>
          </Box>
        )}
      </Container>
    </Box>
  );
};

export default TourDetailPage;
