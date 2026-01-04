import { NextResponse } from 'next/server';
import Tour from '@/models/Tour';
import dbConnect from '@/lib/mongodb';
import { uploadImage, deleteImageFromCloudinary } from '@/utils/imageUpload';

/**
 * GET /api/tours/[id]/images
 * Get all images for a tour
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const tour = await Tour.findById(id).select('images');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: tour.images || []
    });

  } catch (error) {
    console.error('Error fetching images:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch images', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/tours/[id]/images
 * Upload new images for a tour
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const formData = await request.formData();

    const file = formData.get('file');
    const imageType = formData.get('imageType') || 'gallery';
    const alt = formData.get('alt') || '';
    const isPrimary = formData.get('isPrimary') === 'true';

    if (!file) {
      return NextResponse.json(
        { success: false, message: 'No file provided' },
        { status: 400 }
      );
    }

    // Validate image type
    if (!['featured', 'banner', 'gallery'].includes(imageType)) {
      return NextResponse.json(
        { success: false, message: 'Invalid image type' },
        { status: 400 }
      );
    }

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    // Upload image
    const uploadResult = await uploadImage(file, `tours/${id}`);

    if (!uploadResult.success) {
      return NextResponse.json(
        { success: false, message: 'Failed to upload image', error: uploadResult.error },
        { status: 500 }
      );
    }

    // If setting as primary, remove primary flag from other images
    if (isPrimary) {
      tour.images.forEach(img => {
        img.isPrimary = false;
      });
    }

    // Add image to tour
    const newImage = {
      url: uploadResult.data.url,
      alt: alt || tour.title,
      isPrimary: isPrimary,
      imageType: imageType,
      displayOrder: tour.images.length
    };

    tour.images.push(newImage);
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Image uploaded successfully',
      data: newImage
    }, { status: 201 });

  } catch (error) {
    console.error('Error uploading image:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to upload image', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * PUT /api/tours/[id]/images
 * Update image metadata (alt, type, order, primary)
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      imageId,
      alt,
      imageType,
      displayOrder,
      isPrimary
    } = body;

    if (!imageId) {
      return NextResponse.json(
        { success: false, message: 'Image ID is required' },
        { status: 400 }
      );
    }

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    // Find the image
    const image = tour.images.id(imageId);

    if (!image) {
      return NextResponse.json(
        { success: false, message: 'Image not found' },
        { status: 404 }
      );
    }

    // If setting as primary, remove primary flag from other images
    if (isPrimary) {
      tour.images.forEach(img => {
        if (img._id.toString() !== imageId) {
          img.isPrimary = false;
        }
      });
    }

    // Update fields
    if (alt !== undefined) image.alt = alt;
    if (imageType !== undefined) image.imageType = imageType;
    if (displayOrder !== undefined) image.displayOrder = displayOrder;
    if (isPrimary !== undefined) image.isPrimary = isPrimary;

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Image updated successfully',
      data: tour.images
    });

  } catch (error) {
    console.error('Error updating image:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update image', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/tours/[id]/images?imageId=xxx
 * Delete an image
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);
    const imageId = searchParams.get('imageId');

    if (!imageId) {
      return NextResponse.json(
        { success: false, message: 'Image ID is required' },
        { status: 400 }
      );
    }

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    // Find the image
    const image = tour.images.id(imageId);

    if (!image) {
      return NextResponse.json(
        { success: false, message: 'Image not found' },
        { status: 404 }
      );
    }

    // Try to delete from Cloudinary if using it
    // (This will fail gracefully if not using Cloudinary)
    if (image.url.includes('cloudinary.com')) {
      const publicId = image.url.split('/').slice(-2).join('/').split('.')[0];
      await deleteImageFromCloudinary(`leetour/${publicId}`);
    }

    // Remove image from tour
    image.deleteOne();
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Image deleted successfully',
      data: tour.images
    });

  } catch (error) {
    console.error('Error deleting image:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete image', error: error.message },
      { status: 500 }
    );
  }
}
