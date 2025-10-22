'use client';

import React, { useState, useCallback } from 'react';
import {
  Box,
  Button,
  Card,
  CardContent,
  Typography,
  Grid,
  IconButton,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  TextField,
  ImageList,
  ImageListItem,
  ImageListItemBar,
  Chip,
  Stack,
  Alert,
  CircularProgress,
  LinearProgress
} from '@mui/material';
import {
  CloudUpload as UploadIcon,
  Delete as DeleteIcon,
  Star as StarIcon,
  StarBorder as StarBorderIcon,
  Image as ImageIcon
} from '@mui/icons-material';
import { useDropzone } from 'react-dropzone';

const IMAGE_TYPES = [
  { value: 'featured', label: 'Featured', description: 'Main tour image', max: 1 },
  { value: 'banner', label: 'Banner', description: 'Banner slider (max 3)', max: 3 },
  { value: 'gallery', label: 'Gallery', description: 'Gallery images', max: 20 }
];

export default function TourImageUploader({ tourId, initialImages = [], onChange, onUpload }) {
  const [images, setImages] = useState(initialImages);
  const [uploading, setUploading] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [selectedType, setSelectedType] = useState('gallery');

  const onDrop = useCallback(async (acceptedFiles) => {
    setUploading(true);
    setUploadProgress(0);

    try {
      const totalFiles = acceptedFiles.length;
      let uploadedCount = 0;

      for (const file of acceptedFiles) {
        const formData = new FormData();
        formData.append('file', file);
        formData.append('imageType', selectedType);
        formData.append('alt', file.name.replace(/\.[^/.]+$/, ''));

        // Call upload API
        const result = await onUpload?.(formData);

        if (result?.success) {
          setImages(prev => [...prev, result.data]);
        }

        uploadedCount++;
        setUploadProgress((uploadedCount / totalFiles) * 100);
      }

      onChange?.(images);
    } catch (error) {
      console.error('Upload error:', error);
      alert('Failed to upload images');
    } finally {
      setUploading(false);
      setUploadProgress(0);
    }
  }, [selectedType, onUpload, onChange, images]);

  const { getRootProps, getInputProps, isDragActive } = useDropzone({
    onDrop,
    accept: {
      'image/*': ['.jpeg', '.jpg', '.png', '.webp']
    },
    maxSize: 10 * 1024 * 1024, // 10MB
    disabled: uploading
  });

  const handleDelete = async (imageId) => {
    if (!confirm('Are you sure you want to delete this image?')) return;

    const updatedImages = images.filter(img => img._id !== imageId);
    setImages(updatedImages);
    onChange?.(updatedImages);
  };

  const handleSetPrimary = (imageId) => {
    const updatedImages = images.map(img => ({
      ...img,
      isPrimary: img._id === imageId
    }));
    setImages(updatedImages);
    onChange?.(updatedImages);
  };

  const handleUpdateType = (imageId, newType) => {
    const updatedImages = images.map(img =>
      img._id === imageId ? { ...img, imageType: newType } : img
    );
    setImages(updatedImages);
    onChange?.(updatedImages);
  };

  const getImagesByType = (type) => {
    return images.filter(img => img.imageType === type);
  };

  const canAddMore = (type) => {
    const typeConfig = IMAGE_TYPES.find(t => t.value === type);
    if (!typeConfig) return true;
    return getImagesByType(type).length < typeConfig.max;
  };

  return (
    <Box>
      <Typography variant="h6" gutterBottom>
        Tour Images
      </Typography>

      {/* Upload Area */}
      <Card sx={{ mb: 3 }}>
        <CardContent>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={4}>
              <FormControl fullWidth>
                <InputLabel>Image Type</InputLabel>
                <Select
                  value={selectedType}
                  onChange={(e) => setSelectedType(e.target.value)}
                  label="Image Type"
                  disabled={uploading}
                >
                  {IMAGE_TYPES.map(type => (
                    <MenuItem
                      key={type.value}
                      value={type.value}
                      disabled={!canAddMore(type.value)}
                    >
                      <Box>
                        <Typography variant="body2">{type.label}</Typography>
                        <Typography variant="caption" color="text.secondary">
                          {type.description} ({getImagesByType(type.value).length}/{type.max})
                        </Typography>
                      </Box>
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Grid>

            <Grid item xs={12} sm={8}>
              <Box
                {...getRootProps()}
                sx={{
                  border: '2px dashed',
                  borderColor: isDragActive ? 'primary.main' : 'grey.300',
                  borderRadius: 1,
                  p: 3,
                  textAlign: 'center',
                  cursor: uploading ? 'not-allowed' : 'pointer',
                  bgcolor: isDragActive ? 'action.hover' : 'background.paper',
                  '&:hover': {
                    borderColor: 'primary.main',
                    bgcolor: 'action.hover'
                  }
                }}
              >
                <input {...getInputProps()} />
                {uploading ? (
                  <Box>
                    <CircularProgress size={40} />
                    <Typography variant="body2" sx={{ mt: 1 }}>
                      Uploading... {Math.round(uploadProgress)}%
                    </Typography>
                    <LinearProgress variant="determinate" value={uploadProgress} sx={{ mt: 1 }} />
                  </Box>
                ) : (
                  <Box>
                    <UploadIcon sx={{ fontSize: 48, color: 'text.secondary' }} />
                    <Typography variant="body1" gutterBottom>
                      {isDragActive ? 'Drop images here...' : 'Drag & drop images or click to browse'}
                    </Typography>
                    <Typography variant="caption" color="text.secondary">
                      Supported: JPEG, PNG, WebP (max 10MB per file)
                    </Typography>
                  </Box>
                )}
              </Box>
            </Grid>
          </Grid>

          {!canAddMore(selectedType) && (
            <Alert severity="warning" sx={{ mt: 2 }}>
              Maximum number of {IMAGE_TYPES.find(t => t.value === selectedType)?.label} images reached.
              Delete existing images or select a different type.
            </Alert>
          )}
        </CardContent>
      </Card>

      {/* Featured Image */}
      {getImagesByType('featured').length > 0 && (
        <Box sx={{ mb: 3 }}>
          <Typography variant="subtitle1" gutterBottom>
            Featured Image
          </Typography>
          <ImageList cols={1} gap={8}>
            {getImagesByType('featured').map((image) => (
              <ImageListItem key={image._id}>
                <img
                  src={image.url}
                  alt={image.alt}
                  loading="lazy"
                  style={{ height: 300, objectFit: 'cover' }}
                />
                <ImageListItemBar
                  title={image.alt}
                  subtitle={`Type: ${image.imageType}`}
                  actionIcon={
                    <Stack direction="row" spacing={1} sx={{ mr: 1 }}>
                      <IconButton
                        onClick={() => handleSetPrimary(image._id)}
                        sx={{ color: 'white' }}
                      >
                        {image.isPrimary ? <StarIcon /> : <StarBorderIcon />}
                      </IconButton>
                      <IconButton
                        onClick={() => handleDelete(image._id)}
                        sx={{ color: 'white' }}
                      >
                        <DeleteIcon />
                      </IconButton>
                    </Stack>
                  }
                />
              </ImageListItem>
            ))}
          </ImageList>
        </Box>
      )}

      {/* Banner Images */}
      {getImagesByType('banner').length > 0 && (
        <Box sx={{ mb: 3 }}>
          <Typography variant="subtitle1" gutterBottom>
            Banner Images (Slider)
          </Typography>
          <ImageList cols={3} gap={8}>
            {getImagesByType('banner').map((image) => (
              <ImageListItem key={image._id}>
                <img
                  src={image.url}
                  alt={image.alt}
                  loading="lazy"
                  style={{ height: 200, objectFit: 'cover' }}
                />
                <ImageListItemBar
                  title={`Order: ${image.displayOrder}`}
                  actionIcon={
                    <IconButton
                      onClick={() => handleDelete(image._id)}
                      sx={{ color: 'white' }}
                    >
                      <DeleteIcon />
                    </IconButton>
                  }
                />
              </ImageListItem>
            ))}
          </ImageList>
        </Box>
      )}

      {/* Gallery Images */}
      {getImagesByType('gallery').length > 0 && (
        <Box>
          <Typography variant="subtitle1" gutterBottom>
            Gallery Images
          </Typography>
          <ImageList cols={4} gap={8}>
            {getImagesByType('gallery').map((image) => (
              <ImageListItem key={image._id}>
                <img
                  src={image.url}
                  alt={image.alt}
                  loading="lazy"
                  style={{ height: 150, objectFit: 'cover' }}
                />
                <ImageListItemBar
                  actionIcon={
                    <Stack direction="row" spacing={1} sx={{ mr: 1 }}>
                      <IconButton
                        onClick={() => handleSetPrimary(image._id)}
                        sx={{ color: 'white' }}
                        size="small"
                      >
                        {image.isPrimary ? <StarIcon fontSize="small" /> : <StarBorderIcon fontSize="small" />}
                      </IconButton>
                      <IconButton
                        onClick={() => handleDelete(image._id)}
                        sx={{ color: 'white' }}
                        size="small"
                      >
                        <DeleteIcon fontSize="small" />
                      </IconButton>
                    </Stack>
                  }
                />
              </ImageListItem>
            ))}
          </ImageList>
        </Box>
      )}

      {images.length === 0 && (
        <Alert severity="info">
          No images uploaded yet. Upload images using the form above.
        </Alert>
      )}
    </Box>
  );
}
