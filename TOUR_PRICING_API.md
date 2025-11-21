# Tour Pricing API Documentation

## Overview
This document describes the tour pricing API endpoint that replaces the C# `GetTourPriceDetail` function.

## Endpoint

### GET `/api/tours/[id]/pricing`

Calculate tour pricing with surcharges, promotions, and tax based on departure date and passenger count.

---

## Request Parameters

### Path Parameters

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| `id` | String | Yes | Tour ID (MongoDB ObjectId) |

### Query Parameters

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| `date` | String | Yes | - | Departure date (ISO 8601 format or timestamp) |
| `adults` | Number | No | 1 | Number of adult passengers (minimum: 1) |
| `children` | Number | No | 0 | Number of children (ages 4-8, pay 75% of adult price) |
| `optionId` | String | No | - | Specific tour option ID to calculate (if omitted, returns all options) |

---

## Response Format

### Success Response (200 OK)

#### Single Option Response (when `optionId` is provided):

```json
{
  "status": 200,
  "data": {
    "tourId": "507f1f77bcf86cd799439011",
    "tourName": "Ha Long Bay Day Cruise",
    "option": {
      "id": "507f1f77bcf86cd799439012",
      "name": "Private Tour",
      "description": "Exclusive private boat tour"
    },
    "departureDate": "2025-12-25T09:00:00.000Z",
    "bookingDate": "2025-11-21T10:30:00.000Z",
    "passengers": {
      "adults": 2,
      "children": 1,
      "total": 3
    },
    "pricing": {
      "basePrice": 150.00,
      "passengerCount": 2,
      "subtotal": 300.00,
      "surcharges": {
        "total": 50.00,
        "breakdown": [
          {
            "name": "Holiday Season",
            "type": "peak_season",
            "amountType": "percentage",
            "rate": 10,
            "calculatedAmount": 30.00,
            "description": "Christmas & New Year surcharge"
          },
          {
            "name": "Weekend Premium",
            "type": "weekend",
            "amountType": "fixed",
            "rate": 20,
            "calculatedAmount": 20.00,
            "description": "Saturday & Sunday surcharge"
          }
        ]
      },
      "amountAfterSurcharges": 350.00,
      "promotions": {
        "total": 35.00,
        "breakdown": [
          {
            "name": "Early Bird 10%",
            "type": "early_bird",
            "discountType": "percentage",
            "rate": 10,
            "calculatedAmount": 35.00,
            "conditions": "Book 30+ days in advance"
          }
        ]
      },
      "subtotalAfterDiscount": 315.00,
      "tax": {
        "rate": 15,
        "amount": 47.25
      },
      "total": 362.25,
      "children": {
        "count": 1,
        "pricePerChild": 112.50,
        "subtotal": 112.50
      },
      "grandTotal": 474.75,
      "currency": "USD"
    }
  },
  "msg": "success"
}
```

#### Multiple Options Response (when `optionId` is NOT provided):

```json
{
  "status": 200,
  "data": {
    "tourId": "507f1f77bcf86cd799439011",
    "tourName": "Ha Long Bay Day Cruise",
    "departureDate": "2025-12-25T09:00:00.000Z",
    "bookingDate": "2025-11-21T10:30:00.000Z",
    "passengers": {
      "adults": 2,
      "children": 1,
      "total": 3
    },
    "options": [
      {
        "id": "507f1f77bcf86cd799439012",
        "name": "Private Tour",
        "description": "Exclusive private boat",
        "basePrice": 150.00,
        "pricing": {
          "basePrice": 150.00,
          "passengerCount": 2,
          "subtotal": 300.00,
          "surcharges": { "total": 50.00, "breakdown": [...] },
          "amountAfterSurcharges": 350.00,
          "promotions": { "total": 35.00, "breakdown": [...] },
          "subtotalAfterDiscount": 315.00,
          "tax": { "rate": 15, "amount": 47.25 },
          "total": 362.25,
          "children": {
            "count": 1,
            "pricePerChild": 112.50,
            "subtotal": 112.50
          },
          "grandTotal": 474.75
        }
      },
      {
        "id": "507f1f77bcf86cd799439013",
        "name": "Group Tour",
        "description": "Join a small group",
        "basePrice": 80.00,
        "pricing": {
          "basePrice": 80.00,
          "passengerCount": 2,
          "subtotal": 160.00,
          "surcharges": { "total": 16.00, "breakdown": [...] },
          "amountAfterSurcharges": 176.00,
          "promotions": { "total": 17.60, "breakdown": [...] },
          "subtotalAfterDiscount": 158.40,
          "tax": { "rate": 15, "amount": 23.76 },
          "total": 182.16,
          "children": {
            "count": 1,
            "pricePerChild": 60.00,
            "subtotal": 60.00
          },
          "grandTotal": 242.16
        }
      }
    ],
    "currency": "USD"
  },
  "msg": "success"
}
```

### Error Responses

#### 400 Bad Request
```json
{
  "status": 400,
  "msg": "Departure date is required"
}
```

```json
{
  "status": 400,
  "msg": "Invalid date format"
}
```

```json
{
  "status": 400,
  "msg": "At least 1 adult passenger is required"
}
```

#### 404 Not Found
```json
{
  "status": 404,
  "msg": "Tour not found"
}
```

```json
{
  "status": 404,
  "msg": "Tour option not found"
}
```

#### 500 Internal Server Error
```json
{
  "status": 500,
  "msg": "Failed to calculate tour pricing",
  "error": "Error message details"
}
```

---

## Pricing Calculation Logic

### 1. Base Price Calculation
- Fetches the tour option's pricing tiers
- Selects appropriate tier based on passenger count
- Falls back to `basePrice` if no tier matches

### 2. Surcharge Application
Surcharges are applied if:
- Surcharge is `isActive: true`
- Booking date falls between `startDate` and `endDate`

Two surcharge types:
- **Percentage**: `(baseAmount * surcharge.amount) / 100`
- **Fixed**: `surcharge.amount`

### 3. Promotion Application
Promotions are applied if:
- Promotion is `isActive: true`
- Booking date is between `validFrom` and `validTo`
- Optional: Booking window check (`bookingWindowStart` to `bookingWindowEnd`)
- Optional: Days before departure check (for early bird / last minute)
- Optional: Minimum passenger count requirement

Only the **best promotion** (highest discount) is applied.

Two discount types:
- **Percentage**: `(amountAfterSurcharges * promotion.discountAmount) / 100`
- **Fixed**: `promotion.discountAmount`

### 4. Tax Calculation
- Tax rate: **15%** (matching C# implementation's 1.15 multiplier)
- Applied to subtotal after discounts
- Formula: `(subtotalAfterDiscount * 15) / 100`

### 5. Children Pricing
- Children pay **75%** of adult base price
- Formula: `childPrice = basePrice * 0.75`
- Children subtotal: `childPrice * numberOfChildren`

### 6. Grand Total
```
grandTotal = (adults total with tax) + (children subtotal)
```

---

## Example Usage

### Example 1: Get pricing for all tour options

**Request:**
```http
GET /api/tours/507f1f77bcf86cd799439011/pricing?date=2025-12-25&adults=2&children=1
```

**Response:**
Returns all active tour options with calculated pricing for 2 adults and 1 child.

---

### Example 2: Get pricing for specific tour option

**Request:**
```http
GET /api/tours/507f1f77bcf86cd799439011/pricing?date=2025-12-25&adults=2&children=1&optionId=507f1f77bcf86cd799439012
```

**Response:**
Returns pricing for only the "Private Tour" option.

---

### Example 3: Simple adult-only booking

**Request:**
```http
GET /api/tours/507f1f77bcf86cd799439011/pricing?date=2025-12-01&adults=4
```

**Response:**
Returns pricing for 4 adults, no children.

---

## Comparison with C# Implementation

| Feature | C# `GetTourPriceDetail` | Node.js `/api/tours/[id]/pricing` |
|---------|-------------------------|-----------------------------------|
| **Base rates** | 3 separate rate tables (tourRates, tourRates2, tourRates3) | Single `tourOptions` with `pricingTiers` |
| **Surcharges** | Date-based, percentage or fixed | Same ✅ |
| **Promotions** | Date-based, percentage or fixed, per-person or all | Enhanced with booking windows and early-bird logic ✅ |
| **Tax** | 15% (via `* 1.15`) | 15% explicitly calculated ✅ |
| **Children** | 75% of adult rate | Same ✅ |
| **Currency exchange** | VND exchange rate support | Configurable currency (extensible) |
| **Response format** | Partial view `_TourRates` | JSON API response ✅ |

---

## Integration Notes

### Frontend Integration

Update your tour detail page to call this API when users select a date:

```javascript
const handleCheckAvailability = async () => {
  const response = await fetch(
    `/api/tours/${tourId}/pricing?date=${selectedDate}&adults=${selectedPeople}&children=${childrenCount}`
  );
  const data = await response.json();

  if (data.status === 200) {
    // Display pricing options to user
    setTourOptions(data.data.options);
  }
};
```

### Database Requirements

Ensure your tour documents include:
- `tourOptions` array with pricing tiers
- `surcharges` array with date ranges
- `promotions` array with date ranges and conditions
- `currency` field (default: 'USD')

---

## Testing Checklist

- [ ] Test with valid date and passengers
- [ ] Test with children (verify 75% calculation)
- [ ] Test with surcharges active on date
- [ ] Test with promotions active on date
- [ ] Test with both surcharges and promotions
- [ ] Test with specific `optionId`
- [ ] Test without `optionId` (returns all options)
- [ ] Test with invalid tour ID (404)
- [ ] Test with invalid option ID (404)
- [ ] Test with missing date parameter (400)
- [ ] Test with invalid date format (400)
- [ ] Test with 0 adults (400)

---

## Migration from C# to Node.js

### Step 1: Update Tour Schema
Ensure all tours have the new fields added in `TOUR_SCHEMA_UPDATE.md`.

### Step 2: Migrate Existing Data
Run a migration script to convert:
- Old `TourRate`, `TourRate2`, `TourRate3` tables → `tourOptions` with `pricingTiers`
- Old `Tour_Surcharge` table → `surcharges` array
- Old `Tour_Promotion` table → `promotions` array

### Step 3: Update Frontend
Replace calls to `/tour/GetTourPriceDetail` with `/api/tours/[id]/pricing`.

### Step 4: Test
Verify pricing matches the old C# implementation for existing tours.

---

## Future Enhancements

1. **Currency Exchange**: Add real-time currency conversion
2. **Group Discounts**: Automatic discounts for large groups
3. **Promo Codes**: User-entered discount codes
4. **Multi-Day Tours**: Different pricing for multi-day itineraries
5. **Season Pricing**: Automatic peak/off-peak season pricing
6. **Payment Plans**: Installment payment calculations

---

## Support

For questions or issues with the pricing API:
1. Check the Tour schema documentation: `TOUR_SCHEMA_UPDATE.md`
2. Review the pricing calculator utility: `apps/api/src/utils/pricingCalculator.js`
3. Test with the API using the examples above
