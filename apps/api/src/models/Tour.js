const mongoose = require('mongoose');

// DayTripRate Schema - pricing tiers based on group size and age
const dayTripRateSchema = new mongoose.Schema({
  persons: {
    type: Number,
    required: true,
    min: 1,
    description: 'Number of persons in group (e.g., 1, 2-5, 6+)'
  },
  netRate: {
    type: mongoose.Schema.Types.Decimal128,
    required: true,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Net cost/wholesale price'
  },
  retailRate: {
    type: mongoose.Schema.Types.Decimal128,
    required: true,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Retail selling price'
  },
  ageFrom: {
    type: Number,
    min: 0,
    default: null,
    description: 'Minimum age for this rate (null for all ages)'
  },
  ageTo: {
    type: Number,
    min: 0,
    default: null,
    description: 'Maximum age for this rate (null for all ages)'
  },
  description: {
    type: String,
    trim: true,
    default: '',
    description: 'Rate description (e.g., "Adult", "Child 6-12", "Group 1-2 pax")'
  }
}, {
  timestamps: true,
  toJSON: { getters: true },
  toObject: { getters: true }
});

const reviewSchema = new mongoose.Schema({
  user: {
    name: { type: String, required: true },
    email: { type: String, required: true },
    avatar: { type: String, default: '/images/profile/user-1.jpg' }
  },
  guestName: { type: String },
  title: { type: String, default: '' },
  rating: { type: Number, required: true, min: 1, max: 5 },
  comment: { type: String, required: true },
  reviewContent: { type: String },
  date: { type: Date, default: Date.now },
  verified: { type: Boolean, default: false }
});

const locationSchema = new mongoose.Schema({
  country: { type: String, required: true },
  state: { type: String, default: '' },
  city: { type: String, required: true },
  address: { type: String, default: '' },
  coordinates: {
    lat: { type: Number, default: 0 },
    lng: { type: Number, default: 0 }
  }
});

const scheduleSchema = new mongoose.Schema({
  startTime: { type: String, default: '09:00 AM' },
  endTime: { type: String, default: '05:00 PM' },
  meetingPoint: { type: String, default: 'City Center' },
  availableDays: [{
    type: String,
    enum: ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
  }],
  timeSlots: [{ type: String }]
});

const capacitySchema = new mongoose.Schema({
  minimum: { type: Number, default: 1 },
  maximum: { type: Number, default: 20 }
});

const ageRestrictionSchema = new mongoose.Schema({
  minimum: { type: Number, default: 0 },
  maximum: { type: Number, default: 99 }
});

const cancellationSchema = new mongoose.Schema({
  policy: { type: String, default: 'Free cancellation up to 24 hours before' },
  refundable: { type: Boolean, default: true },
  cutoffHours: { type: Number, default: 24 }
});

const guideSchema = new mongoose.Schema({
  name: { type: String, default: 'Professional Guide' },
  bio: { type: String, default: 'Experienced local guide' },
  image: { type: String, default: '/images/profile/user-1.jpg' },
  languages: [{ type: String }],
  rating: { type: Number, default: 4.8 },
  experience: { type: String, default: '5+ years' }
});

const bookingSchema = new mongoose.Schema({
  instantBooking: { type: Boolean, default: true },
  requiresApproval: { type: Boolean, default: false },
  advanceBooking: { type: Number, default: 1 }
});

const seoSchema = new mongoose.Schema({
  slug: { type: String, unique: true },
  metaTitle: { type: String },
  metaDescription: { type: String },
  keywords: [{ type: String }]
});

const imageSchema = new mongoose.Schema({
  url: { type: String, required: true },
  alt: { type: String, required: true },
  isPrimary: { type: Boolean, default: false },
  imageType: {
    type: String,
    enum: ['featured', 'banner', 'gallery'],
    default: 'gallery'
  },
  displayOrder: { type: Number, default: 0 }
}, { timestamps: true });

const featuredImageSchema = new mongoose.Schema({
  url: { type: String, default: '' },
  alt: { type: String, default: '' }
});

const galleryImageSchema = new mongoose.Schema({
  url: { type: String, default: '' },
  alt: { type: String, default: '' },
  name: { type: String, default: '' },
  type: {
    type: String,
    enum: ['Banner', 'Gallery', 'Logo', 'Map'],
    default: 'Gallery'
  },
  isPrimary: { type: Boolean, default: false }
});

// Tour Itinerary Schema (for multi-day tours)
const itinerarySchema = new mongoose.Schema({
  dayNumber: {
    type: Number,
    required: [true, 'Day number is required'],
    min: 1
  },
  header: {
    type: String,
    required: [true, 'Day header is required'],
    trim: true,
    maxlength: [255, 'Header cannot exceed 255 characters']
  },
  activity: { type: String, trim: true }, // Alias for header/title
  title: { type: String, trim: true }, // Another alias for header
  textDetail: {
    type: String,
    trim: true,
    default: ''
  },
  description: { type: String, trim: true, default: '' }, // Alias for textDetail
  activities: [{ type: String }],
  meals: {
    breakfast: { type: Boolean, default: false },
    lunch: { type: Boolean, default: false },
    dinner: { type: Boolean, default: false }
  },
  meal: { type: String, default: '' }, // String format: "Breakfast, Lunch, Dinner"
  accommodation: { type: String, default: '' },
  overnight: { type: String, default: '' }, // Alias for accommodation
  transport: { type: String, default: '' }, // Transportation details
  image: { type: String, default: '' } // Day-specific image URL
}, { timestamps: true });

// Surcharge Schema
const surchargeSchema = new mongoose.Schema({
  surchargeName: {
    type: String,
    required: [true, 'Surcharge name is required'],
    trim: true
  },
  surchargeType: {
    type: String,
    enum: ['holiday', 'weekend', 'peak_season', 'special_event', 'custom'],
    default: 'custom'
  },
  startDate: {
    type: Date,
    required: [true, 'Start date is required']
  },
  endDate: {
    type: Date,
    required: [true, 'End date is required']
  },
  amountType: {
    type: String,
    enum: ['percentage', 'fixed'],
    default: 'percentage'
  },
  amount: {
    type: Number,
    required: [true, 'Surcharge amount is required'],
    min: [0, 'Amount must be positive']
  },
  description: {
    type: String,
    trim: true,
    default: ''
  },
  isActive: {
    type: Boolean,
    default: true
  }
}, { timestamps: true });

// Promotion Schema
const promotionSchema = new mongoose.Schema({
  promotionName: {
    type: String,
    required: [true, 'Promotion name is required'],
    trim: true
  },
  promotionType: {
    type: String,
    enum: ['early_bird', 'last_minute', 'seasonal', 'group_discount', 'custom'],
    default: 'custom'
  },
  discountType: {
    type: String,
    enum: ['percentage', 'fixed'],
    default: 'percentage'
  },
  discountAmount: {
    type: Number,
    required: [true, 'Discount amount is required'],
    min: [0, 'Discount must be positive']
  },
  validFrom: {
    type: Date,
    required: [true, 'Valid from date is required']
  },
  validTo: {
    type: Date,
    required: [true, 'Valid to date is required']
  },
  bookingWindowStart: {
    type: Date
  },
  bookingWindowEnd: {
    type: Date
  },
  daysBeforeDeparture: {
    type: Number,
    min: 0,
    default: 0
  },
  minPassengers: {
    type: Number,
    min: 1
  },
  conditions: {
    type: String,
    trim: true,
    default: ''
  },
  isActive: {
    type: Boolean,
    default: true
  }
}, { timestamps: true });

// Cancellation Policy Schema
const cancellationPolicySchema = new mongoose.Schema({
  daysBeforeDeparture: {
    type: Number,
    required: [true, 'Days before departure is required'],
    min: [0, 'Days must be non-negative']
  },
  refundPercentage: {
    type: Number,
    required: [true, 'Refund percentage is required'],
    min: [0, 'Refund percentage must be between 0 and 100'],
    max: [100, 'Refund percentage must be between 0 and 100']
  },
  description: {
    type: String,
    trim: true,
    default: ''
  },
  displayOrder: {
    type: Number,
    default: 0
  }
}, { timestamps: true });

// Tour Pricing Options Schema
const tourOptionSchema = new mongoose.Schema({
  optionName: {
    type: String,
    required: [true, 'Option name is required'],
    trim: true
  },
  description: {
    type: String,
    trim: true,
    default: ''
  },
  basePrice: {
    type: Number,
    required: [true, 'Base price is required'],
    min: [0, 'Base price must be positive']
  },
  pricingTiers: [{
    minPassengers: {
      type: Number,
      required: true,
      min: 1
    },
    maxPassengers: {
      type: Number,
      required: true,
      min: 1
    },
    pricePerPerson: {
      type: Number,
      required: true,
      min: 0
    }
  }],
  departureTimes: {
    type: String,
    default: '08:00 AM',
    trim: true
  }, // Semicolon-separated times: "08:00 AM;10:30 AM;02:00 PM"
  includeItems: { type: String, default: '' }, // HTML content for what's included in this option
  isActive: {
    type: Boolean,
    default: true
  }
}, { _id: true });

const tourSchema = new mongoose.Schema({
  // Core Tour Information (aligned with DAYTRIP.cs)
  daytripId: {
    type: Number,
    unique: true,
    sparse: true,
    description: 'Legacy C# DAYTRIPID for migration compatibility'
  },
  name: {
    type: String,
    required: [true, 'Tour name is required'],
    trim: true,
    maxlength: [500, 'Name cannot exceed 500 characters'],
    description: 'Tour name (DAYTRIP.NAME)'
  },
  title: {
    type: String,
    trim: true,
    maxlength: [500, 'Title cannot exceed 500 characters'],
    description: 'Tour title (DAYTRIP.Title)'
  },
  description: {
    type: String,
    required: [true, 'Tour description is required'],
    trim: true,
    description: 'Full tour description (DAYTRIP.DESCRIPTION)'
  },
  overView: {
    type: String,
    trim: true,
    default: '',
    description: 'Tour overview summary (DAYTRIP.OverView)'
  },
  shortDescription: {
    type: String,
    trim: true,
    maxlength: [300, 'Short description cannot exceed 300 characters']
  },

  // Pricing
  priceFrom: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Starting price (DAYTRIP.PRICE_FROM)'
  },
  price: {
    type: Number,
    min: [0, 'Price must be positive'],
    description: 'Current/default price'
  },
  originalPrice: {
    type: Number,
    min: [0, 'Original price must be positive']
  },
  currency: {
    type: String,
    default: 'USD',
    enum: ['USD', 'EUR', 'GBP', 'JPY', 'AUD', 'VND']
  },
  commissionRate: {
    type: Number,
    min: 0,
    max: 100,
    description: 'Commission percentage (DAYTRIP.CommissionRate)'
  },

  // Duration and Timing
  duration: {
    type: String,
    default: '8 hours',
    description: 'Tour duration (DAYTRIP.Duration)'
  },
  startingTime: {
    type: String,
    description: 'Default start time (DAYTRIP.StartingTime)'
  },

  // Location (aligned with DAYTRIP.cs)
  location: {
    type: String,
    trim: true,
    description: 'Location name/description (DAYTRIP.Location)'
  },
  locationId: {
    type: Number,
    description: 'Location ID (DAYTRIP.LocationId)'
  },
  city: {
    type: String,
    trim: true,
    description: 'City name (DAYTRIP.City)'
  },
  country: {
    type: String,
    trim: true,
    description: 'Country name (DAYTRIP.Country)'
  },
  countryId: {
    type: Number,
    description: 'Country ID (DAYTRIP.CountryId)'
  },
  locationDetails: {
    type: locationSchema,
    description: 'Extended location information for map/coordinates'
  },

  category: {
    type: String,
    required: [true, 'Category is required'],
    enum: ['Cultural', 'Adventure', 'Nature', 'Historical', 'Food & Drink', 'Entertainment', 'Sports', 'Other']
  },
  subcategory: {
    type: String,
    default: 'General Tours'
  },
  travelStyle: {
    type: String,
    trim: true,
    description: 'Travel style (DAYTRIP.TravelStyle)'
  },

  // Tour Details (aligned with DAYTRIP.cs)
  pickupPoint: {
    type: String,
    trim: true,
    description: 'Pickup location (DAYTRIP.PickupPoint)'
  },
  dropOffPoint: {
    type: String,
    trim: true,
    description: 'Drop-off location (DAYTRIP.DropOffPoint)'
  },
  groupSize: {
    type: String,
    trim: true,
    description: 'Group size information (DAYTRIP.GroupSize)'
  },
  transport: {
    type: String,
    trim: true,
    description: 'Transportation details (DAYTRIP.Transport)'
  },

  // Content Fields
  hightLight: {
    type: String,
    trim: true,
    description: 'Tour highlights HTML (DAYTRIP.HightLight)'
  },
  include: {
    type: String,
    trim: true,
    description: 'What\'s included HTML (DAYTRIP.Include)'
  },
  exclude: {
    type: String,
    trim: true,
    description: 'What\'s excluded HTML (DAYTRIP.Exclude)'
  },
  programeDetail: {
    type: String,
    trim: true,
    description: 'Detailed program/itinerary (DAYTRIP.ProgrameDetail)'
  },
  notes: {
    type: String,
    trim: true,
    default: '',
    description: 'Additional notes (DAYTRIP.Notes)'
  },

  // Images
  image: {
    type: String,
    trim: true,
    description: 'Primary image URL (DAYTRIP.IMAGE)'
  },
  images: [imageSchema],
  featuredImage: featuredImageSchema,
  galleryImages: [galleryImageSchema],

  // Schedule and Capacity
  schedule: scheduleSchema,
  capacity: capacitySchema,

  // Compatibility fields (array format)
  included: [{ type: String }],
  excluded: [{ type: String }],
  includeActivity: { type: String, default: '' },
  excludeActivity: { type: String, default: '' },
  highlights: [{ type: String }],

  difficulty: {
    type: String,
    enum: ['Easy', 'Moderate', 'Hard', 'Expert'],
    default: 'Easy'
  },

  // SEO Fields (aligned with DAYTRIP.cs)
  url: {
    type: String,
    trim: true,
    description: 'SEO-friendly URL slug (DAYTRIP.URL)'
  },
  seoKeyword: {
    type: String,
    trim: true,
    description: 'SEO keywords (DAYTRIP.SEO_Keyword)'
  },
  seoDescription: {
    type: String,
    trim: true,
    description: 'SEO meta description (DAYTRIP.SEO_DESCRIPTION)'
  },
  seo: seoSchema,

  // Rating and Reviews
  startRating: {
    type: Number,
    min: 0,
    max: 5,
    default: 0,
    description: 'Average rating (DAYTRIP.START_RATING)'
  },
  rating: {
    average: { type: Number, default: 0, min: 0, max: 5 },
    count: { type: Number, default: 0 }
  },
  reviews: [reviewSchema],

  // Booking Settings (aligned with DAYTRIP.cs)
  startBooking: {
    type: Number,
    description: 'Start booking days before departure (DAYTRIP.STARTBOOKING)'
  },
  endBooking: {
    type: Number,
    description: 'End booking days before departure (DAYTRIP.ENDBOOKING)'
  },

  // Cancellation Policy Fields (aligned with DAYTRIP.cs)
  cancelPolicyType: {
    type: Number,
    description: 'Cancellation policy type (DAYTRIP.CANCELPOLICYTYPE)'
  },
  cancelPolicyFromDay: {
    type: Number,
    description: 'Cancellation policy from days (DAYTRIP.CANCELPOLICY_FROMDAY)'
  },
  cancelPolicyToDay: {
    type: Number,
    description: 'Cancellation policy to days (DAYTRIP.CANCELPOLICY_TODAY)'
  },
  cancelPolicyValue1: {
    type: String,
    trim: true,
    description: 'Cancellation policy value 1 (DAYTRIP.CANCELPOLICYVALUE1)'
  },
  cancelPolicyValue1Vn: {
    type: String,
    trim: true,
    description: 'Cancellation policy value 1 Vietnamese (DAYTRIP.CANCELPOLICYVALUE1_VN)'
  },
  cancelPolicyValue2: {
    type: String,
    trim: true,
    description: 'Cancellation policy value 2 (DAYTRIP.CANCELPOLICYVALUE2)'
  },
  cancelPolicyValue2Vn: {
    type: String,
    trim: true,
    description: 'Cancellation policy value 2 Vietnamese (DAYTRIP.CANCELPOLICYVALUE2_VN)'
  },

  // Operator Information
  operatorId: {
    type: Number,
    description: 'Tour operator ID (DAYTRIP.OperatorId)'
  },

  // Status
  status: {
    type: Number,
    default: 1,
    description: 'Tour status: 0=inactive, 1=active (DAYTRIP.Status)'
  },
  isActive: {
    type: Boolean,
    default: true
  },
  isFeatured: {
    type: Boolean,
    default: false
  },

  ageRestriction: ageRestrictionSchema,
  cancellation: cancellationSchema,
  guide: guideSchema,
  booking: bookingSchema,

  // Pricing Tiers (aligned with DayTripRate.cs)
  dayTripRates: [dayTripRateSchema],

  // Tour Options and Itinerary
  tourOptions: [tourOptionSchema],
  itinerary: [itinerarySchema],

  // Promotions and Surcharges
  surcharges: [surchargeSchema],
  promotions: [promotionSchema],
  cancellationPolicies: [cancellationPolicySchema],

  tags: [{ type: String }],
  keywords: [{ type: String }],
  type: { type: String, enum: ['daytrip', 'tour'], default: 'daytrip' },
  tourType: {
    type: String,
    enum: ['daytrip', 'tour'],
    default: 'daytrip'
  },

  createdBy: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'User',
    required: true
  }
}, {
  timestamps: true,
  toJSON: { virtuals: true },
  toObject: { virtuals: true }
});

// Indexes for better performance
tourSchema.index({ name: 'text', title: 'text', description: 'text' });
tourSchema.index({ daytripId: 1 });
tourSchema.index({ category: 1, isActive: 1 });
tourSchema.index({ status: 1 });
tourSchema.index({ country: 1, city: 1 });
tourSchema.index({ 'locationDetails.country': 1, 'locationDetails.city': 1 });
tourSchema.index({ price: 1, priceFrom: 1 });
tourSchema.index({ createdBy: 1 });
tourSchema.index({ operatorId: 1 });
tourSchema.index({ startRating: -1, 'rating.average': -1 });
tourSchema.index({ createdAt: -1 });

// Pre-save middleware to generate slug and sync fields
tourSchema.pre('save', function(next) {
  // Sync name and title
  if (!this.name && this.title) {
    this.name = this.title;
  } else if (!this.title && this.name) {
    this.title = this.name;
  }

  // Generate URL slug from name or title
  if (this.isModified('name') || this.isModified('title')) {
    const baseText = this.name || this.title;
    if (baseText && !this.url) {
      this.url = baseText.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
    }
    if (baseText && !this.seo.slug) {
      this.seo.slug = baseText.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
    }
  }

  // Sync SEO fields
  if (this.isModified('name') || this.isModified('title')) {
    const titleText = this.name || this.title;
    if (titleText && !this.seo.metaTitle) {
      this.seo.metaTitle = titleText;
    }
  }

  if (this.isModified('description') && !this.seo.metaDescription) {
    this.seo.metaDescription = this.description.substring(0, 160);
  }

  if (this.isModified('overView') && !this.seoDescription) {
    this.seoDescription = this.overView.substring(0, 160);
  }

  if (this.isModified('description') && !this.shortDescription) {
    this.shortDescription = this.description.substring(0, 100) + '...';
  }

  // Sync location fields with locationDetails
  if (this.locationDetails) {
    if (!this.country && this.locationDetails.country) {
      this.country = this.locationDetails.country;
    }
    if (!this.city && this.locationDetails.city) {
      this.city = this.locationDetails.city;
    }
  }

  // Sync status and isActive
  if (this.isModified('status')) {
    this.isActive = this.status === 1;
  } else if (this.isModified('isActive')) {
    this.status = this.isActive ? 1 : 0;
  }

  // Sync rating fields
  if (this.isModified('rating.average') && !this.startRating) {
    this.startRating = this.rating.average;
  } else if (this.isModified('startRating') && !this.rating.average) {
    this.rating.average = this.startRating;
  }

  // Sync type with tourType
  if (this.isModified('tourType') && !this.type) {
    this.type = this.tourType;
  } else if (this.isModified('type') && !this.tourType) {
    this.tourType = this.type;
  }

  // Sync keywords with seo.keywords and seoKeyword
  if (this.isModified('seo.keywords') && (!this.keywords || this.keywords.length === 0)) {
    this.keywords = this.seo.keywords;
  } else if (this.isModified('keywords') && (!this.seo.keywords || this.seo.keywords.length === 0)) {
    this.seo.keywords = this.keywords;
  }
  if (this.isModified('seoKeyword') && (!this.keywords || this.keywords.length === 0)) {
    this.keywords = this.seoKeyword.split(',').map(k => k.trim());
  }

  // Sync review fields
  if (this.reviews && this.reviews.length > 0) {
    this.reviews.forEach(review => {
      // Sync guestName with user.name if not set
      if (!review.guestName && review.user && review.user.name) {
        review.guestName = review.user.name;
      }
      // Sync reviewContent with comment if not set
      if (!review.reviewContent && review.comment) {
        review.reviewContent = review.comment;
      }
    });
  }

  // Sync itinerary fields
  if (this.itinerary && this.itinerary.length > 0) {
    this.itinerary.forEach(day => {
      // Sync activity/title with header
      if (!day.activity && day.header) {
        day.activity = day.header;
      }
      if (!day.title && day.header) {
        day.title = day.header;
      }
      // Sync description with textDetail
      if (!day.description && day.textDetail) {
        day.description = day.textDetail;
      }
      // Sync overnight with accommodation
      if (!day.overnight && day.accommodation) {
        day.overnight = day.accommodation;
      }
      // Generate meal string from meals object if not set
      if (!day.meal && day.meals) {
        const mealTypes = [];
        if (day.meals.breakfast) mealTypes.push('Breakfast');
        if (day.meals.lunch) mealTypes.push('Lunch');
        if (day.meals.dinner) mealTypes.push('Dinner');
        if (mealTypes.length > 0) {
          day.meal = mealTypes.join(', ');
        }
      }
    });
  }

  next();
});

// Virtual for discount percentage
tourSchema.virtual('discountPercentage').get(function() {
  if (this.originalPrice && this.originalPrice > this.price) {
    return Math.round(((this.originalPrice - this.price) / this.originalPrice) * 100);
  }
  return 0;
});

// Virtual for formatted price
tourSchema.virtual('formattedPrice').get(function() {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: this.currency || 'USD'
  }).format(this.price);
});

module.exports = mongoose.models.Tour || mongoose.model('Tour', tourSchema);