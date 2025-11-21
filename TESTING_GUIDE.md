# Testing Guide - Tour Pricing System

## Overview
This guide will help you test the integrated tour pricing system end-to-end.

---

## Prerequisites

### 1. Start the API Server

```bash
cd apps/api
npm run dev
```

The API should be running on `http://localhost:3001`

### 2. Start the Frontend Server

```bash
cd apps/frontend
npm run dev
```

The frontend should be running on `http://localhost:3000`

### 3. Ensure Database is Running

```bash
# If using local MongoDB
mongod

# If using MongoDB Atlas, ensure connection string is configured
```

---

## Test Scenarios

### Scenario 1: Basic Pricing Check

**Steps:**
1. Navigate to a tour detail page: `http://localhost:3000/tours/[tourId]`
2. Select a departure date (future date)
3. Select number of adults (e.g., 2)
4. Leave children at 0
5. Click "Check Availability"

**Expected Result:**
- Loading indicator appears on button
- After ~1-2 seconds, pricing options appear below
- Page auto-scrolls to pricing section
- At least one tour option is displayed
- Prices are formatted correctly in VND

**Verification:**
```javascript
// Check browser console for:
console.log('Pricing data:', pricingData);

// Should see structure:
{
  tourId: "...",
  tourName: "...",
  options: [
    {
      id: "...",
      name: "Option 1",
      pricing: {
        basePrice: 150000,
        grandTotal: 345000,
        // ...
      }
    }
  ]
}
```

---

### Scenario 2: Pricing with Children

**Steps:**
1. Navigate to tour detail page
2. Select departure date
3. Select adults: 2
4. Set children: 1
5. Click "Check Availability"

**Expected Result:**
- Pricing options show children pricing
- Children pay 75% of adult price
- Grand total includes children cost
- Display: "X VND × 2 Adult + Y VND × 1 Children = Total VND"

**Verification:**
```javascript
// Children price should be 75% of base price
childPrice = basePrice * 0.75
// Example: 150000 * 0.75 = 112500 VND
```

---

### Scenario 3: Time Slot Selection

**Steps:**
1. Complete Scenario 1 to show pricing options
2. Click on the first pricing option (it's auto-selected)
3. View available departure times
4. Click on a time slot (e.g., "08:00 AM")
5. Click "Book Now"

**Expected Result:**
- Time slot highlights when selected
- Other time slots deselect
- Past times are disabled (if booking today)
- After clicking "Book Now", redirected to booking page with query params

**URL After Redirect:**
```
/tours/[tourId]/booking?date=2025-12-25&time=08:00 AM&adults=2&children=0&optionId=...&optionName=Private Tour&totalPrice=345000
```

---

### Scenario 4: Past Time Validation (Same-Day Booking)

**Steps:**
1. Select TODAY's date
2. Check availability
3. View time slots

**Expected Result:**
- Past times are grayed out and disabled
- Cannot select past times
- Only future times are clickable
- Hover shows "not-allowed" cursor on past times

**Verification:**
```javascript
// Check time comparison logic
const now = new Date();
const selectedTime = "08:00 AM";
// If current time is 10:00 AM, 08:00 AM should be disabled
```

---

### Scenario 5: Error Handling - No Date Selected

**Steps:**
1. Navigate to tour detail page
2. Do NOT select a date
3. Click "Check Availability"

**Expected Result:**
- Red error alert appears: "Please select a departure date"
- No pricing options load
- Button is disabled when no date is selected

---

### Scenario 6: Error Handling - Invalid Tour

**Steps:**
1. Navigate to: `http://localhost:3000/tours/invalidtourid123`

**Expected Result:**
- "Tour Not Found" message appears
- "Browse All Tours" button displays
- No crash or white screen

---

### Scenario 7: Multiple Tour Options

**Setup Required:**
Create a tour with multiple pricing options in the database:

```javascript
// Add to tour document
tourOptions: [
  {
    optionName: "Private Luxury Tour",
    description: "Private boat with seafood lunch",
    basePrice: 3500000,
    departureTimes: "08:00 AM;10:30 AM;02:00 PM",
    isActive: true
  },
  {
    optionName: "Group Tour",
    description: "Shared boat with set menu",
    basePrice: 1800000,
    departureTimes: "07:30 AM;01:00 PM",
    isActive: true
  },
  {
    optionName: "Budget Tour",
    description: "Basic tour with packed lunch",
    basePrice: 950000,
    departureTimes: "09:00 AM",
    isActive: true
  }
]
```

**Steps:**
1. Navigate to this tour
2. Check availability

**Expected Result:**
- All 3 options display
- First option has "MOST POPULAR" badge
- Can select different options
- Each option shows its own departure times
- Prices differ between options

---

### Scenario 8: Surcharge Application

**Setup Required:**
Add a surcharge to the tour:

```javascript
surcharges: [
  {
    surchargeName: "Christmas Holiday",
    surchargeType: "holiday",
    startDate: new Date("2025-12-20"),
    endDate: new Date("2025-12-26"),
    amountType: "percentage",
    amount: 20, // 20% surcharge
    isActive: true
  }
]
```

**Steps:**
1. Select date between Dec 20-26, 2025
2. Check availability

**Expected Result:**
- Pricing breakdown shows "Surcharges" section
- 20% surcharge applied
- Total increased accordingly
- Surcharge name and amount displayed

**Verification:**
```javascript
// Check pricing breakdown
pricing: {
  subtotal: 300000,
  surcharges: {
    total: 60000,  // 20% of 300000
    breakdown: [{
      name: "Christmas Holiday",
      calculatedAmount: 60000
    }]
  },
  amountAfterSurcharges: 360000
}
```

---

### Scenario 9: Promotion Application

**Setup Required:**
Add a promotion to the tour:

```javascript
promotions: [
  {
    promotionName: "Early Bird 10%",
    promotionType: "early_bird",
    discountType: "percentage",
    discountAmount: 10,
    validFrom: new Date("2025-11-01"),
    validTo: new Date("2026-03-31"),
    daysBeforeDeparture: 30,
    isActive: true
  }
]
```

**Steps:**
1. Book with date 30+ days in advance
2. Check availability

**Expected Result:**
- Pricing breakdown shows "Promotions" section
- 10% discount applied
- Total reduced accordingly
- Promotion name and amount displayed

**Verification:**
```javascript
// Check pricing breakdown
pricing: {
  amountAfterSurcharges: 360000,
  promotions: {
    total: 36000,  // 10% of 360000
    breakdown: [{
      name: "Early Bird 10%",
      calculatedAmount: 36000
    }]
  },
  subtotalAfterDiscount: 324000
}
```

---

### Scenario 10: Responsive Design

**Steps:**
1. Open tour detail page on desktop
2. Check availability
3. Resize browser to mobile width (< 600px)
4. Check availability again

**Expected Result:**
- Booking sidebar stacks below on mobile
- Pricing options are readable
- Time slots wrap properly
- Book Now button is accessible
- No horizontal scroll

**Test Breakpoints:**
- Desktop: > 960px
- Tablet: 600-960px
- Mobile: < 600px

---

## API Testing

### Direct API Call Testing

#### Test 1: Get Pricing for Tour

```bash
curl "http://localhost:3001/api/tours/[tourId]/pricing?date=2025-12-25&adults=2&children=1"
```

**Expected Response:**
```json
{
  "status": 200,
  "data": {
    "tourId": "...",
    "tourName": "...",
    "passengers": {
      "adults": 2,
      "children": 1,
      "total": 3
    },
    "options": [...]
  },
  "msg": "success"
}
```

#### Test 2: Missing Date Parameter

```bash
curl "http://localhost:3001/api/tours/[tourId]/pricing?adults=2"
```

**Expected Response:**
```json
{
  "status": 400,
  "msg": "Departure date is required"
}
```

#### Test 3: Invalid Date Format

```bash
curl "http://localhost:3001/api/tours/[tourId]/pricing?date=invalid-date&adults=2"
```

**Expected Response:**
```json
{
  "status": 400,
  "msg": "Invalid date format"
}
```

---

## Database Verification

### Check Tour Document Structure

```javascript
// Connect to MongoDB
use leetour;

// Find a tour and check structure
db.tours.findOne({ _id: ObjectId("...") })

// Verify required fields exist:
{
  tourOptions: [
    {
      optionName: "...",
      basePrice: 150000,
      departureTimes: "08:00 AM;10:30 AM",
      isActive: true
    }
  ],
  surcharges: [...],
  promotions: [...],
  isActive: true
}
```

---

## Performance Testing

### Load Testing

```bash
# Install Apache Bench
apt-get install apache2-utils

# Test API endpoint
ab -n 100 -c 10 "http://localhost:3001/api/tours/[tourId]/pricing?date=2025-12-25&adults=2&children=0"

# Expected: < 500ms response time for 95th percentile
```

### Memory Leak Testing

1. Open Chrome DevTools
2. Go to Performance tab
3. Record while checking availability multiple times
4. Check for memory growth
5. Take heap snapshot before and after

**Expected:** No significant memory increase after multiple checks

---

## Browser Compatibility

Test on:
- [ ] Chrome (latest)
- [ ] Firefox (latest)
- [ ] Safari (latest)
- [ ] Edge (latest)
- [ ] Mobile Safari (iOS)
- [ ] Chrome Mobile (Android)

---

## Common Issues & Solutions

### Issue 1: Pricing Not Loading

**Symptoms:**
- Clicking "Check Availability" does nothing
- No loading indicator
- No error message

**Solutions:**
1. Check browser console for errors
2. Verify API server is running
3. Check CORS settings
4. Verify `NEXT_PUBLIC_API_URL` env variable

### Issue 2: Incorrect Prices

**Symptoms:**
- Prices don't match expected values
- Children pricing wrong
- Tax not applied

**Solutions:**
1. Check tour document in database
2. Verify `pricingCalculator.js` logic
3. Check surcharges/promotions date ranges
4. Console.log pricing breakdown

### Issue 3: Time Slots Not Showing

**Symptoms:**
- No departure times displayed
- Empty time slot section

**Solutions:**
1. Check `departureTimes` field in tour option
2. Verify semicolon-separated format
3. Check for empty strings
4. Default should be "08:00 AM"

### Issue 4: Booking Page Not Loading

**Symptoms:**
- After clicking "Book Now", 404 error
- Blank page

**Solutions:**
1. Create booking page: `apps/frontend/src/app/tours/[id]/booking/page.jsx`
2. Check router push URL format
3. Verify query parameters

---

## Success Criteria

✅ All scenarios pass without errors
✅ Pricing calculations are accurate
✅ Time validation works correctly
✅ Error messages are clear and helpful
✅ Responsive design works on all devices
✅ API response time < 500ms
✅ No console errors
✅ Smooth user experience

---

## Reporting Issues

When reporting bugs, include:
1. Steps to reproduce
2. Expected vs actual result
3. Browser and version
4. Screenshot/video
5. Console errors
6. Network tab data (for API issues)

---

## Next Steps After Testing

1. Fix any issues found
2. Add automated tests (Jest, Cypress)
3. Set up monitoring (Sentry, LogRocket)
4. Deploy to staging
5. User acceptance testing
6. Deploy to production

---

## Quick Test Checklist

Before deployment, verify:
- [ ] Can view tour details
- [ ] Can select date and passengers
- [ ] Check availability works
- [ ] Pricing options display correctly
- [ ] Time slots are selectable
- [ ] Past times are disabled
- [ ] Children pricing is 75% of adult
- [ ] Surcharges apply correctly
- [ ] Promotions apply correctly
- [ ] 15% tax is added
- [ ] Book Now redirects properly
- [ ] Error handling works
- [ ] Mobile responsive
- [ ] No console errors
- [ ] API returns within 500ms

---

Happy Testing! 🚀
