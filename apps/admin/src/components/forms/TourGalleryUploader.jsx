'use client';

import React, { useState, useCallback } from 'react';
import { useDropzone } from 'react-dropzone';
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

      const response = await fetch(`/api/tours/${tourId}/images`, {
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

  const uploadFiles = async (files) => {
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

        const response = await fetch(`/api/tours/${tourId}/images`, {
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

  const handleGalleryUpload = async (e) => {
    const files = Array.from(e.target.files || []);
    await uploadFiles(files);
  };

  // Dropzone callback
  const onDrop = useCallback(async (acceptedFiles) => {
    await uploadFiles(acceptedFiles);
  }, [tourId, galleryImages, onGalleryChange]);

  // Configure dropzone
  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: {
      'image/*': ['.jpeg', '.jpg', '.png', '.gif', '.webp']
    },
    multiple: true,
    disabled: uploading
  });

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

        {/* Drag and Drop Zone */}
        <Box
          {...getRootProps()}
          sx={{
            border: '2px dashed',
            borderColor: isDragActive ? 'primary.main' : 'grey.400',
            borderRadius: 2,
            p: 3,
            mb: 2,
            textAlign: 'center',
            bgcolor: isDragActive ? 'action.hover' : 'background.paper',
            cursor: uploading ? 'not-allowed' : 'pointer',
            transition: 'all 0.2s',
            '&:hover': {
              borderColor: uploading ? 'grey.400' : 'primary.main',
              bgcolor: uploading ? 'background.paper' : 'action.hover'
            }
          }}
        >
          <input {...getInputProps()} />
          {uploading ? (
            <Box sx={{ display: 'flex', flexDirection: 'column', alignItems: 'center', gap: 1 }}>
              <CircularProgress />
              <Typography variant="body2" color="text.secondary">
                Uploading images...
              </Typography>
            </Box>
          ) : (
            <Box>
              <UploadIcon sx={{ fontSize: 48, color: 'text.secondary', mb: 1 }} />
              <Typography variant="body1" gutterBottom>
                {isDragActive ? 'Drop images here...' : 'Drag & drop images here, or click to select'}
              </Typography>
              <Typography variant="caption" color="text.secondary">
                Supports: JPG, PNG, GIF, WEBP (multiple files)
              </Typography>
            </Box>
          )}
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
            No gallery images uploaded yet. Click &quot;Upload Images&quot; to add images.
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
