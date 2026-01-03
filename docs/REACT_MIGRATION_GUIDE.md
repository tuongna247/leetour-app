# React Migration Guide: C# .cshtml to React.js

## Overview
This guide documents the migration from C# Razor views (.cshtml) to React.js components for the tour booking system.

## Current Status

### ✅ Already Completed
- **Admin Tour Management** - Basic React structure exists at `apps/admin/src/app/(DashboardLayout)/admin/tours/`
- **API Routes** - Tour pricing and options endpoints functional
- **Models** - MongoDB models aligned with C# DAYTRIP and DAYTRIPBOOKING structures

### 🔄 Needs Enhancement
The existing React components need to be updated to include all new fields from the C# models.

---

## Admin Components Structure

### Current Admin Pages

| Page | Path | Status | Enhancement Needed |
|------|------|--------|-------------------|
| Tour List | `admin/tours/page.jsx` | ✅ Exists | Add new field columns |
| New Tour | `admin/tours/new/page.jsx` | ✅ Exists | Add DAYTRIP fields |
| Edit Tour | `admin/tours/[id]/edit/page.jsx` | ✅ Exists | Add DAYTRIP fields |
| Pricing | `admin/tours/[id]/pricing/page.jsx` | ✅ Exists | Add DayTripRates management |
| Pricing Options | `admin/tours/[id]/pricing-options/page.jsx` | ✅ Exists | OK |
| Images | `admin/tours/[id]/images/page.jsx` | ✅ Exists | OK |
| Promotions | `admin/tours/[id]/promotions/page.jsx` | ✅ Exists | OK |
| Surcharges | `admin/tours/[id]/surcharges/page.jsx` | ✅ Exists | OK |
| Cancellation | `admin/tours/[id]/cancellation-policies/page.jsx` | ✅ Exists | OK |
| Itinerary | - | ❌ Missing | Create new page |

---

## Required Enhancements

### 1. Add New Tour Form (`admin/tours/new/page.jsx`)

**Fields to Add:**

#### Core Information (DAYTRIP fields)
```javascript
{
  // Already exists: title, description
  name: '', // Primary name field (DAYTRIP.NAME)
  overView: '', // Overview summary (DAYTRIP.OverView)

  // Pricing
  priceFrom: '', // Starting price (DAYTRIP.PRICE_FROM)
  commissionRate: '', // Commission % (DAYTRIP.CommissionRate)

  // Location (flat structure)
  city: '', // City name (DAYTRIP.City)
  country: '', // Country name (DAYTRIP.Country)
  location: '', // Location description (DAYTRIP.Location)
  locationId: null, // Location ID (DAYTRIP.LocationId)
  countryId: null, // Country ID (DAYTRIP.CountryId)

  // Tour Details
  pickupPoint: '', // Pickup location (DAYTRIP.PickupPoint)
  dropOffPoint: '', // Dropoff location (DAYTRIP.DropOffPoint)
  groupSize: '', // Group size info (DAYTRIP.GroupSize)
  transport: '', // Transportation (DAYTRIP.Transport)
  startingTime: '', // Start time (DAYTRIP.StartingTime)
  travelStyle: '', // Travel style (DAYTRIP.TravelStyle)

  // Content (HTML fields)
  hightLight: '', // Highlights HTML (DAYTRIP.HightLight)
  include: '', // Included HTML (DAYTRIP.Include)
  exclude: '', // Excluded HTML (DAYTRIP.Exclude)
  programeDetail: '', // Program HTML (DAYTRIP.ProgrameDetail)
  notes: '', // Notes (DAYTRIP.Notes)

  // Images
  image: '', // Primary image URL (DAYTRIP.IMAGE)

  // SEO
  url: '', // URL slug (DAYTRIP.URL)
  seoKeyword: '', // Keywords (DAYTRIP.SEO_Keyword)
  seoDescription: '', // Meta description (DAYTRIP.SEO_DESCRIPTION)

  // Rating
  startRating: 0, // Average rating (DAYTRIP.START_RATING)

  // Booking Settings
  startBooking: null, // Booking start days (DAYTRIP.STARTBOOKING)
  endBooking: null, // Booking end days (DAYTRIP.ENDBOOKING)

  // Cancellation Policy
  cancelPolicyType: null, // Policy type (DAYTRIP.CANCELPOLICYTYPE)
  cancelPolicyFromDay: null, // From days (DAYTRIP.CANCELPOLICY_FROMDAY)
  cancelPolicyToDay: null, // To days (DAYTRIP.CANCELPOLICY_TODAY)
  cancelPolicyValue1: '', // Policy value 1 (DAYTRIP.CANCELPOLICYVALUE1)
  cancelPolicyValue1Vn: '', // Policy VN 1 (DAYTRIP.CANCELPOLICYVALUE1_VN)
  cancelPolicyValue2: '', // Policy value 2 (DAYTRIP.CANCELPOLICYVALUE2)
  cancelPolicyValue2Vn: '', // Policy VN 2 (DAYTRIP.CANCELPOLICYVALUE2_VN)

  // Operator
  operatorId: null, // Operator ID (DAYTRIP.OperatorId)

  // Status
  status: 1, // 0=inactive, 1=active (DAYTRIP.Status)
}
```

### 2. Create Itinerary Management Page

**New File:** `apps/admin/src/app/(DashboardLayout)/admin/tours/[id]/itinerary/page.jsx`

**Features:**
- Timeline-based UI (similar to C# CD-timeline component)
- Day-by-day itinerary blocks
- Rich text editor for descriptions
- Meal checkboxes (Breakfast, Lunch, Dinner)
- Transport options
- Image upload per day
- Overnight/accommodation info
- Drag-and-drop reordering

**Data Structure:**
```javascript
{
  itinerary: [
    {
      dayNumber: 1,
      header: 'Day 1: Arrival',
      textDetail: 'Detailed HTML description',
      activity: 'Arrival and city tour',
      meals: {
        breakfast: false,
        lunch: true,
        dinner: true
      },
      meal: 'Lunch, Dinner', // String format
      accommodation: 'Hotel XYZ',
      overnight: 'Hotel XYZ',
      transport: 'Private car',
      image: '/images/day1.jpg'
    }
  ]
}
```

### 3. Create DayTripRates Management Component

**New File:** `apps/admin/src/components/forms/DayTripRatesManager.jsx`

**Features:**
- Table with columns: Persons, Net Rate, Retail Rate, Age From, Age To, Description
- Add/Edit/Delete rates
- Age range selector (for child/adult pricing)
- Real-time retail rate calculation

**Data Structure:**
```javascript
{
  dayTripRates: [
    {
      persons: 1,
      netRate: 100,
      retailRate: 120,
      ageFrom: 12,
      ageTo: 65,
      description: 'Adult (12-65 years)'
    },
    {
      persons: 1,
      netRate: 70,
      retailRate: 84,
      ageFrom: 3,
      ageTo: 11,
      description: 'Child (3-11 years)'
    }
  ]
}
```

---

## Frontend Components Structure

### Required Frontend Pages

| Page | Path | Status | C# Equivalent |
|------|------|--------|---------------|
| Tour List | `frontend/tours/page.jsx` | ❌ Create | TourList.cshtml |
| Tour Detail | `frontend/tours/[id]/page.jsx` | ❌ Create | Detail.cshtml |
| Booking Widget | Component | ❌ Create | Sidebar in Detail.cshtml |
| Payment Info | `frontend/payment/page.jsx` | ❌ Create | Payment.cshtml |
| Inquiry Form | `frontend/inquiry/page.jsx` | ❌ Create | Inquiry.cshtml |

### 1. Tour List Page

**File:** `apps/frontend/src/app/tours/page.jsx`

**Features:**
- 4-column responsive grid
- Tour cards with:
  - Image thumbnail
  - Tour name
  - Price (from priceFrom or price)
  - Rating stars
  - "Book Now" button
- Filters (country, city, category, price range)
- Pagination
- Search functionality

**API Calls:**
```javascript
GET /api/tours?page=1&limit=12&category=Adventure&country=Vietnam
```

### 2. Tour Detail Page

**File:** `apps/frontend/src/app/tours/[id]/page.jsx`

**Layout:**
- **Hero Section:** Image carousel (1200x470px)
- **Breadcrumb:** Home > Tours > [Country] > [Tour Name]
- **Title Bar:** Tour name, rating, location
- **Sticky Sidebar:** Booking widget (date picker, person count, "Check Availability")
- **Content Tabs/Sections:**
  - Overview
  - Itinerary (Brief table + Detailed expandable)
  - What's Included/Excluded
  - Important Notes
  - Reviews
  - Similar Tours

**Components Needed:**
```
- TourHero.jsx (image carousel)
- BookingWidget.jsx (sidebar booking form)
- TourOverview.jsx
- TourItinerary.jsx (brief + detailed)
- TourInclusions.jsx
- TourReviews.jsx (rating distribution + review cards)
- SimilarTours.jsx (carousel)
```

### 3. Booking Widget Component

**File:** `apps/frontend/src/components/BookingWidget.jsx`

**Features:**
- Date picker (with availability check)
- Adult count dropdown
- Children count input (with age range)
- Infant count input
- "Check Availability" button
- Price display (updates dynamically)
- "Book Now" button
- "Customize Itinerary" link

**API Calls:**
```javascript
// Get pricing for selected date and passengers
GET /api/tours/[id]/pricing?date=2024-01-15&adults=2&children=1&infants=0

// Get pricing options
GET /api/tours/[id]/options?date=2024-01-15&adults=2&children=1
```

### 4. Tour Review Section

**Features:**
- Overall rating score (large display)
- Star distribution chart (5-star to 1-star with progress bars)
- Individual review cards:
  - User avatar (letter circle if no image)
  - User name
  - Star rating
  - Review title
  - Review content
  - Date
- Pagination for reviews

**Data Structure:**
```javascript
{
  rating: {
    average: 4.5,
    count: 234
  },
  reviews: [
    {
      user: {
        name: 'John Doe',
        avatar: '/images/users/john.jpg'
      },
      guestName: 'John Doe',
      rating: 5,
      title: 'Amazing experience!',
      comment: 'This tour exceeded all expectations...',
      date: '2024-01-15T10:30:00Z',
      verified: true
    }
  ]
}
```

---

## Recommended React Libraries

### UI Components
- **Material-UI (MUI)** - Already in use for admin
- **Tailwind CSS** - For frontend styling
- **React Bootstrap** - Alternative for frontend

### Form Management
- **React Hook Form** - Form state management
- **Yup** or **Zod** - Validation schemas

### Rich Text Editors
- **TipTap** - Modern rich text editor (already in admin)
- **Draft.js** - Alternative

### Date/Time
- **React DatePicker** - Date selection
- **date-fns** - Date manipulation (already in use)

### Image Management
- **React Dropzone** - File upload (already in admin)
- **React Image Gallery** - Image carousel
- **Swiper** or **React Slick** - Carousels

### Tables
- **React Table** or **TanStack Table** - Data tables with sorting/filtering

### State Management
- **Context API** - For booking flow state
- **Zustand** - Lightweight state management
- **Redux Toolkit** - If complex state needed

### Other
- **React Star Ratings** - Star rating display
- **React Paginate** - Pagination
- **Chart.js** or **Recharts** - Rating distribution charts

---

## Migration Checklist

### Admin Panel

- [x] Basic tour form structure exists
- [ ] Add all DAYTRIP fields to create/edit forms
- [ ] Create DayTripRates management component
- [ ] Create Itinerary management page with timeline UI
- [ ] Update tour list to show new fields
- [ ] Add rich text editor for HTML content fields
- [ ] Add multi-select for cities/countries
- [ ] Add travel style checkboxes
- [ ] Implement commission rate calculator

### Frontend

- [ ] Create tour listing page with grid layout
- [ ] Create tour detail page with sidebar
- [ ] Implement booking widget with date picker
- [ ] Create pricing display with real-time calculations
- [ ] Implement review section with rating charts
- [ ] Create itinerary display (brief + detailed)
- [ ] Add image carousel/gallery
- [ ] Implement similar tours carousel
- [ ] Create inquiry form
- [ ] Add payment information page

### Shared Components

- [ ] Create reusable star rating component
- [ ] Create price display component
- [ ] Create image upload/gallery component
- [ ] Create rich text display component
- [ ] Create breadcrumb component (frontend)
- [ ] Create pagination component
- [ ] Create filter/search component

---

## Implementation Priority

### Phase 1: Admin Enhancements (High Priority)
1. Add new DAYTRIP fields to create/edit forms
2. Create DayTripRates management component
3. Create Itinerary management page
4. Test full tour creation workflow

### Phase 2: Frontend Core (High Priority)
1. Create tour listing page
2. Create tour detail page structure
3. Implement booking widget
4. Connect to pricing API

### Phase 3: Frontend Features (Medium Priority)
1. Add review section
2. Implement image carousels
3. Add similar tours
4. Create inquiry form

### Phase 4: Polish & Optimization (Low Priority)
1. Add loading states
2. Implement error handling
3. Add animations
4. Optimize performance
5. Mobile responsiveness
6. SEO optimization

---

## API Integration Examples

### Creating a Tour (Admin)
```javascript
const createTour = async (formData) => {
  const response = await fetch('/api/admin/tours', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      name: formData.name,
      title: formData.title,
      description: formData.description,
      overView: formData.overView,
      priceFrom: formData.priceFrom,
      city: formData.city,
      country: formData.country,
      pickupPoint: formData.pickupPoint,
      dropOffPoint: formData.dropOffPoint,
      dayTripRates: formData.dayTripRates,
      itinerary: formData.itinerary,
      // ... other fields
    })
  });
  return response.json();
};
```

### Fetching Tour Pricing (Frontend)
```javascript
const fetchPricing = async (tourId, date, adults, children) => {
  const params = new URLSearchParams({
    date,
    adults: adults.toString(),
    children: children.toString()
  });

  const response = await fetch(
    `/api/tours/${tourId}/pricing?${params}`
  );
  return response.json();
};
```

### Displaying Pricing
```javascript
const PricingDisplay = ({ pricing }) => {
  return (
    <div>
      <div>Base Price: ${pricing.basePrice}</div>
      <div>Adults ({pricing.adults} x ${pricing.adultPrice}):
        ${pricing.adults * pricing.adultPrice}
      </div>
      {pricing.children > 0 && (
        <div>Children ({pricing.children} x ${pricing.childPrice}):
          ${pricing.children * pricing.childPrice}
        </div>
      )}
      <div>Subtotal: ${pricing.subtotal}</div>
      {pricing.discount > 0 && (
        <div>Discount: -${pricing.discount}</div>
      )}
      {pricing.surcharge > 0 && (
        <div>Surcharge: +${pricing.surcharge}</div>
      )}
      <div>Taxes: ${pricing.taxes}</div>
      <div><strong>Total: ${pricing.total}</strong></div>
    </div>
  );
};
```

---

## Styling Guidelines

### Admin Panel
- Use Material-UI components for consistency
- Follow existing admin theme
- Use MUI Grid v2 for layouts
- Stick to MUI color palette

### Frontend
- Create custom theme matching brand colors
- Use Tailwind for utility classes
- Responsive design (mobile-first)
- Match C# frontend styling where appropriate
- Use CSS modules or styled-components for component styles

### Responsive Breakpoints
```javascript
const breakpoints = {
  xs: 0,      // phones
  sm: 600,    // tablets
  md: 900,    // small laptops
  lg: 1200,   // desktops
  xl: 1536    // large screens
};
```

---

## Testing Strategy

1. **Unit Tests:** Test individual components
2. **Integration Tests:** Test API interactions
3. **E2E Tests:** Test full booking flow
4. **Visual Regression:** Compare with C# screenshots
5. **Accessibility:** WCAG 2.1 compliance
6. **Performance:** Lighthouse scores

---

## Next Steps

1. Review this guide with the team
2. Prioritize components to build
3. Set up development environment
4. Create component library/storybook
5. Start with Phase 1 (Admin Enhancements)
6. Parallel track: Start Phase 2 (Frontend Core)
