#!/bin/bash

# Setup Nginx sites for goreise.com domains
echo "=== Setting up Nginx sites ==="

# Move config files to sites-available
echo "Moving config files..."
mv /tmp/admin.goreise.com.conf /etc/nginx/sites-available/admin.goreise.com
mv /tmp/api.goreise.com.conf /etc/nginx/sites-available/api.goreise.com
mv /tmp/tour.goreise.com.conf /etc/nginx/sites-available/tour.goreise.com

# Set proper permissions
chmod 644 /etc/nginx/sites-available/admin.goreise.com
chmod 644 /etc/nginx/sites-available/api.goreise.com
chmod 644 /etc/nginx/sites-available/tour.goreise.com

# Create symlinks to sites-enabled
echo "Enabling sites..."
ln -sf /etc/nginx/sites-available/admin.goreise.com /etc/nginx/sites-enabled/
ln -sf /etc/nginx/sites-available/api.goreise.com /etc/nginx/sites-enabled/
ln -sf /etc/nginx/sites-available/tour.goreise.com /etc/nginx/sites-enabled/

# Test nginx configuration
echo "Testing Nginx configuration..."
nginx -t

if [ $? -eq 0 ]; then
    echo "✓ Nginx configuration is valid"
    echo "Reloading Nginx..."
    systemctl reload nginx
    echo "✓ Nginx reloaded successfully"
else
    echo "✗ Nginx configuration test failed"
    exit 1
fi

echo ""
echo "=== Done! ==="
echo "Nginx sites enabled:"
echo "  - admin.goreise.com -> localhost:3000"
echo "  - api.goreise.com -> localhost:3001"
echo "  - tour.goreise.com -> localhost:3002"
