import { NextResponse } from 'next/server';
import Review from '@/models/Review';
import Tour from '@/models/Tour';
import Booking from '@/models/Booking';
import dbConnect from '@/lib/mongodb';
import { verifyRecaptcha, getClientIP } from '@/utils/recaptcha';
import mongoose from 'mongoose';

/**
 * GET /api/tours/[id]/reviews
 * Get all approved reviews for a tour (public endpoint)
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);

    const page = parseInt(searchParams.get('page')) || 1;
    const limit = parseInt(searchParams.get('limit')) || 10;
    const sortBy = searchParams.get('sortBy') || 'createdAt';
    const order = searchParams.get('order') === 'asc' ? 1 : -1;

    const skip = (page - 1) * limit;

    // Get approved reviews only
    const reviews = await Review.find({
      tour: id,
      status: 'approved'
    })
      .populate('user', 'firstName lastName email')
      .sort({ [sortBy]: order })
      .skip(skip)
      .limit(limit)
      .lean();

    const totalReviews = await Review.countDocuments({
      tour: id,
      status: 'approved'
    });

    // Calculate rating distribution
    const ratingDistribution = await Review.aggregate([
      { $match: { tour: mongoose.Types.ObjectId(id), status: 'approved' } },
      { $group: { _id: '$rating', count: { $sum: 1 } } },
      { $sort: { _id: -1 } }
    ]);

    return NextResponse.json({
      success: true,
      data: {
        reviews,
        pagination: {
          currentPage: page,
          totalPages: Math.ceil(totalReviews / limit),
          totalReviews,
          limit
        },
        ratingDistribution
      }
    });

  } catch (error) {
    console.error('Error fetching reviews:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch reviews', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/tours/[id]/reviews
 * Submit a new review for a tour (with reCAPTCHA validation)
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      userId,
      bookingId,
      rating,
      title,
      comment,
      images,
      recaptchaToken
    } = body;

    // Validate reCAPTCHA
    const clientIP = getClientIP(request);
    const recaptchaResult = await verifyRecaptcha(recaptchaToken, clientIP);

    if (!recaptchaResult.success) {
      return NextResponse.json(
        {
          success: false,
          message: 'reCAPTCHA verification failed. Please try again.',
          error: recaptchaResult.error
        },
        { status: 400 }
      );
    }

    // Validate required fields
    if (!userId || !rating || !title || !comment) {
      return NextResponse.json(
        { success: false, message: 'User ID, rating, title, and comment are required' },
        { status: 400 }
      );
    }

    // Validate rating
    if (rating < 1 || rating > 5) {
      return NextResponse.json(
        { success: false, message: 'Rating must be between 1 and 5' },
        { status: 400 }
      );
    }

    // Check if tour exists
    const tour = await Tour.findById(id);
    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    // Check if user already reviewed this tour
    const existingReview = await Review.findOne({
      tour: id,
      user: userId
    });

    if (existingReview) {
      return NextResponse.json(
        { success: false, message: 'You have already reviewed this tour' },
        { status: 400 }
      );
    }

    // Verify booking if provided
    let verifiedPurchase = false;
    if (bookingId) {
      const booking = await Booking.findOne({
        _id: bookingId,
        'tour.tourId': id,
        status: 'completed'
      });

      verifiedPurchase = !!booking;
    }

    // Create new review
    const newReview = new Review({
      tour: id,
      user: userId,
      booking: bookingId || undefined,
      rating,
      title,
      comment,
      images: images || [],
      verifiedPurchase,
      status: 'pending', // Reviews need admin approval
      recaptchaScore: recaptchaResult.score,
      ipAddress: clientIP
    });

    await newReview.save();

    return NextResponse.json({
      success: true,
      message: 'Review submitted successfully. It will be published after moderation.',
      data: newReview
    }, { status: 201 });

  } catch (error) {
    console.error('Error submitting review:', error);

    // Handle duplicate review error
    if (error.code === 11000) {
      return NextResponse.json(
        { success: false, message: 'You have already reviewed this tour' },
        { status: 400 }
      );
    }

    return NextResponse.json(
      { success: false, message: 'Failed to submit review', error: error.message },
      { status: 500 }
    );
  }
}
