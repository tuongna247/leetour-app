const mongoose = require('mongoose');

const reviewSchema = new mongoose.Schema({
  tour: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'Tour',
    required: [true, 'Tour reference is required']
  },
  user: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'User',
    required: [true, 'User reference is required']
  },
  booking: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'Booking',
    required: false
  },
  rating: {
    type: Number,
    required: [true, 'Rating is required'],
    min: [1, 'Rating must be at least 1'],
    max: [5, 'Rating cannot exceed 5']
  },
  title: {
    type: String,
    required: [true, 'Review title is required'],
    trim: true,
    maxlength: [200, 'Title cannot exceed 200 characters']
  },
  comment: {
    type: String,
    required: [true, 'Review comment is required'],
    trim: true,
    maxlength: [2000, 'Comment cannot exceed 2000 characters']
  },
  verifiedPurchase: {
    type: Boolean,
    default: false
  },
  status: {
    type: String,
    enum: ['pending', 'approved', 'rejected'],
    default: 'pending'
  },
  helpful: {
    count: { type: Number, default: 0 },
    users: [{ type: mongoose.Schema.Types.ObjectId, ref: 'User' }]
  },
  images: [{
    url: { type: String },
    alt: { type: String }
  }],
  adminNotes: {
    type: String,
    default: ''
  },
  recaptchaScore: {
    type: Number,
    default: 0
  },
  ipAddress: {
    type: String,
    default: ''
  }
}, {
  timestamps: true,
  toJSON: { virtuals: true },
  toObject: { virtuals: true }
});

// Indexes for better performance
reviewSchema.index({ tour: 1, status: 1 });
reviewSchema.index({ user: 1 });
reviewSchema.index({ booking: 1 });
reviewSchema.index({ status: 1, createdAt: -1 });
reviewSchema.index({ rating: 1 });

// Compound index for verified purchases
reviewSchema.index({ tour: 1, verifiedPurchase: 1, status: 1 });

// Virtual for formatted date
reviewSchema.virtual('formattedDate').get(function() {
  return this.createdAt.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
});

// Static method to calculate average rating for a tour
reviewSchema.statics.calculateAverageRating = async function(tourId) {
  const stats = await this.aggregate([
    {
      $match: { tour: mongoose.Types.ObjectId(tourId), status: 'approved' }
    },
    {
      $group: {
        _id: '$tour',
        averageRating: { $avg: '$rating' },
        totalReviews: { $sum: 1 }
      }
    }
  ]);

  if (stats.length > 0) {
    return {
      average: Math.round(stats[0].averageRating * 10) / 10,
      count: stats[0].totalReviews
    };
  }

  return { average: 0, count: 0 };
};

// Method to update tour rating after review is approved/rejected
reviewSchema.post('save', async function() {
  if (this.status === 'approved') {
    const Tour = mongoose.model('Tour');
    const ratingStats = await this.constructor.calculateAverageRating(this.tour);
    await Tour.findByIdAndUpdate(this.tour, {
      'rating.average': ratingStats.average,
      'rating.count': ratingStats.count
    });
  }
});

// Prevent duplicate reviews from same user for same tour
reviewSchema.index({ tour: 1, user: 1 }, { unique: true });

module.exports = mongoose.models.Review || mongoose.model('Review', reviewSchema);
