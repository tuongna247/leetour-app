# Password Recovery Guide for LeeTour Server

## 🔐 Forgot Deployer Password

Don't worry! Here are several ways to regain access to your server.

---

## 🎯 Solution 1: Contabo Control Panel (Easiest)

### Step 1: Access Contabo Control Panel

1. Go to: **https://my.contabo.com/**
2. Login with your Contabo account credentials
3. Find your VPS (IP: 157.173.124.250)

### Step 2: Use VNC Console

1. Click on your VPS
2. Look for **"VNC Console"** or **"Console Access"**
3. Click to open the console in browser
4. You should see the server terminal

### Step 3: Login as Root

In the VNC console:
```bash
# Login as root
# Username: root
# Password: [Your root password]
```

### Step 4: Reset Deployer Password

```bash
# Reset password for deployer
passwd deployer

# Enter new password twice
# Example: deployer@2024! (remember this!)
```

### Step 5: Add Your SSH Key

```bash
# Switch to deployer user
su - deployer

# Create SSH directory
mkdir -p ~/.ssh
chmod 700 ~/.ssh

# Create/edit authorized_keys
nano ~/.ssh/authorized_keys
```

**Paste your SSH public key:**
```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment
```

Save and exit: `Ctrl+X`, `Y`, `Enter`

```bash
# Set permissions
chmod 600 ~/.ssh/authorized_keys

# Exit back to root
exit

# Exit VNC console
exit
```

### Step 6: Test Connection

From your local machine:
```bash
# Try SSH with new password
ssh deployer@157.173.124.250
# Enter the new password you set

# Or try with SSH key
ssh leetour
```

---

## 🎯 Solution 2: Contact Contabo Support

If you don't have root access:

1. **Open Support Ticket**
   - Login to Contabo control panel
   - Go to Support section
   - Create new ticket

2. **Request Password Reset**
   ```
   Subject: Password Reset Request for VPS

   Hello,

   I need to reset the password for user "deployer" on my VPS.

   Server IP: 157.173.124.250
   Username: deployer

   Could you please help me reset the password or provide
   temporary root access?

   Thank you.
   ```

3. **Wait for Response**
   - Usually 24-48 hours
   - They will provide instructions

---

## 🎯 Solution 3: Use Root SSH (If You Have It)

If you have root SSH access:

```bash
# Connect as root
ssh root@157.173.124.250

# Reset deployer password
passwd deployer
# Enter new password twice

# Add SSH key
mkdir -p /home/deployer/.ssh
chmod 700 /home/deployer/.ssh

# Copy your public key
echo "ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment" >> /home/deployer/.ssh/authorized_keys

# Set permissions
chmod 600 /home/deployer/.ssh/authorized_keys
chown -R deployer:deployer /home/deployer/.ssh

# Test
su - deployer
```

---

## 🎯 Solution 4: Automated Script

I've created a script to help you: `reset-deployer-access.sh`

**To use it:**

1. **Get root access** (via VNC console or SSH)

2. **Copy script to server:**
   ```bash
   # From your local machine
   scp reset-deployer-access.sh root@157.173.124.250:/root/
   ```

3. **Run on server:**
   ```bash
   # As root user
   sudo bash /root/reset-deployer-access.sh
   ```

4. **Follow prompts:**
   - Set new password for deployer
   - Paste your SSH public key

---

## 📋 What You Need

To recover access, you need **ONE** of these:

- [ ] Root password for SSH
- [ ] Root SSH key
- [ ] Contabo control panel login
- [ ] VNC/Console access from Contabo
- [ ] Contact Contabo support

---

## 🔑 Your SSH Public Key

Keep this handy - you'll need to add it to the server:

```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment
```

You can always view it with:
```bash
cat ~/.ssh/leetour_server.pub
```

---

## ✅ After Regaining Access

Once you can access the server as deployer:

1. **Test SSH key works:**
   ```bash
   ssh leetour
   ```

2. **Continue with deployment:**
   - See [QUICK_START.md](./QUICK_START.md)
   - See [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md)

3. **Save your new password securely!**
   - Use a password manager
   - Or write it down in a secure place

---

## 🆘 Still Can't Access?

### Check These:

1. **Do you have Contabo account access?**
   - Email used for signup
   - Contabo customer ID
   - Check email for welcome message

2. **Try password recovery for Contabo:**
   - https://my.contabo.com/
   - Click "Forgot password"

3. **Check if you have root credentials:**
   - Check email from when you first created the VPS
   - Look for "VPS Credentials" email from Contabo

4. **Last resort - Recreate VPS:**
   - This will delete everything
   - Only do if you have no other option
   - You'll need to redeploy everything

---

## 💡 Prevention - Don't Lose Access Again!

After regaining access:

1. **Save passwords securely:**
   - Use LastPass, 1Password, Bitwarden
   - Or write down in secure physical location

2. **Setup SSH key (already done!):**
   - No need to remember password
   - More secure

3. **Create backup admin user:**
   ```bash
   sudo useradd -m -s /bin/bash backup-admin
   sudo passwd backup-admin
   sudo usermod -aG sudo backup-admin
   ```

4. **Save root password:**
   - Check Contabo welcome email
   - Store in password manager

5. **Document everything:**
   - Server IP
   - Usernames
   - Where passwords are stored
   - VPS provider login

---

## 📞 Contact Information

**Contabo Support:**
- Website: https://contabo.com/en/support/
- Email: support@contabo.com
- Panel: https://my.contabo.com/

**When contacting support, provide:**
- Your customer ID
- Server IP: 157.173.124.250
- Description of issue
- What access you need

---

## 🎯 Next Steps

1. [ ] Access Contabo control panel
2. [ ] Use VNC console to get root access
3. [ ] Reset deployer password
4. [ ] Add SSH key to deployer user
5. [ ] Test connection: `ssh leetour`
6. [ ] Continue with deployment

Good luck! Let me know which solution works for you.
