const mongoose = require('mongoose');
const bcrypt = require('bcryptjs');

const UserSchema = new mongoose.Schema({
  username: {
    type: String,
    required: true,
    unique: true,
    trim: true,
    minlength: 3,
    maxlength: 30
  },
  name: {
    type: String,
    required: true,
    trim: true,
    maxlength: 100
  },
  email: {
    type: String,
    required: true,
    unique: true,
    trim: true,
    lowercase: true
  },
  password: {
    type: String,
    required: function() {
      return !this.googleId && !this.facebookId;
    },
    minlength: 6
  },
  // OAuth provider information
  googleId: {
    type: String,
    sparse: true
  },
  facebookId: {
    type: String,
    sparse: true
  },
  provider: {
    type: String,
    enum: ['local', 'google', 'facebook'],
    default: 'local'
  },
  profilePicture: {
    type: String,
    default: ''
  },
  isEmailVerified: {
    type: Boolean,
    default: false
  },
  role: {
    type: String,
    enum: ['admin', 'mod', 'customer'],
    default: 'customer'
  },
  isActive: {
    type: Boolean,
    default: true
  },
  lastLogin: {
    type: Date
  }
}, {
  timestamps: true
});

// Index for performance
UserSchema.index({ username: 1 });
UserSchema.index({ email: 1 });
UserSchema.index({ googleId: 1 });
UserSchema.index({ facebookId: 1 });

// Hash password before saving (only for local auth)
UserSchema.pre('save', async function(next) {
  if (!this.isModified('password') || !this.password) return next();
  
  try {
    const salt = await bcrypt.genSalt(12);
    this.password = await bcrypt.hash(this.password, salt);
    next();
  } catch (error) {
    next(error);
  }
});

// Compare password method
UserSchema.methods.comparePassword = async function(candidatePassword) {
  return await bcrypt.compare(candidatePassword, this.password);
};

// Remove password from JSON output
UserSchema.methods.toJSON = function() {
  const user = this.toObject();
  delete user.password;
  return user;
};

module.exports = mongoose.models.User || mongoose.model('User', UserSchema);