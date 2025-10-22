import { NextResponse } from 'next/server';
import Receipt from '@/models/Receipt';
import Booking from '@/models/Booking';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/admin/receipts
 * Get all receipts with filtering (admin only)
 */
export async function GET(request) {
  try {
    await dbConnect();

    const { searchParams } = new URL(request.url);

    const page = parseInt(searchParams.get('page')) || 1;
    const limit = parseInt(searchParams.get('limit')) || 20;
    const paymentStatus = searchParams.get('paymentStatus');
    const bookingId = searchParams.get('bookingId');
    const userId = searchParams.get('userId');
    const sortBy = searchParams.get('sortBy') || 'createdAt';
    const order = searchParams.get('order') === 'asc' ? 1 : -1;
    const search = searchParams.get('search'); // Search by receipt number

    const skip = (page - 1) * limit;

    // Build filter query
    const filter = {};
    if (paymentStatus) filter.paymentStatus = paymentStatus;
    if (bookingId) filter.booking = bookingId;
    if (userId) filter.user = userId;
    if (search) {
      filter.receiptNumber = { $regex: search, $options: 'i' };
    }

    // Get receipts
    const receipts = await Receipt.find(filter)
      .populate('booking', 'bookingReference status')
      .populate('tour', 'title')
      .populate('user', 'firstName lastName email')
      .sort({ [sortBy]: order })
      .skip(skip)
      .limit(limit)
      .lean();

    const totalReceipts = await Receipt.countDocuments(filter);

    // Get statistics
    const stats = await Receipt.aggregate([
      {
        $group: {
          _id: '$paymentStatus',
          count: { $sum: 1 },
          totalAmount: { $sum: '$totalAmount' }
        }
      }
    ]);

    return NextResponse.json({
      success: true,
      data: {
        receipts,
        pagination: {
          currentPage: page,
          totalPages: Math.ceil(totalReceipts / limit),
          totalReceipts,
          limit
        },
        statistics: stats
      }
    });

  } catch (error) {
    console.error('Error fetching receipts:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch receipts', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/admin/receipts
 * Create a new receipt for a booking
 */
export async function POST(request) {
  try {
    await dbConnect();

    const body = await request.json();

    const {
      bookingId,
      items,
      subtotal,
      surcharges,
      discounts,
      tax,
      totalAmount,
      currency,
      paymentMethod,
      paymentStatus,
      notes,
      createdBy
    } = body;

    // Validate required fields
    if (!bookingId || !totalAmount || !paymentMethod) {
      return NextResponse.json(
        { success: false, message: 'Booking ID, total amount, and payment method are required' },
        { status: 400 }
      );
    }

    // Get booking details
    const booking = await Booking.findById(bookingId);
    if (!booking) {
      return NextResponse.json(
        { success: false, message: 'Booking not found' },
        { status: 404 }
      );
    }

    // Check if receipt already exists for this booking
    const existingReceipt = await Receipt.findOne({ booking: bookingId });
    if (existingReceipt) {
      return NextResponse.json(
        { success: false, message: 'Receipt already exists for this booking' },
        { status: 400 }
      );
    }

    // Create new receipt
    const newReceipt = new Receipt({
      booking: bookingId,
      tour: booking.tour.tourId,
      user: booking.customer.userId || createdBy,
      items: items || [{
        description: booking.tour.title,
        quantity: booking.participants.totalCount,
        unitPrice: booking.pricing.basePrice,
        totalPrice: booking.pricing.subtotal
      }],
      subtotal: subtotal || booking.pricing.subtotal,
      surcharges: surcharges || 0,
      discounts: discounts || booking.pricing.discount,
      tax: {
        rate: tax?.rate || 0,
        amount: tax?.amount || booking.pricing.taxes
      },
      totalAmount,
      currency: currency || booking.pricing.currency,
      paymentMethod,
      paymentStatus: paymentStatus || 'paid',
      notes: notes || '',
      createdBy: createdBy
    });

    await newReceipt.save();

    // Populate for response
    await newReceipt.populate('booking', 'bookingReference');
    await newReceipt.populate('tour', 'title');
    await newReceipt.populate('user', 'firstName lastName email');

    return NextResponse.json({
      success: true,
      message: 'Receipt created successfully',
      data: newReceipt
    }, { status: 201 });

  } catch (error) {
    console.error('Error creating receipt:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to create receipt', error: error.message },
      { status: 500 }
    );
  }
}
