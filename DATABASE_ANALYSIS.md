# Database Collections Analysis

## Current API Models (NEEDED ✅)

Based on the models in `apps/api/src/models/`, your LeeTour app needs these collections:

1. **users** ✅ - User accounts (admin, supplier, customer, etc.)
   - Model: `User.js`
   - Currently has: 4 documents

2. **tours** ✅ - Tour listings with pricing, itinerary, images, etc.
   - Model: `Tour.js`
   - Currently has: 20 documents

3. **bookings** ✅ - Tour bookings by customers
   - Model: `Booking.js`
   - Currently has: 20 documents

4. **reviews** ✅ - Tour reviews (separate collection)
   - Model: `Review.js`
   - Currently has: 0 documents (needs to be created)

5. **receipts** ✅ - Booking receipts
   - Model: `Receipt.js`
   - Currently has: 0 documents (needs to be created)

6. **suppliers** ✅ - Supplier/vendor information
   - Model: `Supplier.js`
   - Currently has: 0 documents

7. **countries** (Referenced but model not found)
   - Referenced in: `User.js`, `Supplier.js`
   - Model: `Country.js` (exists)

8. **cities** (Referenced but model not found)
   - Model: `City.js` (exists)

9. **categories** (Referenced but model not found)
   - Model: `Category.js` (exists)

10. **supplierusers** (Referenced but model not found)
    - Model: `SupplierUser.js` (exists)

---

## Collections from Server (Currently Seeded)

From your backup, these collections were imported:

### ❌ NOT NEEDED - SPA/Service Related (Different Business Domain)

These collections appear to be from a **SPA/beauty salon system**, NOT a tour booking system:

1. **servicecategories** ❌ (2 docs) - Spa service categories
2. **combos** ❌ (3 docs) - Spa combo packages
3. **coupons** ❌ (4 docs) - Spa coupons/vouchers
4. **spacustomers** ❌ (3 docs) - Spa customers
5. **customerservices** ❌ (9 docs) - Customer service records
6. **customerbuyservicesusingservices** ❌ (0 docs) - Service usage tracking
7. **customercombos** ❌ (1 doc) - Customer combo purchases
8. **spaservicepromotions** ❌ (2 docs) - Spa service promotions
9. **customerbuyservices** ❌ (1 doc) - Customer service purchases
10. **services** ❌ (8 docs) - Spa services
11. **spapromotions** ❌ (2 docs) - Spa promotions
12. **customers** ❌ (3 docs) - Spa customers (duplicate with spacustomers)
13. **spacustomerpurchases** ❌ (0 docs) - Purchase tracking
14. **spaserviceusages** ❌ (0 docs) - Service usage tracking
15. **spaservices** ❌ (3 docs) - Spa services (duplicate)

### ✅ NEEDED - Tour Related

1. **users** ✅ (4 docs) - User accounts
2. **tours** ✅ (20 docs) - Tour listings
3. **bookings** ✅ (20 docs) - Tour bookings
4. **suppliers** ✅ (0 docs) - Supplier information

---

## Recommendation Summary

### Keep These Collections (4 total):
- ✅ **users** (4 documents)
- ✅ **tours** (20 documents)
- ✅ **bookings** (20 documents)
- ✅ **suppliers** (0 documents - empty but model exists)

### Delete These Collections (15 total):
All SPA-related collections should be removed:
- ❌ servicecategories
- ❌ combos
- ❌ coupons
- ❌ spacustomers
- ❌ customerservices
- ❌ customerbuyservicesusingservices
- ❌ customercombos
- ❌ spaservicepromotions
- ❌ customerbuyservices
- ❌ services
- ❌ spapromotions
- ❌ customers
- ❌ spacustomerpurchases
- ❌ spaserviceusages
- ❌ spaservices

### Missing Collections (Need to Add):
These are defined in your models but don't have data yet:
- ⚠️ **reviews** - No documents yet (model exists)
- ⚠️ **receipts** - No documents yet (model exists)
- ⚠️ **countries** - No documents yet (model exists)
- ⚠️ **cities** - No documents yet (model exists)
- ⚠️ **categories** - No documents yet (model exists)
- ⚠️ **supplierusers** - No documents yet (model exists)

---

## Database Size Impact

**Before cleanup:**
- Total collections: 19
- Total documents: 85
- Relevant documents: 44 (users + tours + bookings + suppliers)
- Irrelevant documents: 41 (SPA-related)

**After cleanup:**
- Total collections: 4 (possibly 10 with empty model collections)
- Total documents: 44
- Space saved: ~48% reduction in documents
- Cleaner database structure

---

## Next Steps

1. ✅ Create cleanup script to remove SPA collections
2. ✅ Create clean seed script with only tour-related data
3. ⚠️ Optionally add seed data for missing collections (countries, cities, categories)
4. ✅ Update database to use clean data
