# Tour Database Schema Updates

## Overview
This document outlines the database schema updates made to support the tour detail page UI from Detail.cshtml.

## Date: 2025-11-21

---

## New Fields Added

### 1. Tour Level Fields

#### `overview` (String)
- **Purpose**: Separate marketing content for "Why you'll love this trip" section
- **Type**: String (HTML supported)
- **Default**: Empty string
- **Usage**: `tour.overview`
- **Example**:
  ```
  "Experience the best of Vietnam with our carefully curated tour..."
  ```

#### `includeActivity` (String)
- **Purpose**: HTML-formatted content for included activities/services
- **Type**: String (HTML)
- **Default**: Empty string
- **Usage**: `tour.includeActivity`
- **Note**: Use this for rich HTML content. Array field `included` still available for simple lists

#### `excludeActivity` (String)
- **Purpose**: HTML-formatted content for excluded activities/services
- **Type**: String (HTML)
- **Default**: Empty string
- **Usage**: `tour.excludeActivity`
- **Note**: Use this for rich HTML content. Array field `excluded` still available for simple lists

#### `notes` (String)
- **Purpose**: Additional tour notes and important information
- **Type**: String (HTML)
- **Default**: Empty string
- **Usage**: `tour.notes`
- **Example**:
  ```
  "<ul><li>Please bring comfortable walking shoes</li><li>Sunscreen recommended</li></ul>"
  ```

#### `keywords` (Array of Strings)
- **Purpose**: Searchable keywords for tour discovery
- **Type**: Array of String
- **Default**: Empty array
- **Usage**: `tour.keywords`
- **Note**: Auto-synced with `seo.keywords`
- **Example**: `['adventure', 'hiking', 'mountains', 'nature']`

#### `type` (String)
- **Purpose**: Tour type classification
- **Type**: String (enum: 'daytrip', 'tour')
- **Default**: 'daytrip'
- **Usage**: `tour.type`
- **Note**: This is an alias for `tourType` for frontend compatibility

---

### 2. Review Schema Updates

#### `guestName` (String)
- **Purpose**: Guest name for reviews (compatibility field)
- **Type**: String
- **Usage**: `review.guestName`
- **Note**: Auto-populated from `review.user.name` if not provided

#### `title` (String)
- **Purpose**: Review title/summary
- **Type**: String
- **Default**: Empty string
- **Usage**: `review.title`
- **Example**: "Amazing experience!"

#### `reviewContent` (String)
- **Purpose**: Alias for comment field
- **Type**: String
- **Usage**: `review.reviewContent`
- **Note**: Auto-synced with `review.comment`

---

### 3. Itinerary Schema Updates

#### `activity` (String)
- **Purpose**: Activity title (alias for header)
- **Type**: String
- **Usage**: `day.activity`
- **Note**: Auto-synced with `day.header`

#### `title` (String)
- **Purpose**: Day title (alias for header)
- **Type**: String
- **Usage**: `day.title`
- **Note**: Auto-synced with `day.header`

#### `description` (String)
- **Purpose**: Day description (alias for textDetail)
- **Type**: String
- **Usage**: `day.description`
- **Note**: Auto-synced with `day.textDetail`

#### `meal` (String)
- **Purpose**: Meal information as formatted string
- **Type**: String
- **Default**: Empty string
- **Usage**: `day.meal`
- **Example**: "Breakfast, Lunch, Dinner"
- **Note**: Auto-generated from `day.meals` object if not provided

#### `overnight` (String)
- **Purpose**: Overnight accommodation location (alias for accommodation)
- **Type**: String
- **Default**: Empty string
- **Usage**: `day.overnight`
- **Note**: Auto-synced with `day.accommodation`

#### `transport` (String)
- **Purpose**: Transportation details for the day
- **Type**: String
- **Default**: Empty string
- **Usage**: `day.transport`
- **Example**: "Private van, Boat"

#### `image` (String)
- **Purpose**: Day-specific image URL
- **Type**: String
- **Default**: Empty string
- **Usage**: `day.image`
- **Example**: "/images/tours/day1-halong-bay.jpg"

---

## Auto-Sync Functionality

The schema includes pre-save middleware that automatically syncs related fields:

### 1. Tour Type Sync
```javascript
// tourType ⟷ type
if (tourType is modified) → sync to type
if (type is modified) → sync to tourType
```

### 2. Keywords Sync
```javascript
// seo.keywords ⟷ keywords
if (seo.keywords is modified) → sync to keywords
if (keywords is modified) → sync to seo.keywords
```

### 3. Review Fields Sync
```javascript
// For each review:
if (!guestName && user.name exists) → guestName = user.name
if (!reviewContent && comment exists) → reviewContent = comment
```

### 4. Itinerary Fields Sync
```javascript
// For each itinerary day:
if (!activity && header exists) → activity = header
if (!title && header exists) → title = header
if (!description && textDetail exists) → description = textDetail
if (!overnight && accommodation exists) → overnight = accommodation

// Generate meal string from meals object:
if (!meal && meals object exists) →
  meal = join(['Breakfast', 'Lunch', 'Dinner'] where true)
```

---

## Migration Guide

### For Existing Tours

No migration is required for existing tours. The new fields are:
- **Optional** with default values
- **Auto-synced** from existing fields via pre-save middleware

### Recommended Actions

1. **Update Admin UI** to include new fields:
   - Add "Overview" rich text editor
   - Add "Include Activity" HTML editor
   - Add "Exclude Activity" HTML editor
   - Add "Notes" rich text editor
   - Add "Keywords" tag input
   - Add "Transport" field to itinerary days
   - Add "Image" upload to itinerary days

2. **Update API Responses** to include new fields in tour detail endpoints

3. **Populate New Fields** for existing tours (optional):
   ```javascript
   // Example: Populate overview from description
   tours.forEach(tour => {
     if (!tour.overview && tour.description) {
       tour.overview = tour.description.substring(0, 500);
       tour.save();
     }
   });
   ```

---

## Field Mapping Reference

| UI Field (Detail.cshtml) | Database Field | Type | Notes |
|--------------------------|----------------|------|-------|
| `tour.overview` | `overview` | String | New field |
| `tour.includeActivity` | `includeActivity` | String (HTML) | New field |
| `tour.excludeActivity` | `excludeActivity` | String (HTML) | New field |
| `tour.notes` | `notes` | String (HTML) | New field |
| `tour.keywords` | `keywords` | Array | New field, synced with seo.keywords |
| `tour.type` | `type` or `tourType` | String | New alias |
| `review.guestName` | `guestName` | String | New field, synced with user.name |
| `review.title` | `title` | String | New field |
| `review.reviewContent` | `reviewContent` | String | New field, synced with comment |
| `day.activity` | `activity` | String | New field, synced with header |
| `day.title` | `title` | String | New field, synced with header |
| `day.description` | `description` | String | New field, synced with textDetail |
| `day.meal` | `meal` | String | New field, generated from meals object |
| `day.overnight` | `overnight` | String | New field, synced with accommodation |
| `day.transport` | `transport` | String | New field |
| `day.image` | `image` | String (URL) | New field |

---

## Examples

### Creating a Tour with New Fields

```javascript
const newTour = new Tour({
  title: "Ha Long Bay Day Trip",
  description: "Explore the stunning Ha Long Bay...",
  overview: "<p>Why you'll love this trip: Experience breathtaking limestone karsts...</p>",
  price: 99,
  category: "Nature",
  location: {
    city: "Ha Long",
    country: "Vietnam"
  },
  includeActivity: "<ul><li>Hotel pickup and drop-off</li><li>English-speaking guide</li></ul>",
  excludeActivity: "<ul><li>Personal expenses</li><li>Tips and gratuities</li></ul>",
  notes: "<p><strong>Important:</strong> Please bring sunscreen and comfortable shoes.</p>",
  keywords: ["halong", "bay", "cruise", "nature", "UNESCO"],
  type: "daytrip",
  itinerary: [{
    dayNumber: 1,
    header: "Ha Long Bay Cruise",
    activity: "Ha Long Bay Cruise", // Auto-synced
    textDetail: "Start your day with hotel pickup...",
    description: "Start your day with hotel pickup...", // Auto-synced
    meals: {
      breakfast: true,
      lunch: true,
      dinner: false
    },
    meal: "Breakfast, Lunch", // Auto-generated
    transport: "Private van, Cruise boat",
    image: "/images/tours/halong-cruise.jpg",
    accommodation: "N/A",
    overnight: "N/A" // Auto-synced
  }],
  reviews: [{
    user: {
      name: "John Doe",
      email: "john@example.com"
    },
    guestName: "John Doe", // Auto-synced
    title: "Incredible experience!",
    rating: 5,
    comment: "This tour exceeded all expectations...",
    reviewContent: "This tour exceeded all expectations..." // Auto-synced
  }],
  createdBy: userId
});

await newTour.save();
```

---

## Testing Checklist

- [ ] Create new tour with all new fields
- [ ] Verify auto-sync works for aliased fields
- [ ] Update existing tour and verify fields persist
- [ ] Test frontend tour detail page displays all fields correctly
- [ ] Verify meal string generation from meals object
- [ ] Test keywords search functionality
- [ ] Verify HTML rendering for includeActivity, excludeActivity, notes
- [ ] Test itinerary with transport and day images
- [ ] Verify review display with guestName and title

---

## Questions or Issues?

If you encounter any issues with the schema updates:
1. Check that mongoose models are properly imported
2. Verify database connection
3. Clear any cached schemas
4. Restart API server after schema changes

For field-specific questions, refer to the field mapping table above.
