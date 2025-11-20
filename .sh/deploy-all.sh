#!/bin/bash

# Full Deployment Script for LeeTour
# This script pulls latest code, rebuilds, and restarts all applications

set -e  # Exit on any error

echo "================================================"
echo "Starting Full Deployment (Admin + API + Frontend)"
echo "================================================"

# Navigate to project directory
echo "📁 Navigating to project directory..."
cd /var/www/leetour

# Pull latest changes from git
echo "🔄 Pulling latest changes from git..."
git pull

# Build API
echo ""
echo "📦 Building API application..."
cd apps/api
npm run build
cd ../..

# Build Admin
echo ""
echo "📦 Building Admin application..."
cd apps/admin
npm run build
cd ../..

# Build Frontend
echo ""
echo "📦 Building Frontend application..."
cd apps/frontend
npm run build
cd ../..

# Restart all PM2 processes
echo ""
echo "🔄 Restarting all PM2 processes..."
pm2 restart all

# Show PM2 status
echo ""
echo "✅ Deployment complete! Current status:"
pm2 status

echo ""
echo "================================================"
echo "Full Deployment Complete!"
echo "================================================"
