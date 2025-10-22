/**
 * Google reCAPTCHA v3 Validation Utility
 *
 * This utility provides server-side verification for Google reCAPTCHA v3
 * to prevent spam and bot submissions on review forms and other user inputs.
 */

const RECAPTCHA_SECRET_KEY = process.env.RECAPTCHA_SECRET_KEY;
const RECAPTCHA_VERIFY_URL = 'https://www.google.com/recaptcha/api/siteverify';

// Minimum score threshold for reCAPTCHA v3 (0.0 to 1.0)
// 0.5 is recommended, but can be adjusted based on requirements
const MIN_SCORE_THRESHOLD = 0.5;

/**
 * Verify reCAPTCHA token with Google's API
 *
 * @param {string} token - The reCAPTCHA token from client
 * @param {string} remoteip - Optional: The user's IP address
 * @returns {Promise<Object>} Verification result
 */
async function verifyRecaptcha(token, remoteip = null) {
  if (!RECAPTCHA_SECRET_KEY) {
    console.error('RECAPTCHA_SECRET_KEY is not configured in environment variables');
    return {
      success: false,
      error: 'reCAPTCHA is not configured on the server',
      score: 0
    };
  }

  if (!token) {
    return {
      success: false,
      error: 'reCAPTCHA token is required',
      score: 0
    };
  }

  try {
    const params = new URLSearchParams({
      secret: RECAPTCHA_SECRET_KEY,
      response: token
    });

    if (remoteip) {
      params.append('remoteip', remoteip);
    }

    const response = await fetch(RECAPTCHA_VERIFY_URL, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/x-www-form-urlencoded'
      },
      body: params.toString()
    });

    const data = await response.json();

    if (!data.success) {
      return {
        success: false,
        error: 'reCAPTCHA verification failed',
        errorCodes: data['error-codes'] || [],
        score: 0
      };
    }

    // Check score threshold for v3
    const score = data.score || 0;
    if (score < MIN_SCORE_THRESHOLD) {
      return {
        success: false,
        error: `reCAPTCHA score too low: ${score}`,
        score: score,
        action: data.action
      };
    }

    return {
      success: true,
      score: score,
      action: data.action,
      challengeTimestamp: data.challenge_ts,
      hostname: data.hostname
    };

  } catch (error) {
    console.error('reCAPTCHA verification error:', error);
    return {
      success: false,
      error: 'Failed to verify reCAPTCHA',
      details: error.message,
      score: 0
    };
  }
}

/**
 * Middleware to verify reCAPTCHA token from request
 *
 * Usage in routes:
 * app.post('/api/reviews', verifyRecaptchaMiddleware, async (req, res) => { ... })
 */
async function verifyRecaptchaMiddleware(req, res, next) {
  const token = req.body.recaptchaToken || req.headers['x-recaptcha-token'];
  const remoteip = req.ip || req.connection.remoteAddress;

  const result = await verifyRecaptcha(token, remoteip);

  if (!result.success) {
    return res.status(400).json({
      success: false,
      message: 'reCAPTCHA verification failed',
      error: result.error
    });
  }

  // Attach score to request for logging
  req.recaptchaScore = result.score;
  req.recaptchaAction = result.action;

  next();
}

/**
 * Validate reCAPTCHA with custom score threshold
 *
 * @param {string} token - The reCAPTCHA token
 * @param {number} minScore - Minimum acceptable score (0.0 to 1.0)
 * @param {string} remoteip - Optional: User's IP address
 * @returns {Promise<Object>} Verification result
 */
async function verifyRecaptchaWithScore(token, minScore = MIN_SCORE_THRESHOLD, remoteip = null) {
  const result = await verifyRecaptcha(token, remoteip);

  if (result.success && result.score < minScore) {
    return {
      success: false,
      error: `Score ${result.score} is below required threshold ${minScore}`,
      score: result.score
    };
  }

  return result;
}

/**
 * Get client IP address from request
 *
 * @param {Object} req - Express request object
 * @returns {string} IP address
 */
function getClientIP(req) {
  return (
    req.headers['x-forwarded-for']?.split(',')[0] ||
    req.headers['x-real-ip'] ||
    req.connection.remoteAddress ||
    req.socket.remoteAddress ||
    req.ip ||
    'unknown'
  );
}

module.exports = {
  verifyRecaptcha,
  verifyRecaptchaMiddleware,
  verifyRecaptchaWithScore,
  getClientIP,
  MIN_SCORE_THRESHOLD
};
