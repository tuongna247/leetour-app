import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';
import jwt from 'jsonwebtoken';
import User from '@/models/User';

// Helper function to get user from request (optional)
async function getUserFromRequest(request) {
  try {
    // First try to get user from NextAuth session
    const { getServerSession } = await import('next-auth');
    const { authOptions } = await import('../../auth/[...nextauth]/route.js');

    try {
      const session = await getServerSession(authOptions);
      if (session?.user?.id) {
        const user = await User.findById(session.user.id).select('-password');
        if (user && user.isActive) {
          return user;
        }
      }
    } catch (sessionError) {
      // Continue to JWT fallback
    }

    // Fallback to JWT token
    const token = request.headers.get('authorization')?.replace('Bearer ', '');
    if (!token) {
      return null;
    }

    const decoded = jwt.verify(token, process.env.JWT_SECRET || 'your-secret-key');
    const user = await User.findById(decoded.userId).select('-password');

    if (!user || !user.isActive) {
      return null;
    }

    return user;
  } catch (error) {
    return null;
  }
}

// GET single tour by ID or slug (public + admin)
export async function GET(request, { params }) {
  try {
    await connectDB();

    const { searchParams } = new URL(request.url);
    const isAdminRequest = searchParams.get('admin') === 'true';
    const user = await getUserFromRequest(request);

    const identifier = params.id;
    let tour;

    // Check if identifier is a valid MongoDB ObjectId (24 hex characters)
    const isObjectId = /^[0-9a-fA-F]{24}$/.test(identifier);

    if (isAdminRequest && user) {
      // Admin can see all tours (including inactive ones)
      if (isObjectId) {
        tour = await Tour.findById(identifier);
      } else {
        tour = await Tour.findOne({ 'seo.slug': identifier });
      }
    } else {
      // Public can only see active tours
      if (isObjectId) {
        tour = await Tour.findOne({ _id: identifier, isActive: true });
      } else {
        tour = await Tour.findOne({ 'seo.slug': identifier, isActive: true });
      }
    }

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    return NextResponse.json({
      status: 200,
      data: tour,
      msg: 'success'
    });

  } catch (error) {
    console.error('Get tour error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to fetch tour',
      error: error.message
    }, { status: 500 });
  }
}

// PUT - Update tour (admin only)
export async function PUT(request, { params }) {
  try {
    await connectDB();

    const user = await getUserFromRequest(request);

    if (!user) {
      return NextResponse.json({
        status: 401,
        msg: "Authentication required"
      }, { status: 401 });
    }

    // Check if user has permission
    if (!['admin', 'mod'].includes(user.role)) {
      return NextResponse.json({
        status: 403,
        msg: "Access denied. Only admins and moderators can update tours."
      }, { status: 403 });
    }

    const { id } = params;
    const data = await request.json();

    const tour = await Tour.findById(id);
    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    // Check if mod is trying to update someone else's tour
    if (user.role === 'mod' && tour.createdBy?.toString() !== user._id.toString()) {
      return NextResponse.json({
        status: 403,
        msg: 'Access denied. You can only update tours you created.'
      }, { status: 403 });
    }

    const updatedTour = await Tour.findByIdAndUpdate(
      id,
      { ...data, updatedAt: new Date() },
      { new: true, runValidators: true }
    );

    return NextResponse.json({
      status: 200,
      data: updatedTour,
      msg: "Tour updated successfully"
    });

  } catch (error) {
    console.error('Update tour error:', error);

    if (error.name === 'ValidationError') {
      const validationErrors = Object.values(error.errors).map(err => ({
        field: err.path,
        message: err.message,
        value: err.value
      }));

      return NextResponse.json({
        status: 400,
        msg: 'Validation error',
        errors: validationErrors,
        details: error.message
      }, { status: 400 });
    }

    return NextResponse.json({
      status: 500,
      msg: "Failed to update tour",
      error: error.message
    }, { status: 500 });
  }
}

// DELETE - Delete tour (admin only, soft delete)
export async function DELETE(request, { params }) {
  try {
    await connectDB();

    const user = await getUserFromRequest(request);

    if (!user) {
      return NextResponse.json({
        status: 401,
        msg: "Authentication required"
      }, { status: 401 });
    }

    // Check if user has permission
    if (!['admin', 'mod'].includes(user.role)) {
      return NextResponse.json({
        status: 403,
        msg: "Access denied. Only admins and moderators can delete tours."
      }, { status: 403 });
    }

    const { id } = params;

    const tour = await Tour.findById(id);
    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    // Check if mod is trying to delete someone else's tour
    if (user.role === 'mod' && tour.createdBy?.toString() !== user._id.toString()) {
      return NextResponse.json({
        status: 403,
        msg: 'Access denied. You can only delete tours you created.'
      }, { status: 403 });
    }

    // Soft delete by setting isActive to false
    const deletedTour = await Tour.findByIdAndUpdate(
      id,
      { isActive: false, deletedAt: new Date() },
      { new: true }
    );

    return NextResponse.json({
      status: 200,
      data: deletedTour,
      msg: "Tour deleted successfully"
    });

  } catch (error) {
    console.error('Delete tour error:', error);
    return NextResponse.json({
      status: 500,
      msg: "Failed to delete tour",
      error: error.message
    }, { status: 500 });
  }
}
