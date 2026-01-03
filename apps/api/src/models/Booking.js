import mongoose from 'mongoose';
import { generateBookingId, generateBookingReference } from '../utils/bookingId.js';

const BookingSchema = new mongoose.Schema({
  // Primary Keys (aligned with DAYTRIPBOOKING.cs)
  id: {
    type: Number,
    unique: true,
    sparse: true,
    description: 'Legacy C# ID for migration (DAYTRIPBOOKING.ID)'
  },
  bookingId: {
    type: String,
    required: true,
    unique: true,
    default: () => generateBookingId()
  },
  bookingReference: {
    type: String,
    required: false,
    unique: true,
    sparse: true
  },
  receiptId: {
    type: String,
    trim: true,
    description: 'Receipt ID (DAYTRIPBOOKING.RECEIPTID)'
  },

  // Tour/Daytrip Reference (aligned with DAYTRIPBOOKING.cs)
  daytripId: {
    type: Number,
    description: 'C# daytrip ID (DAYTRIPBOOKING.DaytripID)'
  },
  tour: {
    tourId: { type: mongoose.Schema.Types.ObjectId, ref: 'Tour', required: true },
    name: { type: String, description: 'Tour name (DAYTRIPBOOKING.NAME)' },
    title: { type: String, required: true },
    shortDescription: { type: String },
    description: { type: String },
    duration: { type: String },
    price: { type: Number, required: true },
    originalPrice: { type: Number },
    currency: { type: String, default: 'USD' },
    category: { type: String },
    difficulty: { type: String },
    location: {
      city: { type: String },
      country: { type: String },
      address: { type: String },
      coordinates: {
        lat: { type: Number },
        lng: { type: Number }
      }
    },
    images: [{
      url: { type: String },
      alt: { type: String },
      isPrimary: { type: Boolean, default: false }
    }],
    highlights: [{ type: String }],
    included: [{ type: String }],
    excluded: [{ type: String }],
    schedule: {
      startTime: { type: String },
      endTime: { type: String },
      meetingPoint: { type: String },
      availableDays: [{ type: String }],
      timeSlots: [{ type: String }]
    },
    capacity: {
      minimum: { type: Number },
      maximum: { type: Number }
    },
    guide: {
      name: { type: String },
      bio: { type: String },
      image: { type: String },
      languages: [{ type: String }],
      rating: { type: Number },
      experience: { type: String }
    },
    rating: {
      average: { type: Number },
      count: { type: Number }
    },
    cancellation: {
      policy: { type: String },
      refundable: { type: Boolean },
      cutoffHours: { type: Number }
    },
    selectedDate: { type: Date, required: true },
    selectedTimeSlot: { type: String, required: true }
  },
  
  // Customer Information (aligned with DAYTRIPBOOKING.cs)
  customerId: {
    type: Number,
    description: 'Customer ID reference (DAYTRIPBOOKING.CUSTOMERID)'
  },
  customer: {
    firstName: { type: String, required: true },
    lastName: { type: String, required: true },
    email: { type: String, required: true },
    phone: { type: String, required: true },
    address: {
      street: { type: String },
      city: { type: String },
      state: { type: String },
      country: { type: String },
      zipCode: { type: String }
    },
    dateOfBirth: { type: Date },
    nationality: { type: String }
  },

  // Guest Information (aligned with DAYTRIPBOOKING.cs)
  guestFirstName: {
    type: String,
    trim: true,
    description: 'Guest first name (DAYTRIPBOOKING.GuestFirstName)'
  },
  guestLastName: {
    type: String,
    trim: true,
    description: 'Guest last name (DAYTRIPBOOKING.GuestLastName)'
  },
  guestNationality: {
    type: Number,
    description: 'Guest nationality ID (DAYTRIPBOOKING.GuestNationality)'
  },
  ownerNotStayAtHotel: {
    type: Boolean,
    default: false,
    description: 'Owner not staying flag (DAYTRIPBOOKING.OwnerNotStayAtHotel)'
  },

  // Booking Details (aligned with DAYTRIPBOOKING.cs)
  date: {
    type: Date,
    description: 'Booking date (DAYTRIPBOOKING.Date)'
  },
  checkIn: {
    type: Date,
    description: 'Check-in date (DAYTRIPBOOKING.CHECK_IN)'
  },
  checkOut: {
    type: Date,
    description: 'Check-out date (DAYTRIPBOOKING.CHECK_OUT)'
  },
  day: {
    type: Number,
    description: 'Number of days (DAYTRIPBOOKING.DAY)'
  },
  startTime: {
    type: String,
    trim: true,
    description: 'Start time (DAYTRIPBOOKING.STARTTIME)'
  },
  rooms: {
    type: Number,
    description: 'Number of rooms (DAYTRIPBOOKING.ROOMS)'
  },

  // Participants (extended from C# model)
  person: {
    type: Number,
    description: 'Number of persons (DAYTRIPBOOKING.Person)'
  },
  participants: {
    adults: { type: Number, required: true, min: 1 },
    children: { type: Number, default: 0, min: 0 },
    infants: { type: Number, default: 0, min: 0 },
    totalCount: { type: Number, required: true },
    details: [{
      type: { type: String, enum: ['adult', 'child', 'infant'] },
      firstName: { type: String },
      lastName: { type: String },
      age: { type: Number },
      specialRequirements: { type: String }
    }]
  },
  
  // Pricing (aligned with DAYTRIPBOOKING.cs)
  roomRate: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Room/person rate (DAYTRIPBOOKING.ROOM_RATE)'
  },
  feeTax: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Tax and fees (DAYTRIPBOOKING.FEE_TAX)'
  },
  surcharge: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Additional surcharge (DAYTRIPBOOKING.SURCHARGE)'
  },
  surchargeName: {
    type: String,
    trim: true,
    description: 'Surcharge description (DAYTRIPBOOKING.SURCHARGENAME)'
  },
  total: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Total booking cost (DAYTRIPBOOKING.TOTAL)'
  },
  pricing: {
    basePrice: { type: Number, required: true },
    adultPrice: { type: Number, required: true },
    childPrice: { type: Number, default: 0 },
    infantPrice: { type: Number, default: 0 },
    subtotal: { type: Number, required: true },
    taxes: { type: Number, default: 0 },
    fees: { type: Number, default: 0 },
    discount: { type: Number, default: 0 },
    total: { type: Number, required: true },
    currency: { type: String, default: 'USD' }
  },

  // Payment (aligned with DAYTRIPBOOKING.cs)
  paymentStatus: {
    type: Number,
    default: 0,
    description: 'Payment status code (DAYTRIPBOOKING.PaymentStatus)'
  },
  paymentType: {
    type: Number,
    description: 'Payment type code (DAYTRIPBOOKING.PaymentType)'
  },
  payment: {
    method: {
      type: String,
      enum: ['credit_card', 'debit_card', 'paypal', 'stripe', 'bank_transfer'],
      required: false
    },
    status: {
      type: String,
      enum: ['pending', 'processing', 'completed', 'failed', 'refunded'],
      default: 'pending'
    },
    transactionId: { type: String },
    paymentDate: { type: Date },
    refundAmount: { type: Number, default: 0 },
    refundDate: { type: Date }
  },

  // Refund Information (aligned with DAYTRIPBOOKING.cs)
  isRefund: {
    type: Boolean,
    default: false,
    description: 'Refund status (DAYTRIPBOOKING.ISREFUND)'
  },
  refundFee: {
    type: mongoose.Schema.Types.Decimal128,
    get: val => val ? parseFloat(val.toString()) : 0,
    description: 'Refund fee amount (DAYTRIPBOOKING.RefundFee)'
  },
  

  // Additional Booking Details (aligned with DAYTRIPBOOKING.cs)
  description: {
    type: String,
    trim: true,
    description: 'Booking description/notes (DAYTRIPBOOKING.DESCRIPTION)'
  },
  specialRequest: {
    type: String,
    trim: true,
    description: 'Special requests (DAYTRIPBOOKING.SpecialRequest)'
  },
  specialRequests: { type: String }, // Alias for compatibility

  // System Fields (aligned with DAYTRIPBOOKING.cs)
  amenBooking: {
    type: Boolean,
    default: false,
    description: 'Amended booking flag (DAYTRIPBOOKING.AMENBOOKING)'
  },
  sendReceipt: {
    type: Boolean,
    default: false,
    description: 'Receipt sent flag (DAYTRIPBOOKING.SENDRECEIPT)'
  },
  sendVoucher: {
    type: Boolean,
    default: false,
    description: 'Voucher sent flag (DAYTRIPBOOKING.SENDVOUCHER)'
  },
  ipLocation: {
    type: String,
    trim: true,
    description: 'IP location of booking (DAYTRIPBOOKING.IPLOCATION)'
  },
  editBy: {
    type: Number,
    description: 'User ID who edited (DAYTRIPBOOKING.EDITBY)'
  },

  status: {
    type: String,
    enum: ['pending', 'confirmed', 'cancelled', 'completed', 'refunded'],
    default: 'pending'
  },
  
  confirmation: {
    code: { type: String },
    sentAt: { type: Date },
    method: { type: String, enum: ['email', 'sms'] }
  },
  
  cancellation: {
    reason: { type: String },
    cancelledAt: { type: Date },
    cancelledBy: { type: String, enum: ['customer', 'admin'] },
    refundStatus: { type: String },
    refundAmount: { type: Number }
  },

  // Staff Payment Processing
  pointsEligible: {
    type: Boolean,
    default: true
  },
  processedByStaff: {
    type: Boolean,
    default: false
  },
  staffUser: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'User',
    required: false
  },
  staffNotes: {
    type: String,
    default: ''
  },
  pointsAwarded: {
    type: Number,
    default: 0
  },
  pointsWaived: {
    type: Number,
    default: 0
  }
}, {
  timestamps: true
});

// Pre-save middleware to generate booking reference
BookingSchema.pre('save', async function(next) {
  try {
    if (this.isNew && !this.bookingReference) {
      // Get the count of existing bookings to generate sequential reference
      const count = await mongoose.model('Booking').countDocuments();
      this.bookingReference = generateBookingReference(count + 1);
    }
    next();
  } catch (error) {
    next(error);
  }
});

// Indexes for efficient queries
BookingSchema.index({ id: 1 });
BookingSchema.index({ bookingId: 1 });
BookingSchema.index({ bookingReference: 1 });
BookingSchema.index({ receiptId: 1 });
BookingSchema.index({ daytripId: 1 });
BookingSchema.index({ customerId: 1 });
BookingSchema.index({ 'customer.email': 1 });
BookingSchema.index({ 'tour.tourId': 1 });
BookingSchema.index({ status: 1 });
BookingSchema.index({ paymentStatus: 1 });
BookingSchema.index({ 'payment.status': 1 });
BookingSchema.index({ date: -1 });
BookingSchema.index({ checkIn: 1 });
BookingSchema.index({ createdAt: -1 });

export default mongoose.models.Booking || mongoose.model('Booking', BookingSchema);