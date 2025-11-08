#!/bin/bash

# MongoDB Remote Access Setup Script
# Run this on your Ubuntu server to allow remote connections

echo "================================================"
echo "MongoDB Remote Access Setup"
echo "================================================"

# Backup original config
echo "📦 Backing up MongoDB configuration..."
sudo cp /etc/mongod.conf /etc/mongod.conf.backup

# Update MongoDB config to allow remote connections
echo "🔧 Configuring MongoDB to allow remote connections..."
sudo sed -i 's/bindIp: 127.0.0.1/bindIp: 0.0.0.0/' /etc/mongod.conf

# Enable authentication
echo "🔒 Enabling MongoDB authentication..."
if ! grep -q "security:" /etc/mongod.conf; then
    echo "security:" | sudo tee -a /etc/mongod.conf
    echo "  authorization: enabled" | sudo tee -a /etc/mongod.conf
fi

# Restart MongoDB
echo "🔄 Restarting MongoDB..."
sudo systemctl restart mongod

# Check status
echo "✅ Checking MongoDB status..."
sudo systemctl status mongod --no-pager

# Allow port through firewall
echo "🔓 Opening MongoDB port in firewall..."
sudo ufw allow 27017/tcp

echo ""
echo "================================================"
echo "Configuration Complete!"
echo "================================================"
echo ""
echo "Next steps:"
echo "1. Create MongoDB users by running:"
echo "   mongosh"
echo ""
echo "2. Then execute these commands in mongosh:"
echo ""
echo "   use admin"
echo "   db.createUser({"
echo "     user: 'admin',"
echo "     pwd: 'your-secure-password',"
echo "     roles: ['userAdminAnyDatabase', 'readWriteAnyDatabase']"
echo "   })"
echo ""
echo "   use leetour"
echo "   db.createUser({"
echo "     user: 'leetour',"
echo "     pwd: 'your-leetour-password',"
echo "     roles: [{ role: 'readWrite', db: 'leetour' }]"
echo "   })"
echo ""
echo "3. Update your local .env files with:"
echo "   MONGODB_URI=mongodb://leetour:your-password@157.173.124.250:27017/leetour?authSource=leetour"
echo ""
