# Setup Deployer User - Quick Guide

Since you have the **root password**, follow these simple steps:

---

## 🚀 Method 1: Automated Setup (Easiest)

Run this command in your terminal:

```bash
./setup-deployer-user.sh
```

The script will:
1. Connect to server as root
2. Create deployer user
3. Ask you to set deployer password
4. Add your SSH key
5. Configure sudo access
6. Test the connection

**You'll need:** Root password when prompted

---

## 🔧 Method 2: Manual Setup (Step by Step)

### Step 1: Connect as Root

```bash
ssh root@157.173.124.250
```
Enter your root password when prompted.

### Step 2: Create Deployer User

```bash
# Create user
adduser deployer
# Set password when prompted (e.g., deployer@leetour2024)

# Add to sudo group
usermod -aG sudo deployer

# Enable passwordless sudo (optional)
echo "deployer ALL=(ALL) NOPASSWD:ALL" >> /etc/sudoers
```

### Step 3: Setup SSH for Deployer

```bash
# Create SSH directory
mkdir -p /home/deployer/.ssh
chmod 700 /home/deployer/.ssh

# Add your SSH public key
nano /home/deployer/.ssh/authorized_keys
```

**Paste this entire key** (one line):
```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment
```

Save and exit: `Ctrl+X`, `Y`, `Enter`

```bash
# Set correct permissions
chmod 600 /home/deployer/.ssh/authorized_keys
chown -R deployer:deployer /home/deployer/.ssh

# Exit from root
exit
```

### Step 4: Test Connection

From your local machine:

```bash
# Test with password
ssh deployer@157.173.124.250

# Or test with SSH key (should work without password!)
ssh leetour
```

---

## ✅ Verification

After setup, this should work **without password**:

```bash
ssh leetour
```

You should see:
```
Welcome to Ubuntu 22.04 LTS ...
deployer@hostname:~$
```

---

## 🆘 Troubleshooting

### Problem: "Permission denied (publickey)"

**Check SSH key was added correctly:**
```bash
# Connect as root
ssh root@157.173.124.250

# Check the file
cat /home/deployer/.ssh/authorized_keys

# Should show your public key starting with "ssh-rsa AAAAB3..."

# Check permissions
ls -la /home/deployer/.ssh/
# Should show:
# drwx------ deployer deployer .ssh/
# -rw------- deployer deployer authorized_keys

# Fix permissions if needed
chmod 700 /home/deployer/.ssh
chmod 600 /home/deployer/.ssh/authorized_keys
chown -R deployer:deployer /home/deployer/.ssh
```

### Problem: "Host key verification failed"

Already fixed! But if it happens again:
```bash
ssh-keygen -R 157.173.124.250
```

### Problem: Can't connect as root

Make sure you're using the correct root password that you set during OS installation.

---

## 📝 Quick Reference

**Your SSH Public Key Location:**
```bash
~/.ssh/leetour_server.pub
```

**View your public key:**
```bash
cat ~/.ssh/leetour_server.pub
```

**Server Information:**
- IP: 157.173.124.250
- Root User: root
- Deploy User: deployer
- SSH Key: ~/.ssh/leetour_server

**Connection Commands:**
```bash
# As root
ssh root@157.173.124.250

# As deployer (with password)
ssh deployer@157.173.124.250

# As deployer (with SSH key - no password!)
ssh leetour
```

---

## 🎯 What to Do Next

After successfully connecting as deployer:

1. ✅ **Follow Fresh Server Setup**
   - See: [FRESH_SERVER_SETUP.md](./FRESH_SERVER_SETUP.md)
   - Install Node.js, PM2, Nginx, MongoDB

2. ✅ **Deploy Application**
   - See: [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md)

3. ✅ **Configure DNS & SSL**
   - See: [QUICK_START.md](./QUICK_START.md)

---

## 💡 Pro Tip

After setup works, you can disable root SSH login for security:

```bash
# Connect as deployer
ssh leetour

# Edit SSH config
sudo nano /etc/ssh/sshd_config

# Find and change:
PermitRootLogin no

# Save and restart SSH
sudo systemctl restart sshd
```

**⚠️ Only do this AFTER confirming deployer SSH works!**

---

Good luck! Let me know if you encounter any issues.
