#!/bin/bash

# ========================================
# Reset Deployer User Access
# Run this script as ROOT user on the server
# ========================================

set -e

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

echo "========================================="
echo "Reset Deployer User Access"
echo "========================================="
echo ""

# Check if running as root
if [ "$EUID" -ne 0 ]; then
    echo -e "${RED}ERROR: This script must be run as root${NC}"
    echo "Please run: sudo bash reset-deployer-access.sh"
    exit 1
fi

echo -e "${GREEN}Running as root user${NC}"
echo ""

# Check if deployer user exists
if id "deployer" &>/dev/null; then
    echo -e "${GREEN}Deployer user exists${NC}"
else
    echo -e "${YELLOW}Creating deployer user...${NC}"
    useradd -m -s /bin/bash deployer
    usermod -aG sudo deployer
    echo -e "${GREEN}Deployer user created${NC}"
fi

# Reset password
echo ""
echo "========================================="
echo "Setting new password for deployer user"
echo "========================================="
echo ""
passwd deployer

# Setup SSH directory
echo ""
echo -e "${YELLOW}Setting up SSH directory...${NC}"
mkdir -p /home/deployer/.ssh
chmod 700 /home/deployer/.ssh

# Add SSH public key
echo ""
echo "========================================="
echo "Add SSH Public Key"
echo "========================================="
echo ""
echo "Paste your SSH public key (from ~/.ssh/leetour_server.pub)"
echo "Then press Ctrl+D when done:"
echo ""

cat >> /home/deployer/.ssh/authorized_keys

# Set proper permissions
chmod 600 /home/deployer/.ssh/authorized_keys
chown -R deployer:deployer /home/deployer/.ssh

echo ""
echo -e "${GREEN}SSH key added successfully${NC}"

# Ensure deployer has sudo access
echo ""
echo -e "${YELLOW}Configuring sudo access...${NC}"
if ! grep -q "deployer ALL=(ALL) NOPASSWD:ALL" /etc/sudoers; then
    echo "deployer ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers
fi

echo ""
echo "========================================="
echo -e "${GREEN}Setup Complete!${NC}"
echo "========================================="
echo ""
echo "Deployer user is now configured with:"
echo "  • New password (that you just set)"
echo "  • SSH key access"
echo "  • Sudo privileges"
echo ""
echo "You can now connect using:"
echo "  ssh deployer@157.173.124.250"
echo "  or: ssh leetour"
echo ""
