# Quick Reference: Tour Management System

## 📁 Project Structure

```
leetour-app/
├── apps/
│   ├── api/                     # Node.js API (Port 3001)
│   │   ├── src/
│   │   │   ├── models/
│   │   │   │   ├── Tour.js      ✅ UPDATED (DAYTRIP aligned)
│   │   │   │   └── Booking.js   ✅ UPDATED (DAYTRIPBOOKING aligned)
│   │   │   └── app/api/
│   │   │       ├── tours/       ✅ Pricing endpoints
│   │   │       └── admin/tours/ ✅ CRUD endpoints
│   │
│   ├── admin/                   # Admin Panel (Port 3000)
│   │   └── src/app/(DashboardLayout)/admin/tours/
│   │       ├── new/page.jsx     ⚠️ Needs enhancement
│   │       ├── [id]/edit/page.jsx
│   │       ├── [id]/pricing/page.jsx
│   │       └── [id]/itinerary/  ❌ To create
│   │
│   ├── frontend/                # Public Site (Port 3002)
│   │   └── src/app/
│   │       ├── tours/           ❌ To create
│   │       └── booking/         ❌ To create
│   │
│   └── cs_source/               # C# Reference Code
│       ├── Admin/Views/Tour/    # C# Admin Views
│       └── FrontEnd/Views/Tour/ # C# Frontend Views
│
└── docs/
    ├── API_MIGRATION_SUMMARY.md      ✅ Field mapping guide
    ├── REACT_MIGRATION_GUIDE.md      ✅ UI implementation guide
    ├── MIGRATION_COMPLETE_SUMMARY.md ✅ Complete overview
    └── QUICK_REFERENCE.md            ✅ This file
```

---

## 🔄 Data Flow

```
┌─────────────┐         ┌─────────────┐         ┌─────────────┐
│   Admin     │ ──API── │   Node.js   │ ──ORM── │  MongoDB    │
│   (React)   │ ◄─JSON─ │   API       │ ◄─Data─ │  Database   │
└─────────────┘         └─────────────┘         └─────────────┘
                               ▲
                               │
┌─────────────┐                │
│  Frontend   │ ───────────────┘
│  (React)    │
└─────────────┘
```

---

## 🗄️ Database Schema

### Tour Collection
```javascript
{
  _id: ObjectId,

  // C# Legacy
  daytripId: 123,              // DAYTRIP.DAYTRIPID

  // Core
  name: "Halong Bay Cruise",   // DAYTRIP.NAME
  title: "Halong Bay Cruise",  // DAYTRIP.Title
  description: "...",          // DAYTRIP.DESCRIPTION
  overView: "...",             // DAYTRIP.OverView

  // Pricing
  priceFrom: Decimal128(100),  // DAYTRIP.PRICE_FROM
  price: 120,
  currency: "USD",
  commissionRate: 10,          // DAYTRIP.CommissionRate

  // Location (Flat)
  city: "Halong",              // DAYTRIP.City
  country: "Vietnam",          // DAYTRIP.Country
  locationId: 5,               // DAYTRIP.LocationId
  countryId: 1,                // DAYTRIP.CountryId

  // Location (Nested - for maps)
  locationDetails: {
    city: "Halong",
    country: "Vietnam",
    coordinates: { lat: 20.95, lng: 107.08 }
  },

  // Tour Details
  pickupPoint: "Hotel lobby",  // DAYTRIP.PickupPoint
  dropOffPoint: "Hotel lobby", // DAYTRIP.DropOffPoint
  groupSize: "2-15 people",    // DAYTRIP.GroupSize
  transport: "Bus",            // DAYTRIP.Transport
  startingTime: "08:00 AM",    // DAYTRIP.StartingTime
  duration: "1 day",           // DAYTRIP.Duration
  travelStyle: "Adventure",    // DAYTRIP.TravelStyle

  // Content (HTML)
  hightLight: "<ul>...</ul>",  // DAYTRIP.HightLight
  include: "<ul>...</ul>",     // DAYTRIP.Include
  exclude: "<ul>...</ul>",     // DAYTRIP.Exclude
  programeDetail: "<div>...</div>", // DAYTRIP.ProgrameDetail
  notes: "...",                // DAYTRIP.Notes

  // Images
  image: "/uploads/tour.jpg",  // DAYTRIP.IMAGE
  images: [...],
  galleryImages: [...],

  // SEO
  url: "halong-bay-cruise",    // DAYTRIP.URL
  seoKeyword: "halong, cruise", // DAYTRIP.SEO_Keyword
  seoDescription: "...",       // DAYTRIP.SEO_DESCRIPTION

  // Rating
  startRating: 4.5,            // DAYTRIP.START_RATING
  rating: { average: 4.5, count: 100 },

  // Booking Settings
  startBooking: 1,             // DAYTRIP.STARTBOOKING (days before)
  endBooking: 30,              // DAYTRIP.ENDBOOKING (days before)

  // Cancellation Policy
  cancelPolicyType: 1,         // DAYTRIP.CANCELPOLICYTYPE
  cancelPolicyFromDay: 7,      // DAYTRIP.CANCELPOLICY_FROMDAY
  cancelPolicyToDay: 1,        // DAYTRIP.CANCELPOLICY_TODAY
  cancelPolicyValue1: "...",   // DAYTRIP.CANCELPOLICYVALUE1
  cancelPolicyValue1Vn: "...", // Vietnamese version
  cancelPolicyValue2: "...",
  cancelPolicyValue2Vn: "...",

  // Pricing Tiers (NEW!)
  dayTripRates: [
    {
      persons: 1,              // DayTripRate.persons
      netRate: Decimal128(100), // DayTripRate.NetRate
      retailRate: Decimal128(120), // DayTripRate.RetailRate
      ageFrom: 12,             // DayTripRate.AgeFrom
      ageTo: 65,               // DayTripRate.AgeTo
      description: "Adult"     // DayTripRate.Description
    },
    {
      persons: 1,
      netRate: Decimal128(70),
      retailRate: Decimal128(84),
      ageFrom: 3,
      ageTo: 11,
      description: "Child"
    }
  ],

  // Itinerary
  itinerary: [
    {
      dayNumber: 1,
      header: "Day 1: Arrival",
      textDetail: "...",
      meals: { breakfast: false, lunch: true, dinner: true },
      accommodation: "Hotel XYZ",
      transport: "Private car"
    }
  ],

  // Promotions & Surcharges
  promotions: [...],
  surcharges: [...],

  // Status
  status: 1,                   // DAYTRIP.Status (0=inactive, 1=active)
  isActive: true,              // Auto-synced with status
  isFeatured: false,

  // Other
  operatorId: 1,               // DAYTRIP.OperatorId
  createdBy: ObjectId,
  createdAt: ISODate,
  updatedAt: ISODate
}
```

### Booking Collection
```javascript
{
  _id: ObjectId,

  // C# Legacy
  id: 456,                     // DAYTRIPBOOKING.ID
  daytripId: 123,              // DAYTRIPBOOKING.DaytripID
  customerId: 789,             // DAYTRIPBOOKING.CUSTOMERID

  // IDs
  bookingId: "BK-2024-00001",
  receiptId: "RC-2024-00001",  // DAYTRIPBOOKING.RECEIPTID

  // Tour Reference
  tour: {
    tourId: ObjectId,
    name: "Halong Bay Cruise", // DAYTRIPBOOKING.NAME
    title: "Halong Bay Cruise",
    price: 120
  },

  // Guest Info
  guestFirstName: "John",      // DAYTRIPBOOKING.GuestFirstName
  guestLastName: "Doe",        // DAYTRIPBOOKING.GuestLastName
  guestNationality: 1,         // DAYTRIPBOOKING.GuestNationality
  ownerNotStayAtHotel: false,  // DAYTRIPBOOKING.OwnerNotStayAtHotel

  // Booking Details
  date: ISODate,               // DAYTRIPBOOKING.Date
  checkIn: ISODate,            // DAYTRIPBOOKING.CHECK_IN
  checkOut: ISODate,           // DAYTRIPBOOKING.CHECK_OUT
  day: 1,                      // DAYTRIPBOOKING.DAY
  startTime: "08:00 AM",       // DAYTRIPBOOKING.STARTTIME
  rooms: 1,                    // DAYTRIPBOOKING.ROOMS
  person: 2,                   // DAYTRIPBOOKING.Person

  // Participants
  participants: {
    adults: 2,
    children: 0,
    infants: 0,
    totalCount: 2
  },

  // Pricing
  roomRate: Decimal128(120),   // DAYTRIPBOOKING.ROOM_RATE
  feeTax: Decimal128(12),      // DAYTRIPBOOKING.FEE_TAX
  surcharge: Decimal128(10),   // DAYTRIPBOOKING.SURCHARGE
  surchargeName: "Weekend",    // DAYTRIPBOOKING.SURCHARGENAME
  total: Decimal128(142),      // DAYTRIPBOOKING.TOTAL

  // Payment
  paymentStatus: 0,            // DAYTRIPBOOKING.PaymentStatus
  paymentType: 1,              // DAYTRIPBOOKING.PaymentType
  payment: {
    method: "credit_card",
    status: "completed",
    transactionId: "TXN-123"
  },

  // Refund
  isRefund: false,             // DAYTRIPBOOKING.ISREFUND
  refundFee: Decimal128(0),    // DAYTRIPBOOKING.RefundFee

  // System Fields
  description: "...",          // DAYTRIPBOOKING.DESCRIPTION
  specialRequest: "...",       // DAYTRIPBOOKING.SpecialRequest
  amenBooking: false,          // DAYTRIPBOOKING.AMENBOOKING
  sendReceipt: true,           // DAYTRIPBOOKING.SENDRECEIPT
  sendVoucher: true,           // DAYTRIPBOOKING.SENDVOUCHER
  ipLocation: "192.168.1.1",   // DAYTRIPBOOKING.IPLOCATION
  editBy: 1,                   // DAYTRIPBOOKING.EDITBY

  status: "confirmed",
  createdAt: ISODate,
  updatedAt: ISODate
}
```

---

## 🔌 API Endpoints

### Tours

```
GET    /api/tours              # List public tours
GET    /api/tours/:id          # Get single tour
GET    /api/tours/:id/pricing  # Get pricing details
GET    /api/tours/:id/options  # Get pricing options

# Query parameters for pricing:
?date=2024-01-15&adults=2&children=1&infants=0
```

### Admin Tours

```
GET    /api/admin/tours        # List all tours (with inactive)
POST   /api/admin/tours        # Create tour
GET    /api/admin/tours/:id    # Get single tour
PUT    /api/admin/tours/:id    # Update tour
DELETE /api/admin/tours/:id    # Soft delete (sets isActive=false)

# Query parameters:
?page=1&limit=10&category=Adventure&status=active
```

### Bookings (To Implement)

```
POST   /api/bookings/create-tour-order  # Create booking
GET    /api/bookings/:id                # Get booking
PUT    /api/bookings/:id                # Update booking
POST   /api/bookings/:id/cancel         # Cancel with refund
```

---

## 💻 Code Examples

### Create Tour (Admin)
```javascript
const createTour = async () => {
  const response = await fetch('/api/admin/tours', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      name: 'Halong Bay Cruise',
      title: 'Halong Bay Cruise',
      description: 'Beautiful cruise experience',
      overView: 'One day cruise in Halong Bay',
      priceFrom: 100,
      price: 120,
      city: 'Halong',
      country: 'Vietnam',
      pickupPoint: 'Hotel lobby',
      dropOffPoint: 'Hotel lobby',
      dayTripRates: [
        {
          persons: 1,
          netRate: 100,
          retailRate: 120,
          ageFrom: 12,
          ageTo: 65,
          description: 'Adult'
        }
      ],
      itinerary: [
        {
          dayNumber: 1,
          header: 'Day 1: Cruise',
          textDetail: '<p>Enjoy the cruise...</p>',
          meals: { breakfast: false, lunch: true, dinner: true }
        }
      ],
      isActive: true
    })
  });
  return response.json();
};
```

### Get Pricing (Frontend)
```javascript
const getPricing = async (tourId, date, adults, children) => {
  const params = new URLSearchParams({
    date,
    adults: adults.toString(),
    children: children.toString()
  });

  const response = await fetch(
    `/api/tours/${tourId}/pricing?${params}`
  );
  const { data } = await response.json();

  return data.pricing; // { basePrice, adultPrice, childPrice, total, ... }
};
```

### Display Pricing
```jsx
function PricingDisplay({ pricing }) {
  return (
    <div className="pricing">
      <div>Adults: {pricing.adults} × ${pricing.adultPrice}</div>
      {pricing.children > 0 && (
        <div>Children: {pricing.children} × ${pricing.childPrice}</div>
      )}
      <div>Subtotal: ${pricing.subtotal}</div>
      {pricing.discount > 0 && (
        <div className="discount">Discount: -${pricing.discount}</div>
      )}
      {pricing.surcharge > 0 && (
        <div className="surcharge">Surcharge: +${pricing.surcharge}</div>
      )}
      <div>Taxes: ${pricing.taxes}</div>
      <div className="total">Total: ${pricing.total}</div>
    </div>
  );
}
```

---

## 🎨 UI Components Checklist

### Admin (apps/admin)

**Existing:**
- [x] Tour list with filters
- [x] Basic tour form
- [x] Pricing options management
- [x] Promotions management
- [x] Surcharges management
- [x] Cancellation policies management
- [x] Image gallery management

**To Add:**
- [ ] Name field (in addition to title)
- [ ] Overview field (in addition to description)
- [ ] Price From field (Decimal)
- [ ] Commission rate field
- [ ] Flat location fields (city, country separate)
- [ ] Pickup/dropoff points
- [ ] Group size, transport, travel style
- [ ] HTML editors for content (hightLight, include, exclude, programeDetail)
- [ ] SEO fields (url, seoKeyword, seoDescription)
- [ ] Booking window (startBooking, endBooking)
- [ ] Cancellation policy fields (all 8)
- [ ] DayTripRates table management
- [ ] Itinerary timeline page
- [ ] Operator ID selector

### Frontend (apps/frontend)

**To Create:**
- [ ] Tour listing page (grid + filters)
- [ ] Tour detail page (hero + sidebar)
- [ ] Booking widget (date picker + passenger count)
- [ ] Pricing display (real-time calculation)
- [ ] Tour itinerary display (brief + detailed)
- [ ] Review section (rating chart + cards)
- [ ] Similar tours carousel
- [ ] Booking form
- [ ] Confirmation page
- [ ] Inquiry form

---

## 🔍 Field Sync Reference

The following fields are automatically synchronized:

| Primary Field | Synced Field | Direction |
|---------------|--------------|-----------|
| `name` | `title` | ↔️ Bidirectional |
| `status` (0/1) | `isActive` (false/true) | ↔️ Bidirectional |
| `startRating` | `rating.average` | ↔️ Bidirectional |
| `seoKeyword` | `keywords[]` | → Split on comma |
| `locationDetails.city` | `city` | → Copy if city empty |
| `locationDetails.country` | `country` | → Copy if country empty |

---

## ⚡ Quick Commands

```bash
# Start all services
npm run dev  # In each app directory (api, admin, frontend)

# API only
cd apps/api && npm run dev  # Port 3001

# Admin only
cd apps/admin && npm run dev  # Port 3000

# Frontend only
cd apps/frontend && npm run dev  # Port 3002

# Build for production
npm run build

# Database
mongosh  # Connect to MongoDB
use leetour  # Switch to database
db.tours.find()  # Query tours
db.bookings.find()  # Query bookings
```

---

## 📞 Support

- **Documentation:** See `/docs` folder
- **API Models:** `apps/api/src/models/`
- **C# Reference:** `apps/cs_source/`
- **Issues:** Create issue with detailed description

---

## ✅ Pre-Launch Checklist

- [ ] All DAYTRIP fields in admin forms
- [ ] DayTripRates management working
- [ ] Itinerary management page created
- [ ] Frontend tour listing page
- [ ] Frontend tour detail page
- [ ] Booking widget functional
- [ ] Pricing calculations correct
- [ ] Email notifications setup
- [ ] Payment integration complete
- [ ] Data migration from C# completed
- [ ] Testing completed (unit + integration + E2E)
- [ ] Performance optimized
- [ ] Security audit passed
- [ ] Documentation updated
- [ ] Training materials created

---

Last Updated: 2026-01-03
