import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';

export async function GET(request, { params }) {
  try {
    await connectDB();

    const identifier = params.id;
    let tour;

    // Check if identifier is a valid MongoDB ObjectId (24 hex characters)
    const isObjectId = /^[0-9a-fA-F]{24}$/.test(identifier);

    if (isObjectId) {
      // Search by _id
      tour = await Tour.findOne({ _id: identifier, isActive: true });
    } else {
      // Search by slug
      tour = await Tour.findOne({ 'seo.slug': identifier, isActive: true });
    }

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    return NextResponse.json({
      status: 200,
      data: tour,
      msg: 'success'
    });

  } catch (error) {
    console.error('Get tour error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to fetch tour',
      error: error.message
    }, { status: 500 });
  }
}