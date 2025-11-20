#!/bin/bash

# API Deployment Script for LeeTour
# This script pulls latest code, rebuilds, and restarts the API application

set -e  # Exit on any error

echo "================================================"
echo "Starting API Deployment"
echo "================================================"

# Navigate to project directory
echo "📁 Navigating to project directory..."
cd /var/www/leetour

# Pull latest changes from git
echo "🔄 Pulling latest changes from git..."
git pull

# Navigate to API directory
echo "📦 Building API application..."
cd apps/api

# Build the application
npm run build

# Restart PM2 process
echo "🔄 Restarting PM2 API process..."
pm2 restart leetour-api

# Show PM2 status
echo "✅ Deployment complete! Current status:"
pm2 status leetour-api

echo "================================================"
echo "API Deployment Complete!"
echo "================================================"
