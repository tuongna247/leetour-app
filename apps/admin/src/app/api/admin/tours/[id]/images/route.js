import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';

/**
 * GET /api/admin/tours/[id]/images
 * Get all images for a tour
 */
export async function GET(request, { params }) {
  try {
    await connectDB();
    const { id } = await params;

    const tour = await Tour.findById(id).select('featuredImage galleryImages images');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: {
        featuredImage: tour.featuredImage,
        galleryImages: tour.galleryImages,
        images: tour.images || []
      }
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
 * POST /api/admin/tours/[id]/images
 * Upload new images for a tour
 */
export async function POST(request, { params }) {
  try {
    await connectDB();
    const { id } = await params;
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

    // Upload image (Cloudinary or local storage)
    const cloudName = process.env.NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME;
    let imageUrl;

    if (cloudName) {
      // Use Cloudinary
      const uploadPreset = process.env.NEXT_PUBLIC_CLOUDINARY_UPLOAD_PRESET || 'leetour_preset';

      const bytes = await file.arrayBuffer();
      const buffer = Buffer.from(bytes);
      const base64 = buffer.toString('base64');
      const dataURI = `data:${file.type};base64,${base64}`;

      const cloudinaryUrl = `https://api.cloudinary.com/v1_1/${cloudName}/image/upload`;
      const cloudinaryFormData = new FormData();
      cloudinaryFormData.append('file', dataURI);
      cloudinaryFormData.append('upload_preset', uploadPreset);
      cloudinaryFormData.append('folder', `leetour/tours/${id}`);

      const uploadResponse = await fetch(cloudinaryUrl, {
        method: 'POST',
        body: cloudinaryFormData
      });

      const uploadResult = await uploadResponse.json();

      if (!uploadResponse.ok) {
        throw new Error(uploadResult.error?.message || 'Upload to Cloudinary failed');
      }

      imageUrl = uploadResult.secure_url;
    } else {
      // Fallback to local storage for development
      const { writeFile, mkdir } = await import('fs/promises');
      const path = await import('path');
      const { existsSync } = await import('fs');

      const uploadDir = path.join(process.cwd(), 'public', 'uploads', 'tours', id);

      if (!existsSync(uploadDir)) {
        await mkdir(uploadDir, { recursive: true });
      }

      const timestamp = Date.now();
      const random = Math.random().toString(36).substring(7);
      const ext = file.name.substring(file.name.lastIndexOf('.'));
      const filename = `${timestamp}-${random}${ext}`;
      const filePath = path.join(uploadDir, filename);

      const bytes = await file.arrayBuffer();
      const buffer = Buffer.from(bytes);
      await writeFile(filePath, buffer);

      imageUrl = `/uploads/tours/${id}/${filename}`;
    }

    // Save image data based on type
    if (imageType === 'featured') {
      tour.featuredImage = {
        url: imageUrl,
        alt: alt || tour.title
      };
    } else if (imageType === 'banner') {
      // This is for slider images
      const newImage = {
        url: imageUrl,
        alt: alt || tour.title
      };
      return NextResponse.json({
        success: true,
        message: 'Image uploaded successfully',
        data: newImage
      }, { status: 201 });
    } else {
      // Gallery images
      if (!tour.images) {
        tour.images = [];
      }

      if (isPrimary) {
        tour.images.forEach(img => {
          img.isPrimary = false;
        });
      }

      const newImage = {
        url: imageUrl,
        alt: alt || tour.title,
        isPrimary: isPrimary,
        imageType: imageType,
        displayOrder: tour.images.length
      };

      tour.images.push(newImage);
    }

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Image uploaded successfully',
      data: {
        url: imageUrl,
        alt: alt || tour.title
      }
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
 * DELETE /api/admin/tours/[id]/images?imageId=xxx
 * Delete an image
 */
export async function DELETE(request, { params }) {
  try {
    await connectDB();
    const { id } = await params;
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

    // Find and remove the image
    const image = tour.images.id(imageId);

    if (!image) {
      return NextResponse.json(
        { success: false, message: 'Image not found' },
        { status: 404 }
      );
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
