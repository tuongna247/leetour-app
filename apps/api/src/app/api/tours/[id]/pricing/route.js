import { NextResponse } from 'next/server';
import connectDB from '@/lib/mongodb';
import Tour from '@/models/Tour';
import { calculateCompleteBookingPrice, getActiveOptionsWithPrices } from '@/utils/pricingCalculator';

/**
 * GET /api/tours/[id]/pricing
 * Calculate tour pricing based on date, passengers, and children
 * Equivalent to C# GetTourPriceDetail function
 *
 * Query Parameters:
 * - date: Departure date (ISO format or timestamp)
 * - adults: Number of adult passengers (default: 1)
 * - children: Number of children (default: 0)
 * - optionId: Specific tour option ID (optional)
 */
export async function GET(request, { params }) {
  try {
    await connectDB();

    const { searchParams } = new URL(request.url);
    const tourId = params.id;

    // Parse query parameters
    const date = searchParams.get('date');
    const adults = parseInt(searchParams.get('adults') || '1');
    const children = parseInt(searchParams.get('children') || '0');
    const optionId = searchParams.get('optionId');

    // Validate parameters
    if (!date) {
      return NextResponse.json({
        status: 400,
        msg: 'Departure date is required'
      }, { status: 400 });
    }

    if (adults < 1) {
      return NextResponse.json({
        status: 400,
        msg: 'At least 1 adult passenger is required'
      }, { status: 400 });
    }

    const departureDate = new Date(date);
    if (isNaN(departureDate.getTime())) {
      return NextResponse.json({
        status: 400,
        msg: 'Invalid date format'
      }, { status: 400 });
    }

    // Find tour
    const tour = await Tour.findById(tourId);

    if (!tour) {
      return NextResponse.json({
        status: 404,
        msg: 'Tour not found'
      }, { status: 404 });
    }

    if (!tour.isActive) {
      return NextResponse.json({
        status: 400,
        msg: 'Tour is not currently available'
      }, { status: 400 });
    }

    // Calculate total passenger count
    const totalPassengers = adults + children;
    const childrenRatio = 0.75; // Children pay 75% of adult price

    // Prepare booking date (current date/time)
    const bookingDate = new Date();

    // If specific option requested, calculate for that option only
    if (optionId) {
      const tourOption = tour.tourOptions.id(optionId);

      if (!tourOption) {
        return NextResponse.json({
          status: 404,
          msg: 'Tour option not found'
        }, { status: 404 });
      }

      if (!tourOption.isActive) {
        return NextResponse.json({
          status: 400,
          msg: 'This tour option is not currently available'
        }, { status: 400 });
      }

      const pricing = calculateCompleteBookingPrice({
        tourOption,
        passengerCount: adults,
        bookingDate,
        departureDate,
        surcharges: tour.surcharges || [],
        promotions: tour.promotions || [],
        taxRate: 15, // 15% tax (matching the C# code's 1.15 multiplier)
        currency: tour.currency || 'USD'
      });

      // Add children cost if applicable
      let childrenCost = 0;
      if (children > 0) {
        const childPrice = pricing.basePrice * childrenRatio;
        childrenCost = childPrice * children;
      }

      const grandTotal = pricing.total + childrenCost;

      return NextResponse.json({
        status: 200,
        data: {
          tourId: tour._id,
          tourName: tour.title,
          option: {
            id: tourOption._id,
            name: tourOption.optionName,
            description: tourOption.description
          },
          departureDate: departureDate.toISOString(),
          bookingDate: bookingDate.toISOString(),
          passengers: {
            adults,
            children,
            total: totalPassengers
          },
          pricing: {
            ...pricing,
            children: {
              count: children,
              pricePerChild: children > 0 ? pricing.basePrice * childrenRatio : 0,
              subtotal: childrenCost
            },
            grandTotal,
            currency: tour.currency || 'USD'
          }
        },
        msg: 'success'
      });
    }

    // If no specific option, return all active options with pricing
    if (!tour.tourOptions || tour.tourOptions.length === 0) {
      return NextResponse.json({
        status: 400,
        msg: 'No tour options available for this tour'
      }, { status: 400 });
    }

    const optionsWithPricing = tour.tourOptions
      .filter(option => option.isActive)
      .map(option => {
        const pricing = calculateCompleteBookingPrice({
          tourOption: option,
          passengerCount: adults,
          bookingDate,
          departureDate,
          surcharges: tour.surcharges || [],
          promotions: tour.promotions || [],
          taxRate: 15,
          currency: tour.currency || 'USD'
        });

        // Add children cost
        let childrenCost = 0;
        if (children > 0) {
          const childPrice = pricing.basePrice * childrenRatio;
          childrenCost = childPrice * children;
        }

        const grandTotal = pricing.total + childrenCost;

        return {
          id: option._id,
          name: option.optionName,
          description: option.description || option.includeItems || '',
          basePrice: option.basePrice,
          departureTimes: option.departureTimes || '08:00 AM',
          pricing: {
            ...pricing,
            children: {
              count: children,
              pricePerChild: children > 0 ? pricing.basePrice * childrenRatio : 0,
              subtotal: childrenCost
            },
            grandTotal
          }
        };
      });

    if (optionsWithPricing.length === 0) {
      return NextResponse.json({
        status: 400,
        msg: 'No active tour options available'
      }, { status: 400 });
    }

    return NextResponse.json({
      status: 200,
      data: {
        tourId: tour._id,
        tourName: tour.title,
        departureDate: departureDate.toISOString(),
        bookingDate: bookingDate.toISOString(),
        passengers: {
          adults,
          children,
          total: totalPassengers
        },
        options: optionsWithPricing,
        currency: tour.currency || 'USD'
      },
      msg: 'success'
    });

  } catch (error) {
    console.error('Tour pricing calculation error:', error);
    return NextResponse.json({
      status: 500,
      msg: 'Failed to calculate tour pricing',
      error: error.message
    }, { status: 500 });
  }
}
