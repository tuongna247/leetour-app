# Git Troubleshooting & Experience Guide

## Issue: Cannot Commit Code

### Problem Description
Unable to commit code changes to the repository.

### Investigation Steps

1. **Check Git Status**
   ```bash
   git status
   ```

2. **Check Git Configuration**
   ```bash
   git config user.name
   git config user.email
   git config --list --local
   ```

3. **Check Git Hooks**
   ```bash
   ls -la .git/hooks/
   ```

### Common Solutions

#### Solution 1: Stage Changes Before Commit
**Issue:** Changes not staged for commit

```bash
# Stage specific file
git add path/to/file

# Stage all changes
git add .

# Then commit
git commit -m "Your commit message"
```

#### Solution 2: Configure Git User Info
**Issue:** Missing user name or email

```bash
# Set user name
git config user.name "Your Name"

# Set user email
git config user.email "your.email@example.com"

# Or set globally
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

#### Solution 3: Pre-commit Hook Failures
**Issue:** Pre-commit hook blocking the commit

```bash
# Bypass hooks (use cautiously)
git commit --no-verify -m "Your commit message"

# Or fix the hook issue
# Check .git/hooks/pre-commit file for errors
```

#### Solution 4: Permission Issues
**Issue:** File permission problems

```bash
# Check file permissions
ls -la

# Fix if needed (Linux/Mac)
chmod +x .git/hooks/pre-commit
```

### Current Repository Status (2025-12-24)

**Branch:** codebase

**Modified Files:**
- .claude/settings.local.json

**Untracked Files:**
- nul (may need cleanup)

**Git User:**
- Name: Tuong Nguyenanh
- Email: tuong.nguyenanh@elinext.com

### Additional Notes & Experiences

*(Add your experiences and solutions here)*

---

## Useful Git Commands

### Basic Workflow
```bash
# Check status
git status

# Stage changes
git add <file>
git add .

# Commit changes
git commit -m "message"

# Push to remote
git push origin <branch>
```

### Inspecting History
```bash
# View commit history
git log
git log --oneline
git log -1 --format='%H %an %ae %s'

# View changes
git diff
git diff --staged
```

### Branch Management
```bash
# List branches
git branch
git branch -a

# Create and switch branch
git checkout -b <branch-name>

# Switch branch
git checkout <branch-name>
```

### Undoing Changes
```bash
# Unstage file
git restore --staged <file>

# Discard changes in working directory
git restore <file>

# Amend last commit
git commit --amend
```

### Remote Operations
```bash
# View remotes
git remote -v

# Fetch from remote
git fetch origin

# Pull changes
git pull origin <branch>

# Push changes
git push origin <branch>
```

---

## SSH Configuration & Management

### Search and List SSH Files

**List SSH directory contents**
```bash
ls -la ~/.ssh
```

**Find all .ssh directories on the system**
```bash
find ~ -name ".ssh" -type d 2>/dev/null
```

**Find all SSH key files (private keys)**
```bash
find ~/.ssh -type f ! -name "*.pub" ! -name "known_hosts*" ! -name "config" 2>/dev/null
```

**Find all SSH public keys**
```bash
find ~/.ssh -name "*.pub" 2>/dev/null
```

**Show SSH config and all keys**
```bash
cat ~/.ssh/config && echo -e "\n--- SSH Keys ---" && ls -1 ~/.ssh/*.pub
```

### SSH Key Management

**Generate new SSH key**
```bash
ssh-keygen -t rsa -b 4096 -C "your.email@example.com" -f ~/.ssh/keyname
```

**Test SSH connection to GitHub**
```bash
ssh -T git@github.com
```

**Add SSH key to ssh-agent**
```bash
eval "$(ssh-agent -s)"
ssh-add ~/.ssh/keyname
```

**Copy SSH public key to clipboard (Windows)**
```bash
cat ~/.ssh/keyname.pub | clip
```

**Copy SSH public key to clipboard (Mac)**
```bash
pbcopy < ~/.ssh/keyname.pub
```

**Copy SSH public key to clipboard (Linux)**
```bash
xclip -sel clip < ~/.ssh/keyname.pub
```

### Configure Multiple GitHub Accounts

**Example SSH config for multiple accounts**
```bash
# Account 1 (default)
Host github.com
    HostName github.com
    User git
    IdentityFile ~/.ssh/id_rsa_account1

# Account 2
Host github-account2
    HostName github.com
    User git
    IdentityFile ~/.ssh/id_rsa_account2
```

**Use specific SSH key for a repository**
```bash
# Clone with specific host
git clone git@github-account2:username/repo.git

# Or change existing remote
git remote set-url origin git@github-account2:username/repo.git
```

### Switch Between HTTPS and SSH

**Check current remote URL**
```bash
git remote -v
```

**Change from HTTPS to SSH**
```bash
git remote set-url origin git@github.com:username/repo.git
```

**Change from SSH to HTTPS**
```bash
git remote set-url origin https://github.com/username/repo.git
```

---

## Cleanup Tasks

- [ ] Remove `nul` file if not needed
- [ ] Review .claude/settings.local.json changes

---

*Last Updated: 2025-12-24*
