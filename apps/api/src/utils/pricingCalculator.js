/**
 * Calculate price per person based on tour option and passenger count
 * @param {Object} tourOption - The tour pricing option object
 * @param {number} passengerCount - Number of passengers
 * @returns {number} - Price per person
 */
export function calculatePricePerPerson(tourOption, passengerCount) {
  if (!tourOption) {
    throw new Error('Tour option is required');
  }

  if (!passengerCount || passengerCount < 1) {
    throw new Error('Passenger count must be at least 1');
  }

  // If no pricing tiers defined, use base price
  if (!tourOption.pricingTiers || tourOption.pricingTiers.length === 0) {
    return tourOption.basePrice;
  }

  // Find the appropriate pricing tier based on passenger count
  const applicableTier = tourOption.pricingTiers.find(tier =>
    passengerCount >= tier.minPassengers && passengerCount <= tier.maxPassengers
  );

  // If a tier matches, use its price; otherwise fall back to base price
  return applicableTier ? applicableTier.pricePerPerson : tourOption.basePrice;
}

/**
 * Calculate total price for a tour booking
 * @param {Object} tourOption - The tour pricing option object
 * @param {number} passengerCount - Number of passengers
 * @returns {Object} - Pricing breakdown { pricePerPerson, subtotal, passengerCount }
 */
export function calculateTotalPrice(tourOption, passengerCount) {
  const pricePerPerson = calculatePricePerPerson(tourOption, passengerCount);
  const subtotal = pricePerPerson * passengerCount;

  return {
    pricePerPerson,
    passengerCount,
    subtotal,
    currency: 'USD' // This should come from the tour or be passed as parameter
  };
}

/**
 * Get all active tour options with calculated prices for a given passenger count
 * @param {Array} tourOptions - Array of tour option objects
 * @param {number} passengerCount - Number of passengers
 * @returns {Array} - Tour options with calculated prices
 */
export function getActiveOptionsWithPrices(tourOptions, passengerCount) {
  if (!tourOptions || !Array.isArray(tourOptions)) {
    return [];
  }

  return tourOptions
    .filter(option => option.isActive)
    .map(option => {
      const pricing = calculateTotalPrice(option, passengerCount);
      return {
        ...option,
        calculatedPricing: pricing
      };
    });
}

/**
 * Find the cheapest tour option for a given passenger count
 * @param {Array} tourOptions - Array of tour option objects
 * @param {number} passengerCount - Number of passengers
 * @returns {Object|null} - The cheapest tour option with pricing or null
 */
export function findCheapestOption(tourOptions, passengerCount) {
  const optionsWithPrices = getActiveOptionsWithPrices(tourOptions, passengerCount);

  if (optionsWithPrices.length === 0) {
    return null;
  }

  return optionsWithPrices.reduce((cheapest, current) => {
    return current.calculatedPricing.subtotal < cheapest.calculatedPricing.subtotal
      ? current
      : cheapest;
  });
}

/**
 * Validate tour option data
 * @param {Object} tourOption - The tour option to validate
 * @returns {Object} - { valid: boolean, errors: Array }
 */
export function validateTourOption(tourOption) {
  const errors = [];

  if (!tourOption.optionName || tourOption.optionName.trim() === '') {
    errors.push('Option name is required');
  }

  if (tourOption.basePrice === undefined || tourOption.basePrice < 0) {
    errors.push('Base price must be a positive number');
  }

  // Validate pricing tiers if they exist
  if (tourOption.pricingTiers && tourOption.pricingTiers.length > 0) {
    tourOption.pricingTiers.forEach((tier, index) => {
      if (tier.minPassengers < 1) {
        errors.push(`Tier ${index + 1}: Minimum passengers must be at least 1`);
      }

      if (tier.maxPassengers < tier.minPassengers) {
        errors.push(`Tier ${index + 1}: Maximum passengers must be greater than or equal to minimum`);
      }

      if (tier.pricePerPerson < 0) {
        errors.push(`Tier ${index + 1}: Price per person must be positive`);
      }
    });

    // Check for overlapping tiers
    for (let i = 0; i < tourOption.pricingTiers.length; i++) {
      for (let j = i + 1; j < tourOption.pricingTiers.length; j++) {
        const tier1 = tourOption.pricingTiers[i];
        const tier2 = tourOption.pricingTiers[j];

        const overlaps =
          (tier1.minPassengers <= tier2.maxPassengers && tier1.maxPassengers >= tier2.minPassengers) ||
          (tier2.minPassengers <= tier1.maxPassengers && tier2.maxPassengers >= tier1.minPassengers);

        if (overlaps) {
          errors.push(`Tiers ${i + 1} and ${j + 1} have overlapping passenger ranges`);
        }
      }
    }
  }

  return {
    valid: errors.length === 0,
    errors
  };
}

/**
 * Check if a date falls within a surcharge period
 * @param {Date} bookingDate - The booking date to check
 * @param {Object} surcharge - Surcharge object with startDate and endDate
 * @returns {boolean} - Whether the date falls within the surcharge period
 */
function isDateInSurchargePeriod(bookingDate, surcharge) {
  if (!surcharge.isActive) return false;

  const booking = new Date(bookingDate);
  const start = new Date(surcharge.startDate);
  const end = new Date(surcharge.endDate);

  return booking >= start && booking <= end;
}

/**
 * Calculate applicable surcharges for a booking
 * @param {Date} bookingDate - The booking date
 * @param {Array} surcharges - Array of surcharge objects
 * @param {number} baseAmount - Base amount before surcharges
 * @returns {Object} - { totalSurcharge, appliedSurcharges[], breakdown[] }
 */
export function calculateSurcharges(bookingDate, surcharges, baseAmount) {
  if (!surcharges || !Array.isArray(surcharges) || surcharges.length === 0) {
    return {
      totalSurcharge: 0,
      appliedSurcharges: [],
      breakdown: []
    };
  }

  const appliedSurcharges = surcharges.filter(s =>
    isDateInSurchargePeriod(bookingDate, s)
  );

  let totalSurcharge = 0;
  const breakdown = [];

  appliedSurcharges.forEach(surcharge => {
    let amount = 0;

    if (surcharge.amountType === 'percentage') {
      amount = (baseAmount * surcharge.amount) / 100;
    } else {
      amount = surcharge.amount;
    }

    totalSurcharge += amount;
    breakdown.push({
      name: surcharge.surchargeName,
      type: surcharge.surchargeType,
      amountType: surcharge.amountType,
      rate: surcharge.amount,
      calculatedAmount: amount,
      description: surcharge.description
    });
  });

  return {
    totalSurcharge,
    appliedSurcharges,
    breakdown
  };
}

/**
 * Check if a booking date qualifies for a promotion
 * @param {Date} bookingDate - The booking date
 * @param {Date} departureDate - The tour departure date
 * @param {Object} promotion - Promotion object
 * @param {number} passengerCount - Number of passengers
 * @returns {boolean} - Whether the booking qualifies
 */
function isPromotionApplicable(bookingDate, departureDate, promotion, passengerCount) {
  if (!promotion.isActive) return false;

  const now = new Date(bookingDate);
  const validFrom = new Date(promotion.validFrom);
  const validTo = new Date(promotion.validTo);
  const departure = new Date(departureDate);

  // Check if booking is within promotion validity period
  if (now < validFrom || now > validTo) return false;

  // Check booking window if specified
  if (promotion.bookingWindowStart && promotion.bookingWindowEnd) {
    const windowStart = new Date(promotion.bookingWindowStart);
    const windowEnd = new Date(promotion.bookingWindowEnd);
    if (now < windowStart || now > windowEnd) return false;
  }

  // Check days before departure (for early bird / last minute)
  if (promotion.daysBeforeDeparture) {
    const daysUntilDeparture = Math.ceil((departure - now) / (1000 * 60 * 60 * 24));

    if (promotion.promotionType === 'early_bird') {
      if (daysUntilDeparture < promotion.daysBeforeDeparture) return false;
    } else if (promotion.promotionType === 'last_minute') {
      if (daysUntilDeparture > promotion.daysBeforeDeparture) return false;
    }
  }

  // Check minimum passengers requirement
  if (promotion.minPassengers && passengerCount < promotion.minPassengers) {
    return false;
  }

  return true;
}

/**
 * Calculate applicable promotions for a booking
 * @param {Date} bookingDate - The booking date
 * @param {Date} departureDate - The tour departure date
 * @param {Array} promotions - Array of promotion objects
 * @param {number} baseAmount - Base amount before discounts
 * @param {number} passengerCount - Number of passengers
 * @returns {Object} - { totalDiscount, appliedPromotions[], breakdown[] }
 */
export function calculatePromotions(bookingDate, departureDate, promotions, baseAmount, passengerCount) {
  if (!promotions || !Array.isArray(promotions) || promotions.length === 0) {
    return {
      totalDiscount: 0,
      appliedPromotions: [],
      breakdown: []
    };
  }

  const applicablePromotions = promotions.filter(p =>
    isPromotionApplicable(bookingDate, departureDate, p, passengerCount)
  );

  // Sort by discount amount (descending) to apply best promotion first
  // In most cases, only the best promotion should apply
  applicablePromotions.sort((a, b) => {
    const discountA = a.discountType === 'percentage'
      ? (baseAmount * a.discountAmount) / 100
      : a.discountAmount;
    const discountB = b.discountType === 'percentage'
      ? (baseAmount * b.discountAmount) / 100
      : b.discountAmount;
    return discountB - discountA;
  });

  // Apply only the best promotion (can be modified to allow stacking)
  const bestPromotion = applicablePromotions[0];

  if (!bestPromotion) {
    return {
      totalDiscount: 0,
      appliedPromotions: [],
      breakdown: []
    };
  }

  let discountAmount = 0;

  if (bestPromotion.discountType === 'percentage') {
    discountAmount = (baseAmount * bestPromotion.discountAmount) / 100;
  } else {
    discountAmount = bestPromotion.discountAmount;
  }

  // Ensure discount doesn't exceed base amount
  discountAmount = Math.min(discountAmount, baseAmount);

  return {
    totalDiscount: discountAmount,
    appliedPromotions: [bestPromotion],
    breakdown: [{
      name: bestPromotion.promotionName,
      type: bestPromotion.promotionType,
      discountType: bestPromotion.discountType,
      rate: bestPromotion.discountAmount,
      calculatedAmount: discountAmount,
      conditions: bestPromotion.conditions
    }]
  };
}

/**
 * Calculate refund amount based on cancellation policy
 * @param {Date} cancellationDate - When the cancellation is requested
 * @param {Date} departureDate - The tour departure date
 * @param {Array} cancellationPolicies - Array of cancellation policy objects
 * @param {number} totalAmount - Total booking amount
 * @returns {Object} - { refundAmount, refundPercentage, applicablePolicy }
 */
export function calculateCancellationRefund(cancellationDate, departureDate, cancellationPolicies, totalAmount) {
  if (!cancellationPolicies || !Array.isArray(cancellationPolicies) || cancellationPolicies.length === 0) {
    return {
      refundAmount: 0,
      refundPercentage: 0,
      applicablePolicy: null
    };
  }

  const cancel = new Date(cancellationDate);
  const departure = new Date(departureDate);
  const daysBeforeDeparture = Math.ceil((departure - cancel) / (1000 * 60 * 60 * 24));

  // Sort policies by days before departure (descending)
  const sortedPolicies = [...cancellationPolicies].sort((a, b) =>
    b.daysBeforeDeparture - a.daysBeforeDeparture
  );

  // Find the applicable policy
  let applicablePolicy = null;
  for (const policy of sortedPolicies) {
    if (daysBeforeDeparture >= policy.daysBeforeDeparture) {
      applicablePolicy = policy;
      break;
    }
  }

  // If no policy matches, use the strictest (lowest days)
  if (!applicablePolicy) {
    applicablePolicy = sortedPolicies[sortedPolicies.length - 1];
  }

  const refundPercentage = applicablePolicy ? applicablePolicy.refundPercentage : 0;
  const refundAmount = (totalAmount * refundPercentage) / 100;

  return {
    refundAmount,
    refundPercentage,
    applicablePolicy,
    daysBeforeDeparture
  };
}

/**
 * Calculate complete booking price with all fees, surcharges, and promotions
 * @param {Object} params - Pricing parameters
 * @returns {Object} - Complete pricing breakdown
 */
export function calculateCompleteBookingPrice({
  tourOption,
  passengerCount,
  bookingDate,
  departureDate,
  surcharges = [],
  promotions = [],
  taxRate = 0,
  currency = 'USD'
}) {
  // Calculate base price
  const basePrice = calculateTotalPrice(tourOption, passengerCount);

  // Calculate surcharges
  const surchargeResult = calculateSurcharges(bookingDate, surcharges, basePrice.subtotal);

  // Calculate amount after surcharges (before discount)
  const amountAfterSurcharges = basePrice.subtotal + surchargeResult.totalSurcharge;

  // Calculate promotions/discounts
  const promotionResult = calculatePromotions(
    bookingDate,
    departureDate,
    promotions,
    amountAfterSurcharges,
    passengerCount
  );

  // Calculate subtotal after discount
  const subtotalAfterDiscount = amountAfterSurcharges - promotionResult.totalDiscount;

  // Calculate tax
  const taxAmount = (subtotalAfterDiscount * taxRate) / 100;

  // Calculate final total
  const total = subtotalAfterDiscount + taxAmount;

  return {
    basePrice: basePrice.pricePerPerson,
    passengerCount,
    subtotal: basePrice.subtotal,
    surcharges: {
      total: surchargeResult.totalSurcharge,
      breakdown: surchargeResult.breakdown
    },
    amountAfterSurcharges,
    promotions: {
      total: promotionResult.totalDiscount,
      breakdown: promotionResult.breakdown
    },
    subtotalAfterDiscount,
    tax: {
      rate: taxRate,
      amount: taxAmount
    },
    total,
    currency,
    breakdown: {
      'Base Price': basePrice.subtotal,
      'Surcharges': surchargeResult.totalSurcharge,
      'Promotions': -promotionResult.totalDiscount,
      'Tax': taxAmount,
      'Total': total
    }
  };
}

module.exports = {
  calculatePricePerPerson,
  calculateTotalPrice,
  getActiveOptionsWithPrices,
  findCheapestOption,
  validateTourOption,
  calculateSurcharges,
  calculatePromotions,
  calculateCancellationRefund,
  calculateCompleteBookingPrice
};
