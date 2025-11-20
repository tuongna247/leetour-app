#!/bin/bash

# ========================================
# SSH Setup Script for LeeTour Server
# ========================================

set -e

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m'

SERVER_IP="157.173.124.250"
SERVER_USER="deployer"

print_header() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}LeeTour Server SSH Setup${NC}"
    echo -e "${BLUE}========================================${NC}"
    echo ""
}

print_success() {
    echo -e "${GREEN}✓ $1${NC}"
}

print_error() {
    echo -e "${RED}✗ $1${NC}"
}

print_info() {
    echo -e "${YELLOW}→ $1${NC}"
}

print_header

# Check if SSH key exists
if [ ! -f ~/.ssh/leetour_server ]; then
    print_error "SSH key not found!"
    echo ""
    echo "Run this command first to generate the key:"
    echo "ssh-keygen -t rsa -b 4096 -f ~/.ssh/leetour_server -C \"leetour-deployment\""
    exit 1
fi

print_success "SSH key found: ~/.ssh/leetour_server"

# Display public key
echo ""
print_info "Your SSH public key:"
echo "─────────────────────────────────────────────────────────"
cat ~/.ssh/leetour_server.pub
echo "─────────────────────────────────────────────────────────"
echo ""

# Test if we can connect with password first
print_info "Testing server connection..."
echo ""

# Try to copy SSH key to server
print_info "Attempting to copy SSH key to server..."
echo ""
echo "You will be prompted for the server password."
echo "If you don't have the password, copy the public key manually (shown above)."
echo ""

read -p "Do you want to copy the SSH key to the server now? (y/n): " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    # Use ssh-copy-id if available, otherwise manual
    if command -v ssh-copy-id &> /dev/null; then
        print_info "Using ssh-copy-id..."
        ssh-copy-id -i ~/.ssh/leetour_server.pub ${SERVER_USER}@${SERVER_IP}
    else
        print_info "Copying key manually..."
        cat ~/.ssh/leetour_server.pub | ssh ${SERVER_USER}@${SERVER_IP} "mkdir -p ~/.ssh && chmod 700 ~/.ssh && cat >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"
    fi

    if [ $? -eq 0 ]; then
        print_success "SSH key copied successfully!"
    else
        print_error "Failed to copy SSH key"
        echo ""
        echo "Manual setup instructions:"
        echo "1. Copy the public key shown above"
        echo "2. Connect to server: ssh ${SERVER_USER}@${SERVER_IP}"
        echo "3. Run: mkdir -p ~/.ssh && nano ~/.ssh/authorized_keys"
        echo "4. Paste the public key"
        echo "5. Save and exit (Ctrl+X, Y, Enter)"
        echo "6. Run: chmod 600 ~/.ssh/authorized_keys"
        exit 1
    fi
else
    print_info "Manual setup required"
    echo ""
    echo "To manually add your SSH key to the server:"
    echo ""
    echo "1. Copy the public key shown above"
    echo "2. Connect to server: ssh ${SERVER_USER}@${SERVER_IP}"
    echo "3. Run: mkdir -p ~/.ssh && nano ~/.ssh/authorized_keys"
    echo "4. Paste the public key at the end of the file"
    echo "5. Save and exit (Ctrl+X, Y, Enter)"
    echo "6. Run: chmod 600 ~/.ssh/authorized_keys"
    echo "7. Exit and run this script again"
    echo ""
    exit 0
fi

# Test connection
echo ""
print_info "Testing SSH connection..."
if ssh -i ~/.ssh/leetour_server -o ConnectTimeout=5 ${SERVER_USER}@${SERVER_IP} "echo 'Connection successful!'" 2>/dev/null; then
    print_success "SSH connection works!"
else
    print_error "SSH connection failed"
    echo "Please check your setup and try again"
    exit 1
fi

# Setup SSH config
echo ""
print_info "Setting up SSH config for easy access..."

SSH_CONFIG="$HOME/.ssh/config"

# Backup existing config if it exists
if [ -f "$SSH_CONFIG" ]; then
    cp "$SSH_CONFIG" "$SSH_CONFIG.backup.$(date +%Y%m%d-%H%M%S)"
    print_info "Backed up existing SSH config"
fi

# Check if leetour config already exists
if grep -q "Host leetour" "$SSH_CONFIG" 2>/dev/null; then
    print_info "LeeTour SSH config already exists, updating..."
    # Remove old config
    sed -i '/Host leetour/,/^$/d' "$SSH_CONFIG"
fi

# Add new config
cat >> "$SSH_CONFIG" << EOF

# LeeTour Server Configuration
Host leetour
    HostName ${SERVER_IP}
    User ${SERVER_USER}
    IdentityFile ~/.ssh/leetour_server
    IdentitiesOnly yes
    ServerAliveInterval 60
    ServerAliveCountMax 3

EOF

print_success "SSH config updated!"

echo ""
echo -e "${GREEN}========================================${NC}"
echo -e "${GREEN}SSH Setup Complete!${NC}"
echo -e "${GREEN}========================================${NC}"
echo ""
echo "You can now connect to the server using:"
echo ""
echo "  ${BLUE}ssh leetour${NC}"
echo ""
echo "Or the full command:"
echo ""
echo "  ${BLUE}ssh -i ~/.ssh/leetour_server ${SERVER_USER}@${SERVER_IP}${NC}"
echo ""
echo "To test the connection now, run:"
echo ""
echo "  ${BLUE}ssh leetour${NC}"
echo ""

# Ask if user wants to connect now
read -p "Do you want to connect to the server now? (y/n): " -n 1 -r
echo ""

if [[ $REPLY =~ ^[Yy]$ ]]; then
    print_info "Connecting to server..."
    ssh leetour
fi
