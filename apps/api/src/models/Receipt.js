const mongoose = require('mongoose');

const receiptItemSchema = new mongoose.Schema({
  description: {
    type: String,
    required: true,
    trim: true
  },
  quantity: {
    type: Number,
    default: 1,
    min: 1
  },
  unitPrice: {
    type: Number,
    required: true,
    min: 0
  },
  totalPrice: {
    type: Number,
    required: true,
    min: 0
  }
});

const receiptSchema = new mongoose.Schema({
  receiptNumber: {
    type: String,
    required: [true, 'Receipt number is required'],
    unique: true,
    trim: true
  },
  booking: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'Booking',
    required: [true, 'Booking reference is required']
  },
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
  issueDate: {
    type: Date,
    default: Date.now,
    required: true
  },
  items: [receiptItemSchema],
  subtotal: {
    type: Number,
    required: true,
    min: 0
  },
  surcharges: {
    type: Number,
    default: 0,
    min: 0
  },
  discounts: {
    type: Number,
    default: 0,
    min: 0
  },
  tax: {
    rate: { type: Number, default: 0, min: 0, max: 100 },
    amount: { type: Number, default: 0, min: 0 }
  },
  totalAmount: {
    type: Number,
    required: true,
    min: 0
  },
  currency: {
    type: String,
    default: 'USD',
    enum: ['USD', 'EUR', 'GBP', 'JPY', 'AUD', 'VND']
  },
  paymentMethod: {
    type: String,
    required: true,
    enum: ['credit_card', 'debit_card', 'paypal', 'bank_transfer', 'cash', 'other']
  },
  paymentStatus: {
    type: String,
    required: true,
    enum: ['pending', 'paid', 'partially_paid', 'refunded', 'cancelled'],
    default: 'pending'
  },
  pdfUrl: {
    type: String,
    default: ''
  },
  notes: {
    type: String,
    default: '',
    trim: true
  },
  createdBy: {
    type: mongoose.Schema.Types.ObjectId,
    ref: 'User'
  }
}, {
  timestamps: true,
  toJSON: { virtuals: true },
  toObject: { virtuals: true }
});

// Indexes for better performance
receiptSchema.index({ receiptNumber: 1 });
receiptSchema.index({ booking: 1 });
receiptSchema.index({ user: 1 });
receiptSchema.index({ tour: 1 });
receiptSchema.index({ paymentStatus: 1 });
receiptSchema.index({ issueDate: -1 });
receiptSchema.index({ createdAt: -1 });

// Virtual for formatted total amount
receiptSchema.virtual('formattedTotal').get(function() {
  return new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: this.currency || 'USD'
  }).format(this.totalAmount);
});

// Virtual for formatted issue date
receiptSchema.virtual('formattedIssueDate').get(function() {
  return this.issueDate.toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  });
});

// Static method to generate unique receipt number
receiptSchema.statics.generateReceiptNumber = async function() {
  const date = new Date();
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');

  // Format: RCP-YYYYMMDD-XXXX
  const prefix = `RCP-${year}${month}${day}`;

  // Find the last receipt number for today
  const lastReceipt = await this.findOne({
    receiptNumber: new RegExp(`^${prefix}`)
  }).sort({ receiptNumber: -1 });

  let sequence = 1;
  if (lastReceipt) {
    const lastSequence = parseInt(lastReceipt.receiptNumber.split('-').pop());
    sequence = lastSequence + 1;
  }

  const sequenceStr = String(sequence).padStart(4, '0');
  return `${prefix}-${sequenceStr}`;
};

// Pre-save middleware to generate receipt number if not provided
receiptSchema.pre('save', async function(next) {
  if (!this.receiptNumber) {
    this.receiptNumber = await this.constructor.generateReceiptNumber();
  }
  next();
});

module.exports = mongoose.models.Receipt || mongoose.model('Receipt', receiptSchema);
