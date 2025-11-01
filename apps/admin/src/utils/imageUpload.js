/**
 * Image Upload Utility for Admin App
 * Supports Cloudinary and local file storage
 */

const MAX_FILE_SIZE = 10 * 1024 * 1024; // 10MB
const ALLOWED_TYPES = ['image/jpeg', 'image/jpg', 'image/png', 'image/webp'];

/**
 * Validate image file
 */
export function validateImage(file, maxSize = MAX_FILE_SIZE) {
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
export function generateFilename(originalName) {
  const timestamp = Date.now();
  const random = Math.random().toString(36).substring(7);
  const ext = originalName.substring(originalName.lastIndexOf('.'));
  const name = originalName.substring(0, originalName.lastIndexOf('.')).toLowerCase().replace(/[^a-z0-9]/g, '-');
  return `${name}-${timestamp}-${random}${ext}`;
}

/**
 * Upload image to Cloudinary
 */
export async function uploadToCloudinary(file, folder = 'tours') {
  try {
    // Convert File/Blob to base64
    const arrayBuffer = await file.arrayBuffer();
    const buffer = Buffer.from(arrayBuffer);
    const base64 = buffer.toString('base64');
    const dataURI = `data:${file.type};base64,${base64}`;

    // Use Cloudinary upload API
    const cloudinaryUrl = `https://api.cloudinary.com/v1_1/${process.env.NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME}/image/upload`;

    const formData = new FormData();
    formData.append('file', dataURI);
    formData.append('upload_preset', process.env.NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET || 'leetour_preset');
    formData.append('folder', `leetour/${folder}`);

    const response = await fetch(cloudinaryUrl, {
      method: 'POST',
      body: formData
    });

    const result = await response.json();

    if (!response.ok) {
      throw new Error(result.error?.message || 'Cloudinary upload failed');
    }

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
 * Upload single image (client-side for now, uses direct Cloudinary upload)
 */
export async function uploadImage(file, folder = 'tours', options = {}) {
  try {
    // Validate image
    validateImage(file, options.maxSize);

    // Upload to Cloudinary
    const result = await uploadToCloudinary(file, folder);

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

export default {
  uploadImage,
  validateImage,
  generateFilename
};
