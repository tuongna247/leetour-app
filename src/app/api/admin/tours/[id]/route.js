import { NextResponse } from "next/server";

const BACKEND_URL = process.env.BACKEND_URL || 'http://localhost:5000';

// GET single tour by ID (admin can see inactive tours)
export async function GET(request, { params }) {
  try {
    const { id } = await params;
    
    const response = await fetch(`${BACKEND_URL}/api/admin/tours/${id}`, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    
    const data = await response.json();
    
    if (!response.ok) {
      return NextResponse.json(data, { status: response.status });
    }
    
    return NextResponse.json(data);
  } catch (error) {
    return NextResponse.json({
      status: 500,
      msg: "Failed to fetch tour from backend",
      error: error.message
    });
  }
}

// PUT - Update tour
export async function PUT(request, { params }) {
  try {
    const { id } = await params;
    const data = await request.json();
    
    const response = await fetch(`${BACKEND_URL}/api/admin/tours/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(data),
    });
    
    const result = await response.json();
    
    if (!response.ok) {
      return NextResponse.json(result, { status: response.status });
    }
    
    return NextResponse.json(result);
  } catch (error) {
    return NextResponse.json({
      status: 500,
      msg: "Failed to update tour",
      error: error.message
    });
  }
}

// DELETE - Delete tour (soft delete)
export async function DELETE(request, { params }) {
  try {
    const { id } = await params;
    
    const response = await fetch(`${BACKEND_URL}/api/admin/tours/${id}`, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json',
      },
    });
    
    const result = await response.json();
    
    if (!response.ok) {
      return NextResponse.json(result, { status: response.status });
    }
    
    return NextResponse.json(result);
  } catch (error) {
    return NextResponse.json({
      status: 500,
      msg: "Failed to delete tour",
      error: error.message
    });
  }
}