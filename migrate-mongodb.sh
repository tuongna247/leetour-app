#!/bin/bash

# ========================================
# MongoDB Migration Script
# Backup from Atlas and Restore to Local
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
    echo "Usage: sudo bash migrate-mongodb.sh"
    exit 1
fi

print_section "MongoDB Migration - Atlas to Local"

# MongoDB Atlas connection string
ATLAS_URI="mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0"
LOCAL_URI="mongodb://localhost:27017/leetour"
BACKUP_DIR="/tmp/leetour-backup"

# Step 1: Install MongoDB Database Tools
print_section "Installing MongoDB Database Tools"
print_info "Updating package list..."
apt update -qq

print_info "Installing mongodb-database-tools..."
if ! command -v mongodump &> /dev/null; then
    apt install -y mongodb-database-tools
    print_success "MongoDB database tools installed"
else
    print_success "MongoDB database tools already installed"
fi

# Verify installation
MONGODUMP_VERSION=$(mongodump --version | head -1)
print_success "mongodump version: $MONGODUMP_VERSION"

# Step 2: Install MongoDB Server
print_section "Installing MongoDB Server"

# Check if MongoDB is already installed
if command -v mongod &> /dev/null; then
    print_success "MongoDB is already installed"
    MONGOD_VERSION=$(mongod --version | head -1)
    print_success "$MONGOD_VERSION"
else
    print_info "Installing MongoDB 7.0..."

    # Import MongoDB GPG key
    print_info "Importing MongoDB GPG key..."
    curl -fsSL https://www.mongodb.org/static/pgp/server-7.0.asc | gpg --dearmor -o /usr/share/keyrings/mongodb-server-7.0.gpg

    # Add MongoDB repository
    print_info "Adding MongoDB repository..."
    echo "deb [ arch=amd64,arm64 signed-by=/usr/share/keyrings/mongodb-server-7.0.gpg ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/7.0 multiverse" | tee /etc/apt/sources.list.d/mongodb-org-7.0.list

    # Update package list
    print_info "Updating package list..."
    apt update -qq

    # Install MongoDB
    print_info "Installing MongoDB packages..."
    apt install -y mongodb-org

    print_success "MongoDB installed successfully"
fi

# Step 3: Start MongoDB Service
print_section "Starting MongoDB Service"

print_info "Starting MongoDB..."
systemctl start mongod

print_info "Enabling MongoDB to start on boot..."
systemctl enable mongod

sleep 3

# Check if MongoDB is running
if systemctl is-active --quiet mongod; then
    print_success "MongoDB is running"
else
    print_error "MongoDB failed to start"
    print_info "Checking logs..."
    journalctl -u mongod -n 20 --no-pager
    exit 1
fi

# Verify MongoDB is listening
if netstat -tlnp | grep -q ":27017"; then
    print_success "MongoDB is listening on port 27017"
else
    print_error "MongoDB is not listening on port 27017"
    exit 1
fi

# Step 4: Backup from MongoDB Atlas
print_section "Backing up from MongoDB Atlas"

print_info "Creating backup directory..."
rm -rf "$BACKUP_DIR"
mkdir -p "$BACKUP_DIR"

print_info "Downloading data from MongoDB Atlas..."
print_info "This may take a few minutes depending on database size..."

if mongodump --uri="$ATLAS_URI" --out="$BACKUP_DIR"; then
    print_success "Backup completed successfully"

    # Show backup size
    BACKUP_SIZE=$(du -sh "$BACKUP_DIR" | cut -f1)
    print_info "Backup size: $BACKUP_SIZE"
else
    print_error "Backup failed"
    exit 1
fi

# Step 5: Restore to Local MongoDB
print_section "Restoring to Local MongoDB"

print_info "Restoring database to local MongoDB..."
print_info "Database: leetour"

if mongorestore --uri="$LOCAL_URI" --drop "$BACKUP_DIR/leetour"; then
    print_success "Database restored successfully"
else
    print_error "Restore failed"
    exit 1
fi

# Step 6: Verify Restoration
print_section "Verifying Database Restoration"

print_info "Connecting to local MongoDB to verify..."

# Count documents in collections
TOURS_COUNT=$(mongosh --quiet --eval "db.getSiblingDB('leetour').tours.countDocuments()" 2>/dev/null || echo "0")
BOOKINGS_COUNT=$(mongosh --quiet --eval "db.getSiblingDB('leetour').bookings.countDocuments()" 2>/dev/null || echo "0")
USERS_COUNT=$(mongosh --quiet --eval "db.getSiblingDB('leetour').users.countDocuments()" 2>/dev/null || echo "0")

print_success "Tours: $TOURS_COUNT documents"
print_success "Bookings: $BOOKINGS_COUNT documents"
print_success "Users: $USERS_COUNT documents"

# Step 7: Update Application Configuration
print_section "Updating Application Configuration"

# Update API .env
API_ENV_FILE="/var/www/leetour/apps/api/.env"
if [ -f "$API_ENV_FILE" ]; then
    print_info "Updating API .env file..."
    sed -i 's|MONGODB_URI=mongodb+srv://.*|MONGODB_URI=mongodb://localhost:27017/leetour|g' "$API_ENV_FILE"
    print_success "API .env updated"
else
    print_error "API .env file not found: $API_ENV_FILE"
fi

# Update Admin .env.local
ADMIN_ENV_FILE="/var/www/leetour/apps/admin/.env.local"
if [ -f "$ADMIN_ENV_FILE" ]; then
    print_info "Updating Admin .env.local file..."
    sed -i 's|MONGODB_URI=mongodb+srv://.*|MONGODB_URI=mongodb://localhost:27017/leetour|g' "$ADMIN_ENV_FILE"
    print_success "Admin .env.local updated"
else
    print_error "Admin .env.local file not found: $ADMIN_ENV_FILE"
fi

# Step 8: Restart Applications
print_section "Restarting Applications"

print_info "Restarting PM2 applications..."
if command -v pm2 &> /dev/null; then
    # Find the actual user running PM2
    PM2_USER=$(ps aux | grep "PM2" | grep -v grep | awk '{print $1}' | head -1)

    if [ -n "$PM2_USER" ]; then
        print_info "Restarting as user: $PM2_USER"
        sudo -u "$PM2_USER" pm2 restart all
        print_success "PM2 applications restarted"

        sleep 3

        print_info "PM2 Status:"
        sudo -u "$PM2_USER" pm2 status
    else
        print_error "Could not find PM2 user"
    fi
else
    print_error "PM2 not found"
fi

# Step 9: Cleanup
print_section "Cleanup"

print_info "Removing backup files..."
rm -rf "$BACKUP_DIR"
print_success "Backup files cleaned up"

# Final Summary
print_section "Migration Complete!"
echo ""
print_success "MongoDB has been successfully migrated from Atlas to local server"
echo ""
print_info "Summary:"
echo "  • MongoDB Server: Running on localhost:27017"
echo "  • Database Name: leetour"
echo "  • Collections restored: $TOURS_COUNT tours, $BOOKINGS_COUNT bookings, $USERS_COUNT users"
echo "  • Application configs updated"
echo "  • PM2 services restarted"
echo ""
print_info "Next Steps:"
echo "  1. Test the admin dashboard: http://admin.goreise.com/dashboard-main"
echo "  2. Test the API: http://api.goreise.com/api/tours"
echo "  3. Test the frontend: http://tour.goreise.com/tours"
echo "  4. Monitor PM2 logs: pm2 logs"
echo ""
print_info "Backup recommendations:"
echo "  • Set up regular backups with: /home/deployer/backup-mongodb.sh"
echo "  • Keep MongoDB Atlas as a backup option"
echo "  • Monitor disk space on the server"
echo ""
print_success "All done! Your local MongoDB is now the primary database."
