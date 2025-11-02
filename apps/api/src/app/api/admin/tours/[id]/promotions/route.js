import { NextResponse } from 'next/server';
import Tour from '@/models/Tour';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/admin/tours/[id]/promotions
 * Get all promotions for a tour
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const tour = await Tour.findById(id).select('promotions');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: tour.promotions || []
    });

  } catch (error) {
    console.error('Error fetching promotions:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch promotions', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/admin/tours/[id]/promotions
 * Add a new promotion to a tour
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      promotionName,
      promotionType,
      discountType,
      discountAmount,
      validFrom,
      validTo,
      bookingWindowStart,
      bookingWindowEnd,
      daysBeforeDeparture,
      minPassengers,
      conditions,
      isActive
    } = body;

    // Validate required fields
    if (!promotionName || !discountAmount || !validFrom || !validTo) {
      return NextResponse.json(
        { success: false, message: 'Promotion name, discount amount, and validity dates are required' },
        { status: 400 }
      );
    }

    // Validate dates
    if (new Date(validFrom) > new Date(validTo)) {
      return NextResponse.json(
        { success: false, message: 'Valid from date must be before valid to date' },
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

    // Add new promotion
    const newPromotion = {
      promotionName,
      promotionType: promotionType || 'custom',
      discountType: discountType || 'percentage',
      discountAmount: parseFloat(discountAmount),
      validFrom: new Date(validFrom),
      validTo: new Date(validTo),
      bookingWindowStart: bookingWindowStart ? new Date(bookingWindowStart) : undefined,
      bookingWindowEnd: bookingWindowEnd ? new Date(bookingWindowEnd) : undefined,
      daysBeforeDeparture: daysBeforeDeparture ? parseInt(daysBeforeDeparture) : 0,
      minPassengers: minPassengers ? parseInt(minPassengers) : undefined,
      conditions: conditions || '',
      isActive: isActive !== undefined ? isActive : true
    };

    tour.promotions.push(newPromotion);
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Promotion added successfully',
      data: tour.promotions
    }, { status: 201 });

  } catch (error) {
    console.error('Error adding promotion:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to add promotion', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * PUT /api/admin/tours/[id]/promotions
 * Update an existing promotion
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      promotionId,
      promotionName,
      promotionType,
      discountType,
      discountAmount,
      validFrom,
      validTo,
      bookingWindowStart,
      bookingWindowEnd,
      daysBeforeDeparture,
      minPassengers,
      conditions,
      isActive
    } = body;

    if (!promotionId) {
      return NextResponse.json(
        { success: false, message: 'Promotion ID is required' },
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

    // Find and update the promotion
    const promotion = tour.promotions.id(promotionId);

    if (!promotion) {
      return NextResponse.json(
        { success: false, message: 'Promotion not found' },
        { status: 404 }
      );
    }

    // Update fields
    if (promotionName !== undefined) promotion.promotionName = promotionName;
    if (promotionType !== undefined) promotion.promotionType = promotionType;
    if (discountType !== undefined) promotion.discountType = discountType;
    if (discountAmount !== undefined) promotion.discountAmount = parseFloat(discountAmount);
    if (validFrom !== undefined) promotion.validFrom = new Date(validFrom);
    if (validTo !== undefined) promotion.validTo = new Date(validTo);
    if (bookingWindowStart !== undefined) promotion.bookingWindowStart = bookingWindowStart ? new Date(bookingWindowStart) : undefined;
    if (bookingWindowEnd !== undefined) promotion.bookingWindowEnd = bookingWindowEnd ? new Date(bookingWindowEnd) : undefined;
    if (daysBeforeDeparture !== undefined) promotion.daysBeforeDeparture = parseInt(daysBeforeDeparture);
    if (minPassengers !== undefined) promotion.minPassengers = minPassengers ? parseInt(minPassengers) : undefined;
    if (conditions !== undefined) promotion.conditions = conditions;
    if (isActive !== undefined) promotion.isActive = isActive;

    // Validate dates
    if (promotion.validFrom > promotion.validTo) {
      return NextResponse.json(
        { success: false, message: 'Valid from date must be before valid to date' },
        { status: 400 }
      );
    }

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Promotion updated successfully',
      data: tour.promotions
    });

  } catch (error) {
    console.error('Error updating promotion:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update promotion', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/admin/tours/[id]/promotions?promotionId=xxx
 * Delete a promotion
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);
    const promotionId = searchParams.get('promotionId');

    if (!promotionId) {
      return NextResponse.json(
        { success: false, message: 'Promotion ID is required' },
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

    // Remove the promotion
    const promotion = tour.promotions.id(promotionId);

    if (!promotion) {
      return NextResponse.json(
        { success: false, message: 'Promotion not found' },
        { status: 404 }
      );
    }

    promotion.deleteOne();
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Promotion deleted successfully',
      data: tour.promotions
    });

  } catch (error) {
    console.error('Error deleting promotion:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete promotion', error: error.message },
      { status: 500 }
    );
  }
}
