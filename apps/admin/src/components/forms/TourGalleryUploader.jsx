'use client';

import React, { useState } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Grid,
  IconButton,
  CircularProgress,
  Alert,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  ImageList,
  ImageListItem,
  ImageListItemBar,
  Chip
} from '@mui/material';
import {
  CloudUpload as UploadIcon,
  Delete as DeleteIcon,
  Star as StarIcon,
  StarBorder as StarBorderIcon,
  Image as ImageIcon
} from '@mui/icons-material';

const IMAGE_TYPES = ['Banner', 'Gallery', 'Logo', 'Map'];

export default function TourGalleryUploader({
  tourId,
  featuredImage = { url: '', alt: '' },
  galleryImages = [],
  onFeaturedChange,
  onGalleryChange
}) {
  const [uploading, setUploading] = useState(false);
  const [uploadingFeatured, setUploadingFeatured] = useState(false);
  const [error, setError] = useState('');

  const handleFeaturedUpload = async (e) => {
    const file = e.target.files?.[0];
    if (!file) return;

    try {
      setUploadingFeatured(true);
      setError('');

      const formData = new FormData();
      formData.append('file', file);
      formData.append('imageType', 'featured');
      formData.append('alt', file.name.replace(/\.[^/.]+$/, ''));
      formData.append('isPrimary', 'true');

      const response = await fetch(`/api/admin/tours/${tourId}/images`, {
        method: 'POST',
        body: formData
      });

      const result = await response.json();

      if (result.success) {
        onFeaturedChange({ url: result.data.url, alt: result.data.alt });
      } else {
        throw new Error(result.message || 'Upload failed');
      }
    } catch (err) {
      console.error('Upload error:', err);
      setError(err.message || 'Failed to upload featured image');
    } finally {
      setUploadingFeatured(false);
    }
  };

  const handleGalleryUpload = async (e) => {
    const files = Array.from(e.target.files || []);
    if (files.length === 0) return;

    try {
      setUploading(true);
      setError('');

      const uploadedImages = [];

      for (const file of files) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('imageType', 'gallery');
        formData.append('alt', file.name.replace(/\.[^/.]+$/, ''));

        const response = await fetch(`/api/admin/tours/${tourId}/images`, {
          method: 'POST',
          body: formData
        });

        const result = await response.json();

        if (result.success) {
          uploadedImages.push({
            url: result.data.url,
            alt: result.data.alt,
            name: file.name.replace(/\.[^/.]+$/, ''),
            type: 'Gallery',
            isPrimary: false
          });
        }
      }

      // Add new images to existing gallery
      onGalleryChange([...galleryImages, ...uploadedImages]);
    } catch (err) {
      console.error('Upload error:', err);
      setError(err.message || 'Failed to upload gallery images');
    } finally {
      setUploading(false);
    }
  };

  const handleDeleteFeatured = () => {
    onFeaturedChange({ url: '', alt: '' });
  };

  const handleDeleteGallery = (index) => {
    const updated = galleryImages.filter((_, i) => i !== index);
    onGalleryChange(updated);
  };

  const handleUpdateGalleryImage = (index, field, value) => {
    const updated = galleryImages.map((img, i) =>
      i === index ? { ...img, [field]: value } : img
    );
    onGalleryChange(updated);
  };

  const handleSetPrimary = (index) => {
    const updated = galleryImages.map((img, i) => ({
      ...img,
      isPrimary: i === index
    }));
    onGalleryChange(updated);
  };

  const getImagesByType = (type) => {
    return galleryImages.filter(img => img.type === type);
  };

  return (
    <Box>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError('')}>
          {error}
        </Alert>
      )}

      {/* Featured Image */}
      <Box sx={{ mb: 4, p: 2, border: 1, borderColor: 'divider', borderRadius: 1, bgcolor: 'warning.lighter' }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
          <StarIcon color="warning" />
          <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
            Featured Image (Main)
          </Typography>
        </Box>

        {featuredImage.url ? (
          <Box sx={{ position: 'relative' }}>
            <img
              src={featuredImage.url}
              alt={featuredImage.alt || 'Featured image'}
              style={{
                width: '100%',
                maxHeight: 300,
                objectFit: 'cover',
                borderRadius: 8
              }}
            />
            <IconButton
              onClick={handleDeleteFeatured}
              sx={{
                position: 'absolute',
                top: 8,
                right: 8,
                bgcolor: 'rgba(0,0,0,0.6)',
                color: 'white',
                '&:hover': { bgcolor: 'rgba(0,0,0,0.8)' }
              }}
              size="small"
            >
              <DeleteIcon fontSize="small" />
            </IconButton>
            <Box sx={{ mt: 2 }}>
              <TextField
                fullWidth
                size="small"
                label="Alt Text / Description"
                value={featuredImage.alt}
                onChange={(e) => onFeaturedChange({ ...featuredImage, alt: e.target.value })}
                placeholder="Image description for SEO"
              />
            </Box>
          </Box>
        ) : (
          <Box>
            <Button
              variant="outlined"
              component="label"
              startIcon={uploadingFeatured ? <CircularProgress size={20} /> : <UploadIcon />}
              disabled={uploadingFeatured}
              fullWidth
              sx={{ py: 3 }}
            >
              {uploadingFeatured ? 'Uploading...' : 'Click to Upload Featured Image'}
              <input
                type="file"
                hidden
                accept="image/*"
                onChange={handleFeaturedUpload}
                disabled={uploadingFeatured}
              />
            </Button>
          </Box>
        )}
      </Box>

      {/* Gallery Images */}
      <Box>
        <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 2 }}>
          <Typography variant="h6" gutterBottom sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <ImageIcon /> Gallery Images
          </Typography>
          <Button
            variant="contained"
            component="label"
            startIcon={uploading ? <CircularProgress size={20} color="inherit" /> : <UploadIcon />}
            disabled={uploading}
          >
            {uploading ? 'Uploading...' : 'Upload Images'}
            <input
              type="file"
              hidden
              accept="image/*"
              multiple
              onChange={handleGalleryUpload}
              disabled={uploading}
            />
          </Button>
        </Box>

        {/* Summary by type */}
        <Box sx={{ mb: 2, display: 'flex', gap: 1, flexWrap: 'wrap' }}>
          {IMAGE_TYPES.map(type => {
            const count = getImagesByType(type).length;
            return (
              <Chip
                key={type}
                label={`${type}: ${count}`}
                color={count > 0 ? 'primary' : 'default'}
                size="small"
              />
            );
          })}
        </Box>

        {galleryImages.length === 0 ? (
          <Alert severity="info">
            No gallery images uploaded yet. Click "Upload Images" to add images.
          </Alert>
        ) : (
          <Grid container spacing={2}>
            {galleryImages.map((image, index) => (
              <Grid item xs={12} sm={6} md={4} key={index}>
                <Card variant="outlined">
                  <Box sx={{ position: 'relative' }}>
                    <img
                      src={image.url}
                      alt={image.alt || image.name}
                      style={{
                        width: '100%',
                        height: 200,
                        objectFit: 'cover'
                      }}
                    />
                    <Box sx={{ position: 'absolute', top: 8, right: 8, display: 'flex', gap: 0.5 }}>
                      <IconButton
                        size="small"
                        onClick={() => handleSetPrimary(index)}
                        sx={{
                          bgcolor: 'rgba(0,0,0,0.6)',
                          color: image.isPrimary ? 'warning.main' : 'white',
                          '&:hover': { bgcolor: 'rgba(0,0,0,0.8)' }
                        }}
                      >
                        {image.isPrimary ? <StarIcon fontSize="small" /> : <StarBorderIcon fontSize="small" />}
                      </IconButton>
                      <IconButton
                        size="small"
                        onClick={() => handleDeleteGallery(index)}
                        sx={{
                          bgcolor: 'rgba(0,0,0,0.6)',
                          color: 'white',
                          '&:hover': { bgcolor: 'rgba(0,0,0,0.8)' }
                        }}
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Box>
                    <Chip
                      label={image.type}
                      size="small"
                      color="primary"
                      sx={{ position: 'absolute', bottom: 8, left: 8 }}
                    />
                  </Box>
                  <CardContent>
                    <TextField
                      fullWidth
                      size="small"
                      label="Image Name"
                      value={image.name || ''}
                      onChange={(e) => handleUpdateGalleryImage(index, 'name', e.target.value)}
                      sx={{ mb: 1 }}
                    />
                    <TextField
                      fullWidth
                      size="small"
                      label="Alt Text"
                      value={image.alt || ''}
                      onChange={(e) => handleUpdateGalleryImage(index, 'alt', e.target.value)}
                      sx={{ mb: 1 }}
                    />
                    <FormControl fullWidth size="small">
                      <InputLabel>Type</InputLabel>
                      <Select
                        value={image.type || 'Gallery'}
                        label="Type"
                        onChange={(e) => handleUpdateGalleryImage(index, 'type', e.target.value)}
                      >
                        {IMAGE_TYPES.map(type => (
                          <MenuItem key={type} value={type}>{type}</MenuItem>
                        ))}
                      </Select>
                    </FormControl>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        )}

        <Box sx={{ mt: 2, p: 2, bgcolor: 'info.lighter', borderRadius: 1 }}>
          <Typography variant="caption" color="info.dark">
            <strong>Guidelines:</strong> Select multiple images to upload at once. Each image can be categorized as Banner, Gallery, Logo, or Map.
            Featured image is the main tour image. Gallery images can be used throughout the tour details page.
          </Typography>
        </Box>
      </Box>
    </Box>
  );
}
