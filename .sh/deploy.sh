#!/bin/bash

# ========================================
# LeeTour Deployment Script
# Automated deployment for Contabo VPS
# ========================================

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
APP_DIR="/var/www/leetour"
ADMIN_DIR="$APP_DIR/apps/admin"
API_DIR="$APP_DIR/apps/api"
FRONTEND_DIR="$APP_DIR/apps/frontend"
LOG_DIR="$APP_DIR/logs"
BACKUP_DIR="$APP_DIR/backups"

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

# Check if .env files exist
check_env_files() {
    print_section "Checking Environment Files"

    local missing_env=0

    if [ ! -f "$ADMIN_DIR/.env.local" ]; then
        print_error "Missing: $ADMIN_DIR/.env.local"
        missing_env=1
    else
        print_success "Found: $ADMIN_DIR/.env.local"
    fi

    if [ ! -f "$API_DIR/.env" ]; then
        print_error "Missing: $API_DIR/.env"
        missing_env=1
    else
        print_success "Found: $API_DIR/.env"
    fi

    if [ ! -f "$FRONTEND_DIR/.env" ]; then
        print_error "Missing: $FRONTEND_DIR/.env"
        missing_env=1
    else
        print_success "Found: $FRONTEND_DIR/.env"
    fi

    if [ $missing_env -eq 1 ]; then
        print_error "Missing required .env files. Please create them before deploying."
        print_info "See .env.production.example files for reference"
        exit 1
    fi
}

# Create necessary directories
create_directories() {
    print_section "Creating Directories"

    mkdir -p "$LOG_DIR"
    mkdir -p "$BACKUP_DIR"

    print_success "Directories created"
}

# Pull latest code from git
pull_latest_code() {
    print_section "Pulling Latest Code"

    cd "$APP_DIR"

    # Stash any local changes
    if [ -n "$(git status --porcelain)" ]; then
        print_info "Stashing local changes..."
        git stash
    fi

    print_info "Fetching latest changes from repository..."
    git fetch origin

    print_info "Pulling main branch..."
    git pull origin main

    print_success "Code updated to latest version"
}

# Install dependencies
install_dependencies() {
    print_section "Installing Dependencies"

    print_info "Installing admin dependencies..."
    cd "$ADMIN_DIR"
    npm install --production=false
    print_success "Admin dependencies installed"

    print_info "Installing API dependencies..."
    cd "$API_DIR"
    npm install --production=false
    print_success "API dependencies installed"

    print_info "Installing frontend dependencies..."
    cd "$FRONTEND_DIR"
    npm install --production=false
    print_success "Frontend dependencies installed"
}

# Build applications
build_applications() {
    print_section "Building Applications"

    print_info "Building admin application..."
    cd "$ADMIN_DIR"
    NODE_ENV=production npm run build
    print_success "Admin built successfully"

    print_info "Building API application..."
    cd "$API_DIR"
    NODE_ENV=production npm run build
    print_success "API built successfully"

    print_info "Building frontend application..."
    cd "$FRONTEND_DIR"
    NODE_ENV=production npm run build
    print_success "Frontend built successfully"
}

# Restart PM2 applications
restart_applications() {
    print_section "Restarting Applications"

    cd "$APP_DIR"

    # Check if PM2 is already running any apps
    if pm2 list | grep -q "leetour"; then
        print_info "Reloading existing PM2 processes..."
        pm2 reload ecosystem.config.js --env production
    else
        print_info "Starting PM2 processes for the first time..."
        pm2 start ecosystem.config.js --env production
    fi

    # Save PM2 process list
    pm2 save

    print_success "Applications restarted"

    # Show status
    echo ""
    pm2 status
    echo ""
}

# Run database migrations (if needed)
run_migrations() {
    print_section "Database Migrations"

    print_info "Checking for database migrations..."
    # Add your migration commands here if you have any
    # Example:
    # cd "$API_DIR"
    # npm run migrate

    print_info "No migrations to run (skipped)"
}

# Cleanup old files
cleanup() {
    print_section "Cleanup"

    print_info "Clearing Node.js cache..."
    cd "$APP_DIR"
    npm cache clean --force 2>/dev/null || true

    print_info "Removing old log files (older than 30 days)..."
    find "$LOG_DIR" -name "*.log" -mtime +30 -delete 2>/dev/null || true

    print_info "Removing old backups (older than 7 days)..."
    find "$BACKUP_DIR" -mtime +7 -delete 2>/dev/null || true

    print_success "Cleanup completed"
}

# Create backup before deployment
create_backup() {
    print_section "Creating Backup"

    local backup_name="backup-$(date +%Y%m%d-%H%M%S).tar.gz"
    local backup_path="$BACKUP_DIR/$backup_name"

    print_info "Creating backup: $backup_name"

    cd "$APP_DIR"
    tar -czf "$backup_path" \
        --exclude='node_modules' \
        --exclude='.next' \
        --exclude='logs' \
        --exclude='backups' \
        . 2>/dev/null || true

    if [ -f "$backup_path" ]; then
        print_success "Backup created: $backup_path"
    else
        print_info "Backup skipped (no changes or first deployment)"
    fi
}

# Health check
health_check() {
    print_section "Health Check"

    sleep 5  # Wait for apps to start

    print_info "Checking admin (port 3000)..."
    if curl -f http://localhost:3000 >/dev/null 2>&1; then
        print_success "Admin is running"
    else
        print_error "Admin is not responding"
    fi

    print_info "Checking API (port 3001)..."
    if curl -f http://localhost:3001 >/dev/null 2>&1; then
        print_success "API is running"
    else
        print_error "API is not responding"
    fi

    print_info "Checking frontend (port 3002)..."
    if curl -f http://localhost:3002 >/dev/null 2>&1; then
        print_success "Frontend is running"
    else
        print_error "Frontend is not responding"
    fi
}

# Display summary
display_summary() {
    print_section "Deployment Summary"

    echo ""
    print_info "Application Status:"
    pm2 status

    echo ""
    print_info "Memory Usage:"
    pm2 monit --lines 5

    echo ""
    print_success "Deployment completed successfully!"
    echo ""
    print_info "Useful commands:"
    echo "  • View logs: pm2 logs"
    echo "  • Monitor: pm2 monit"
    echo "  • Restart: pm2 restart all"
    echo "  • Stop: pm2 stop all"
    echo "  • Check status: pm2 status"
    echo ""
}

# Main deployment flow
main() {
    print_section "LeeTour Deployment Started"
    echo "Date: $(date)"
    echo "User: $(whoami)"
    echo "Directory: $(pwd)"
    echo ""

    # Check if application directory exists
    if [ ! -d "$APP_DIR" ]; then
        print_error "Application directory does not exist: $APP_DIR"
        print_info "Please follow these steps first:"
        echo "  1. Create the directory: sudo mkdir -p $APP_DIR"
        echo "  2. Set ownership: sudo chown -R deployer:deployer $APP_DIR"
        echo "  3. Clone your repository: git clone YOUR_REPO_URL $APP_DIR"
        echo "  4. Navigate to directory: cd $APP_DIR"
        echo "  5. Run this script again: ./deploy.sh"
        exit 1
    fi

    # Check if running from correct directory
    if [ "$(pwd)" != "$APP_DIR" ]; then
        print_info "Changing to application directory: $APP_DIR"
        cd "$APP_DIR" || {
            print_error "Failed to change to directory: $APP_DIR"
            exit 1
        }
    fi

    # Run deployment steps
    create_directories
    create_backup
    check_env_files
    pull_latest_code
    install_dependencies
    build_applications
    run_migrations
    restart_applications
    cleanup
    health_check
    display_summary
}

# Run main function
main "$@"
