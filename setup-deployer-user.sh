#!/bin/bash

# ========================================
# Setup Deployer User on LeeTour Server
# Run this script locally - it will SSH to server
# ========================================

set -e

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
RED='\033[0;31m'
NC='\033[0m'

SERVER_IP="157.173.124.250"
SSH_KEY_PATH="$HOME/.ssh/leetour_server.pub"

echo -e "${BLUE}========================================${NC}"
echo -e "${BLUE}LeeTour Deployer User Setup${NC}"
echo -e "${BLUE}========================================${NC}"
echo ""

# Check if SSH key exists
if [ ! -f "$SSH_KEY_PATH" ]; then
    echo -e "${RED}Error: SSH key not found at $SSH_KEY_PATH${NC}"
    exit 1
fi

echo -e "${GREEN}✓ SSH key found${NC}"
echo ""

# Read SSH public key
SSH_PUBLIC_KEY=$(cat "$SSH_KEY_PATH")

echo -e "${YELLOW}This script will:${NC}"
echo "  1. Connect to server as root"
echo "  2. Create/update deployer user"
echo "  3. Add your SSH key"
echo "  4. Configure sudo access"
echo ""
echo -e "${YELLOW}You will need the ROOT password${NC}"
echo ""

read -p "Press Enter to continue or Ctrl+C to cancel..."

echo ""
echo -e "${BLUE}Connecting to server as root...${NC}"
echo ""

# Create and execute setup script on server
ssh root@${SERVER_IP} << 'ENDSSH'
#!/bin/bash

set -e

echo ""
echo "========================================="
echo "Setting up deployer user..."
echo "========================================="
echo ""

# Check if deployer user exists
if id "deployer" &>/dev/null; then
    echo "✓ Deployer user already exists"
else
    echo "→ Creating deployer user..."
    useradd -m -s /bin/bash deployer
    echo "✓ Deployer user created"
fi

# Set password
echo ""
echo "========================================="
echo "Set password for deployer user"
echo "========================================="
echo "Please enter a password for the deployer user:"
passwd deployer

# Add to sudo group
echo ""
echo "→ Adding deployer to sudo group..."
usermod -aG sudo deployer

# Configure passwordless sudo (optional but convenient)
echo ""
echo "→ Configuring sudo access..."
if ! grep -q "deployer ALL=(ALL) NOPASSWD:ALL" /etc/sudoers 2>/dev/null; then
    echo "deployer ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers
    echo "✓ Passwordless sudo configured"
else
    echo "✓ Sudo already configured"
fi

# Setup SSH directory
echo ""
echo "→ Setting up SSH directory..."
mkdir -p /home/deployer/.ssh
chmod 700 /home/deployer/.ssh

echo ""
echo "✓ Setup complete on server"
echo ""
ENDSSH

# Now add SSH key
echo ""
echo -e "${BLUE}Adding your SSH key to deployer user...${NC}"
echo ""

echo "$SSH_PUBLIC_KEY" | ssh root@${SERVER_IP} "cat >> /home/deployer/.ssh/authorized_keys && chmod 600 /home/deployer/.ssh/authorized_keys && chown -R deployer:deployer /home/deployer/.ssh"

echo ""
echo -e "${GREEN}✓ SSH key added successfully${NC}"

# Test connection
echo ""
echo -e "${BLUE}Testing SSH connection as deployer...${NC}"
echo ""

if ssh -i ~/.ssh/leetour_server -o StrictHostKeyChecking=no deployer@${SERVER_IP} "echo 'SSH connection successful!' && whoami && hostname" 2>/dev/null; then
    echo ""
    echo -e "${GREEN}=========================================${NC}"
    echo -e "${GREEN}✓ Setup Complete!${NC}"
    echo -e "${GREEN}=========================================${NC}"
    echo ""
    echo "You can now connect using:"
    echo ""
    echo -e "  ${BLUE}ssh leetour${NC}"
    echo ""
    echo "Or:"
    echo ""
    echo -e "  ${BLUE}ssh deployer@${SERVER_IP}${NC}"
    echo ""
else
    echo ""
    echo -e "${RED}Warning: Could not verify SSH connection${NC}"
    echo "Please test manually: ssh deployer@${SERVER_IP}"
    echo ""
fi
