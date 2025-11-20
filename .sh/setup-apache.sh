#!/bin/bash

# ========================================
# Apache Virtual Host Setup Script
# Configures reverse proxy for LeeTour apps
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
    echo "Usage: sudo bash setup-apache.sh"
    exit 1
fi

print_section "Apache Virtual Host Setup for LeeTour"

# Enable required Apache modules
print_section "Enabling Apache Modules"
print_info "Enabling proxy module..."
a2enmod proxy
print_info "Enabling proxy_http module..."
a2enmod proxy_http
print_info "Enabling headers module..."
a2enmod headers
print_success "Apache modules enabled"

# Create virtual host for admin.goreise.com
print_section "Creating Admin Virtual Host"
cat > /etc/apache2/sites-available/admin.goreise.com.conf <<'EOF'
<VirtualHost *:80>
    ServerName admin.goreise.com
    ServerAdmin webmaster@goreise.com

    # Reverse proxy to Next.js app on port 3000
    ProxyPreserveHost On
    ProxyPass / http://localhost:3000/
    ProxyPassReverse / http://localhost:3000/

    # WebSocket support for Next.js hot reload
    ProxyPass /ws ws://localhost:3000/ws
    ProxyPassReverse /ws ws://localhost:3000/ws

    # Logging
    ErrorLog ${APACHE_LOG_DIR}/admin.goreise.com-error.log
    CustomLog ${APACHE_LOG_DIR}/admin.goreise.com-access.log combined
</VirtualHost>
EOF
print_success "Created /etc/apache2/sites-available/admin.goreise.com.conf"

# Create virtual host for api.goreise.com
print_section "Creating API Virtual Host"
cat > /etc/apache2/sites-available/api.goreise.com.conf <<'EOF'
<VirtualHost *:80>
    ServerName api.goreise.com
    ServerAdmin webmaster@goreise.com

    # Reverse proxy to Next.js API on port 3001
    ProxyPreserveHost On
    ProxyPass / http://localhost:3001/
    ProxyPassReverse / http://localhost:3001/

    # WebSocket support
    ProxyPass /ws ws://localhost:3001/ws
    ProxyPassReverse /ws ws://localhost:3001/ws

    # Logging
    ErrorLog ${APACHE_LOG_DIR}/api.goreise.com-error.log
    CustomLog ${APACHE_LOG_DIR}/api.goreise.com-access.log combined
</VirtualHost>
EOF
print_success "Created /etc/apache2/sites-available/api.goreise.com.conf"

# Create virtual host for tour.goreise.com (frontend)
print_section "Creating Frontend Virtual Host"
cat > /etc/apache2/sites-available/tour.goreise.com.conf <<'EOF'
<VirtualHost *:80>
    ServerName tour.goreise.com
    ServerAlias www.tour.goreise.com
    ServerAdmin webmaster@goreise.com

    # Reverse proxy to Next.js frontend on port 3002
    ProxyPreserveHost On
    ProxyPass / http://localhost:3002/
    ProxyPassReverse / http://localhost:3002/

    # WebSocket support for Next.js hot reload
    ProxyPass /ws ws://localhost:3002/ws
    ProxyPassReverse /ws ws://localhost:3002/ws

    # Logging
    ErrorLog ${APACHE_LOG_DIR}/tour.goreise.com-error.log
    CustomLog ${APACHE_LOG_DIR}/tour.goreise.com-access.log combined
</VirtualHost>
EOF
print_success "Created /etc/apache2/sites-available/tour.goreise.com.conf"

# Enable the sites
print_section "Enabling Virtual Hosts"
print_info "Enabling admin.goreise.com..."
a2ensite admin.goreise.com.conf
print_info "Enabling api.goreise.com..."
a2ensite api.goreise.com.conf
print_info "Enabling tour.goreise.com..."
a2ensite tour.goreise.com.conf
print_success "Virtual hosts enabled"

# Disable default site
print_section "Disabling Default Site"
print_info "Disabling default Apache site..."
a2dissite 000-default.conf 2>/dev/null || print_info "Default site already disabled"
print_success "Default site disabled"

# Test Apache configuration
print_section "Testing Apache Configuration"
if apache2ctl configtest 2>&1 | grep -q "Syntax OK"; then
    print_success "Apache configuration is valid"
else
    print_error "Apache configuration has errors"
    apache2ctl configtest
    exit 1
fi

# Restart Apache
print_section "Restarting Apache"
print_info "Restarting Apache service..."
systemctl restart apache2
print_success "Apache restarted successfully"

# Display summary
print_section "Setup Complete!"
echo ""
print_info "Virtual Hosts Configured:"
echo "  • http://admin.goreise.com → localhost:3000 (Admin Panel)"
echo "  • http://api.goreise.com → localhost:3001 (API Server)"
echo "  • http://tour.goreise.com → localhost:3002 (Frontend)"
echo ""
print_info "Apache Status:"
systemctl status apache2 --no-pager -l | head -5
echo ""
print_info "Next Steps:"
echo "  1. Ensure your DNS records point to this server's IP"
echo "  2. Test the domains in your browser"
echo "  3. Set up SSL certificates with: sudo certbot --apache"
echo ""
print_success "Apache setup completed successfully!"
