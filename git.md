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

## Cleanup Tasks

- [ ] Remove `nul` file if not needed
- [ ] Review .claude/settings.local.json changes

---

*Last Updated: 2025-12-24*
