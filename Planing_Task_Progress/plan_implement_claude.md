# Implementation Plan for Leetour App

## Overview
This document outlines the implementation plan for the Leetour Tour Booking Application based on the requirements in CLAUDE.md.

---

## Phase 1: Tour Management System

### 1.1 Tour Pricing Options (Tour Options)
**Objective**: Implement flexible pricing based on tour options and group size

**Tasks**:
- [ ] Design database schema for `tour_options` table
  - Fields: `id`, `tour_id`, `option_name`, `base_price`, `min_passengers`, `max_passengers`, `description`
- [ ] Create API endpoints for tour options CRUD
  - POST `/api/tours/:tourId/options` - Create option
  - GET `/api/tours/:tourId/options` - List options
  - PUT `/api/tours/:tourId/options/:optionId` - Update option
  - DELETE `/api/tours/:tourId/options/:optionId` - Delete option
- [ ] Build admin UI for managing tour options
  - Form to add/edit option name and base price
  - Dynamic pricing based on number of passengers
  - Validation for price ranges
- [ ] Implement pricing calculation logic
  - Calculate price based on passenger count
  - Consider shared vs private room/vehicle pricing

---

### 1.2 Surcharge System
**Objective**: Add surcharges for holidays, weekends, and special dates

**Tasks**:
- [ ] Design database schema for `surcharges` table
  - Fields: `id`, `tour_id`, `surcharge_type`, `start_date`, `end_date`, `amount_type` (percentage/fixed), `amount`, `description`
- [ ] Create API endpoints for surcharges
  - POST `/api/tours/:tourId/surcharges` - Create surcharge
  - GET `/api/tours/:tourId/surcharges` - List surcharges
  - PUT `/api/tours/:tourId/surcharges/:id` - Update surcharge
  - DELETE `/api/tours/:tourId/surcharges/:id` - Delete surcharge
- [ ] Build admin UI for surcharge management
  - Date range picker for applicable dates
  - Toggle between percentage and fixed amount
  - Holiday/weekend presets
- [ ] Implement surcharge calculation in booking flow
  - Check if booking date falls within surcharge period
  - Apply surcharge to total price
  - Display surcharge details to customer

---

### 1.3 Promotion System
**Objective**: Implement Early Bird, Last Minute, and other promotional discounts

**Tasks**:
- [ ] Design database schema for `promotions` table
  - Fields: `id`, `tour_id`, `promotion_type`, `discount_type` (percentage/fixed), `discount_amount`, `valid_from`, `valid_to`, `booking_window_start`, `booking_window_end`, `conditions`
- [ ] Create API endpoints for promotions
  - POST `/api/tours/:tourId/promotions` - Create promotion
  - GET `/api/tours/:tourId/promotions` - List promotions
  - PUT `/api/tours/:tourId/promotions/:id` - Update promotion
  - DELETE `/api/tours/:tourId/promotions/:id` - Delete promotion
- [ ] Build admin UI for promotion management
  - Promotion type selector (Early Bird, Last Minute, Custom)
  - Date range for promotion validity
  - Booking window configuration
  - Discount configuration (% or fixed amount)
- [ ] Implement promotion logic in booking flow
  - Check applicable promotions based on booking date
  - Apply promotion discount to price
  - Display promotion details prominently
  - Handle multiple promotion scenarios (priority rules)

---

### 1.4 Cancellation Policy
**Objective**: Define and enforce cancellation policies per tour

**Tasks**:
- [ ] Design database schema for `cancellation_policies` table
  - Fields: `id`, `tour_id`, `days_before_departure`, `refund_percentage`, `description`
- [ ] Create API endpoints for cancellation policies
  - POST `/api/tours/:tourId/cancellation-policies` - Create policy
  - GET `/api/tours/:tourId/cancellation-policies` - List policies
  - PUT `/api/tours/:tourId/cancellation-policies/:id` - Update policy
  - DELETE `/api/tours/:tourId/cancellation-policies/:id` - Delete policy
- [ ] Build admin UI for cancellation policy management
  - Tiered refund structure (e.g., 30 days = 100%, 14 days = 50%, 7 days = 0%)
  - Policy description editor
- [ ] Display cancellation policy on tour detail page
- [ ] Implement cancellation logic in booking management
  - Calculate refund amount based on cancellation date
  - Handle cancellation requests
  - Send cancellation confirmation emails

---

### 1.5 Tour Images
**Objective**: Manage tour images including featured image and banner slider

**Tasks**:
- [ ] Design database schema for `tour_images` table
  - Fields: `id`, `tour_id`, `image_url`, `image_type` (featured/banner), `display_order`, `alt_text`
- [ ] Implement image upload functionality
  - File validation (size, format)
  - Image optimization/compression
  - Cloud storage integration (AWS S3, Cloudinary, etc.)
- [ ] Create API endpoints for tour images
  - POST `/api/tours/:tourId/images` - Upload image
  - GET `/api/tours/:tourId/images` - List images
  - PUT `/api/tours/:tourId/images/:id` - Update image metadata
  - DELETE `/api/tours/:tourId/images/:id` - Delete image
- [ ] Build admin UI for image management
  - Drag-and-drop image uploader
  - Featured image selector (1 image)
  - Banner slider selector (3 images)
  - Image reordering for banner
  - Alt text editor for SEO
- [ ] Implement frontend image display
  - Featured image on tour cards/listings
  - Banner slider on tour detail page

---

### 1.6 Tour Types and Itinerary
**Objective**: Support two tour types (Daytrip and Multi-day Tour) with detailed itineraries

**Tasks**:
- [ ] Update `tours` table schema
  - Add field: `tour_type` (enum: 'daytrip', 'tour')
- [ ] Design database schema for `tour_itineraries` table
  - Fields: `id`, `tour_id`, `day_number`, `header`, `text_detail`, `created_at`, `updated_at`
- [ ] Create API endpoints for itineraries
  - POST `/api/tours/:tourId/itineraries` - Create itinerary day
  - GET `/api/tours/:tourId/itineraries` - List all days
  - PUT `/api/tours/:tourId/itineraries/:dayId` - Update day
  - DELETE `/api/tours/:tourId/itineraries/:dayId` - Delete day
- [ ] Build admin UI for tour type selection and itinerary management
  - Tour type toggle (Daytrip / Tour)
  - Itinerary editor (only for Multi-day Tour type)
  - Add/remove/reorder day-by-day itinerary
  - Rich text editor for day details (header + text detail)
- [ ] Implement frontend itinerary display
  - Accordion or tabs for each day
  - Print-friendly itinerary view

---

## Phase 2: Review System

### 2.1 Review Functionality
**Objective**: Allow customers to write and view reviews with spam protection

**Tasks**:
- [ ] Design database schema for `reviews` table
  - Fields: `id`, `tour_id`, `user_id`, `booking_id`, `rating`, `title`, `comment`, `verified_purchase`, `status` (pending/approved/rejected), `created_at`
- [ ] Integrate Google reCAPTCHA v3
  - Set up reCAPTCHA API keys
  - Create validation utility function
  - Implement server-side verification
- [ ] Create API endpoints for reviews
  - POST `/api/tours/:tourId/reviews` - Submit review (with reCAPTCHA)
  - GET `/api/tours/:tourId/reviews` - Get approved reviews
  - GET `/api/admin/reviews` - Admin list all reviews
  - PUT `/api/admin/reviews/:id/approve` - Approve review
  - PUT `/api/admin/reviews/:id/reject` - Reject review
  - DELETE `/api/admin/reviews/:id` - Delete review
- [ ] Build frontend review submission form
  - Star rating component
  - Title and comment fields
  - Google reCAPTCHA integration
  - Validation (require booking to review)
- [ ] Build frontend review display
  - Review list on tour detail page
  - Star rating summary
  - Filter/sort options (most recent, highest rated)
  - Pagination
- [ ] Build admin review moderation panel
  - List pending reviews
  - Approve/reject actions
  - Spam detection flags
  - Bulk actions

---

## Phase 3: Receipt Functionality

### 3.1 Receipt Management
**Objective**: Generate and manage receipts for bookings

**Tasks**:
- [ ] Design database schema for `receipts` table
  - Fields: `id`, `booking_id`, `receipt_number`, `issue_date`, `total_amount`, `payment_method`, `payment_status`, `pdf_url`, `created_at`
- [ ] Create receipt generation logic
  - Generate unique receipt numbers
  - Calculate totals (base price + surcharges - promotions + taxes)
  - Itemized breakdown
- [ ] Create API endpoints for receipts
  - POST `/api/bookings/:bookingId/receipt` - Generate receipt
  - GET `/api/receipts/:receiptId` - Get receipt details
  - GET `/api/receipts/:receiptId/pdf` - Download receipt PDF
  - GET `/api/admin/receipts` - List all receipts
- [ ] Build receipt PDF generator
  - Use library like PDFKit or Puppeteer
  - Company branding and logo
  - Receipt template with itemized details
  - Tax and payment information
- [ ] Build admin UI for receipt management
  - List all receipts
  - Search by receipt number, booking ID, customer
  - View receipt details
  - Regenerate receipt if needed
  - Download/email receipt
- [ ] Implement customer receipt access
  - View receipt in booking history
  - Download receipt PDF
  - Email receipt option

---

## Phase 4: Payment System Enhancement

### 4.1 Staff Payment Without Points
**Objective**: Allow staff to process tour payments without awarding customer loyalty points

**Tasks**:
- [ ] Update `bookings` table schema
  - Add field: `points_eligible` (boolean, default: true)
  - Add field: `processed_by_staff` (boolean, default: false)
  - Add field: `staff_user_id` (foreign key, nullable)
- [ ] Create staff payment processing endpoint
  - POST `/api/admin/bookings/:bookingId/process-payment` - Process payment as staff
  - Include flag to disable points accumulation
- [ ] Build admin UI for staff payment processing
  - Checkbox: "Process without loyalty points"
  - Reason field (optional, for audit trail)
  - Payment method selector
  - Confirmation dialog
- [ ] Update points calculation logic
  - Check `points_eligible` flag before awarding points
  - Log points waived in audit trail
- [ ] Create admin report for payments processed by staff
  - Filter bookings by staff-processed
  - Track points waived
  - Staff activity log

---

## Phase 5: Database Schema Summary

### Core Tables

```sql
-- Tours
CREATE TABLE tours (
  id UUID PRIMARY KEY,
  tour_type ENUM('daytrip', 'tour'),
  title VARCHAR(255),
  description TEXT,
  duration_days INT,
  created_at TIMESTAMP,
  updated_at TIMESTAMP
);

-- Tour Options
CREATE TABLE tour_options (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  option_name VARCHAR(255),
  base_price DECIMAL(10,2),
  min_passengers INT,
  max_passengers INT,
  description TEXT,
  created_at TIMESTAMP
);

-- Surcharges
CREATE TABLE surcharges (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  surcharge_type VARCHAR(50),
  start_date DATE,
  end_date DATE,
  amount_type ENUM('percentage', 'fixed'),
  amount DECIMAL(10,2),
  description TEXT,
  created_at TIMESTAMP
);

-- Promotions
CREATE TABLE promotions (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  promotion_type VARCHAR(50),
  discount_type ENUM('percentage', 'fixed'),
  discount_amount DECIMAL(10,2),
  valid_from DATE,
  valid_to DATE,
  booking_window_start DATE,
  booking_window_end DATE,
  conditions JSON,
  created_at TIMESTAMP
);

-- Cancellation Policies
CREATE TABLE cancellation_policies (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  days_before_departure INT,
  refund_percentage DECIMAL(5,2),
  description TEXT,
  display_order INT,
  created_at TIMESTAMP
);

-- Tour Images
CREATE TABLE tour_images (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  image_url VARCHAR(500),
  image_type ENUM('featured', 'banner'),
  display_order INT,
  alt_text VARCHAR(255),
  created_at TIMESTAMP
);

-- Tour Itineraries
CREATE TABLE tour_itineraries (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  day_number INT,
  header VARCHAR(255),
  text_detail TEXT,
  created_at TIMESTAMP,
  updated_at TIMESTAMP
);

-- Reviews
CREATE TABLE reviews (
  id UUID PRIMARY KEY,
  tour_id UUID REFERENCES tours(id),
  user_id UUID REFERENCES users(id),
  booking_id UUID REFERENCES bookings(id),
  rating INT CHECK (rating >= 1 AND rating <= 5),
  title VARCHAR(255),
  comment TEXT,
  verified_purchase BOOLEAN,
  status ENUM('pending', 'approved', 'rejected'),
  created_at TIMESTAMP,
  updated_at TIMESTAMP
);

-- Receipts
CREATE TABLE receipts (
  id UUID PRIMARY KEY,
  booking_id UUID REFERENCES bookings(id),
  receipt_number VARCHAR(50) UNIQUE,
  issue_date DATE,
  total_amount DECIMAL(10,2),
  payment_method VARCHAR(50),
  payment_status VARCHAR(50),
  pdf_url VARCHAR(500),
  created_at TIMESTAMP
);

-- Updated Bookings Table
ALTER TABLE bookings ADD COLUMN points_eligible BOOLEAN DEFAULT true;
ALTER TABLE bookings ADD COLUMN processed_by_staff BOOLEAN DEFAULT false;
ALTER TABLE bookings ADD COLUMN staff_user_id UUID REFERENCES users(id);
```

---

## Phase 6: Implementation Priorities

### Sprint 1 (Weeks 1-2): Foundation
1. Tour Options and Pricing
2. Tour Images
3. Tour Types and Itinerary

### Sprint 2 (Weeks 3-4): Pricing Enhancements
1. Surcharge System
2. Promotion System
3. Cancellation Policy

### Sprint 3 (Weeks 5-6): User Engagement
1. Review System
2. Google reCAPTCHA Integration
3. Review Moderation

### Sprint 4 (Weeks 7-8): Admin Tools
1. Receipt Functionality
2. Staff Payment Processing
3. Reporting and Analytics

---

## Technical Considerations

### Frontend Stack
- React/Next.js for UI
- Tailwind CSS for styling
- React Hook Form for form management
- Zod for validation
- TanStack Query for data fetching

### Backend Stack
- Node.js/Express or Next.js API routes
- PostgreSQL or MySQL database
- Prisma ORM
- AWS S3 or Cloudinary for image storage
- Google reCAPTCHA v3 API
- PDF generation library (PDFKit/Puppeteer)

### Security
- Input validation and sanitization
- SQL injection prevention (use ORM)
- XSS protection
- CSRF tokens
- Rate limiting on API endpoints
- Google reCAPTCHA for spam prevention
- Secure file upload validation

### Performance
- Image optimization and lazy loading
- Database indexing on frequently queried fields
- Caching strategy (Redis for session/pricing data)
- API response pagination
- CDN for static assets

---

## Testing Strategy

### Unit Tests
- Pricing calculation logic
- Promotion and surcharge application
- Points calculation
- Receipt generation

### Integration Tests
- API endpoint testing
- Database operations
- File upload flow
- Payment processing

### E2E Tests
- Complete booking flow
- Review submission
- Admin tour management
- Staff payment processing

---

## Deployment Plan

1. **Development Environment Setup**
   - Local database setup
   - Environment variables configuration
   - Third-party API keys (reCAPTCHA, storage)

2. **Staging Deployment**
   - Deploy to staging server
   - Run migration scripts
   - Seed test data
   - QA testing

3. **Production Deployment**
   - Database backup
   - Run migrations
   - Deploy application
   - Monitor logs and performance
   - Gradual rollout if needed

---

## Success Metrics

- Time to create/edit tours reduced by 50%
- Review submission rate > 20% of bookings
- Average review rating visibility on tour pages
- Receipt generation success rate 100%
- Staff payment processing time < 2 minutes
- Image upload success rate > 95%
- Zero spam reviews (with reCAPTCHA)

---

## Future Enhancements

- Multi-language support for tours and reviews
- Advanced analytics dashboard
- Customer loyalty points dashboard
- Automated email notifications for promotions
- Social media sharing for reviews
- Tour comparison feature
- Wishlist/favorites functionality
- Mobile app for tour management

---

---

## Phase 7: Current Implementation Status

### Completed Components

#### 1. Tour Options System ✅
- **Database Models**: Tour model updated with tour options support
  - Location: [apps/admin/src/models/Tour.js](apps/admin/src/models/Tour.js)
  - Location: [apps/api/src/models/Tour.js](apps/api/src/models/Tour.js)
- **API Endpoints**: Tour options CRUD endpoints implemented
  - Admin API: [apps/api/src/app/api/tours/[id]/options/](apps/api/src/app/api/tours/[id]/options/)
  - Public API: [apps/api/src/app/api/tours/[id]/options/](apps/api/src/app/api/tours/[id]/options/)
- **Pricing Calculator**: Dynamic pricing calculation utility
  - Location: [apps/api/src/utils/pricingCalculator.js](apps/api/src/utils/pricingCalculator.js)

#### 2. Admin UI Forms ✅
- **Tour Creation Form**: Enhanced with tour options
  - Location: [apps/admin/src/app/(DashboardLayout)/admin/tours/new/page.jsx](apps/admin/src/app/(DashboardLayout)/admin/tours/new/page.jsx)
- **Tour Edit Form**: Support for editing tour options
  - Location: [apps/admin/src/app/(DashboardLayout)/admin/tours/[id]/edit/page.jsx](apps/admin/src/app/(DashboardLayout)/admin/tours/[id]/edit/page.jsx)
- **Form Components**: Reusable form components
  - Location: [apps/admin/src/components/forms/](apps/admin/src/components/forms/)

#### 3. Frontend Application ✅
- **Separate Frontend App**: Public-facing tour booking application
  - Location: [apps/frontend/](apps/frontend/)
  - Package configuration: [apps/frontend/package-lock.json](apps/frontend/package-lock.json)
  - Documentation: [apps/frontend/README.md](apps/frontend/README.md)
  - Public assets: [apps/frontend/public/](apps/frontend/public/)

#### 4. Static Assets ✅
- **Admin Public Folder**: Static assets for admin panel
  - Location: [apps/admin/public/](apps/admin/public/)

#### 5. Authentication System ✅
- **Updated Authentication**: Enhanced auth system
  - Mentioned in recent commits: "Update authen" (commit 14ab1ad)

#### 6. Documentation ✅
- **Implementation Guide**: Detailed tour options implementation
  - Location: [TOUR_OPTIONS_IMPLEMENTATION.md](TOUR_OPTIONS_IMPLEMENTATION.md)

---

## Phase 8: Immediate Next Steps

### Priority 1: Complete Core Features

#### A. Image Management System
**Status**: Not Started
**Estimated Time**: 1-2 weeks

1. **Database Setup**
   ```sql
   -- Add to migration file
   CREATE TABLE tour_images (
     id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
     tour_id UUID REFERENCES tours(id) ON DELETE CASCADE,
     image_url VARCHAR(500) NOT NULL,
     image_type VARCHAR(20) CHECK (image_type IN ('featured', 'banner')),
     display_order INT DEFAULT 0,
     alt_text VARCHAR(255),
     created_at TIMESTAMP DEFAULT NOW(),
     updated_at TIMESTAMP DEFAULT NOW()
   );

   CREATE INDEX idx_tour_images_tour_id ON tour_images(tour_id);
   CREATE INDEX idx_tour_images_type ON tour_images(image_type);
   ```

2. **Implementation Tasks**
   - [ ] Set up image storage (Cloudinary/AWS S3)
   - [ ] Create image upload API endpoint
   - [ ] Build image uploader component in admin
   - [ ] Implement image optimization
   - [ ] Add drag-and-drop reordering
   - [ ] Display images on frontend tour pages

3. **Files to Create/Modify**
   - `apps/api/src/app/api/tours/[id]/images/route.js` (new)
   - `apps/api/src/utils/imageUpload.js` (new)
   - `apps/admin/src/components/forms/TourImageUploader.jsx` (new)
   - `apps/frontend/src/components/TourImageGallery.jsx` (new)

#### B. Tour Itinerary System
**Status**: Partially Complete (model ready)
**Estimated Time**: 1 week

1. **Database Setup**
   ```sql
   CREATE TABLE tour_itineraries (
     id UUID PRIMARY KEY DEFAULT uuid_generate_v4(),
     tour_id UUID REFERENCES tours(id) ON DELETE CASCADE,
     day_number INT NOT NULL,
     header VARCHAR(255) NOT NULL,
     text_detail TEXT,
     created_at TIMESTAMP DEFAULT NOW(),
     updated_at TIMESTAMP DEFAULT NOW(),
     CONSTRAINT unique_tour_day UNIQUE(tour_id, day_number)
   );

   CREATE INDEX idx_tour_itineraries_tour_id ON tour_itineraries(tour_id);
   ```

2. **Implementation Tasks**
   - [ ] Create itinerary CRUD API endpoints
   - [ ] Build itinerary editor in admin (day-by-day)
   - [ ] Add rich text editor for itinerary details
   - [ ] Display itinerary on frontend (accordion/tabs)
   - [ ] Add reordering capability

3. **Files to Create/Modify**
   - `apps/api/src/app/api/tours/[id]/itineraries/route.js` (new)
   - `apps/admin/src/components/forms/ItineraryEditor.jsx` (new)
   - `apps/frontend/src/components/TourItinerary.jsx` (new)

#### C. Surcharge System
**Status**: Not Started
**Estimated Time**: 1-2 weeks

1. **Implementation Tasks**
   - [ ] Create surcharge database table
   - [ ] Build surcharge CRUD API
   - [ ] Create admin UI for surcharge management
   - [ ] Integrate surcharges into pricing calculator
   - [ ] Display surcharge info in booking flow
   - [ ] Add date range validation

2. **Files to Create/Modify**
   - `apps/api/src/models/Surcharge.js` (new)
   - `apps/api/src/app/api/tours/[id]/surcharges/route.js` (new)
   - `apps/admin/src/components/forms/SurchargeManager.jsx` (new)
   - Update: `apps/api/src/utils/pricingCalculator.js`

#### D. Promotion System
**Status**: Not Started
**Estimated Time**: 1-2 weeks

1. **Implementation Tasks**
   - [ ] Create promotion database table
   - [ ] Build promotion CRUD API
   - [ ] Create admin UI with promotion types
   - [ ] Implement Early Bird logic (booking X days in advance)
   - [ ] Implement Last Minute logic (booking within X days)
   - [ ] Integrate promotions into pricing calculator
   - [ ] Add promotion validation and priority rules

2. **Files to Create/Modify**
   - `apps/api/src/models/Promotion.js` (new)
   - `apps/api/src/app/api/tours/[id]/promotions/route.js` (new)
   - `apps/admin/src/components/forms/PromotionManager.jsx` (new)
   - Update: `apps/api/src/utils/pricingCalculator.js`

---

### Priority 2: Review System

#### Status: Not Started
**Estimated Time**: 2 weeks

1. **Google reCAPTCHA Setup**
   - [ ] Register for reCAPTCHA v3 API keys
   - [ ] Add keys to environment variables
   - [ ] Create reCAPTCHA validation utility
   - [ ] Implement server-side verification

2. **Implementation Tasks**
   - [ ] Create reviews database table
   - [ ] Build review submission API with reCAPTCHA
   - [ ] Create review moderation endpoints
   - [ ] Build frontend review form
   - [ ] Display reviews on tour detail page
   - [ ] Build admin moderation panel
   - [ ] Add star rating component
   - [ ] Implement review filtering/sorting

3. **Files to Create/Modify**
   - `apps/api/src/models/Review.js` (new)
   - `apps/api/src/utils/recaptcha.js` (new)
   - `apps/api/src/app/api/tours/[id]/reviews/route.js` (new)
   - `apps/api/src/app/api/admin/reviews/route.js` (new)
   - `apps/frontend/src/components/ReviewForm.jsx` (new)
   - `apps/frontend/src/components/ReviewList.jsx` (new)
   - `apps/admin/src/app/(DashboardLayout)/admin/reviews/page.jsx` (new)

---

### Priority 3: Receipt System

#### Status: Not Started
**Estimated Time**: 1-2 weeks

1. **Implementation Tasks**
   - [ ] Create receipts database table
   - [ ] Implement receipt number generation
   - [ ] Build receipt PDF generator
   - [ ] Create receipt API endpoints
   - [ ] Build admin receipt management UI
   - [ ] Add receipt download/email functionality
   - [ ] Create receipt template with branding

2. **Dependencies**
   - Install PDF generation library (recommend: `@react-pdf/renderer` or `puppeteer`)
   - Set up email service integration

3. **Files to Create/Modify**
   - `apps/api/src/models/Receipt.js` (new)
   - `apps/api/src/utils/pdfGenerator.js` (new)
   - `apps/api/src/app/api/bookings/[id]/receipt/route.js` (new)
   - `apps/admin/src/app/(DashboardLayout)/admin/receipts/page.jsx` (new)
   - `apps/api/src/templates/receiptTemplate.jsx` (new)

---

### Priority 4: Staff Payment System

#### Status: Not Started
**Estimated Time**: 1 week

1. **Database Changes**
   ```sql
   ALTER TABLE bookings
     ADD COLUMN points_eligible BOOLEAN DEFAULT true,
     ADD COLUMN processed_by_staff BOOLEAN DEFAULT false,
     ADD COLUMN staff_user_id UUID REFERENCES users(id),
     ADD COLUMN staff_notes TEXT;
   ```

2. **Implementation Tasks**
   - [ ] Update booking model
   - [ ] Create staff payment processing endpoint
   - [ ] Build admin UI with "no points" option
   - [ ] Update points calculation logic
   - [ ] Add audit logging
   - [ ] Create staff activity report

3. **Files to Create/Modify**
   - Update: `apps/api/src/models/Booking.js`
   - `apps/api/src/app/api/admin/bookings/[id]/process-payment/route.js` (new)
   - Update: `apps/admin/src/app/(DashboardLayout)/admin/bookings/[id]/page.jsx`
   - `apps/api/src/utils/pointsCalculator.js` (modify)

---

## Phase 9: Testing & Quality Assurance

### Testing Setup
**Status**: Not Started

#### Unit Testing
```bash
# Install testing dependencies
npm install --save-dev jest @testing-library/react @testing-library/jest-dom
npm install --save-dev @testing-library/user-event
```

**Test Files to Create**:
- `apps/api/src/utils/__tests__/pricingCalculator.test.js`
- `apps/api/src/utils/__tests__/recaptcha.test.js`
- `apps/admin/src/components/forms/__tests__/TourForm.test.jsx`
- `apps/frontend/src/components/__tests__/ReviewForm.test.jsx`

#### Integration Testing
- API endpoint tests for all CRUD operations
- Database transaction tests
- File upload flow tests

#### E2E Testing
```bash
# Install Playwright or Cypress
npm install --save-dev @playwright/test
```

**E2E Test Scenarios**:
- Complete tour booking flow
- Tour creation by admin
- Review submission and moderation
- Receipt generation

---

## Phase 10: Configuration & Environment Setup

### Environment Variables Checklist

#### Admin App (.env.local)
```env
# API
NEXT_PUBLIC_API_URL=http://localhost:3001
NEXT_PUBLIC_ADMIN_API_URL=http://localhost:3001/api/admin

# Authentication
NEXTAUTH_URL=http://localhost:3000
NEXTAUTH_SECRET=your-secret-key

# Image Upload
NEXT_PUBLIC_CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret
```

#### API App (.env)
```env
# Database
DATABASE_URL=postgresql://user:password@localhost:5432/leetour_db

# JWT
JWT_SECRET=your-jwt-secret

# Google reCAPTCHA
RECAPTCHA_SECRET_KEY=your-recaptcha-secret

# Email Service (SendGrid/Mailgun/SES)
EMAIL_SERVICE_API_KEY=your-email-api-key
EMAIL_FROM=noreply@leetour.com

# Storage
AWS_S3_BUCKET=your-bucket-name
AWS_ACCESS_KEY_ID=your-access-key
AWS_SECRET_ACCESS_KEY=your-secret-key
```

#### Frontend App (.env.local)
```env
# API
NEXT_PUBLIC_API_URL=http://localhost:3001

# Google reCAPTCHA (Public Key)
NEXT_PUBLIC_RECAPTCHA_SITE_KEY=your-recaptcha-site-key

# Payment Gateway
NEXT_PUBLIC_STRIPE_KEY=your-stripe-public-key
```

---

## Phase 11: Deployment Checklist

### Pre-Deployment

- [ ] All environment variables configured
- [ ] Database migrations tested
- [ ] Test data seeded
- [ ] All tests passing
- [ ] Security audit completed
- [ ] Performance testing done
- [ ] Backup strategy in place

### Staging Deployment

- [ ] Deploy to staging environment
- [ ] Run database migrations
- [ ] Configure CDN for static assets
- [ ] Test all critical user flows
- [ ] Load testing
- [ ] Security penetration testing

### Production Deployment

- [ ] Database backup created
- [ ] Run production migrations
- [ ] Deploy application
- [ ] Configure monitoring (New Relic, Datadog, etc.)
- [ ] Set up error tracking (Sentry)
- [ ] Configure SSL/TLS certificates
- [ ] Set up CI/CD pipeline
- [ ] Monitor application logs
- [ ] Performance monitoring

---

## Phase 12: Monitoring & Maintenance

### Monitoring Setup

#### Application Monitoring
```bash
# Install monitoring tools
npm install @sentry/nextjs
npm install @vercel/analytics
```

#### Key Metrics to Track
- API response times
- Database query performance
- Image upload success rate
- Booking conversion rate
- Review submission rate
- Payment success rate
- Error rates by endpoint
- User session duration

#### Alerts to Configure
- API downtime
- Database connection failures
- Failed payment transactions
- Image upload failures
- High error rates (> 5%)
- Slow API responses (> 3s)

### Regular Maintenance Tasks

**Daily**:
- Monitor error logs
- Check payment transactions
- Review spam/suspicious reviews

**Weekly**:
- Database performance review
- Backup verification
- Security updates

**Monthly**:
- Dependency updates
- Performance optimization
- User feedback review
- Analytics review

---

## Phase 13: Additional Features & Enhancements

### Cancellation Policy
**Priority**: Medium
**Estimated Time**: 1 week

1. **Implementation**
   - [ ] Create cancellation_policies table
   - [ ] Build policy management UI
   - [ ] Display policies on tour pages
   - [ ] Implement cancellation request handling
   - [ ] Calculate refund amounts
   - [ ] Send cancellation confirmations

### Booking Management Enhancements
**Priority**: Medium

- [ ] Booking calendar view
- [ ] Availability management
- [ ] Waitlist functionality
- [ ] Group booking discounts
- [ ] Booking reminders (email/SMS)

### Customer Portal
**Priority**: Low

- [ ] User dashboard
- [ ] Booking history
- [ ] Loyalty points tracker
- [ ] Saved payment methods
- [ ] Wishlist/favorites

### Analytics Dashboard
**Priority**: Medium

- [ ] Revenue analytics
- [ ] Booking trends
- [ ] Popular tours
- [ ] Customer demographics
- [ ] Review analytics
- [ ] Promotion effectiveness

---

## Phase 14: Optimization Strategies

### Performance Optimization

#### Database Optimization
```sql
-- Add indexes for frequently queried fields
CREATE INDEX idx_tours_tour_type ON tours(tour_type);
CREATE INDEX idx_bookings_user_id ON bookings(user_id);
CREATE INDEX idx_bookings_tour_id ON bookings(tour_id);
CREATE INDEX idx_bookings_booking_date ON bookings(booking_date);
CREATE INDEX idx_reviews_tour_id_status ON reviews(tour_id, status);
```

#### Caching Strategy
```javascript
// Implement Redis caching for:
// - Tour listings
// - Pricing calculations
// - Available promotions
// - User sessions

// Example caching implementation
import { Redis } from 'ioredis';
const redis = new Redis(process.env.REDIS_URL);

// Cache tour data for 5 minutes
const cacheKey = `tour:${tourId}`;
const cached = await redis.get(cacheKey);
if (cached) return JSON.parse(cached);

const tour = await fetchTour(tourId);
await redis.setex(cacheKey, 300, JSON.stringify(tour));
```

#### Frontend Optimization
- [ ] Implement lazy loading for images
- [ ] Code splitting for route-based chunks
- [ ] Optimize bundle size
- [ ] Use CDN for static assets
- [ ] Implement service worker for offline support
- [ ] Optimize font loading

### Security Hardening

- [ ] Implement rate limiting on all API endpoints
- [ ] Add input sanitization for all user inputs
- [ ] SQL injection prevention (use parameterized queries)
- [ ] XSS protection headers
- [ ] CSRF token validation
- [ ] Secure file upload validation (type, size, content)
- [ ] Implement API key rotation
- [ ] Add audit logging for sensitive operations
- [ ] Regular security scans
- [ ] Dependency vulnerability scanning

---

## Success Criteria & KPIs

### Technical KPIs
- API response time < 500ms (95th percentile)
- Page load time < 2s
- Image upload success rate > 98%
- Database query time < 100ms
- Zero critical security vulnerabilities
- Test coverage > 80%

### Business KPIs
- Tour creation time < 5 minutes
- Booking conversion rate > 15%
- Review submission rate > 20%
- Customer satisfaction score > 4.5/5
- Repeat booking rate > 30%
- Staff payment processing time < 2 minutes

---

## Risk Management

### Technical Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Database performance degradation | High | Implement caching, optimize queries, add indexes |
| Image storage costs | Medium | Implement compression, CDN caching, cleanup old images |
| Third-party API failures (reCAPTCHA) | Medium | Implement fallback mechanisms, circuit breakers |
| Payment gateway downtime | High | Support multiple payment gateways, queue retries |

### Business Risks

| Risk | Impact | Mitigation |
|------|--------|------------|
| Spam reviews | Medium | reCAPTCHA, manual moderation, automated filters |
| Pricing calculation errors | High | Extensive testing, manual review option |
| Data loss | High | Regular backups, point-in-time recovery |
| Fraudulent bookings | High | Payment verification, fraud detection |

---

*Document Version: 2.0*
*Last Updated: 2025-10-22*
*Status: In Progress - Phase 7 Completed, Moving to Phase 8*
