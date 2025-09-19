import { NextResponse } from "next/server";
import connectDB from "@/lib/mongodb";
import Tour from "@/models/Tour";
import mongoose from "mongoose";

export async function GET(request) {
  try {
    console.log('=== Debug endpoint called ===');
    await connectDB();
    console.log('=== Database connected ===');
    
    const testId = "68c2d33ed4f270d93a92ca89";
    console.log('Testing with ID:', testId);
    console.log('Is valid ObjectId?', mongoose.Types.ObjectId.isValid(testId));
    
    // Try to find this specific tour
    const tour = await Tour.findById(testId).lean();
    console.log('Direct findById result:', tour ? 'Found' : 'Not found');
    
    if (tour) {
      console.log('Tour title:', tour.title);
    }
    
    return NextResponse.json({
      status: 200,
      data: {
        isValidObjectId: mongoose.Types.ObjectId.isValid(testId),
        tourFound: !!tour,
        tourTitle: tour?.title || null
      },
      msg: "Debug complete"
    });
  } catch (error) {
    console.error('Debug Error:', error);
    return NextResponse.json({
      status: 500,
      msg: "Debug failed",
      error: error.message
    });
  }
}