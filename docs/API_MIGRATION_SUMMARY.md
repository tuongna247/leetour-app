# API Migration Summary: C# to MongoDB Models

## Overview
This document summarizes the migration of the tour booking system from C# (cs_source) models to MongoDB models for the Node.js/Next.js API.

## Updated Models

### 1. Tour Model (DAYTRIP Alignment)

**File:** `apps/api/src/models/Tour.js`

**Key Changes:**
- Added `daytripId` field for C# legacy compatibility
- Renamed `title` → `name` (with `title` as secondary field)
- Added all DAYTRIP.cs fields:
  - `priceFrom` (Decimal128) - Starting price
  - `commissionRate` - Commission percentage
  - `startingTime` - Default start time
  - `pickupPoint`, `dropOffPoint` - Pickup/dropoff locations
  - `groupSize` - Group size information
  - `transport` - Transportation details
  - `hightLight`, `include`, `exclude`, `programeDetail` - Content fields (HTML)
  - `startBooking`, `endBooking` - Booking window settings
  - `cancelPolicyType`, `cancelPolicyFromDay`, `cancelPolicyToDay` - Cancellation policy fields
  - `cancelPolicyValue1`, `cancelPolicyValue2` (+ Vietnamese versions) - Policy descriptions
  - `operatorId` - Tour operator reference
  - `status` (Number) - C# status field (0=inactive, 1=active)
  - `url`, `seoKeyword`, `seoDescription` - SEO fields
  - `startRating` - Average rating field

**New Schema: DayTripRate**
- `persons` - Group size tier (e.g., 1, 2-5, 6+)
- `netRate` (Decimal128) - Wholesale/net price
- `retailRate` (Decimal128) - Retail selling price
- `ageFrom`, `ageTo` - Age range for pricing
- `description` - Rate description

**Location Fields:**
- Flat structure: `location`, `city`, `country`, `locationId`, `countryId`
- Nested structure: `locationDetails` (for coordinates/maps)

**Field Synchronization:**
- `name` ↔ `title` - Automatically synced
- `status` (Number) ↔ `isActive` (Boolean) - Bidirectional sync
- `startRating` ↔ `rating.average` - Synced on save
- `seoKeyword` → `keywords[]` - Auto-split on comma

---

### 2. Booking Model (DAYTRIPBOOKING Alignment)

**File:** `apps/api/src/models/Booking.js`

**Key Changes:**
- Added `id` (Number) for C# legacy ID compatibility
- Added `receiptId` - Receipt/confirmation ID
- Added `daytripId` - Reference to C# daytrip ID
- Added `customerId` - C# customer ID reference

**Guest Information:**
- `guestFirstName`, `guestLastName` - Guest details
- `guestNationality` (Number) - Nationality ID
- `ownerNotStayAtHotel` (Boolean) - Flag for owner not staying

**Booking Details:**
- `date` - Booking date
- `checkIn`, `checkOut` - Check-in/out dates
- `day` - Number of days
- `startTime` - Tour start time
- `rooms` - Number of rooms
- `person` - Number of persons (C# field)

**Pricing Fields (Decimal128):**
- `roomRate` - Rate per room/person
- `feeTax` - Taxes and fees
- `surcharge` - Additional charges
- `surchargeName` - Surcharge description
- `total` - Total booking cost

**Payment Fields:**
- `paymentStatus` (Number) - C# payment status code
- `paymentType` (Number) - C# payment type code
- `payment` (Object) - Extended payment details

**Refund Information:**
- `isRefund` (Boolean) - Refund flag
- `refundFee` (Decimal128) - Refund fee amount

**System Fields:**
- `amenBooking` - Amended booking flag
- `sendReceipt`, `sendVoucher` - Email sent flags
- `ipLocation` - IP address of booking
- `editBy` - User ID who last edited
- `description` - Booking description/notes
- `specialRequest` - Special requests

**New Indexes:**
- `id`, `receiptId`, `daytripId`, `customerId`
- `paymentStatus`, `date`, `checkIn`

---

## Field Mapping Reference

### DAYTRIP.cs → Tour Model

| C# Field | MongoDB Field | Type | Notes |
|----------|---------------|------|-------|
| `DAYTRIPID` | `daytripId` | Number | Legacy ID |
| `NAME` | `name` | String | Primary name field |
| `Title` | `title` | String | Secondary title |
| `DESCRIPTION` | `description` | String | Full description |
| `OverView` | `overView` | String | Summary |
| `PRICE_FROM` | `priceFrom` | Decimal128 | Starting price |
| `Duration` | `duration` | String | Duration text |
| `StartingTime` | `startingTime` | String | Start time |
| `Location` | `location` | String | Location name |
| `LocationId` | `locationId` | Number | Location ID |
| `City` | `city` | String | City name |
| `Country` | `country` | String | Country name |
| `CountryId` | `countryId` | Number | Country ID |
| `PickupPoint` | `pickupPoint` | String | Pickup location |
| `DropOffPoint` | `dropOffPoint` | String | Dropoff location |
| `GroupSize` | `groupSize` | String | Group size info |
| `Transport` | `transport` | String | Transport details |
| `HightLight` | `hightLight` | String | Highlights HTML |
| `Include` | `include` | String | Included HTML |
| `Exclude` | `exclude` | String | Excluded HTML |
| `ProgrameDetail` | `programeDetail` | String | Program details |
| `Notes` | `notes` | String | Additional notes |
| `IMAGE` | `image` | String | Primary image URL |
| `URL` | `url` | String | SEO URL slug |
| `SEO_Keyword` | `seoKeyword` | String | SEO keywords |
| `SEO_DESCRIPTION` | `seoDescription` | String | SEO description |
| `START_RATING` | `startRating` | Number | Average rating |
| `STARTBOOKING` | `startBooking` | Number | Booking start days |
| `ENDBOOKING` | `endBooking` | Number | Booking end days |
| `CANCELPOLICYTYPE` | `cancelPolicyType` | Number | Policy type |
| `CANCELPOLICY_FROMDAY` | `cancelPolicyFromDay` | Number | From days |
| `CANCELPOLICY_TODAY` | `cancelPolicyToDay` | Number | To days |
| `CANCELPOLICYVALUE1` | `cancelPolicyValue1` | String | Policy value 1 |
| `CANCELPOLICYVALUE1_VN` | `cancelPolicyValue1Vn` | String | Policy VN 1 |
| `CANCELPOLICYVALUE2` | `cancelPolicyValue2` | String | Policy value 2 |
| `CANCELPOLICYVALUE2_VN` | `cancelPolicyValue2Vn` | String | Policy VN 2 |
| `CommissionRate` | `commissionRate` | Number | Commission % |
| `OperatorId` | `operatorId` | Number | Operator ID |
| `Status` | `status` | Number | 0=inactive, 1=active |
| `TravelStyle` | `travelStyle` | String | Travel style |

### DayTripRate.cs → dayTripRates[]

| C# Field | MongoDB Field | Type | Notes |
|----------|---------------|------|-------|
| `id` | `_id` | ObjectId | Auto-generated |
| `DaytripId` | (parent ref) | - | Referenced by parent |
| `persons` | `persons` | Number | Group size |
| `NetRate` | `netRate` | Decimal128 | Net price |
| `RetailRate` | `retailRate` | Decimal128 | Retail price |
| `AgeFrom` | `ageFrom` | Number | Min age |
| `AgeTo` | `ageTo` | Number | Max age |
| `Description` | `description` | String | Rate description |

### DAYTRIPBOOKING.cs → Booking Model

| C# Field | MongoDB Field | Type | Notes |
|----------|---------------|------|-------|
| `ID` | `id` | Number | Legacy ID |
| `DaytripID` | `daytripId` | Number | Daytrip reference |
| `CUSTOMERID` | `customerId` | Number | Customer ID |
| `RECEIPTID` | `receiptId` | String | Receipt ID |
| `NAME` | `tour.name` | String | Tour name |
| `GuestFirstName` | `guestFirstName` | String | Guest first name |
| `GuestLastName` | `guestLastName` | String | Guest last name |
| `GuestNationality` | `guestNationality` | Number | Nationality ID |
| `OwnerNotStayAtHotel` | `ownerNotStayAtHotel` | Boolean | Owner not staying |
| `Date` | `date` | Date | Booking date |
| `CHECK_IN` | `checkIn` | Date | Check-in date |
| `CHECK_OUT` | `checkOut` | Date | Check-out date |
| `DAY` | `day` | Number | Number of days |
| `STARTTIME` | `startTime` | String | Start time |
| `ROOMS` | `rooms` | Number | Number of rooms |
| `Person` | `person` | Number | Number of persons |
| `ROOM_RATE` | `roomRate` | Decimal128 | Room rate |
| `FEE_TAX` | `feeTax` | Decimal128 | Taxes/fees |
| `SURCHARGE` | `surcharge` | Decimal128 | Surcharge amount |
| `SURCHARGENAME` | `surchargeName` | String | Surcharge name |
| `TOTAL` | `total` | Decimal128 | Total cost |
| `PaymentStatus` | `paymentStatus` | Number | Payment status |
| `PaymentType` | `paymentType` | Number | Payment type |
| `ISREFUND` | `isRefund` | Boolean | Refund flag |
| `RefundFee` | `refundFee` | Decimal128 | Refund fee |
| `DESCRIPTION` | `description` | String | Booking notes |
| `SpecialRequest` | `specialRequest` | String | Special requests |
| `AMENBOOKING` | `amenBooking` | Boolean | Amended flag |
| `SENDRECEIPT` | `sendReceipt` | Boolean | Receipt sent |
| `SENDVOUCHER` | `sendVoucher` | Boolean | Voucher sent |
| `IPLOCATION` | `ipLocation` | String | IP address |
| `EDITBY` | `editBy` | Number | Editor user ID |

---

## Next Steps

### Remaining Tasks

1. **Update API Routes** - Align endpoints with C# controller patterns
   - [ ] Tour pricing endpoints (GetTourPriceDetail, GetTourPriceOptionDetail)
   - [ ] Booking creation endpoints (CreateTourOrder2, CreateOtherProduct)
   - [ ] Tour search/filter endpoints

2. **Update Admin UI** (`apps/admin`)
   - [ ] Tour management forms (add new fields)
   - [ ] Pricing tier management UI
   - [ ] Booking management pages
   - [ ] Cancellation policy management

3. **Update Frontend UI** (`apps/frontend`)
   - [ ] Tour detail pages (display new fields)
   - [ ] Booking flow (use new pricing structure)
   - [ ] Payment processing integration

4. **Create Migration Scripts**
   - [ ] Data migration script for existing tours
   - [ ] Index creation script
   - [ ] Field transformation utilities

5. **API Documentation**
   - [ ] Swagger/OpenAPI documentation
   - [ ] Field reference guide
   - [ ] Example requests/responses

---

## Migration Notes

### Breaking Changes
- Tour model now requires `name` field (was `title`)
- Location structure now uses flat fields (`city`, `country`) instead of nested `location` object
- Pricing uses Decimal128 for precision
- Status field is now Number (0/1) instead of Boolean

### Backward Compatibility
- Field aliases maintained where possible
- Pre-save middleware syncs related fields automatically
- Existing endpoints still supported

### Performance Considerations
- New indexes added for C# ID fields
- Text search index updated to include `name` field
- Consider archiving old bookings to separate collection

### Testing Recommendations
- Test booking creation with new pricing structure
- Verify all field syncs work correctly
- Test migration with sample C# data
- Performance test with large datasets

---

## References
- C# Models: `apps/cs_source/Vinaday.Data/Model/`
- MongoDB Models: `apps/api/src/models/`
- Agent Analysis: Task a9c64aa (cs_source API structure exploration)
