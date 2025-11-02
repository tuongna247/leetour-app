/**
 * Image Upload Utility
 * Supports Cloudinary and local file storage
 */

import { v2 as cloudinary } from 'cloudinary';
import { writeFile, mkdir } from 'fs/promises';
import path from 'path';
import { existsSync } from 'fs';

// Configure Cloudinary
cloudinary.config({
  cloud_name: process.env.CLOUDINARY_CLOUD_NAME,
  api_key: process.env.CLOUDINARY_API_KEY,
  api_secret: process.env.CLOUDINARY_API_SECRET
});

const USE_CLOUDINARY = process.env.CLOUDINARY_CLOUD_NAME ? true : false;
const UPLOAD_DIR = process.env.UPLOAD_DIR || './public/uploads/tours';
const MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB
const ALLOWED_TYPES = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];

/**
 * Validate image file
 */
function validateImage(file, maxSize = MAX_FILE_SIZE) {
  if (!file) {
    throw new Error('No file provided');
  }

  if (!ALLOWED_TYPES.includes(file.type)) {
    throw new Error(`Invalid file type. Allowed types: ${ALLOWED_TYPES.join(', ')}`);
  }

  if (file.size > maxSize) {
    throw new Error(`File size exceeds ${maxSize / (1024 * 1024)}MB limit`);
  }

  return true;
}

/**
 * Generate unique filename
 */
function generateFilename(originalName) {
  const timestamp = Date.now();
  const random = Math.random().toString(36).substring(7);
  const ext = path.extname(originalName);
  const name = path.basename(originalName, ext).toLowerCase().replace(/[^a-z0-9]/g, '-');
  return `${name}-${timestamp}-${random}${ext}`;
}

/**
 * Upload image to Cloudinary
 */
async function uploadToCloudinary(file, folder = 'tours') {
  try {
    // Convert File/Blob to base64
    const arrayBuffer = await file.arrayBuffer();
    const buffer = Buffer.from(arrayBuffer);
    const base64 = buffer.toString('base64');
    const dataURI = `data:${file.type};base64,${base64}`;

    const result = await cloudinary.uploader.upload(dataURI, {
      folder: `leetour/${folder}`,
      resource_type: 'auto',
      transformation: [
        { width: 1200, height: 800, crop: 'limit' },
        { quality: 'auto:good' },
        { fetch_format: 'auto' }
      ]
    });

    return {
      url: result.secure_url,
      publicId: result.public_id,
      width: result.width,
      height: result.height,
      format: result.format,
      size: result.bytes
    };
  } catch (error) {
    console.error('Cloudinary upload error:', error);
    throw new Error(`Failed to upload to Cloudinary: ${error.message}`);
  }
}

/**
 * Upload image to local storage
 */
async function uploadToLocal(file, folder = 'tours') {
  try {
    const uploadPath = path.join(UPLOAD_DIR, folder);

    // Create directory if it doesn't exist
    if (!existsSync(uploadPath)) {
      await mkdir(uploadPath, { recursive: true });
    }

    const filename = generateFilename(file.name);
    const filePath = path.join(uploadPath, filename);

    // Convert File/Blob to buffer
    const arrayBuffer = await file.arrayBuffer();
    const buffer = Buffer.from(arrayBuffer);

    // Save file
    await writeFile(filePath, buffer);

    // Return relative URL
    const relativeUrl = `/uploads/tours/${folder}/${filename}`;

    return {
      url: relativeUrl,
      filename: filename,
      path: filePath,
      size: file.size
    };
  } catch (error) {
    console.error('Local upload error:', error);
    throw new Error(`Failed to upload locally: ${error.message}`);
  }
}

/**
 * Upload single image
 * @param {File} file - The image file
 * @param {string} folder - Subfolder for organization
 * @param {Object} options - Upload options
 * @returns {Promise<Object>} Upload result
 */
export async function uploadImage(file, folder = 'tours', options = {}) {
  try {
    // Validate image
    validateImage(file, options.maxSize);

    // Upload to configured storage
    let result;
    if (USE_CLOUDINARY) {
      result = await uploadToCloudinary(file, folder);
    } else {
      result = await uploadToLocal(file, folder);
    }

    return {
      success: true,
      data: result
    };
  } catch (error) {
    console.error('Upload error:', error);
    return {
      success: false,
      error: error.message
    };
  }
}

/**
 * Upload multiple images
 * @param {File[]} files - Array of image files
 * @param {string} folder - Subfolder for organization
 * @param {Object} options - Upload options
 * @returns {Promise<Object[]>} Array of upload results
 */
export async function uploadMultipleImages(files, folder = 'tours', options = {}) {
  const results = [];

  for (const file of files) {
    try {
      const result = await uploadImage(file, folder, options);
      results.push(result);
    } catch (error) {
      results.push({
        success: false,
        error: error.message,
        filename: file.name
      });
    }
  }

  return results;
}

/**
 * Delete image from Cloudinary
 */
export async function deleteImageFromCloudinary(publicId) {
  try {
    if (!USE_CLOUDINARY) {
      throw new Error('Cloudinary is not configured');
    }

    const result = await cloudinary.uploader.destroy(publicId);
    return {
      success: result.result === 'ok',
      message: result.result
    };
  } catch (error) {
    console.error('Cloudinary delete error:', error);
    return {
      success: false,
      error: error.message
    };
  }
}

/**
 * Delete image from local storage
 */
export async function deleteImageFromLocal(filePath) {
  try {
    const { unlink } = await import('fs/promises');
    await unlink(filePath);
    return {
      success: true,
      message: 'File deleted successfully'
    };
  } catch (error) {
    console.error('Local delete error:', error);
    return {
      success: false,
      error: error.message
    };
  }
}

/**
 * Process and optimize image before upload
 * @param {File} file - Image file
 * @param {Object} options - Processing options
 * @returns {Promise<File>} Processed file
 */
export async function processImage(file, options = {}) {
  const {
    maxWidth = 1200,
    maxHeight = 800,
    quality = 0.9
  } = options;

  // This is a placeholder - in production, use sharp or jimp for server-side processing
  // Or use client-side processing with browser-image-compression
  return file;
}

export default {
  uploadImage,
  uploadMultipleImages,
  deleteImageFromCloudinary,
  deleteImageFromLocal,
  processImage,
  validateImage
};
