import { NextResponse } from 'next/server';
import Tour from '@/models/Tour';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/tours/[id]/surcharges
 * Get all surcharges for a tour
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const tour = await Tour.findById(id).select('surcharges');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: tour.surcharges || []
    });

  } catch (error) {
    console.error('Error fetching surcharges:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch surcharges', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/tours/[id]/surcharges
 * Add a new surcharge to a tour
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      surchargeName,
      surchargeType,
      startDate,
      endDate,
      amountType,
      amount,
      description,
      isActive
    } = body;

    // Validate required fields
    if (!surchargeName || !startDate || !endDate || !amount) {
      return NextResponse.json(
        { success: false, message: 'Surcharge name, dates, and amount are required' },
        { status: 400 }
      );
    }

    // Validate dates
    if (new Date(startDate) > new Date(endDate)) {
      return NextResponse.json(
        { success: false, message: 'Start date must be before end date' },
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

    // Add new surcharge
    const newSurcharge = {
      surchargeName,
      surchargeType: surchargeType || 'custom',
      startDate: new Date(startDate),
      endDate: new Date(endDate),
      amountType: amountType || 'percentage',
      amount: parseFloat(amount),
      description: description || '',
      isActive: isActive !== undefined ? isActive : true
    };

    tour.surcharges.push(newSurcharge);
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Surcharge added successfully',
      data: tour.surcharges
    }, { status: 201 });

  } catch (error) {
    console.error('Error adding surcharge:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to add surcharge', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * PUT /api/tours/[id]/surcharges
 * Update an existing surcharge
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      surchargeId,
      surchargeName,
      surchargeType,
      startDate,
      endDate,
      amountType,
      amount,
      description,
      isActive
    } = body;

    if (!surchargeId) {
      return NextResponse.json(
        { success: false, message: 'Surcharge ID is required' },
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

    // Find and update the surcharge
    const surcharge = tour.surcharges.id(surchargeId);

    if (!surcharge) {
      return NextResponse.json(
        { success: false, message: 'Surcharge not found' },
        { status: 404 }
      );
    }

    // Update fields
    if (surchargeName !== undefined) surcharge.surchargeName = surchargeName;
    if (surchargeType !== undefined) surcharge.surchargeType = surchargeType;
    if (startDate !== undefined) surcharge.startDate = new Date(startDate);
    if (endDate !== undefined) surcharge.endDate = new Date(endDate);
    if (amountType !== undefined) surcharge.amountType = amountType;
    if (amount !== undefined) surcharge.amount = parseFloat(amount);
    if (description !== undefined) surcharge.description = description;
    if (isActive !== undefined) surcharge.isActive = isActive;

    // Validate dates
    if (surcharge.startDate > surcharge.endDate) {
      return NextResponse.json(
        { success: false, message: 'Start date must be before end date' },
        { status: 400 }
      );
    }

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Surcharge updated successfully',
      data: tour.surcharges
    });

  } catch (error) {
    console.error('Error updating surcharge:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update surcharge', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/tours/[id]/surcharges?surchargeId=xxx
 * Delete a surcharge
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);
    const surchargeId = searchParams.get('surchargeId');

    if (!surchargeId) {
      return NextResponse.json(
        { success: false, message: 'Surcharge ID is required' },
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

    // Remove the surcharge
    const surcharge = tour.surcharges.id(surchargeId);

    if (!surcharge) {
      return NextResponse.json(
        { success: false, message: 'Surcharge not found' },
        { status: 404 }
      );
    }

    surcharge.deleteOne();
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Surcharge deleted successfully',
      data: tour.surcharges
    });

  } catch (error) {
    console.error('Error deleting surcharge:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete surcharge', error: error.message },
      { status: 500 }
    );
  }
}
