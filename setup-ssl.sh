#!/bin/bash

# ========================================
# SSL Certificate Setup Script
# Install Let's Encrypt SSL certificates
# ========================================

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Function to print colored output
print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}→ $1${NC}"
}

print_section() {
    echo -e "${BLUE}=========================================${NC}"
    echo -e "${BLUE}$1${NC}"
    echo -e "${BLUE}=========================================${NC}"
}

# Check if running as root or with sudo
if [ "$EUID" -ne 0 ]; then
    print_error "Please run this script with sudo"
    echo "Usage: sudo bash setup-ssl.sh"
    exit 1
fi

print_section "SSL Certificate Setup for LeeTour"

# Domains to secure
DOMAINS=(
    "admin.goreise.com"
    "api.goreise.com"
    "tour.goreise.com"
)

# Email for Let's Encrypt notifications
EMAIL="webmaster@goreise.com"

# Step 1: Install Certbot
print_section "Installing Certbot"

print_info "Updating package list..."
apt update -qq

print_info "Installing certbot and apache plugin..."
apt install -y certbot python3-certbot-apache

CERTBOT_VERSION=$(certbot --version 2>&1)
print_success "$CERTBOT_VERSION"

# Step 2: Verify Apache Virtual Hosts
print_section "Verifying Apache Configuration"

for domain in "${DOMAINS[@]}"; do
    if [ -f "/etc/apache2/sites-available/${domain}.conf" ]; then
        print_success "Found: ${domain}.conf"
    else
        print_error "Missing: ${domain}.conf"
        print_info "Please run setup-apache.sh first"
        exit 1
    fi
done

# Step 3: Test Apache Configuration
print_section "Testing Apache Configuration"

if apache2ctl configtest 2>&1 | grep -q "Syntax OK"; then
    print_success "Apache configuration is valid"
else
    print_error "Apache configuration has errors"
    apache2ctl configtest
    exit 1
fi

# Step 4: Verify DNS Resolution
print_section "Verifying DNS Resolution"

for domain in "${DOMAINS[@]}"; do
    print_info "Checking DNS for ${domain}..."

    if host "${domain}" > /dev/null 2>&1; then
        IP=$(host "${domain}" | grep "has address" | awk '{print $4}' | head -1)
        print_success "${domain} → ${IP}"
    else
        print_error "${domain} - DNS not resolved"
        print_info "Please ensure DNS records point to this server"
    fi
done

echo ""
read -p "Do all domains point to this server? (yes/no): " confirm

if [ "$confirm" != "yes" ]; then
    print_error "Please configure DNS first before continuing"
    exit 1
fi

# Step 5: Obtain SSL Certificates
print_section "Obtaining SSL Certificates"

print_info "This will obtain certificates for:"
for domain in "${DOMAINS[@]}"; do
    echo "  • ${domain}"
done
echo ""

# Obtain certificates for all domains
print_info "Running Certbot for Apache..."

if certbot --apache \
    --non-interactive \
    --agree-tos \
    --email "${EMAIL}" \
    --redirect \
    --expand \
    -d admin.goreise.com \
    -d api.goreise.com \
    -d tour.goreise.com; then

    print_success "SSL certificates obtained successfully!"
else
    print_error "Failed to obtain SSL certificates"
    print_info "Common issues:"
    echo "  1. DNS not pointing to this server"
    echo "  2. Port 80/443 not accessible from internet"
    echo "  3. Firewall blocking connections"
    exit 1
fi

# Step 6: Verify SSL Installation
print_section "Verifying SSL Installation"

for domain in "${DOMAINS[@]}"; do
    if [ -f "/etc/apache2/sites-available/${domain}-le-ssl.conf" ]; then
        print_success "${domain} - SSL configured"
    else
        print_error "${domain} - SSL configuration not found"
    fi
done

# Step 7: Test Auto-Renewal
print_section "Testing Certificate Auto-Renewal"

print_info "Testing renewal process (dry-run)..."
if certbot renew --dry-run; then
    print_success "Auto-renewal test passed"
else
    print_error "Auto-renewal test failed"
fi

# Step 8: Configure Apache SSL Settings (Optional Security Headers)
print_section "Configuring SSL Security Headers"

SSL_CONF="/etc/apache2/conf-available/ssl-security.conf"

cat > "$SSL_CONF" <<'EOF'
# SSL Security Headers
<IfModule mod_headers.c>
    Header always set Strict-Transport-Security "max-age=31536000; includeSubDomains"
    Header always set X-Frame-Options "SAMEORIGIN"
    Header always set X-Content-Type-Options "nosniff"
    Header always set X-XSS-Protection "1; mode=block"
</IfModule>

# SSL Protocol and Cipher Configuration
SSLProtocol all -SSLv2 -SSLv3 -TLSv1 -TLSv1.1
SSLCipherSuite HIGH:!aNULL:!MD5:!3DES
SSLHonorCipherOrder on
EOF

a2enconf ssl-security
print_success "SSL security headers configured"

# Step 9: Restart Apache
print_section "Restarting Apache"

systemctl restart apache2
print_success "Apache restarted"

# Step 10: Display Certificate Information
print_section "SSL Certificate Information"

for domain in "${DOMAINS[@]}"; do
    echo ""
    print_info "Certificate for: ${domain}"
    certbot certificates -d "${domain}" 2>/dev/null | grep -A 5 "Certificate Name: ${domain}" || true
done

# Final Summary
print_section "SSL Setup Complete!"
echo ""
print_success "HTTPS is now enabled for all domains:"
echo ""
echo "  🔒 https://admin.goreise.com"
echo "  🔒 https://api.goreise.com"
echo "  🔒 https://tour.goreise.com"
echo ""
print_info "Certificate Details:"
echo "  • Provider: Let's Encrypt"
echo "  • Validity: 90 days"
echo "  • Auto-renewal: Enabled (certbot timer)"
echo "  • Renewal check: Twice daily"
echo ""
print_info "Certificate Renewal:"
echo "  • Automatic: certbot systemd timer"
echo "  • Manual test: sudo certbot renew --dry-run"
echo "  • Force renewal: sudo certbot renew --force-renewal"
echo ""
print_info "Certificate Locations:"
echo "  • Certificates: /etc/letsencrypt/live/[domain]/"
echo "  • Apache configs: /etc/apache2/sites-available/*-le-ssl.conf"
echo ""
print_info "Next Steps:"
echo "  1. Test HTTPS access in your browser"
echo "  2. Update application .env files to use HTTPS URLs"
echo "  3. Update NEXTAUTH_URL to use https://"
echo "  4. Clear browser cache if needed"
echo ""
print_success "All done! Your sites are now secured with HTTPS!"
