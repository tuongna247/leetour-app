import { NextResponse } from "next/server";
import connectDB from "@/lib/mongodb";
import Booking from "@/models/Booking";
import Tour from "@/models/Tour";

// Simple test endpoint to create a minimal booking
export async function POST(request) {
  try {
    await connectDB();
    console.log('📝 Testing booking creation...');
    
    // First, find or create a test tour
    let tour = await Tour.findOne({ title: "Test Tour" });
    if (!tour) {
      console.log('🏗️ Creating test tour...');
      tour = new Tour({
        title: "Test Tour",
        description: "A test tour for booking validation",
        price: 100,
        isActive: true
      });
      await tour.save();
      console.log('✅ Test tour created:', tour._id);
    }
    
    // Minimal required data
    const testData = {
      tour: {
        tourId: tour._id,
        title: tour.title,
        price: tour.price,
        selectedDate: new Date("2024-09-20"),
        selectedTimeSlot: "09:00 AM"
      },
      customer: {
        firstName: "Test",
        lastName: "User",
        email: "test@example.com",
        phone: "+1-555-0123"
      },
      participants: {
        adults: 1,
        children: 0,
        infants: 0,
        totalCount: 1
      },
      pricing: {
        basePrice: 100,
        adultPrice: 100,
        childPrice: 0,
        infantPrice: 0,
        subtotal: 100,
        taxes: 10,
        fees: 5,
        discount: 0,
        total: 115,
        currency: "USD"
      }
    };
    
    console.log('🔍 Test data:', JSON.stringify(testData, null, 2));
    
    // Create booking
    const newBooking = new Booking(testData);
    
    // Validate before saving
    const validationError = newBooking.validateSync();
    if (validationError) {
      console.error('❌ Validation failed:', validationError);
      const errors = Object.keys(validationError.errors).map(key => ({
        field: key,
        message: validationError.errors[key].message,
        value: validationError.errors[key].value,
        kind: validationError.errors[key].kind
      }));
      
      return NextResponse.json({
        status: 400,
        msg: "Validation failed",
        errors: errors,
        fullError: validationError.message
      });
    }
    
    console.log('✅ Validation passed, saving...');
    const savedBooking = await newBooking.save();
    console.log('✅ Booking saved successfully:', savedBooking.bookingId);
    
    return NextResponse.json({
      status: 201,
      data: savedBooking,
      msg: "Test booking created successfully"
    });
    
  } catch (error) {
    console.error('💥 Error during booking creation:', error);
    console.error('💥 Error name:', error.name);
    console.error('💥 Error message:', error.message);
    
    if (error.name === 'ValidationError') {
      const detailedErrors = {};
      for (const [key, value] of Object.entries(error.errors)) {
        detailedErrors[key] = {
          message: value.message,
          value: value.value,
          path: value.path,
          kind: value.kind
        };
      }
      
      return NextResponse.json({
        status: 400,
        msg: "Detailed validation error",
        errors: detailedErrors,
        errorMessage: error.message,
        errorDetails: error.errors
      });
    }
    
    if (error.code === 11000) {
      return NextResponse.json({
        status: 400,
        msg: "Duplicate key error",
        error: error.message,
        keyPattern: error.keyPattern
      });
    }
    
    return NextResponse.json({
      status: 500,
      msg: "Unexpected error",
      error: error.message,
      stack: error.stack
    });
  }
}

// GET endpoint to test connection
export async function GET() {
  try {
    await connectDB();
    
    // Test the Booking model
    const count = await Booking.countDocuments();
    
    return NextResponse.json({
      status: 200,
      msg: "Database connection successful",
      bookingCount: count,
      model: "Booking model is working"
    });
  } catch (error) {
    return NextResponse.json({
      status: 500,
      msg: "Database connection failed",
      error: error.message
    });
  }
}