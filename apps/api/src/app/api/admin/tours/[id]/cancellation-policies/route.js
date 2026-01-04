import { NextResponse } from 'next/server';
import Tour from '@/models/Tour';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/tours/[id]/cancellation-policies
 * Get all cancellation policies for a tour
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const tour = await Tour.findById(id).select('cancellationPolicies');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    // Sort by days before departure (descending) for display
    const sortedPolicies = (tour.cancellationPolicies || []).sort(
      (a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture
    );

    return NextResponse.json({
      success: true,
      data: sortedPolicies
    });

  } catch (error) {
    console.error('Error fetching cancellation policies:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch cancellation policies', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/tours/[id]/cancellation-policies
 * Add a new cancellation policy to a tour
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      daysBeforeDeparture,
      refundPercentage,
      description,
      displayOrder
    } = body;

    // Validate required fields
    if (daysBeforeDeparture === undefined || refundPercentage === undefined) {
      return NextResponse.json(
        { success: false, message: 'Days before departure and refund percentage are required' },
        { status: 400 }
      );
    }

    // Validate refund percentage
    if (refundPercentage < 0 || refundPercentage > 100) {
      return NextResponse.json(
        { success: false, message: 'Refund percentage must be between 0 and 100' },
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

    // Check for duplicate days before departure
    const existingPolicy = tour.cancellationPolicies.find(
      p => p.daysBeforeDeparture === parseInt(daysBeforeDeparture)
    );

    if (existingPolicy) {
      return NextResponse.json(
        { success: false, message: `Policy for ${daysBeforeDeparture} days before departure already exists` },
        { status: 400 }
      );
    }

    // Add new policy
    const newPolicy = {
      daysBeforeDeparture: parseInt(daysBeforeDeparture),
      refundPercentage: parseFloat(refundPercentage),
      description: description || '',
      displayOrder: displayOrder !== undefined ? parseInt(displayOrder) : tour.cancellationPolicies.length
    };

    tour.cancellationPolicies.push(newPolicy);
    await tour.save();

    // Sort for response
    const sortedPolicies = tour.cancellationPolicies.sort(
      (a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture
    );

    return NextResponse.json({
      success: true,
      message: 'Cancellation policy added successfully',
      data: sortedPolicies
    }, { status: 201 });

  } catch (error) {
    console.error('Error adding cancellation policy:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to add cancellation policy', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * PUT /api/tours/[id]/cancellation-policies
 * Update an existing cancellation policy
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const {
      policyId,
      daysBeforeDeparture,
      refundPercentage,
      description,
      displayOrder
    } = body;

    if (!policyId) {
      return NextResponse.json(
        { success: false, message: 'Policy ID is required' },
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

    // Find and update the policy
    const policy = tour.cancellationPolicies.id(policyId);

    if (!policy) {
      return NextResponse.json(
        { success: false, message: 'Cancellation policy not found' },
        { status: 404 }
      );
    }

    // Update fields
    if (daysBeforeDeparture !== undefined) policy.daysBeforeDeparture = parseInt(daysBeforeDeparture);
    if (refundPercentage !== undefined) {
      const percentage = parseFloat(refundPercentage);
      if (percentage < 0 || percentage > 100) {
        return NextResponse.json(
          { success: false, message: 'Refund percentage must be between 0 and 100' },
          { status: 400 }
        );
      }
      policy.refundPercentage = percentage;
    }
    if (description !== undefined) policy.description = description;
    if (displayOrder !== undefined) policy.displayOrder = parseInt(displayOrder);

    await tour.save();

    // Sort for response
    const sortedPolicies = tour.cancellationPolicies.sort(
      (a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture
    );

    return NextResponse.json({
      success: true,
      message: 'Cancellation policy updated successfully',
      data: sortedPolicies
    });

  } catch (error) {
    console.error('Error updating cancellation policy:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update cancellation policy', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/tours/[id]/cancellation-policies?policyId=xxx
 * Delete a cancellation policy
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);
    const policyId = searchParams.get('policyId');

    if (!policyId) {
      return NextResponse.json(
        { success: false, message: 'Policy ID is required' },
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

    // Remove the policy
    const policy = tour.cancellationPolicies.id(policyId);

    if (!policy) {
      return NextResponse.json(
        { success: false, message: 'Cancellation policy not found' },
        { status: 404 }
      );
    }

    policy.deleteOne();
    await tour.save();

    // Sort for response
    const sortedPolicies = tour.cancellationPolicies.sort(
      (a, b) => b.daysBeforeDeparture - a.daysBeforeDeparture
    );

    return NextResponse.json({
      success: true,
      message: 'Cancellation policy deleted successfully',
      data: sortedPolicies
    });

  } catch (error) {
    console.error('Error deleting cancellation policy:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete cancellation policy', error: error.message },
      { status: 500 }
    );
  }
}
