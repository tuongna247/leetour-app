const jwt = require('jsonwebtoken');
const User = require('../models/User');

const auth = async (req, res, next) => {
  try {
    const token = req.headers.authorization?.replace('Bearer ', '') || 
                  req.headers.Authorization?.replace('Bearer ', '');
    
    if (!token) {
      return res.status(401).json({
        status: 401,
        msg: 'No token, authorization denied'
      });
    }

    const decoded = jwt.verify(token, process.env.JWT_SECRET || 'your-secret-key');
    const user = await User.findById(decoded.userId).select('-password');
    
    if (!user || !user.isActive) {
      return res.status(401).json({
        status: 401,
        msg: 'Token is not valid'
      });
    }

    req.user = user;
    next();
  } catch (error) {
    console.error('Auth middleware error:', error);
    res.status(401).json({
      status: 401,
      msg: 'Token is not valid'
    });
  }
};

const adminAuth = async (req, res, next) => {
  try {
    await auth(req, res, () => {
      if (req.user.role !== 'admin') {
        return res.status(403).json({
          status: 403,
          msg: 'Access denied. Admin role required.'
        });
      }
      next();
    });
  } catch (error) {
    return res.status(401).json({
      status: 401,
      msg: 'Authentication required'
    });
  }
};

export { auth, adminAuth };