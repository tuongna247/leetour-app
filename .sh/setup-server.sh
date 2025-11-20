#!/bin/bash

# ========================================
# LeeTour Server Setup Script
# For Ubuntu/Debian on Contabo VPS
# ========================================

set -e  # Exit on any error

echo "========================================="
echo "LeeTour Server Setup - Starting..."
echo "========================================="

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
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

# Check if running as root or with sudo
if [[ $EUID -ne 0 ]]; then
   print_error "This script must be run as root or with sudo"
   exit 1
fi

print_info "Updating system packages..."
apt-get update
apt-get upgrade -y
print_success "System packages updated"

# Install basic dependencies
print_info "Installing basic dependencies..."
apt-get install -y curl wget git build-essential software-properties-common ufw fail2ban
print_success "Basic dependencies installed"

# Install Node.js 20.x LTS (recommended for production)
print_info "Installing Node.js 20.x LTS..."
curl -fsSL https://deb.nodesource.com/setup_20.x | bash -
apt-get install -y nodejs
print_success "Node.js installed: $(node --version)"
print_success "NPM installed: $(npm --version)"

# Install PM2 globally
print_info "Installing PM2 process manager..."
npm install -g pm2
pm2 completion install
print_success "PM2 installed: $(pm2 --version)"

# Install Nginx
print_info "Installing Nginx..."
apt-get install -y nginx
systemctl enable nginx
systemctl start nginx
print_success "Nginx installed and started"

# Setup firewall (UFW)
print_info "Configuring firewall..."
ufw --force enable
ufw default deny incoming
ufw default allow outgoing
ufw allow ssh
ufw allow 'Nginx Full'
ufw allow 80/tcp
ufw allow 443/tcp
print_success "Firewall configured"

# Configure fail2ban for SSH protection
print_info "Configuring fail2ban..."
systemctl enable fail2ban
systemctl start fail2ban
cat > /etc/fail2ban/jail.local << EOF
[sshd]
enabled = true
port = ssh
filter = sshd
logpath = /var/log/auth.log
maxretry = 3
bantime = 3600
EOF
systemctl restart fail2ban
print_success "Fail2ban configured"

# Create deployment user (non-root)
print_info "Creating deployment user..."
if id "deployer" &>/dev/null; then
    print_info "User 'deployer' already exists"
else
    useradd -m -s /bin/bash deployer
    usermod -aG sudo deployer
    print_success "User 'deployer' created"
    print_info "Please set password for deployer user:"
    passwd deployer
fi

# Create application directory
print_info "Creating application directory..."
mkdir -p /var/www/leetour
chown -R deployer:deployer /var/www/leetour
print_success "Application directory created: /var/www/leetour"

# Create logs directory
print_info "Creating logs directory..."
mkdir -p /var/www/leetour/logs
chown -R deployer:deployer /var/www/leetour/logs
print_success "Logs directory created"

# Setup PM2 startup script
print_info "Setting up PM2 startup..."
su - deployer -c "pm2 startup systemd -u deployer --hp /home/deployer" | grep "sudo" | bash
print_success "PM2 startup configured"

# Install Certbot for SSL (Let's Encrypt)
print_info "Installing Certbot for SSL certificates..."
apt-get install -y certbot python3-certbot-nginx
print_success "Certbot installed"

# Optimize Node.js for production
print_info "Configuring system limits for Node.js..."
cat >> /etc/security/limits.conf << EOF
deployer soft nofile 65536
deployer hard nofile 65536
deployer soft nproc 65536
deployer hard nproc 65536
EOF
print_success "System limits configured"

# Enable swap (if not exists) - useful for servers with limited RAM
print_info "Checking swap space..."
if [ $(swapon --show | wc -l) -eq 0 ]; then
    print_info "Creating 2GB swap file..."
    fallocate -l 2G /swapfile
    chmod 600 /swapfile
    mkswap /swapfile
    swapon /swapfile
    echo '/swapfile none swap sw 0 0' >> /etc/fstab
    print_success "Swap file created and enabled"
else
    print_info "Swap already exists"
fi

# Install and configure logrotate for application logs
print_info "Configuring log rotation..."
cat > /etc/logrotate.d/leetour << EOF
/var/www/leetour/logs/*.log {
    daily
    missingok
    rotate 14
    compress
    delaycompress
    notifempty
    create 0640 deployer deployer
    sharedscripts
    postrotate
        pm2 reloadLogs
    endscript
}
EOF
print_success "Log rotation configured"

# Create initial Nginx config placeholder
print_info "Setting up Nginx configuration..."
cat > /etc/nginx/sites-available/default << 'EOF'
server {
    listen 80 default_server;
    listen [::]:80 default_server;
    server_name _;

    location / {
        return 200 'LeeTour Server - Nginx is running. Please configure your application.';
        add_header Content-Type text/plain;
    }
}
EOF
systemctl reload nginx
print_success "Default Nginx configuration set"

echo ""
echo "========================================="
echo "Server Setup Complete!"
echo "========================================="
echo ""
print_success "Node.js version: $(node --version)"
print_success "NPM version: $(npm --version)"
print_success "PM2 version: $(pm2 --version)"
print_success "Nginx status: $(systemctl is-active nginx)"
echo ""
print_info "Next steps:"
echo "  1. Switch to deployer user: su - deployer"
echo "  2. Clone your repository to /var/www/leetour"
echo "  3. Configure environment variables (.env files)"
echo "  4. Run the deployment script: ./deploy.sh"
echo "  5. Configure Nginx with your domains"
echo "  6. Setup SSL with: sudo certbot --nginx"
echo ""
print_info "Security recommendations:"
echo "  • Change SSH port in /etc/ssh/sshd_config"
echo "  • Disable password authentication (use SSH keys)"
echo "  • Configure regular backups"
echo "  • Setup monitoring (e.g., Netdata, htop)"
echo ""
echo "========================================="
