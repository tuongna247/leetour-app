import { NextResponse } from "next/server";
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';

// GET single tour option
export async function GET(request, { params }) {
  try {
    await connectDB();
    const { id, optionId } = params;

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    const option = tour.tourOptions?.id(optionId);

    if (!option) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour option not found'
      }, { status: 404 });
    }

    return NextResponse.json({
      status: 200,
      data: option,
      msg: 'success'
    });

  } catch (error) {
    console.error('Get tour option error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to fetch tour option',
      error: error.message
    }, { status: 500 });
  }
}

// PUT - Update tour option
export async function PUT(request, { params }) {
  try {
    await connectDB();
    const { id, optionId } = params;
    const optionData = await request.json();

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    const option = tour.tourOptions?.id(optionId);

    if (!option) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour option not found'
      }, { status: 404 });
    }

    // Update fields
    if (optionData.optionName !== undefined) option.optionName = optionData.optionName;
    if (optionData.description !== undefined) option.description = optionData.description;
    if (optionData.basePrice !== undefined) option.basePrice = optionData.basePrice;
    if (optionData.pricingTiers !== undefined) option.pricingTiers = optionData.pricingTiers;
    if (optionData.isActive !== undefined) option.isActive = optionData.isActive;

    await tour.save();

    return NextResponse.json({
      status: 200,
      data: option,
      msg: 'Tour option updated successfully'
    });

  } catch (error) {
    console.error('Update tour option error:', error);

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
      msg: 'Failed to update tour option',
      error: error.message
    }, { status: 500 });
  }
}

// DELETE - Delete tour option
export async function DELETE(request, { params }) {
  try {
    await connectDB();
    const { id, optionId } = params;

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    const option = tour.tourOptions?.id(optionId);

    if (!option) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour option not found'
      }, { status: 404 });
    }

    // Remove the option using Mongoose pull method
    tour.tourOptions.pull(optionId);
    await tour.save();

    return NextResponse.json({
      status: 200,
      data: { id: optionId },
      msg: 'Tour option deleted successfully'
    });

  } catch (error) {
    console.error('Delete tour option error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to delete tour option',
      error: error.message
    }, { status: 500 });
  }
}
