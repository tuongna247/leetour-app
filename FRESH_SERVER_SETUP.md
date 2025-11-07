# Fresh Server Setup Guide - LeeTour Application

## 🖥️ Recommended Server Specifications

### **Operating System: Ubuntu 22.04 LTS** (Recommended)

**Minimum Requirements:**
- **CPU:** 2 vCPU cores
- **RAM:** 4 GB
- **Storage:** 80 GB SSD
- **Bandwidth:** Unlimited or 10 TB+

**Recommended for Production:**
- **CPU:** 4 vCPU cores
- **RAM:** 8 GB
- **Storage:** 160 GB SSD
- **Bandwidth:** Unlimited

---

## 🎯 Step-by-Step Fresh Installation

### Phase 1: Order & Install OS (Contabo)

1. **Login to Contabo Control Panel**
   - Go to: https://my.contabo.com/

2. **Find Your VPS**
   - Server IP: 157.173.124.250

3. **Reinstall OS**
   - Click "Reinstall"
   - **Select:** Ubuntu 22.04 LTS (64-bit)
   - Set root password (SAVE THIS!)
   - Confirm reinstallation
   - Wait 5-10 minutes

4. **Note Down Credentials**
   ```
   IP Address: 157.173.124.250
   Username: root
   Password: [the password you set]
   ```

---

### Phase 2: Initial Server Setup

#### Step 1: First Login

```bash
# From your local machine
ssh root@157.173.124.250
# Enter root password when prompted
```

#### Step 2: Update System

```bash
# Update package list
apt update

# Upgrade all packages
apt upgrade -y

# Install essential tools
apt install -y curl wget git nano ufw fail2ban
```

#### Step 3: Create Deployer User

```bash
# Create deployer user
adduser deployer
# Set password: deployer@leetour2024 (or your choice)
# Press Enter for all other prompts

# Add to sudo group
usermod -aG sudo deployer

# Grant sudo without password (optional but convenient)
echo "deployer ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers
```

#### Step 4: Setup SSH for Deployer

```bash
# Create SSH directory
mkdir -p /home/deployer/.ssh
chmod 700 /home/deployer/.ssh

# Add your SSH public key
nano /home/deployer/.ssh/authorized_keys
```

**Paste this key:**
```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment
```

Save: `Ctrl+X`, `Y`, `Enter`

```bash
# Set permissions
chmod 600 /home/deployer/.ssh/authorized_keys
chown -R deployer:deployer /home/deployer/.ssh

# Test sudo access
su - deployer
sudo whoami  # Should output: root
exit
```

#### Step 5: Configure Firewall

```bash
# Enable UFW firewall
ufw allow 22/tcp    # SSH
ufw allow 80/tcp    # HTTP
ufw allow 443/tcp   # HTTPS
ufw --force enable

# Check status
ufw status
```

#### Step 6: Secure SSH (Optional but Recommended)

```bash
# Backup SSH config
cp /etc/ssh/sshd_config /etc/ssh/sshd_config.backup

# Edit SSH config
nano /etc/ssh/sshd_config
```

**Find and change these lines:**
```
PermitRootLogin no                    # Disable root login via SSH
PasswordAuthentication yes            # Keep yes for now, can disable later
PubkeyAuthentication yes              # Enable SSH key auth
```

Save and restart SSH:
```bash
systemctl restart sshd
```

**⚠️ IMPORTANT:** Test deployer SSH access BEFORE logging out as root!

```bash
# From another terminal on your local machine
ssh deployer@157.173.124.250
# or
ssh leetour

# If it works, you can safely exit root session
```

---

### Phase 3: Install Application Stack

#### Step 1: Install Node.js 18.x

```bash
# Switch to deployer user if not already
su - deployer

# Install Node.js 18.x
curl -fsSL https://deb.nodesource.com/setup_18.x | sudo -E bash -
sudo apt install -y nodejs

# Verify installation
node --version   # Should show v18.x.x
npm --version    # Should show 9.x.x or higher
```

#### Step 2: Install PM2

```bash
# Install PM2 globally
sudo npm install -g pm2

# Verify
pm2 --version

# Setup PM2 to start on boot
pm2 startup
# Copy and run the command it outputs (starts with sudo env...)

# Test PM2
pm2 list
```

#### Step 3: Install Nginx

```bash
# Install Nginx
sudo apt install -y nginx

# Start and enable Nginx
sudo systemctl start nginx
sudo systemctl enable nginx

# Check status
sudo systemctl status nginx

# Test - open browser and visit: http://157.173.124.250
# You should see Nginx welcome page
```

#### Step 4: Install MongoDB

**Option A: Local MongoDB (Recommended for starting)**

```bash
# Import MongoDB GPG key
curl -fsSL https://www.mongodb.org/static/pgp/server-6.0.asc | sudo gpg --dearmor -o /usr/share/keyrings/mongodb-archive-keyring.gpg

# Add MongoDB repository
echo "deb [ arch=amd64,arm64 signed-by=/usr/share/keyrings/mongodb-archive-keyring.gpg ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/6.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-6.0.list

# Update and install
sudo apt update
sudo apt install -y mongodb-org

# Start MongoDB
sudo systemctl start mongod
sudo systemctl enable mongod

# Check status
sudo systemctl status mongod

# Test connection
mongosh
# Type: exit
```

**Option B: MongoDB Atlas (Cloud - Can use later)**
- Skip local MongoDB
- Use connection string in .env files
- See [ENV_SETUP.md](./ENV_SETUP.md)

---

### Phase 4: Deploy LeeTour Application

#### Step 1: Clone Repository

```bash
# Create app directory
sudo mkdir -p /var/www/leetour
sudo chown -R deployer:deployer /var/www/leetour

# Clone repository
cd /var/www/leetour
git clone https://github.com/tuongna247/leetour-app.git .

# Or if already initialized
git init
git remote add origin https://github.com/tuongna247/leetour-app.git
git pull origin main
```

#### Step 2: Setup Environment Files

See [ENV_SETUP.md](./ENV_SETUP.md) for detailed instructions.

**Quick setup:**

```bash
# Admin .env.local
nano apps/admin/.env.local
```

Paste:
```env
MONGODB_URI=mongodb://localhost:27017/leetour
NEXTAUTH_URL=https://admin.goreise.com
NEXTAUTH_SECRET=generate-a-secret-key-here-min-32-chars
NEXT_PUBLIC_API_URL=https://api.goreise.com
```

```bash
# API .env
nano apps/api/.env
```

Paste:
```env
MONGODB_URI=mongodb://localhost:27017/leetour
PORT=3001
NODE_ENV=production
ALLOWED_ORIGINS=https://admin.goreise.com,https://tour.goreise.com
JWT_SECRET=generate-another-secret-key-here
```

```bash
# Frontend .env
nano apps/frontend/.env
```

Paste:
```env
NEXT_PUBLIC_API_URL=https://api.goreise.com
NEXT_PUBLIC_APP_NAME=LeeTour
NEXT_PUBLIC_APP_URL=https://tour.goreise.com
```

**Generate secrets:**
```bash
# Generate NEXTAUTH_SECRET
openssl rand -base64 32

# Generate JWT_SECRET
openssl rand -base64 32
```

#### Step 3: Install Dependencies & Build

```bash
cd /var/www/leetour

# Install for each app
cd apps/admin && npm install
cd ../api && npm install
cd ../frontend && npm install
cd ../..

# Build each app
cd apps/admin && npm run build
cd ../api && npm run build
cd ../frontend && npm run build
cd ../..
```

#### Step 4: Configure Nginx

```bash
# Copy nginx config
sudo cp nginx-leetour.conf /etc/nginx/sites-available/leetour

# Create symbolic link
sudo ln -s /etc/nginx/sites-available/leetour /etc/nginx/sites-enabled/

# Remove default config
sudo rm /etc/nginx/sites-enabled/default

# Test configuration
sudo nginx -t

# Reload Nginx
sudo systemctl reload nginx
```

#### Step 5: Start Applications with PM2

```bash
cd /var/www/leetour

# Make deploy script executable
chmod +x deploy.sh

# Start with PM2
pm2 start ecosystem.config.js --env production

# Save PM2 process list
pm2 save

# Check status
pm2 status

# View logs
pm2 logs
```

---

### Phase 5: Configure DNS

**In your domain registrar (Namecheap, GoDaddy, Cloudflare, etc.):**

Add these A records:

| Type | Name  | Value           | TTL  |
|------|-------|-----------------|------|
| A    | admin | 157.173.124.250 | Auto |
| A    | api   | 157.173.124.250 | Auto |
| A    | tour  | 157.173.124.250 | Auto |

Wait 5-30 minutes for DNS propagation.

**Test DNS propagation:**
```bash
# From your local machine
nslookup admin.goreise.com
nslookup api.goreise.com
nslookup tour.goreise.com
```

---

### Phase 6: Install SSL Certificates

**After DNS has propagated:**

```bash
# Install Certbot
sudo apt install -y certbot python3-certbot-nginx

# Get certificates for all domains
sudo certbot --nginx -d admin.goreise.com
sudo certbot --nginx -d api.goreise.com
sudo certbot --nginx -d tour.goreise.com

# Follow prompts:
# - Enter email address
# - Agree to terms
# - Choose to redirect HTTP to HTTPS (option 2)

# Test auto-renewal
sudo certbot renew --dry-run

# Enable auto-renewal timer
sudo systemctl enable certbot.timer
sudo systemctl start certbot.timer
```

---

### Phase 7: Verification

#### Check All Services

```bash
# PM2 status
pm2 status

# Nginx status
sudo systemctl status nginx

# MongoDB status
sudo systemctl status mongod

# Check logs
pm2 logs --lines 20

# Nginx error logs
sudo tail -f /var/log/nginx/error.log
```

#### Test URLs

Open in browser:
- https://admin.goreise.com
- https://api.goreise.com
- https://tour.goreise.com

All should load without SSL warnings!

---

## 📝 Post-Installation Checklist

- [ ] Server OS installed (Ubuntu 22.04 LTS)
- [ ] System updated
- [ ] Deployer user created
- [ ] SSH key authentication working
- [ ] Firewall configured (UFW)
- [ ] Node.js installed
- [ ] PM2 installed and configured
- [ ] Nginx installed and running
- [ ] MongoDB installed (or Atlas configured)
- [ ] Repository cloned
- [ ] Environment files created
- [ ] Dependencies installed
- [ ] Applications built
- [ ] Nginx configured
- [ ] PM2 processes running
- [ ] DNS records configured
- [ ] SSL certificates installed
- [ ] All URLs accessible via HTTPS

---

## 🔐 Important Information to Save

**Server Access:**
```
IP: 157.173.124.250
Root Password: [save securely]
Deployer Password: [save securely]
SSH Key: ~/.ssh/leetour_server
```

**Application URLs:**
```
Admin: https://admin.goreise.com
API: https://api.goreise.com
Frontend: https://tour.goreise.com
```

**Secrets (save in password manager):**
```
NEXTAUTH_SECRET: [your generated secret]
JWT_SECRET: [your generated secret]
MongoDB Connection: mongodb://localhost:27017/leetour
```

---

## 🚀 Quick Commands Reference

```bash
# Connect to server
ssh leetour

# Deploy updates
cd /var/www/leetour && ./deploy.sh

# Check status
pm2 status

# View logs
pm2 logs

# Restart apps
pm2 restart all

# Nginx reload
sudo systemctl reload nginx

# MongoDB status
sudo systemctl status mongod
```

---

## 📊 Estimated Installation Time

| Phase | Duration |
|-------|----------|
| OS Installation | 10-15 minutes |
| Initial Setup | 15 minutes |
| Stack Installation | 20 minutes |
| Application Deployment | 15 minutes |
| DNS Propagation | 5-30 minutes |
| SSL Setup | 5 minutes |
| **Total** | **1-2 hours** |

---

## 🆘 Troubleshooting

See detailed troubleshooting in:
- [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md)
- [QUICK_START.md](./QUICK_START.md)

Common issues:
- **Can't connect via SSH:** Check firewall, verify SSH key
- **MongoDB not starting:** Check logs: `sudo journalctl -u mongod`
- **Apps not building:** Check Node version, npm install errors
- **502 Bad Gateway:** Check if PM2 apps are running

---

## 💡 Pro Tips

1. **Take snapshots** before major changes (Contabo offers this)
2. **Keep documentation updated** with passwords and configurations
3. **Test thoroughly** before pointing production DNS
4. **Monitor resources** with `htop` and `pm2 monit`
5. **Setup automated backups** for database

---

Good luck with your fresh installation! 🚀
