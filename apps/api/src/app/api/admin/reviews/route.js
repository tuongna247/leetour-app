import { NextResponse } from 'next/server';
import Review from '@/models/Review';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/admin/reviews
 * Get all reviews with filtering options (admin only)
 */
export async function GET(request) {
  try {
    await dbConnect();

    const { searchParams } = new URL(request.url);

    const page = parseInt(searchParams.get('page')) || 1;
    const limit = parseInt(searchParams.get('limit')) || 20;
    const status = searchParams.get('status'); // pending, approved, rejected
    const tourId = searchParams.get('tourId');
    const sortBy = searchParams.get('sortBy') || 'createdAt';
    const order = searchParams.get('order') === 'asc' ? 1 : -1;

    const skip = (page - 1) * limit;

    // Build filter query
    const filter = {};
    if (status) filter.status = status;
    if (tourId) filter.tour = tourId;

    // Get reviews
    const reviews = await Review.find(filter)
      .populate('tour', 'title')
      .populate('user', 'firstName lastName email')
      .populate('booking', 'bookingReference')
      .sort({ [sortBy]: order })
      .skip(skip)
      .limit(limit)
      .lean();

    const totalReviews = await Review.countDocuments(filter);

    // Get statistics
    const stats = await Review.aggregate([
      {
        $group: {
          _id: '$status',
          count: { $sum: 1 }
        }
      }
    ]);

    const statistics = {
      pending: stats.find(s => s._id === 'pending')?.count || 0,
      approved: stats.find(s => s._id === 'approved')?.count || 0,
      rejected: stats.find(s => s._id === 'rejected')?.count || 0,
      total: stats.reduce((sum, s) => sum + s.count, 0)
    };

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
        statistics
      }
    });

  } catch (error) {
    console.error('Error fetching reviews (admin):', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch reviews', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/admin/reviews?reviewId=xxx
 * Delete a review (admin only)
 */
export async function DELETE(request) {
  try {
    await dbConnect();

    const { searchParams } = new URL(request.url);
    const reviewId = searchParams.get('reviewId');

    if (!reviewId) {
      return NextResponse.json(
        { success: false, message: 'Review ID is required' },
        { status: 400 }
      );
    }

    const review = await Review.findByIdAndDelete(reviewId);

    if (!review) {
      return NextResponse.json(
        { success: false, message: 'Review not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      message: 'Review deleted successfully'
    });

  } catch (error) {
    console.error('Error deleting review:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete review', error: error.message },
      { status: 500 }
    );
  }
}
