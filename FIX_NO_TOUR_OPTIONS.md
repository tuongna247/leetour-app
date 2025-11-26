# Fix: No Tour Options Available

## Problem
API returns: `{"status":400,"msg":"No tour options available for this tour"}`

This means the tour exists in the database but doesn't have any `tourOptions` defined.

---

## Solution: Add Tour Options to Your Tour

### Step 1: Check if Tour Has Options

```bash
# Connect to MongoDB
mongosh

# Switch to database
use leetour

# Check the tour
db.tours.findOne(
  { "seo.slug": "indian-golden-triangle-tour" },
  { title: 1, tourOptions: 1 }
)
```

**If `tourOptions` is empty or undefined**, you need to add them.

---

### Step 2: Add Tour Options

#### Option A: Add a Simple Tour Option

```javascript
// In mongosh
db.tours.updateOne(
  { "seo.slug": "indian-golden-triangle-tour" },
  {
    $set: {
      tourOptions: [{
        optionName: "Standard Package",
        description: "Complete tour with all amenities",
        basePrice: 50000,
        pricingTiers: [],
        departureTimes: "08:00 AM;02:00 PM",
        includeItems: "Accommodation, Meals, Transportation, Guide",
        isActive: true
      }]
    }
  }
)
```

#### Option B: Add Multiple Tour Options

```javascript
// In mongosh
db.tours.updateOne(
  { "seo.slug": "indian-golden-triangle-tour" },
  {
    $set: {
      tourOptions: [
        {
          optionName: "Budget Package",
          description: "Affordable tour with basic amenities",
          basePrice: 30000,
          pricingTiers: [],
          departureTimes: "08:00 AM",
          includeItems: "Transportation, Guide",
          isActive: true
        },
        {
          optionName: "Standard Package",
          description: "Complete tour with all amenities",
          basePrice: 50000,
          pricingTiers: [],
          departureTimes: "08:00 AM;02:00 PM",
          includeItems: "Accommodation, Meals, Transportation, Guide",
          isActive: true
        },
        {
          optionName: "Luxury Package",
          description: "Premium tour with luxury accommodations",
          basePrice: 80000,
          pricingTiers: [],
          departureTimes: "08:00 AM;10:00 AM;02:00 PM",
          includeItems: "5-star Hotel, Fine Dining, Private Car, Expert Guide",
          isActive: true
        }
      ]
    }
  }
)
```

#### Option C: Add Options with Pricing Tiers (Group Discounts)

```javascript
// In mongosh
db.tours.updateOne(
  { "seo.slug": "indian-golden-triangle-tour" },
  {
    $set: {
      tourOptions: [{
        optionName: "Standard Package",
        description: "Complete tour with group pricing",
        basePrice: 50000,
        pricingTiers: [
          {
            minPassengers: 1,
            maxPassengers: 2,
            pricePerPerson: 50000
          },
          {
            minPassengers: 3,
            maxPassengers: 5,
            pricePerPerson: 45000
          },
          {
            minPassengers: 6,
            maxPassengers: 10,
            pricePerPerson: 40000
          }
        ],
        departureTimes: "08:00 AM;02:00 PM",
        includeItems: "Accommodation, Meals, Transportation, Guide",
        isActive: true
      }]
    }
  }
)
```

---

### Step 3: Verify the Update

```javascript
// In mongosh
db.tours.findOne(
  { "seo.slug": "indian-golden-triangle-tour" },
  { tourOptions: 1 }
)

// Should now show your tourOptions
```

---

### Step 4: Test the API Again

```bash
# Test pricing endpoint
curl "http://localhost:3001/api/tours/indian-golden-triangle-tour/pricing?date=2025-11-22&adults=1&children=0"
```

**Expected**: JSON response with pricing data

---

## Quick Fix Script

Save this as `add-tour-options.js`:

```javascript
const mongoose = require('mongoose');

async function addOptions() {
  await mongoose.connect('mongodb://localhost:27017/leetour');

  const Tour = mongoose.model('Tour', new mongoose.Schema({}, { strict: false }));

  await Tour.updateOne(
    { 'seo.slug': 'indian-golden-triangle-tour' },
    {
      $set: {
        tourOptions: [{
          optionName: "Standard Package",
          description: "Complete tour with all amenities",
          basePrice: 50000,
          departureTimes: "08:00 AM;02:00 PM",
          isActive: true
        }]
      }
    }
  );

  console.log('✓ Tour options added!');
  await mongoose.disconnect();
}

addOptions();
```

Run with: `cd apps/api && node ../../add-tour-options.js`

---

## Alternative: Use Sample Tour Data

If you want a complete tour with all fields, use the sample data:

```bash
# In mongosh
use leetour

# Delete old tour
db.tours.deleteOne({ "seo.slug": "indian-golden-triangle-tour" })

# Insert new complete tour from SAMPLE_TOUR_DATA.json
# Copy the content from that file and paste it
```

---

## Check All Tours Missing Options

```javascript
// In mongosh
db.tours.find(
  {
    $or: [
      { tourOptions: { $exists: false } },
      { tourOptions: { $size: 0 } }
    ]
  },
  { title: 1, "seo.slug": 1 }
)

// This will show all tours without tourOptions
```

---

## Add Options to All Tours

If you have multiple tours without options:

```javascript
// In mongosh
db.tours.updateMany(
  {
    $or: [
      { tourOptions: { $exists: false } },
      { tourOptions: { $size: 0 } }
    ]
  },
  {
    $set: {
      tourOptions: [{
        optionName: "Standard Package",
        description: "Complete tour package",
        basePrice: 50000,
        departureTimes: "08:00 AM;02:00 PM",
        isActive: true
      }]
    }
  }
)
```

---

## Tour Options Field Reference

```javascript
{
  optionName: String,           // Required: "Budget Package", "Standard", "Luxury"
  description: String,          // What's included
  basePrice: Number,            // Required: Base price per person
  pricingTiers: [{              // Optional: Group pricing
    minPassengers: Number,
    maxPassengers: Number,
    pricePerPerson: Number
  }],
  departureTimes: String,       // "08:00 AM;02:00 PM;05:00 PM"
  includeItems: String,         // What's included (HTML supported)
  isActive: Boolean             // true/false
}
```

---

## Common Prices (Reference)

| Package Type | Price (VND) | Price (USD) |
|--------------|-------------|-------------|
| Budget       | 30,000      | ~$1.25      |
| Standard     | 50,000      | ~$2.10      |
| Premium      | 80,000      | ~$3.35      |
| Luxury       | 120,000     | ~$5.00      |

*Note: Adjust prices based on your actual tour*

---

## After Adding Options

Once you've added tourOptions, the pricing API should work:

✅ **Before**: `{"status":400,"msg":"No tour options available"}`

✅ **After**:
```json
{
  "status": 200,
  "data": {
    "tourId": "...",
    "options": [{
      "id": "...",
      "name": "Standard Package",
      "pricing": { ... }
    }]
  }
}
```

---

## Need Help?

If still having issues:

1. Share the output of:
   ```javascript
   db.tours.findOne(
     { "seo.slug": "indian-golden-triangle-tour" },
     { title: 1, tourOptions: 1, price: 1 }
   )
   ```

2. Check if tour exists:
   ```javascript
   db.tours.countDocuments({ "seo.slug": "indian-golden-triangle-tour" })
   ```

3. List all tours:
   ```javascript
   db.tours.find({}, { title: 1, "seo.slug": 1 }).limit(10)
   ```
