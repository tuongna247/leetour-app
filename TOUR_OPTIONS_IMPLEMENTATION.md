# Tour Pricing Options - Implementation Guide

## Overview
The Tour Pricing Options feature allows administrators to create flexible pricing structures for tours based on group size, services, and packages. This implementation follows Phase 1.1 of the plan_implement_claude.md.

## Features Implemented

### 1. Database Schema
- **Tour Options Schema** added to Tour model
  - `optionName`: Name of the pricing option (e.g., "Standard Package", "VIP Package")
  - `description`: Description of what's included
  - `basePrice`: Base price per person (fallback if no tiers match)
  - `pricingTiers`: Array of pricing tiers based on passenger count
    - `minPassengers`: Minimum number of passengers for this tier
    - `maxPassengers`: Maximum number of passengers for this tier
    - `pricePerPerson`: Price per person for this tier
  - `isActive`: Whether this option is available for booking

**Files Modified:**
- [apps/api/src/models/Tour.js](apps/api/src/models/Tour.js)
- [apps/admin/src/models/Tour.js](apps/admin/src/models/Tour.js)

### 2. API Endpoints

#### Admin Endpoints (Protected)

**Get All Tour Options**
```
GET /api/admin/tours/:id/options
```
Returns all tour options for a specific tour.

**Add Tour Option**
```
POST /api/admin/tours/:id/options
Body: {
  "optionName": "Standard Package",
  "description": "Includes transport and guide",
  "basePrice": 100,
  "pricingTiers": [
    { "minPassengers": 1, "maxPassengers": 4, "pricePerPerson": 100 },
    { "minPassengers": 5, "maxPassengers": 10, "pricePerPerson": 80 }
  ],
  "isActive": true
}
```

**Get Single Tour Option**
```
GET /api/admin/tours/:id/options/:optionId
```

**Update Tour Option**
```
PUT /api/admin/tours/:id/options/:optionId
Body: {
  "optionName": "Updated Package Name",
  "basePrice": 120,
  ...
}
```

**Delete Tour Option**
```
DELETE /api/admin/tours/:id/options/:optionId
```

**Files Created:**
- [apps/api/src/app/api/admin/tours/[id]/options/route.js](apps/api/src/app/api/admin/tours/[id]/options/route.js)
- [apps/api/src/app/api/admin/tours/[id]/options/[optionId]/route.js](apps/api/src/app/api/admin/tours/[id]/options/[optionId]/route.js)

#### Public Endpoints

**Get Tour Options with Calculated Prices**
```
GET /api/tours/:id/options?passengerCount=5
```
Returns active tour options with calculated prices based on passenger count.

Response:
```json
{
  "status": 200,
  "data": {
    "tourId": "tour123",
    "tourTitle": "Amazing Tour",
    "currency": "USD",
    "passengerCount": 5,
    "options": [
      {
        "optionName": "Standard Package",
        "description": "Includes transport and guide",
        "basePrice": 100,
        "isActive": true,
        "calculatedPricing": {
          "pricePerPerson": 80,
          "passengerCount": 5,
          "subtotal": 400,
          "currency": "USD"
        }
      }
    ]
  }
}
```

**Files Created:**
- [apps/api/src/app/api/tours/[id]/options/route.js](apps/api/src/app/api/tours/[id]/options/route.js)

### 3. Admin UI Component

**TourOptionsSection Component**
- Reusable React component for managing tour options
- Features:
  - Add/remove pricing options
  - Configure option name, description, base price
  - Add multiple pricing tiers with passenger ranges
  - Drag-and-drop friendly accordion interface
  - Visual chips showing price and tier count
  - Toggle option active/inactive status

**Files Created:**
- [apps/admin/src/components/forms/TourOptionsSection.jsx](apps/admin/src/components/forms/TourOptionsSection.jsx)

**Files Modified:**
- [apps/admin/src/app/(DashboardLayout)/admin/tours/new/page.jsx](apps/admin/src/app/(DashboardLayout)/admin/tours/new/page.jsx)
- [apps/admin/src/app/(DashboardLayout)/admin/tours/[id]/edit/page.jsx](apps/admin/src/app/(DashboardLayout)/admin/tours/[id]/edit/page.jsx)

### 4. Pricing Calculator Utility

A comprehensive utility module for calculating prices based on tour options and passenger count.

**Functions:**
- `calculatePricePerPerson(tourOption, passengerCount)` - Calculate price per person
- `calculateTotalPrice(tourOption, passengerCount)` - Calculate total with breakdown
- `getActiveOptionsWithPrices(tourOptions, passengerCount)` - Get all active options with calculated prices
- `findCheapestOption(tourOptions, passengerCount)` - Find the cheapest option for a group
- `validateTourOption(tourOption)` - Validate tour option data

**Files Created:**
- [apps/api/src/utils/pricingCalculator.js](apps/api/src/utils/pricingCalculator.js)

## How It Works

### Pricing Tier Logic

1. **No Tiers Defined**: Base price applies to all group sizes
2. **Tiers Defined**: System finds the tier matching the passenger count
3. **No Tier Match**: Falls back to base price
4. **Multiple Options**: Customers can choose between different packages

### Example Pricing Structure

```javascript
{
  optionName: "Standard Tour",
  basePrice: 100,
  pricingTiers: [
    { minPassengers: 1, maxPassengers: 1, pricePerPerson: 150 },  // Solo traveler
    { minPassengers: 2, maxPassengers: 4, pricePerPerson: 100 },  // Small group
    { minPassengers: 5, maxPassengers: 10, pricePerPerson: 80 },  // Medium group
    { minPassengers: 11, maxPassengers: 20, pricePerPerson: 70 } // Large group
  ]
}
```

**Calculation Examples:**
- 1 passenger: $150 per person = $150 total
- 3 passengers: $100 per person = $300 total
- 7 passengers: $80 per person = $560 total
- 15 passengers: $70 per person = $1,050 total
- 25 passengers: $100 per person (base price, no tier) = $2,500 total

## Usage Guide

### For Administrators

1. **Navigate to Tour Management**
   - Go to `/admin/tours`
   - Click "Add New Tour" or edit an existing tour

2. **Configure Tour Options**
   - Scroll to "Tour Pricing Options" section
   - Click "Add Option" to create a new pricing option
   - Enter option name (e.g., "Standard Package", "VIP Package")
   - Set base price (fallback price)
   - (Optional) Add pricing tiers for different group sizes
   - Toggle "Active" to make option available for booking

3. **Add Pricing Tiers**
   - Click "Add Tier" within an option
   - Set minimum and maximum passenger range
   - Set price per person for this range
   - Add multiple tiers for different group sizes

4. **Save Tour**
   - Click "Create Tour" or "Update Tour"
   - Tour options are saved with the tour

### For Frontend Integration

1. **Fetch Available Options**
```javascript
const response = await fetch(`/api/tours/${tourId}/options?passengerCount=5`);
const data = await response.json();
const options = data.data.options;
```

2. **Display Options to User**
```javascript
options.forEach(option => {
  console.log(`${option.optionName}: $${option.calculatedPricing.subtotal} total`);
  console.log(`($${option.calculatedPricing.pricePerPerson} per person)`);
});
```

3. **Update Prices When Passenger Count Changes**
```javascript
const handlePassengerChange = async (newCount) => {
  const response = await fetch(`/api/tours/${tourId}/options?passengerCount=${newCount}`);
  const data = await response.json();
  setOptions(data.data.options);
};
```

## Validation

The system includes validation for:
- Option name is required
- Base price must be non-negative
- Passenger ranges must be valid (min >= 1, max >= min)
- No overlapping passenger ranges in tiers
- Price per person must be non-negative

## Testing

### Manual Testing Steps

1. **Start the Admin Application**
```bash
cd apps/admin
npm run dev
```

2. **Start the API Application**
```bash
cd apps/api
npm run dev
```

3. **Test Creating Tour with Options**
   - Navigate to http://localhost:3000/admin/tours/new
   - Fill in basic tour information
   - Scroll to "Tour Pricing Options"
   - Add a pricing option with tiers
   - Save the tour

4. **Test Editing Tour Options**
   - Edit an existing tour
   - Modify tour options
   - Save changes

5. **Test API Endpoints**
```bash
# Get tour options with calculated prices
curl "http://localhost:3001/api/tours/{tourId}/options?passengerCount=5"

# Admin: Get all options
curl -H "Authorization: Bearer {token}" "http://localhost:3001/api/admin/tours/{tourId}/options"
```

### Unit Testing Pricing Calculator

```javascript
const { calculatePricePerPerson, validateTourOption } = require('./utils/pricingCalculator');

// Test basic price calculation
const option = {
  optionName: "Standard",
  basePrice: 100,
  pricingTiers: [
    { minPassengers: 1, maxPassengers: 4, pricePerPerson: 100 },
    { minPassengers: 5, maxPassengers: 10, pricePerPerson: 80 }
  ]
};

console.log(calculatePricePerPerson(option, 3));  // Should return 100
console.log(calculatePricePerPerson(option, 7));  // Should return 80
console.log(calculatePricePerPerson(option, 15)); // Should return 100 (base price)

// Test validation
const validation = validateTourOption(option);
console.log(validation); // { valid: true, errors: [] }
```

## Next Steps

This implementation completes Phase 1.1 of the plan. The following features are planned next:

1. **Phase 1.2 - Surcharge System** (plan_implement_claude.md)
   - Date-based surcharges for holidays/weekends
   - API endpoints for surcharge management
   - Admin UI for configuring surcharges

2. **Phase 1.3 - Promotion System**
   - Early bird discounts
   - Last minute deals
   - Percentage or fixed amount discounts

3. **Phase 1.4 - Cancellation Policy**
   - Tiered refund structures
   - Policy display on tour pages

## Troubleshooting

### Common Issues

**1. Tour options not showing in admin UI**
- Check that TourOptionsSection component is imported correctly
- Verify formData.tourOptions is initialized as an array
- Check browser console for errors

**2. API returns 404 for tour options**
- Ensure MongoDB connection is established
- Verify tour ID is valid
- Check that tour exists in database

**3. Pricing calculation returns base price instead of tier price**
- Verify passenger count is within a defined tier range
- Check that pricingTiers array is not empty
- Ensure tier ranges don't have gaps

**4. Cannot save tour with options**
- Check validation errors in network tab
- Ensure option names are not empty
- Verify all tier prices are non-negative

## Architecture Notes

- **Embedded Documents**: Tour options are stored as embedded documents in the Tour model (not separate collection) for better performance
- **Pricing Calculation**: Done server-side to prevent price manipulation
- **Active Flag**: Allows drafting options without making them public
- **Base Price Fallback**: Ensures a price is always available even without tiers

## Support

For issues or questions:
- Check the [plan_implement_claude.md](plan_implement_claude.md) for feature overview
- Review API endpoint documentation above
- Check browser console and server logs for errors
- Verify MongoDB connection and schema

---

**Implementation Date:** 2025-10-18
**Status:** ✅ Complete
**Version:** 1.0
