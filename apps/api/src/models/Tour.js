const mongoose = require('mongoose');

const reviewSchema = new mongoose.Schema({
  user: {
    name: { type: String, required: true },
    email: { type: String, required: true },
    avatar: { type: String, default: '/images/profile/user-1.jpg' }
  },
  rating: { type: Number, required: true, min: 1, max: 5 },
  comment: { type: String, required: true },
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
  textDetail: {
    type: String,
    trim: true,
    default: ''
  },
  activities: [{ type: String }],
  meals: {
    breakfast: { type: Boolean, default: false },
    lunch: { type: Boolean, default: false },
    dinner: { type: Boolean, default: false }
  },
  accommodation: { type: String, default: '' }
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
  isActive: {
    type: Boolean,
    default: true
  }
}, { _id: true });

const tourSchema = new mongoose.Schema({
  title: {
    type: String,
    required: [true, 'Tour title is required'],
    trim: true,
    maxlength: [200, 'Title cannot exceed 200 characters']
  },
  description: {
    type: String,
    required: [true, 'Tour description is required'],
    trim: true
  },
  shortDescription: {
    type: String,
    trim: true,
    maxlength: [300, 'Short description cannot exceed 300 characters']
  },
  duration: {
    type: String,
    default: '8 hours'
  },
  price: {
    type: Number,
    required: [true, 'Tour price is required'],
    min: [0, 'Price must be positive']
  },
  originalPrice: {
    type: Number,
    min: [0, 'Original price must be positive']
  },
  currency: {
    type: String,
    default: 'USD',
    enum: ['USD', 'EUR', 'GBP', 'JPY', 'AUD']
  },
  category: {
    type: String,
    required: [true, 'Category is required'],
    enum: ['Cultural', 'Adventure', 'Nature', 'Historical', 'Food & Drink', 'Entertainment', 'Sports']
  },
  subcategory: {
    type: String,
    default: 'General Tours'
  },
  location: {
    type: locationSchema,
    required: [true, 'Location is required']
  },
  images: [imageSchema],
  schedule: scheduleSchema,
  capacity: capacitySchema,
  included: [{ type: String }],
  excluded: [{ type: String }],
  highlights: [{ type: String }],
  difficulty: {
    type: String,
    enum: ['Easy', 'Moderate', 'Hard', 'Expert'],
    default: 'Easy'
  },
  ageRestriction: ageRestrictionSchema,
  cancellation: cancellationSchema,
  rating: {
    average: { type: Number, default: 0, min: 0, max: 5 },
    count: { type: Number, default: 0 }
  },
  reviews: [reviewSchema],
  guide: guideSchema,
  tags: [{ type: String }],
  isActive: {
    type: Boolean,
    default: true
  },
  isFeatured: {
    type: Boolean,
    default: false
  },
  booking: bookingSchema,
  tourOptions: [tourOptionSchema],
  tourType: {
    type: String,
    enum: ['daytrip', 'tour'],
    default: 'daytrip'
  },
  itinerary: [itinerarySchema],
  surcharges: [surchargeSchema],
  promotions: [promotionSchema],
  cancellationPolicies: [cancellationPolicySchema],
  seo: seoSchema,
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
tourSchema.index({ title: 'text', description: 'text' });
tourSchema.index({ category: 1, isActive: 1 });
tourSchema.index({ 'location.country': 1, 'location.city': 1 });
tourSchema.index({ price: 1 });
tourSchema.index({ createdBy: 1 });
tourSchema.index({ 'rating.average': -1 });
tourSchema.index({ createdAt: -1 });

// Pre-save middleware to generate slug
tourSchema.pre('save', function(next) {
  if (this.isModified('title') && !this.seo.slug) {
    this.seo.slug = this.title.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
  }
  
  if (this.isModified('title') && !this.seo.metaTitle) {
    this.seo.metaTitle = this.title;
  }
  
  if (this.isModified('description') && !this.seo.metaDescription) {
    this.seo.metaDescription = this.description.substring(0, 160);
  }
  
  if (this.isModified('description') && !this.shortDescription) {
    this.shortDescription = this.description.substring(0, 100) + '...';
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