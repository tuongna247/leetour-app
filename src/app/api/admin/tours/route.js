import { NextResponse } from "next/server";
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';

// GET all tours for admin (including inactive ones)
export async function GET(request) {
  try {
    await connectDB();
    
    const { searchParams } = new URL(request.url);
    
    const page = parseInt(searchParams.get('page')) || 1;
    const limit = Math.min(parseInt(searchParams.get('limit')) || 10, 100);
    const category = searchParams.get('category');
    const location = searchParams.get('location');
    const minPrice = searchParams.get('minPrice');
    const maxPrice = searchParams.get('maxPrice');
    const search = searchParams.get('search');
    const status = searchParams.get('status'); // active, inactive, all
    const sortBy = searchParams.get('sortBy') || 'createdAt';
    const sortOrder = searchParams.get('sortOrder') || 'desc';

    // Build filter object (admin can see all tours)
    let filter = {};

    // Filter by status for admin
    if (status === 'active') {
      filter.isActive = true;
    } else if (status === 'inactive') {
      filter.isActive = false;
    }
    // If status is 'all' or not specified, show all tours

    if (category) {
      filter.category = category;
    }

    if (location) {
      filter.$or = [
        { 'location.city': new RegExp(location, 'i') },
        { 'location.country': new RegExp(location, 'i') }
      ];
    }

    if (minPrice || maxPrice) {
      filter.price = {};
      if (minPrice) filter.price.$gte = parseFloat(minPrice);
      if (maxPrice) filter.price.$lte = parseFloat(maxPrice);
    }

    if (search) {
      filter.$text = { $search: search };
    }

    // Build sort object
    const sort = {};
    sort[sortBy] = sortOrder === 'asc' ? 1 : -1;

    // Execute query with pagination
    const skip = (page - 1) * limit;
    const tours = await Tour.find(filter)
      .sort(sort)
      .skip(skip)
      .limit(limit);

    const total = await Tour.countDocuments(filter);

    // Get stats for admin dashboard
    const stats = {
      total: await Tour.countDocuments(),
      active: await Tour.countDocuments({ isActive: true }),
      inactive: await Tour.countDocuments({ isActive: false }),
      featured: await Tour.countDocuments({ isFeatured: true }),
    };

    return NextResponse.json({
      status: 200,
      data: {
        tours,
        stats,
        pagination: {
          page,
          limit,
          total,
          pages: Math.ceil(total / limit)
        }
      },
      msg: 'success'
    });

  } catch (error) {
    console.error('Admin get tours error:', error);
    return NextResponse.json({
      status: 500,
      msg: "Failed to fetch tours",
      error: error.message
    }, { status: 500 });
  }
}

// POST - Create new tour (admin)
export async function POST(request) {
  try {
    await connectDB();
    const data = await request.json();
    
    // Validate required fields
    if (!data.title) {
      return NextResponse.json({
        status: 400,
        msg: "Tour title is required"
      }, { status: 400 });
    }
    
    // Set defaults for optional fields
    const tourData = {
      title: data.title,
      description: data.description || "",
      category: data.category || "Adventure",
      price: data.price || 0,
      duration: data.duration || "1 day",
      location: {
        city: data.location?.city || "",
        country: data.location?.country || "",
        coordinates: data.location?.coordinates || [0, 0]
      },
      images: data.images || [],
      features: data.features || [],
      schedule: data.schedule || [],
      guide: {
        name: data.guide?.name || "",
        experience: data.guide?.experience || "",
        languages: data.guide?.languages || [],
        rating: data.guide?.rating || 0
      },
      maxParticipants: data.maxParticipants || 10,
      rating: data.rating || 0,
      totalReviews: data.totalReviews || 0,
      isActive: data.isActive !== undefined ? data.isActive : true,
      isFeatured: data.isFeatured || false,
      ...data
    };
    
    // Create new tour
    const newTour = new Tour(tourData);
    await newTour.save();
    
    return NextResponse.json({
      status: 201,
      data: newTour,
      msg: "Tour created successfully"
    }, { status: 201 });
    
  } catch (error) {
    console.error('Admin create tour error:', error);
    
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
      msg: "Failed to create tour",
      error: error.message
    }, { status: 500 });
  }
}