#!/bin/bash

# ========================================
# Server Connection Helper Script
# Quick SSH access to LeeTour server
# ========================================

# Server configuration
SERVER_IP="157.173.124.250"
SERVER_USER="deployer"
APP_DIR="/var/www/leetour"

# Colors
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
BLUE='\033[0;34m'
NC='\033[0m'

print_header() {
    echo -e "${BLUE}========================================${NC}"
    echo -e "${BLUE}LeeTour Server Connection${NC}"
    echo -e "${BLUE}========================================${NC}"
}

print_menu() {
    echo ""
    echo -e "${YELLOW}Choose an action:${NC}"
    echo "1. Connect to server"
    echo "2. Connect and deploy"
    echo "3. View PM2 status"
    echo "4. View logs"
    echo "5. Restart applications"
    echo "6. Copy environment files to server"
    echo "7. Exit"
    echo ""
}

connect_server() {
    echo -e "${GREEN}Connecting to server...${NC}"
    ssh ${SERVER_USER}@${SERVER_IP}
}

deploy_app() {
    echo -e "${GREEN}Deploying application...${NC}"
    ssh ${SERVER_USER}@${SERVER_IP} "cd ${APP_DIR} && ./deploy.sh"
}

view_status() {
    echo -e "${GREEN}Fetching PM2 status...${NC}"
    ssh ${SERVER_USER}@${SERVER_IP} "pm2 status"
}

view_logs() {
    echo ""
    echo "Which logs do you want to view?"
    echo "1. All logs"
    echo "2. Admin logs"
    echo "3. API logs"
    echo "4. Frontend logs"
    echo ""
    read -p "Enter choice: " log_choice

    case $log_choice in
        1)
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 logs --lines 50"
            ;;
        2)
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 logs leetour-admin --lines 50"
            ;;
        3)
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 logs leetour-api --lines 50"
            ;;
        4)
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 logs leetour-frontend --lines 50"
            ;;
        *)
            echo "Invalid choice"
            ;;
    esac
}

restart_apps() {
    echo ""
    echo "Which application to restart?"
    echo "1. All applications"
    echo "2. Admin only"
    echo "3. API only"
    echo "4. Frontend only"
    echo ""
    read -p "Enter choice: " restart_choice

    case $restart_choice in
        1)
            echo -e "${GREEN}Restarting all applications...${NC}"
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 restart all"
            ;;
        2)
            echo -e "${GREEN}Restarting admin...${NC}"
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 restart leetour-admin"
            ;;
        3)
            echo -e "${GREEN}Restarting API...${NC}"
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 restart leetour-api"
            ;;
        4)
            echo -e "${GREEN}Restarting frontend...${NC}"
            ssh ${SERVER_USER}@${SERVER_IP} "pm2 restart leetour-frontend"
            ;;
        *)
            echo "Invalid choice"
            ;;
    esac
}

copy_env_files() {
    echo -e "${GREEN}Copying environment files to server...${NC}"

    # Check if env files exist locally
    if [ ! -f "./apps/admin/.env.local" ]; then
        echo -e "${YELLOW}Warning: apps/admin/.env.local not found${NC}"
    else
        scp ./apps/admin/.env.local ${SERVER_USER}@${SERVER_IP}:${APP_DIR}/apps/admin/
        echo "✓ Admin .env.local copied"
    fi

    if [ ! -f "./apps/api/.env" ]; then
        echo -e "${YELLOW}Warning: apps/api/.env not found${NC}"
    else
        scp ./apps/api/.env ${SERVER_USER}@${SERVER_IP}:${APP_DIR}/apps/api/
        echo "✓ API .env copied"
    fi

    if [ ! -f "./apps/frontend/.env" ]; then
        echo -e "${YELLOW}Warning: apps/frontend/.env not found${NC}"
    else
        scp ./apps/frontend/.env ${SERVER_USER}@${SERVER_IP}:${APP_DIR}/apps/frontend/
        echo "✓ Frontend .env copied"
    fi

    echo -e "${GREEN}Environment files copied successfully${NC}"
}

# Main script
main() {
    print_header

    # Check if SSH key exists
    if [ ! -f ~/.ssh/id_rsa ]; then
        echo -e "${YELLOW}Warning: SSH key not found at ~/.ssh/id_rsa${NC}"
        echo "You may need to enter password for each connection"
        echo ""
        echo "To generate SSH key, run:"
        echo "  ssh-keygen -t rsa -b 4096 -C \"your_email@example.com\""
        echo ""
    fi

    while true; do
        print_menu
        read -p "Enter your choice [1-7]: " choice

        case $choice in
            1)
                connect_server
                ;;
            2)
                deploy_app
                ;;
            3)
                view_status
                ;;
            4)
                view_logs
                ;;
            5)
                restart_apps
                ;;
            6)
                copy_env_files
                ;;
            7)
                echo -e "${GREEN}Goodbye!${NC}"
                exit 0
                ;;
            *)
                echo -e "${YELLOW}Invalid option. Please choose 1-7${NC}"
                ;;
        esac

        echo ""
        read -p "Press Enter to continue..."
    done
}

# Run main function
main "$@"
