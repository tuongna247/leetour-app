#!/bin/bash

# Quick Fix for Frontend Client Error
# Run this on the server to fix the "Application error: a client-side exception" issue

set -e

echo "================================================"
echo "Quick Fix: Frontend Client Error"
echo "================================================"
echo ""

# Navigate to frontend directory
echo "📁 Navigating to frontend..."
cd /var/www/leetour/apps/frontend

# Show current status
echo ""
echo "Current PM2 status:"
pm2 status leetour-frontend

# Check if .next exists
echo ""
echo "Checking build directory..."
if [ -d ".next" ]; then
    echo "✓ .next directory exists"
    echo "  Removing old build..."
    rm -rf .next
else
    echo "✗ .next directory missing (first build?)"
fi

# Clean install (if node_modules has issues)
echo ""
echo "🧹 Cleaning node_modules..."
rm -rf node_modules package-lock.json
echo "📦 Installing dependencies..."
npm install --production=false

# Rebuild
echo ""
echo "🔨 Building frontend application..."
NODE_ENV=production npm run build

# Check if build was successful
if [ -d ".next" ]; then
    echo "✓ Build successful!"
else
    echo "✗ Build failed! Check logs above."
    exit 1
fi

# Restart PM2
echo ""
echo "🔄 Restarting PM2 frontend process..."
pm2 restart leetour-frontend

# Wait a moment
sleep 3

# Show logs
echo ""
echo "📋 Recent logs:"
pm2 logs leetour-frontend --lines 30 --nostream

# Show final status
echo ""
echo "================================================"
echo "Final Status"
echo "================================================"
pm2 status leetour-frontend

echo ""
echo "✅ Fix complete!"
echo ""
echo "Next steps:"
echo "1. Open https://tour.goreise.com in browser"
echo "2. Press F12 to open developer console"
echo "3. Check if error is gone"
echo "4. Try navigating to a tour page"
echo ""
echo "If still having issues, check: FIX_CLIENT_ERROR.md"
echo "================================================"
