import { NextResponse } from 'next/server';
import Tour from '@/models/Tour';
import dbConnect from '@/lib/mongodb';

/**
 * GET /api/admin/tours/[id]/itineraries
 * Get all itineraries for a tour
 */
export async function GET(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;

    const tour = await Tour.findById(id).select('itinerary tourType');

    if (!tour) {
      return NextResponse.json(
        { success: false, message: 'Tour not found' },
        { status: 404 }
      );
    }

    return NextResponse.json({
      success: true,
      data: {
        tourType: tour.tourType,
        itinerary: tour.itinerary || []
      }
    });

  } catch (error) {
    console.error('Error fetching itineraries:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to fetch itineraries', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * POST /api/admin/tours/[id]/itineraries
 * Add a new itinerary day to a tour
 */
export async function POST(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const { dayNumber, header, textDetail, activities, meals, accommodation } = body;

    // Validate required fields
    if (!dayNumber || !header) {
      return NextResponse.json(
        { success: false, message: 'Day number and header are required' },
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

    // Check if day number already exists
    const existingDay = tour.itinerary.find(item => item.dayNumber === dayNumber);
    if (existingDay) {
      return NextResponse.json(
        { success: false, message: `Day ${dayNumber} already exists` },
        { status: 400 }
      );
    }

    // Add new itinerary item
    const newItineraryItem = {
      dayNumber,
      header,
      textDetail: textDetail || '',
      activities: activities || [],
      meals: meals || { breakfast: false, lunch: false, dinner: false },
      accommodation: accommodation || ''
    };

    tour.itinerary.push(newItineraryItem);

    // Sort itinerary by day number
    tour.itinerary.sort((a, b) => a.dayNumber - b.dayNumber);

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Itinerary day added successfully',
      data: tour.itinerary
    }, { status: 201 });

  } catch (error) {
    console.error('Error adding itinerary:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to add itinerary', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * PUT /api/admin/tours/[id]/itineraries
 * Update an existing itinerary day
 */
export async function PUT(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const body = await request.json();

    const { itineraryId, dayNumber, header, textDetail, activities, meals, accommodation } = body;

    if (!itineraryId) {
      return NextResponse.json(
        { success: false, message: 'Itinerary ID is required' },
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

    // Find and update the itinerary item
    const itineraryItem = tour.itinerary.id(itineraryId);

    if (!itineraryItem) {
      return NextResponse.json(
        { success: false, message: 'Itinerary item not found' },
        { status: 404 }
      );
    }

    // Update fields
    if (dayNumber !== undefined) itineraryItem.dayNumber = dayNumber;
    if (header !== undefined) itineraryItem.header = header;
    if (textDetail !== undefined) itineraryItem.textDetail = textDetail;
    if (activities !== undefined) itineraryItem.activities = activities;
    if (meals !== undefined) itineraryItem.meals = meals;
    if (accommodation !== undefined) itineraryItem.accommodation = accommodation;

    // Sort itinerary by day number
    tour.itinerary.sort((a, b) => a.dayNumber - b.dayNumber);

    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Itinerary updated successfully',
      data: tour.itinerary
    });

  } catch (error) {
    console.error('Error updating itinerary:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to update itinerary', error: error.message },
      { status: 500 }
    );
  }
}

/**
 * DELETE /api/admin/tours/[id]/itineraries?itineraryId=xxx
 * Delete an itinerary day
 */
export async function DELETE(request, { params }) {
  try {
    await dbConnect();

    const { id } = params;
    const { searchParams } = new URL(request.url);
    const itineraryId = searchParams.get('itineraryId');

    if (!itineraryId) {
      return NextResponse.json(
        { success: false, message: 'Itinerary ID is required' },
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

    // Remove the itinerary item
    const itineraryItem = tour.itinerary.id(itineraryId);

    if (!itineraryItem) {
      return NextResponse.json(
        { success: false, message: 'Itinerary item not found' },
        { status: 404 }
      );
    }

    itineraryItem.deleteOne();
    await tour.save();

    return NextResponse.json({
      success: true,
      message: 'Itinerary deleted successfully',
      data: tour.itinerary
    });

  } catch (error) {
    console.error('Error deleting itinerary:', error);
    return NextResponse.json(
      { success: false, message: 'Failed to delete itinerary', error: error.message },
      { status: 500 }
    );
  }
}
