#!/bin/bash

# Frontend Deployment Script for LeeTour
# This script pulls latest code, rebuilds, and restarts the frontend application

set -e  # Exit on any error

echo "================================================"
echo "Starting Frontend Deployment"
echo "================================================"

# Navigate to project directory
echo "📁 Navigating to project directory..."
cd /var/www/leetour

# Pull latest changes from git
echo "🔄 Pulling latest changes from git..."
git pull

# Navigate to frontend directory
echo "📦 Building frontend application..."
cd apps/frontend

# Build the application
npm run build

# Restart PM2 process
echo "🔄 Restarting PM2 frontend process..."
pm2 restart leetour-frontend

# Show PM2 status
echo "✅ Deployment complete! Current status:"
pm2 status leetour-frontend

echo "================================================"
echo "Frontend Deployment Complete!"
echo "================================================"
