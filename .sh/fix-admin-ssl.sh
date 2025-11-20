#!/bin/bash

# Fix/Renew SSL certificates for all domains
# Run this on the server as root or with sudo

echo "=== Renewing SSL Certificates for All Domains ==="

# List of domains
DOMAINS=(
    "admin.goreise.com"
    "api.goreise.com"
    "tour.goreise.com"
    "vote.kinhthanhmoingay.com"
)

# Step 1: Stop Nginx temporarily
echo "Stopping Nginx..."
systemctl stop nginx

# Step 2: Obtain/renew SSL certificates for each domain
for domain in "${DOMAINS[@]}"; do
    echo ""
    echo "Processing SSL for: $domain"
    echo "================================"
    certbot certonly --standalone -d "$domain" --non-interactive --agree-tos -m webmaster@goreise.com

    if [ $? -eq 0 ]; then
        echo "✓ Success: $domain"
    else
        echo "✗ Failed: $domain"
    fi
done

# Step 3: Start Nginx
echo ""
echo "Starting Nginx..."
systemctl start nginx

# Step 4: Reload Nginx to apply new certificates
echo "Reloading Nginx..."
systemctl reload nginx

# Step 5: Show all certificates
echo ""
echo "=== SSL Certificates Status ==="
certbot certificates

echo ""
echo "=== Done! ==="
echo "Please test all sites:"
echo "  - https://admin.goreise.com"
echo "  - https://api.goreise.com"
echo "  - https://tour.goreise.com"
echo "  - https://vote.kinhthanhmoingay.com"
