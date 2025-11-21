#!/bin/bash

# Tour Pricing System Deployment Script
# Deploys the new tour pricing features to test server

set -e  # Exit on any error

echo "================================================"
echo "Deploying Tour Pricing System"
echo "================================================"
echo ""

# Navigate to project directory
echo "📁 Navigating to project directory..."
cd /var/www/leetour

# Pull latest changes from git
echo "🔄 Pulling latest changes from git..."
git pull origin main

echo ""
echo "================================================"
echo "Building Applications"
echo "================================================"

# Build API (includes new pricing endpoint)
echo ""
echo "📦 Building API with new pricing endpoint..."
cd apps/api
npm run build
cd ../..

# Build Frontend (includes new pricing components)
echo ""
echo "📦 Building Frontend with pricing components..."
cd apps/frontend
npm run build
cd ../..

echo ""
echo "================================================"
echo "Restarting Services"
echo "================================================"

# Restart only API and Frontend (no need to restart Admin)
echo ""
echo "🔄 Restarting API server..."
pm2 restart leetour-api

echo ""
echo "🔄 Restarting Frontend server..."
pm2 restart leetour-frontend

# Show PM2 status
echo ""
echo "================================================"
echo "Deployment Status"
echo "================================================"
pm2 status

echo ""
echo "================================================"
echo "Verification"
echo "================================================"

# Test API endpoint
echo ""
echo "🧪 Testing pricing API endpoint..."
echo "Please manually test with a tour ID:"
echo "curl 'https://api.goreise.com/api/tours/TOUR_ID/pricing?date=2025-12-25&adults=2&children=1'"

echo ""
echo "================================================"
echo "✅ Tour Pricing System Deployment Complete!"
echo "================================================"
echo ""
echo "Next Steps:"
echo "1. Test the API endpoint with a real tour ID"
echo "2. Open https://tour.goreise.com/tours/TOUR_ID"
echo "3. Try checking availability"
echo "4. Verify pricing options appear"
echo "5. Test booking flow"
echo ""
echo "For detailed testing, see: TESTING_GUIDE.md"
echo "================================================"
