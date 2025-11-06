'use client';

import React, { useState, useCallback } from 'react';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Grid,
  IconButton,
  CircularProgress,
  Alert
} from '@mui/material';
import {
  CloudUpload as UploadIcon,
  Delete as DeleteIcon,
  Star as StarIcon,
  Image as ImageIcon
} from '@mui/icons-material';
import { useDropzone } from 'react-dropzone';

export default function FeaturedSliderImageUploader({
  tourId,
  featuredImage = { url: '', alt: '' },
  sliderImages = [{ url: '', alt: '' }, { url: '', alt: '' }, { url: '', alt: '' }, { url: '', alt: '' }],
  onFeaturedChange,
  onSliderChange,
  onUpload
}) {
  const [uploading, setUploading] = useState(null); // 'featured' | 'slider-0' | 'slider-1' | etc
  const [error, setError] = useState('');

  const handleUpload = async (file, type, index = null) => {
    try {
      setUploading(type + (index !== null ? `-${index}` : ''));
      setError('');

      const formData = new FormData();
      formData.append('file', file);
      formData.append('imageType', type === 'featured' ? 'featured' : 'banner');
      formData.append('alt', file.name.replace(/\.[^/.]+$/, ''));

      if (type === 'featured') {
        formData.append('isPrimary', 'true');
      }

      // Call the upload API
      const response = await fetch(`/api/admin/tours/${tourId}/images`, {
        method: 'POST',
        body: formData
      });

      const result = await response.json();

      if (result.success) {
        const imageData = { url: result.data.url, alt: result.data.alt };

        if (type === 'featured') {
          onFeaturedChange(imageData);
        } else {
          const updated = [...sliderImages];
          updated[index] = imageData;
          onSliderChange(updated);
        }
      } else {
        throw new Error(result.message || 'Upload failed');
      }
    } catch (err) {
      console.error('Upload error:', err);
      setError(err.message || 'Failed to upload image');
    } finally {
      setUploading(null);
    }
  };

  const handleDelete = (type, index = null) => {
    if (type === 'featured') {
      onFeaturedChange({ url: '', alt: '' });
    } else {
      const updated = [...sliderImages];
      updated[index] = { url: '', alt: '' };
      onSliderChange(updated);
    }
  };

  const handleAltChange = (type, value, index = null) => {
    if (type === 'featured') {
      onFeaturedChange({ ...featuredImage, alt: value });
    } else {
      const updated = [...sliderImages];
      updated[index] = { ...updated[index], alt: value };
      onSliderChange(updated);
    }
  };

  // Dropzone for featured image
  const featuredDropzone = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) handleUpload(files[0], 'featured');
    },
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    maxSize: 10 * 1024 * 1024,
    maxFiles: 1,
    disabled: uploading !== null
  });

  // Dropzones for slider images - create all 4 at component level
  const slider0Dropzone = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) handleUpload(files[0], 'slider', 0);
    },
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    maxSize: 10 * 1024 * 1024,
    maxFiles: 1,
    disabled: uploading !== null
  });

  const slider1Dropzone = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) handleUpload(files[0], 'slider', 1);
    },
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    maxSize: 10 * 1024 * 1024,
    maxFiles: 1,
    disabled: uploading !== null
  });

  const slider2Dropzone = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) handleUpload(files[0], 'slider', 2);
    },
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    maxSize: 10 * 1024 * 1024,
    maxFiles: 1,
    disabled: uploading !== null
  });

  const slider3Dropzone = useDropzone({
    onDrop: (files) => {
      if (files.length > 0) handleUpload(files[0], 'slider', 3);
    },
    accept: { 'image/*': ['.jpeg', '.jpg', '.png', '.webp'] },
    maxSize: 10 * 1024 * 1024,
    maxFiles: 1,
    disabled: uploading !== null
  });

  const sliderDropzones = [slider0Dropzone, slider1Dropzone, slider2Dropzone, slider3Dropzone];

  const renderUploadBox = (dropzone, image, type, index = null, label) => {
    const uploadingThis = uploading === type + (index !== null ? `-${index}` : '');

    return (
      <Box>
        <Typography variant="subtitle2" gutterBottom sx={{ fontWeight: 600 }}>
          {label}
        </Typography>

        {image.url ? (
          <Box sx={{ position: 'relative' }}>
            <img
              src={image.url}
              alt={image.alt || label}
              style={{
                width: '100%',
                height: type === 'featured' ? 200 : 120,
                objectFit: 'cover',
                borderRadius: 8
              }}
            />
            <IconButton
              onClick={() => handleDelete(type, index)}
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
            <Box sx={{ mt: 1 }}>
              <input
                type="text"
                value={image.alt}
                onChange={(e) => handleAltChange(type, e.target.value, index)}
                placeholder="Alt text for SEO"
                style={{
                  width: '100%',
                  padding: '8px',
                  borderRadius: '4px',
                  border: '1px solid #ccc',
                  fontSize: '14px'
                }}
              />
            </Box>
          </Box>
        ) : (
          <Box
            {...dropzone.getRootProps()}
            sx={{
              border: '2px dashed',
              borderColor: dropzone.isDragActive ? 'primary.main' : 'grey.300',
              borderRadius: 1,
              p: 2,
              textAlign: 'center',
              cursor: uploading ? 'not-allowed' : 'pointer',
              bgcolor: dropzone.isDragActive ? 'action.hover' : 'background.paper',
              height: type === 'featured' ? 200 : 120,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              '&:hover': {
                borderColor: 'primary.main',
                bgcolor: 'action.hover'
              }
            }}
          >
            <input {...dropzone.getInputProps()} />
            {uploadingThis ? (
              <CircularProgress size={32} />
            ) : (
              <>
                <UploadIcon sx={{ fontSize: 40, color: 'text.secondary', mb: 1 }} />
                <Typography variant="caption" color="text.secondary">
                  {dropzone.isDragActive ? 'Drop here' : 'Click or drag image'}
                </Typography>
              </>
            )}
          </Box>
        )}
      </Box>
    );
  };

  return (
    <Box>
      {error && (
        <Alert severity="error" sx={{ mb: 2 }} onClose={() => setError('')}>
          {error}
        </Alert>
      )}

      {/* Featured Image */}
      <Box sx={{ mb: 3, p: 2, border: 1, borderColor: 'divider', borderRadius: 1, bgcolor: 'warning.lighter' }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1, mb: 2 }}>
          <StarIcon color="warning" />
          <Typography variant="subtitle1" sx={{ fontWeight: 600 }}>
            Featured Image (Main)
          </Typography>
        </Box>
        {renderUploadBox(featuredDropzone, featuredImage, 'featured', null, 'Upload or drag image here')}
      </Box>

      {/* Slider Images */}
      <Typography variant="subtitle1" gutterBottom sx={{ fontWeight: 600, mb: 2 }}>
        Slider Images (4 for banner carousel)
      </Typography>
      <Grid container spacing={2}>
        {sliderImages.map((image, index) => {
          const dropzone = sliderDropzones[index];
          return (
            <Grid item xs={12} md={6} key={index}>
              <Box sx={{ p: 2, border: 1, borderColor: 'divider', borderRadius: 1, bgcolor: 'background.default' }}>
                {renderUploadBox(dropzone, image, 'slider', index, `Slider Image ${index + 1}`)}
              </Box>
            </Grid>
          );
        })}
      </Grid>

      <Box sx={{ mt: 2, p: 2, bgcolor: 'info.lighter', borderRadius: 1, border: 1, borderColor: 'info.light' }}>
        <Typography variant="caption" color="info.dark">
          <strong>Upload Guidelines:</strong> Drag & drop or click to upload high-quality images (minimum 1200x800px, max 10MB).
          Featured image appears on tour cards. Slider images create the banner carousel on tour detail page.
        </Typography>
      </Box>
    </Box>
  );
}
