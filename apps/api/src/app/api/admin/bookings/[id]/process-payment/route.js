import { NextResponse } from 'next/server';
import Booking from '@/models/Booking';
import dbConnect from '@/lib/mongodb';

/**
 * POST /api/admin/bookings/[id]/process-payment
 * Process payment for a booking with option to disable points
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      paymentMethod,
      pointsEligible,
      staffUserId,
      staffNotes,
      transactionId
    } = body;

    // Validate required fields
    if (!paymentMethod) {
      return NextResponse.json(
        { success: false, message: 'Payment method is required' },
        { status: 400 }
      );
    }

    if (!staffUserId) {
      return NextResponse.json(
        { success: false, message: 'Staff user ID is required' },
        { status: 400 }
      );
    }

    const booking = await Booking.findById(id);

    if (!booking) {
      return NextResponse.json(
        { success: false, message: 'Booking not found' },
        { status: 404 }
      );
    }

    // Check if payment is already processed
    if (booking.payment.status === 'completed') {
      return NextResponse.json(
        { success: false, message: 'Payment already processed for this booking' },
        { status: 400 }
      );
    }

    // Calculate points
    const shouldAwardPoints = pointsEligible !== false;
    let pointsAwarded = 0;
    let pointsWaived = 0;

    if (shouldAwardPoints) {
      // Award points based on total amount (e.g., 1 point per $10)
      pointsAwarded = Math.floor(booking.pricing.total / 10);
    } else {
      // Calculate points that would have been awarded
      pointsWaived = Math.floor(booking.pricing.total / 10);
    }

    // Update booking
    booking.payment.method = paymentMethod;
    booking.payment.status = 'completed';
    booking.payment.transactionId = transactionId || `STAFF-${Date.now()}`;
    booking.payment.paymentDate = new Date();
    booking.status = 'confirmed';

    // Staff payment tracking
    booking.pointsEligible = shouldAwardPoints;
    booking.processedByStaff = true;
    booking.staffUser = staffUserId;
    booking.staffNotes = staffNotes || '';
    booking.pointsAwarded = pointsAwarded;
    booking.pointsWaived = pointsWaived;

    await booking.save();

    // TODO: If points awarded, update user's points balance
    // if (pointsAwarded > 0) {
    //   await User.findByIdAndUpdate(booking.customer.userId, {
    //     $inc: { 'loyaltyPoints.balance': pointsAwarded }
    //   });
    // }

    return NextResponse.json({
      success: true,
      message: `Payment processed successfully${!shouldAwardPoints ? ' (points waived)' : ''}`,
      data: {
        bookingId: booking._id,
        bookingReference: booking.bookingReference,
        paymentStatus: booking.payment.status,
        transactionId: booking.payment.transactionId,
        pointsAwarded,
        pointsWaived,
        pointsEligible: shouldAwardPoints,
        processedBy: staffUserId,
        processedAt: new Date()
      }
    });

  } catch (error) {
    console.error('Error processing payment:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to process payment', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * GET /api/admin/bookings/[id]/process-payment
 * Get payment processing details for a booking
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const booking = await Booking.findById(id)
      .populate('staffUser', 'firstName lastName email')
      .select('payment pointsEligible processedByStaff staffUser staffNotes pointsAwarded pointsWaived pricing');

    if (!booking) {
      return NextResponse.json(
        { success: false, message: 'Booking not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: {
        paymentStatus: booking.payment.status,
        paymentMethod: booking.payment.method,
        transactionId: booking.payment.transactionId,
        paymentDate: booking.payment.paymentDate,
        totalAmount: booking.pricing.total,
        pointsEligible: booking.pointsEligible,
        processedByStaff: booking.processedByStaff,
        staffUser: booking.staffUser,
        staffNotes: booking.staffNotes,
        pointsAwarded: booking.pointsAwarded,
        pointsWaived: booking.pointsWaived
      }
    });

  } catch (error) {
    console.error('Error fetching payment details:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch payment details', error: error.message },
      { status: 500 }
    );
  }
}
