'use client';

import { useEffect, useState } from 'react';
import {
  Container,
  Typography,
  Grid,
  Box,
  CircularProgress,
  Alert,
  TextField,
  InputAdornment,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Chip,
} from '@mui/material';
import { Search } from '@mui/icons-material';
import Header from '@/components/Header';
import TourCard from '@/components/TourCard';
import { getTours } from '@/lib/api';

export default function ToursPage() {
  const [tours, setTours] = useState([]);
  const [filteredTours, setFilteredTours] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');
  const [difficultyFilter, setDifficultyFilter] = useState('all');
  const [sortBy, setSortBy] = useState('name');

  useEffect(() => {
    fetchTours();
  }, []);

  useEffect(() => {
    filterAndSortTours();
  }, [tours, searchQuery, difficultyFilter, sortBy]);

  const fetchTours = async () => {
    try {
      setLoading(true);
      const response = await getTours();

      if (response.status === 200 && response.data) {
        setTours(response.data);
        setFilteredTours(response.data);
      } else {
        setError('Failed to load tours');
      }
    } catch (err) {
      console.error('Error fetching tours:', err);
      setError(err.message || 'Failed to load tours. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  const filterAndSortTours = () => {
    let filtered = [...tours];

    // Search filter
    if (searchQuery) {
      filtered = filtered.filter(tour =>
        tour.title?.toLowerCase().includes(searchQuery.toLowerCase()) ||
        tour.description?.toLowerCase().includes(searchQuery.toLowerCase()) ||
        tour.location?.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    // Difficulty filter
    if (difficultyFilter !== 'all') {
      filtered = filtered.filter(tour =>
        tour.difficulty?.toLowerCase() === difficultyFilter.toLowerCase()
      );
    }

    // Sort
    filtered.sort((a, b) => {
      switch (sortBy) {
        case 'name':
          return (a.title || '').localeCompare(b.title || '');
        case 'price-low':
          return (a.price || 0) - (b.price || 0);
        case 'price-high':
          return (b.price || 0) - (a.price || 0);
        case 'rating':
          return (b.ratingsAverage || 0) - (a.ratingsAverage || 0);
        case 'duration':
          return (a.duration || 0) - (b.duration || 0);
        default:
          return 0;
      }
    });

    setFilteredTours(filtered);
  };

  return (
    <>
      <Header />

      {/* Hero Section */}
      <Box
        sx={{
          background: 'linear-gradient(135deg, #5D87FF 0%, #49BEFF 100%)',
          color: 'white',
          py: 8,
          mb: 6,
        }}
      >
        <Container maxWidth="lg">
          <Typography variant="h2" component="h1" gutterBottom fontWeight={700} textAlign="center">
            Discover Amazing Tours
          </Typography>
          <Typography variant="h5" textAlign="center" sx={{ opacity: 0.9 }}>
            Explore the world with our curated selection of tours
          </Typography>
        </Container>
      </Box>

      <Container maxWidth="xl" sx={{ mb: 8 }}>
        {/* Filters */}
        <Box sx={{ mb: 4 }}>
          <Grid container spacing={2}>
            <Grid item xs={12} md={6}>
              <TextField
                fullWidth
                placeholder="Search tours..."
                value={searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                InputProps={{
                  startAdornment: (
                    <InputAdornment position="start">
                      <Search />
                    </InputAdornment>
                  ),
                }}
              />
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <FormControl fullWidth>
                <InputLabel>Difficulty</InputLabel>
                <Select
                  value={difficultyFilter}
                  label="Difficulty"
                  onChange={(e) => setDifficultyFilter(e.target.value)}
                >
                  <MenuItem value="all">All Levels</MenuItem>
                  <MenuItem value="easy">Easy</MenuItem>
                  <MenuItem value="medium">Medium</MenuItem>
                  <MenuItem value="difficult">Difficult</MenuItem>
                </Select>
              </FormControl>
            </Grid>
            <Grid item xs={12} sm={6} md={3}>
              <FormControl fullWidth>
                <InputLabel>Sort By</InputLabel>
                <Select
                  value={sortBy}
                  label="Sort By"
                  onChange={(e) => setSortBy(e.target.value)}
                >
                  <MenuItem value="name">Name</MenuItem>
                  <MenuItem value="price-low">Price: Low to High</MenuItem>
                  <MenuItem value="price-high">Price: High to Low</MenuItem>
                  <MenuItem value="rating">Highest Rated</MenuItem>
                  <MenuItem value="duration">Duration</MenuItem>
                </Select>
              </FormControl>
            </Grid>
          </Grid>
        </Box>

        {/* Results Count */}
        <Box sx={{ mb: 3, display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <Typography variant="h6" color="text.secondary">
            {filteredTours.length} {filteredTours.length === 1 ? 'tour' : 'tours'} found
          </Typography>
          {(searchQuery || difficultyFilter !== 'all') && (
            <Box sx={{ display: 'flex', gap: 1 }}>
              {searchQuery && (
                <Chip
                  label={`Search: ${searchQuery}`}
                  onDelete={() => setSearchQuery('')}
                  color="primary"
                  variant="outlined"
                />
              )}
              {difficultyFilter !== 'all' && (
                <Chip
                  label={`Difficulty: ${difficultyFilter}`}
                  onDelete={() => setDifficultyFilter('all')}
                  color="primary"
                  variant="outlined"
                />
              )}
            </Box>
          )}
        </Box>

        {/* Loading State */}
        {loading && (
          <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}>
            <CircularProgress />
          </Box>
        )}

        {/* Error State */}
        {error && !loading && (
          <Alert severity="error" sx={{ mb: 4 }}>
            {error}
          </Alert>
        )}

        {/* Empty State */}
        {!loading && !error && filteredTours.length === 0 && (
          <Box sx={{ textAlign: 'center', py: 8 }}>
            <Typography variant="h5" color="text.secondary" gutterBottom>
              No tours found
            </Typography>
            <Typography color="text.secondary">
              Try adjusting your search or filters
            </Typography>
          </Box>
        )}

        {/* Tours Grid */}
        {!loading && !error && filteredTours.length > 0 && (
          <Grid container spacing={3}>
            {filteredTours.map((tour) => (
              <Grid item xs={12} sm={6} md={4} lg={3} key={tour._id}>
                <TourCard tour={tour} />
              </Grid>
            ))}
          </Grid>
        )}
      </Container>
    </>
  );
}
