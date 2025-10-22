import { NextResponse } from "next/server";
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';

// GET all tour options for a specific tour
export async function GET(request, { params }) {
  try {
    await connectDB();
    const { id } = params;

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    return NextResponse.json({
      status: 200,
      data: tour.tourOptions || [],
      msg: 'success'
    });

  } catch (error) {
    console.error('Get tour options error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to fetch tour options',
      error: error.message
    }, { status: 500 });
  }
}

// POST - Add new tour option
export async function POST(request, { params }) {
  try {
    await connectDB();
    const { id } = params;
    const optionData = await request.json();

    // Validate required fields
    if (!optionData.optionName || optionData.basePrice === undefined) {
      return NextResponse.json({
        status: 400,
        msg: 'Option name and base price are required'
      }, { status: 400 });
    }

    const tour = await Tour.findById(id);
    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    // Initialize tourOptions array if it doesn't exist
    if (!tour.tourOptions) {
      tour.tourOptions = [];
    }

    // Add the new option
    tour.tourOptions.push({
      optionName: optionData.optionName,
      description: optionData.description || '',
      basePrice: optionData.basePrice,
      pricingTiers: optionData.pricingTiers || [],
      isActive: optionData.isActive !== undefined ? optionData.isActive : true
    });

    await tour.save();

    return NextResponse.json({
      status: 201,
      data: tour.tourOptions[tour.tourOptions.length - 1],
      msg: 'Tour option added successfully'
    }, { status: 201 });

  } catch (error) {
    console.error('Add tour option error:', error);

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
      msg: 'Failed to add tour option',
      error: error.message
    }, { status: 500 });
  }
}
