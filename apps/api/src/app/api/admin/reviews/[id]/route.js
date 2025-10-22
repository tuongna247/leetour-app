import { NextResponse } from 'next/server';
import Review from '@/models/Review';
import dbConnect from '@/lib/mongodb';

/**
 * PUT /api/admin/reviews/[id]
 * Update review status (approve/reject) and admin notes
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const { status, adminNotes } = body;

    // Validate status
    if (status && !['pending', 'approved', 'rejected'].includes(status)) {
      return NextResponse.json(
        { success: false, message: 'Invalid status. Must be pending, approved, or rejected' },
        { status: 400 }
      );
    }

    const review = await Review.findById(id);

    if (!review) {
      return NextResponse.json(
        { success: false, message: 'Review not found' },
        { status: 404 }
      );
    }

    // Update fields
    if (status) review.status = status;
    if (adminNotes !== undefined) review.adminNotes = adminNotes;

    await review.save();

    // Populate for response
    await review.populate('tour', 'title');
    await review.populate('user', 'firstName lastName email');

    return NextResponse.json({
      success: true,
      message: `Review ${status || 'updated'} successfully`,
      data: review
    });

  } catch (error) {
    console.error('Error updating review:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update review', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/admin/reviews/[id]
 * Delete a specific review
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const review = await Review.findByIdAndDelete(id);

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
