# SSH Setup Guide for LeeTour Server

## ✅ What Has Been Done

1. **SSH Key Generated**: `~/.ssh/leetour_server`
2. **SSH Config Updated**: You can now use `ssh leetour` as a shortcut
3. **Helper Scripts Created**:
   - `setup-ssh.sh` (Mac/Linux)
   - `setup-ssh.bat` (Windows)

---

## 📋 Next Steps

### Step 1: Copy Your Public Key to the Server

Your public SSH key is:

```
ssh-rsa AAAAB3NzaC1yc2EAAAADAQABAAACAQCFYWYSnebjjUbY6sWsMfEdjQzZQ5EVci6kHd6D6L45YcpUuaTx88UHSnaCukCUewEfA2ptyvLXc+BW0v2KPws4rMdOrTd2pqklA2w2YcLbuXIxGYv+01JB/y/q6JyrNEDVEe/hYDvQNcVv1YaTK5kUsOMnvs2xXzVj9I5dBzgTiSO3QKB2IMoR/pk8/nDj9Xmrn/ECs+41bAcokodxJikr7/U0Ru7CL+vEvtCVl4exyVej32cZFh23wKAbfLqqU4XQZsWuVRCBpxaQPr9a/FngyKx2ROwzY/hB2tbC/a3oZT1lRArJLr4rKuAwiF19jFREgfNbg9XfIM5Dh25coZWWzo63yJahvaBTc2jsH+VXeytHi8liAiI5xBAfQTuX6FHkJSPKPJV+nvozK0uib+WylV9fYOdwkyz9YJ9DqCod/oZQicurtUvDDw9Sbd2irAa4xYgMiifOzFpULjymeKt9YR3w7CvPptB2ANW50Pa0DKcLBWQyvG7PQGva90Z7Efh8vRRgPyW3R8CA5ElgT5HuBGOX2bNL5nxO4gmxkUjve5PSffJgg8AhBKH6AWptOAsAtu53FHtSJuPGZJP/kTPuoyaFFY2cqvQMLwPCkZQmwn11dgPWxacp2rzkO2kdupY2iKl7Q9mktZuOiI0jANFpzCsCozbD2rql8EiWs+vFJw== leetour-deployment
```

You can view it anytime by running:
```bash
cat ~/.ssh/leetour_server.pub
```

---

### Option A: Automatic Setup (If You Have Server Password)

**Windows (Git Bash or PowerShell):**
```bash
# Copy the key to server
type ~/.ssh/leetour_server.pub | ssh deployer@157.173.124.250 "mkdir -p ~/.ssh && cat >> ~/.ssh/authorized_keys && chmod 600 ~/.ssh/authorized_keys"

# Or use the helper script
./setup-ssh.bat
```

**Mac/Linux:**
```bash
# Use ssh-copy-id (easiest)
ssh-copy-id -i ~/.ssh/leetour_server.pub deployer@157.173.124.250

# Or use the helper script
./setup-ssh.sh
```

---

### Option B: Manual Setup (If You Don't Have Password)

1. **Copy the public key above** (the long text starting with `ssh-rsa`)

2. **Ask your server administrator** to add it to the server:
   - They need to add it to `/home/deployer/.ssh/authorized_keys`
   - The file should have permissions `600`

3. **Or if you have access via another method:**
   ```bash
   # Connect to the server however you can
   ssh deployer@157.173.124.250

   # Create SSH directory
   mkdir -p ~/.ssh
   chmod 700 ~/.ssh

   # Edit authorized_keys
   nano ~/.ssh/authorized_keys
   # Paste your public key at the end
   # Press Ctrl+X, then Y, then Enter to save

   # Set correct permissions
   chmod 600 ~/.ssh/authorized_keys

   # Exit
   exit
   ```

---

## Step 2: Test Your Connection

Once your public key is added to the server, test it:

```bash
# Simple short command
ssh leetour

# Or full command
ssh -i ~/.ssh/leetour_server deployer@157.173.124.250
```

If successful, you should see the server prompt without entering a password!

---

## 🚀 Quick Commands After Setup

### Connect to Server
```bash
ssh leetour
```

### Deploy Application
```bash
ssh leetour "cd /var/www/leetour && ./deploy.sh"
```

### Check PM2 Status
```bash
ssh leetour "pm2 status"
```

### View Logs
```bash
ssh leetour "pm2 logs"
```

### Restart Applications
```bash
ssh leetour "pm2 restart all"
```

---

## 🔐 Security Notes

1. **Keep Your Private Key Safe**
   - Location: `~/.ssh/leetour_server`
   - Never share this file with anyone
   - Never commit it to Git

2. **Public Key is Safe to Share**
   - Location: `~/.ssh/leetour_server.pub`
   - You can safely share this with server administrators

3. **SSH Config**
   - Location: `~/.ssh/config`
   - Contains shortcuts for easy connection

---

## 🆘 Troubleshooting

### Problem: "Permission denied (publickey)"

**Possible causes:**
1. Public key not added to server
2. Wrong permissions on server files

**Solution:**
```bash
# On server, check permissions
ssh deployer@157.173.124.250
ls -la ~/.ssh/
# authorized_keys should be 600
# .ssh directory should be 700

# Fix permissions if needed
chmod 700 ~/.ssh
chmod 600 ~/.ssh/authorized_keys
```

### Problem: "Connection refused"

**Check:**
1. Is the server IP correct? `157.173.124.250`
2. Is the server running?
3. Can you ping the server?

```bash
ping 157.173.124.250
```

### Problem: "Host key verification failed"

**Solution:**
```bash
# Remove old host key
ssh-keygen -R 157.173.124.250

# Try connecting again
ssh leetour
# Type 'yes' when asked to add new host key
```

---

## 📝 Quick Reference

| Command | Description |
|---------|-------------|
| `ssh leetour` | Connect to server |
| `ssh leetour "command"` | Run command on server |
| `scp file.txt leetour:~/` | Copy file to server |
| `scp leetour:~/file.txt .` | Copy file from server |
| `ssh-add ~/.ssh/leetour_server` | Add key to SSH agent |

---

## ✅ Connection Test Checklist

Run these commands to verify your setup:

```bash
# 1. Check if key exists
ls -la ~/.ssh/leetour_server*

# 2. View public key
cat ~/.ssh/leetour_server.pub

# 3. Check SSH config
cat ~/.ssh/config | grep -A 6 "Host leetour"

# 4. Test connection
ssh -vT leetour
# Look for "debug1: Authentication succeeded (publickey)"

# 5. Test command execution
ssh leetour "hostname && whoami"
```

If all commands work, you're ready to deploy!

---

## 🎯 Next Steps After SSH Setup

1. ✅ SSH connection working
2. 📄 Follow [ENV_SETUP.md](./ENV_SETUP.md) to configure environment variables
3. 🌐 Setup DNS records (see [QUICK_START.md](./QUICK_START.md))
4. 🚀 Deploy application (see [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md))

---

## 💡 Pro Tips

### Save Time with Aliases

Add to your `~/.bashrc` or `~/.zshrc`:

```bash
# LeeTour server shortcuts
alias lee='ssh leetour'
alias lee-deploy='ssh leetour "cd /var/www/leetour && ./deploy.sh"'
alias lee-status='ssh leetour "pm2 status"'
alias lee-logs='ssh leetour "pm2 logs"'
alias lee-restart='ssh leetour "pm2 restart all"'
```

Then reload:
```bash
source ~/.bashrc  # or source ~/.zshrc
```

Now you can use:
- `lee` instead of `ssh leetour`
- `lee-deploy` to deploy instantly
- `lee-status` to check status
- etc.

---

## 📞 Need Help?

- Check [QUICK_START.md](./QUICK_START.md) for deployment help
- See [DEPLOYMENT_GUIDE.md](./DEPLOYMENT_GUIDE.md) for detailed instructions
- Review SSH debug output: `ssh -vvv leetour`
