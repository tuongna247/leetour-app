# Tour Pricing Options Component Documentation

## Overview
This document describes the **TourPricingOptions** React component that replaces the C# `_TourRateOptions.cshtml` partial view.

---

## Component Location

**File**: `apps/frontend/src/app/components/TourPricingOptions.jsx`

---

## Purpose

The component displays available tour pricing options after a user selects a departure date and passenger count. It allows users to:
1. View multiple pricing tiers/options
2. Select departure times
3. See price breakdowns (adults + children)
4. Book the selected option

---

## Comparison: C# vs React

| Feature | C# `_TourRateOptions.cshtml` | React `TourPricingOptions` |
|---------|------------------------------|----------------------------|
| **Options Display** | 3 hardcoded options (Option 1, 2, 3) | Dynamic array of options |
| **Price Format** | VND with `##,###` format | Configurable currency with Intl formatter |
| **Time Selection** | Semicolon-separated strings | Same (backward compatible) ✅ |
| **Children Pricing** | 75% of adult rate | Same ✅ |
| **Time Validation** | Past times disabled via JavaScript | Same ✅ |
| **Book Action** | Calls `BookTourOptions()` function | Configurable `onBook` callback or default navigation |
| **Styling** | Bootstrap + custom CSS | Material-UI components |
| **State Management** | jQuery DOM manipulation | React hooks (useState) |

---

## Component API

### Props

```typescript
interface TourPricingOptionsProps {
  tourId: string;              // Tour ID
  bookingDate: string;         // ISO date string (e.g., "2025-12-25")
  adults: number;              // Number of adult passengers
  children: number;            // Number of children (ages 4-8)
  pricingData: PricingData;    // Pricing data from API
  onBook?: (data: BookingData) => void; // Optional booking callback
}
```

### PricingData Structure

```typescript
interface PricingData {
  tourId: string;
  tourName: string;
  departureDate: string;
  bookingDate: string;
  passengers: {
    adults: number;
    children: number;
    total: number;
  };
  options: TourOption[];
  currency: string;
}

interface TourOption {
  id: string;
  name: string;                // e.g., "Private Tour", "Group Tour"
  description: string;         // HTML content for "What's included"
  basePrice: number;
  departureTimes: string;      // Semicolon-separated (e.g., "08:00 AM;10:30 AM;02:00 PM")
  pricing: {
    basePrice: number;
    passengerCount: number;
    subtotal: number;
    surcharges: { total: number; breakdown: any[] };
    amountAfterSurcharges: number;
    promotions: { total: number; breakdown: any[] };
    subtotalAfterDiscount: number;
    tax: { rate: number; amount: number };
    total: number;
    children: {
      count: number;
      pricePerChild: number;
      subtotal: number;
    };
    grandTotal: number;
  };
}
```

---

## Usage Example

### Basic Usage

```jsx
import TourPricingOptions from '@/app/components/TourPricingOptions';

function TourDetailPage() {
  const [pricingData, setPricingData] = useState(null);

  const handleCheckAvailability = async () => {
    const response = await fetch(
      `/api/tours/${tourId}/pricing?date=${selectedDate}&adults=${adults}&children=${children}`
    );
    const data = await response.json();

    if (data.status === 200) {
      setPricingData(data.data);
    }
  };

  return (
    <div>
      {/* Date and passenger selection form */}
      <Button onClick={handleCheckAvailability}>
        Check Availability
      </Button>

      {/* Display pricing options */}
      {pricingData && (
        <TourPricingOptions
          tourId={tourId}
          bookingDate={selectedDate}
          adults={adults}
          children={children}
          pricingData={pricingData}
        />
      )}
    </div>
  );
}
```

### With Custom Booking Handler

```jsx
<TourPricingOptions
  tourId={tourId}
  bookingDate="2025-12-25"
  adults={2}
  children={1}
  pricingData={pricingData}
  onBook={async (bookingData) => {
    // Custom booking logic
    console.log('Booking:', bookingData);

    // Add to cart
    await addToCart(bookingData);

    // Show confirmation
    setShowConfirmation(true);
  }}
/>
```

---

## Features

### 1. **Multiple Pricing Options**
   - Displays all active tour options dynamically
   - First option marked as "MOST POPULAR"
   - Radio button selection (one at a time)

### 2. **Price Breakdown**
   - Adult price × number of adults
   - Children price (75%) × number of children
   - Grand total in VND (or configured currency)
   - Formatted with thousand separators

### 3. **Departure Time Selection**
   - Parse semicolon-separated time strings
   - Display time slots as clickable chips
   - Disable past times if booking today
   - Show error if user clicks "Book Now" without selecting time

### 4. **Booking Flow**
   - Validate time selection
   - Collect all booking data
   - Call custom `onBook` handler OR navigate to booking page
   - Show loading state during booking

### 5. **Responsive Design**
   - Mobile-friendly layout
   - Adaptive time slot display
   - Material-UI theming support

---

## Tour Schema Updates

The Tour model now includes the following fields for pricing options:

```javascript
const tourOptionSchema = new mongoose.Schema({
  optionName: { type: String, required: true },
  description: { type: String, default: '' },
  basePrice: { type: Number, required: true },
  pricingTiers: [{ /* ... */ }],
  departureTimes: {
    type: String,
    default: '08:00 AM',
    trim: true
  }, // Semicolon-separated: "08:00 AM;10:30 AM;02:00 PM"
  includeItems: {
    type: String,
    default: ''
  }, // HTML content for what's included
  isActive: { type: Boolean, default: true }
});
```

---

## Migration from C# to React

### Step 1: Update API Response

The `/api/tours/[id]/pricing` endpoint now returns `departureTimes` for each option:

```json
{
  "status": 200,
  "data": {
    "options": [
      {
        "id": "507f...",
        "name": "Private Tour",
        "description": "Hotel pickup, English guide, Lunch included",
        "departureTimes": "08:00 AM;10:30 AM;02:00 PM",
        "pricing": { /* ... */ }
      }
    ]
  }
}
```

### Step 2: Replace Old View

**Before (C#/jQuery):**
```html
@Html.Partial("_TourRateOptions", model)

<script>
  function BookTourOptions(button) {
    // jQuery booking logic
  }
</script>
```

**After (React):**
```jsx
<TourPricingOptions
  tourId={tourId}
  bookingDate={selectedDate}
  adults={adults}
  children={children}
  pricingData={pricingData}
/>
```

### Step 3: Update Tour Data

Ensure existing tours have `departureTimes` set for each option:

```javascript
// Migration script example
await Tour.updateMany(
  { 'tourOptions.departureTimes': { $exists: false } },
  {
    $set: {
      'tourOptions.$[].departureTimes': '08:00 AM;02:00 PM'
    }
  }
);
```

---

## Styling Customization

### Override Material-UI Theme

```jsx
import { ThemeProvider, createTheme } from '@mui/material/styles';

const theme = createTheme({
  palette: {
    primary: {
      main: '#007d9e', // Your brand color
    },
  },
});

<ThemeProvider theme={theme}>
  <TourPricingOptions {...props} />
</ThemeProvider>
```

### Custom Styled Components

The component uses styled components that you can customize:

```javascript
// In TourPricingOptions.jsx
const BookButton = styled(Button)(({ theme }) => ({
  backgroundColor: '#ef4904', // Change this to your color
  // ...
}));
```

---

## Time Format Support

The component supports various time formats:

✅ `08:00 AM` - 12-hour format with AM/PM
✅ `2:30 PM` - Single digit hours
✅ `14:00` - 24-hour format (parsed as is)
✅ `08:00` - 24-hour format without AM/PM

**Recommended format**: `HH:MM AM/PM` for best user experience.

---

## Booking Data Structure

When user clicks "Book Now", the following data is passed to `onBook` callback:

```typescript
interface BookingData {
  tourId: string;
  optionId: string;
  optionName: string;
  departureDate: string;      // ISO format
  departureTime: string;       // Selected time (e.g., "10:30 AM")
  adults: number;
  children: number;
  pricing: PricingBreakdown;
  retailRate: number;          // Base price per person
  totalPrice: number;          // Grand total (all passengers)
  currency: string;
}
```

---

## Error Handling

### No Options Available
```jsx
// Component displays:
<Alert severity="warning">
  No pricing options available for the selected date and passenger count.
</Alert>
```

### No Time Selected
```jsx
// Component displays:
<Alert severity="error">
  Please select a time to proceed.
</Alert>
```

### Booking Failure
```javascript
// Component shows alert:
alert('Failed to proceed with booking. Please try again.');
```

**Recommendation**: Replace `alert()` with a toast notification system like:
- Material-UI Snackbar
- React-Toastify
- Custom notification component

---

## Testing Checklist

### Component Rendering
- [ ] Displays all pricing options from API
- [ ] Shows "MOST POPULAR" badge on first option
- [ ] Formats prices correctly with thousand separators
- [ ] Displays children pricing at 75% of adult rate
- [ ] Shows correct grand total

### Time Selection
- [ ] Parses semicolon-separated time strings
- [ ] Displays all time slots as clickable chips
- [ ] Disables past times when booking today
- [ ] Allows all times for future dates
- [ ] Highlights selected time slot
- [ ] Shows error if booking without time selection

### Booking Flow
- [ ] Selects first option by default
- [ ] Allows switching between options
- [ ] Validates time selection before booking
- [ ] Calls `onBook` callback with correct data
- [ ] Shows loading state during booking
- [ ] Navigates to booking page if no callback provided

### Responsive Design
- [ ] Displays correctly on mobile (< 600px)
- [ ] Displays correctly on tablet (600-960px)
- [ ] Displays correctly on desktop (> 960px)
- [ ] Time slots wrap properly on small screens

---

## Performance Optimization

### Memoization

```jsx
import { memo } from 'react';

const TourPricingOptions = memo(({ tourId, bookingDate, ... }) => {
  // Component code
}, (prevProps, nextProps) => {
  // Custom comparison
  return prevProps.tourId === nextProps.tourId &&
         prevProps.bookingDate === nextProps.bookingDate &&
         prevProps.adults === nextProps.adults &&
         prevProps.children === nextProps.children;
});
```

### Lazy Loading

```jsx
import { lazy, Suspense } from 'react';

const TourPricingOptions = lazy(() =>
  import('@/app/components/TourPricingOptions')
);

function TourDetailPage() {
  return (
    <Suspense fallback={<CircularProgress />}>
      <TourPricingOptions {...props} />
    </Suspense>
  );
}
```

---

## Future Enhancements

1. **Real-time Availability**: Show remaining spots for each time slot
2. **Dynamic Pricing**: Update prices based on selected time (premium times)
3. **Multi-option Booking**: Allow booking multiple options at once
4. **Wishlist Integration**: Add "Save for Later" button
5. **Comparison View**: Side-by-side option comparison
6. **Price Alerts**: Notify users when prices drop
7. **Group Booking**: Special handling for large groups

---

## Support

For questions or issues:
1. Check Tour schema: `TOUR_SCHEMA_UPDATE.md`
2. Check pricing API: `TOUR_PRICING_API.md`
3. Review component props and examples above
4. Test with sample data using Storybook (if available)

---

## Example Data

### Sample API Response for Testing

```json
{
  "status": 200,
  "data": {
    "tourId": "507f1f77bcf86cd799439011",
    "tourName": "Ha Long Bay Day Cruise",
    "departureDate": "2025-12-25T00:00:00.000Z",
    "passengers": {
      "adults": 2,
      "children": 1,
      "total": 3
    },
    "options": [
      {
        "id": "507f1f77bcf86cd799439012",
        "name": "Private Luxury Tour",
        "description": "<ul><li>Private boat</li><li>English-speaking guide</li><li>Seafood lunch</li><li>Hotel pickup</li></ul>",
        "basePrice": 3500000,
        "departureTimes": "08:00 AM;10:30 AM;02:00 PM",
        "pricing": {
          "basePrice": 3500000,
          "passengerCount": 2,
          "subtotal": 7000000,
          "surcharges": { "total": 700000, "breakdown": [] },
          "promotions": { "total": 0, "breakdown": [] },
          "tax": { "rate": 15, "amount": 1155000 },
          "total": 8855000,
          "children": {
            "count": 1,
            "pricePerChild": 2625000,
            "subtotal": 2625000
          },
          "grandTotal": 11480000
        }
      },
      {
        "id": "507f1f77bcf86cd799439013",
        "name": "Group Tour",
        "description": "<ul><li>Shared boat (max 12 people)</li><li>English-speaking guide</li><li>Set lunch menu</li><li>Hotel pickup</li></ul>",
        "basePrice": 1800000,
        "departureTimes": "07:30 AM;01:00 PM",
        "pricing": {
          "basePrice": 1800000,
          "passengerCount": 2,
          "subtotal": 3600000,
          "surcharges": { "total": 360000, "breakdown": [] },
          "promotions": { "total": 0, "breakdown": [] },
          "tax": { "rate": 15, "amount": 594000 },
          "total": 4554000,
          "children": {
            "count": 1,
            "pricePerChild": 1350000,
            "subtotal": 1350000
          },
          "grandTotal": 5904000
        }
      }
    ],
    "currency": "VND"
  }
}
```

Use this sample data for development and testing!
