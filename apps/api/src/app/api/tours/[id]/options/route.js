import { NextResponse } from "next/server";
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';
import { getActiveOptionsWithPrices } from '@/utils/pricingCalculator';

// GET all active tour options with calculated prices for a specific tour
// Query params: passengerCount (optional, default: 1)
export async function GET(request, { params }) {
  try {
    await connectDB();
    const { id } = params;

    // Get passenger count from query params
    const { searchParams } = new URL(request.url);
    const passengerCount = parseInt(searchParams.get('passengerCount')) || 1;

    if (passengerCount < 1) {
      return NextResponse.json({
        status: 400,
        msg: 'Passenger count must be at least 1'
      }, { status: 400 });
    }

    const tour = await Tour.findById(id);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    if (!tour.isActive) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour is not available'
      }, { status: 404 });
    }

    // Get active options with calculated prices
    const optionsWithPrices = getActiveOptionsWithPrices(tour.tourOptions, passengerCount);

    return NextResponse.json({
      status: 200,
      data: {
        tourId: tour._id,
        tourTitle: tour.title,
        currency: tour.currency,
        passengerCount,
        options: optionsWithPrices
      },
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
