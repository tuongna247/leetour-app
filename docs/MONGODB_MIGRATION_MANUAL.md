# MongoDB Migration - Manual Steps
## For when you need to run commands manually via SSH

This guide provides individual commands you can copy and paste one at a time into your SSH session.

## Step 1: SSH into the server

```bash
ssh deployer@157.173.124.250
```

---

## Step 2: Check MongoDB Status

First, let's see if MongoDB is installed and what's wrong:

```bash
which mongod
```

```bash
mongod --version
```

```bash
sudo systemctl status mongod
```

---

## Step 3: Fix MongoDB Permissions (Common Issue)

This is usually why MongoDB won't start:

```bash
sudo chown -R mongodb:mongodb /var/lib/mongodb
```

```bash
sudo chown -R mongodb:mongodb /var/log/mongodb
```

```bash
sudo chmod 755 /var/lib/mongodb
```

---

## Step 4: Start MongoDB

```bash
sudo systemctl start mongod
```

Wait 5 seconds, then check:

```bash
sudo systemctl status mongod
```

Enable it to start on boot:

```bash
sudo systemctl enable mongod
```

---

## Step 5: Verify MongoDB is Running

Check if it's listening on port 27017:

```bash
netstat -tlnp | grep 27017
```

Or:

```bash
ss -tlnp | grep 27017
```

You should see something like:
```
tcp        0      0 127.0.0.1:27017         0.0.0.0:*               LISTEN      12345/mongod
```

---

## Step 6: Install MongoDB Database Tools

If mongodump is not installed:

```bash
sudo apt update
```

```bash
sudo apt install -y mongodb-database-tools
```

Verify:

```bash
mongodump --version
```

```bash
mongorestore --version
```

---

## Step 7: Backup from MongoDB Atlas

Create backup directory:

```bash
mkdir -p /tmp/leetour-backup
```

```bash
cd /tmp
```

Backup from Atlas (this will take a few minutes):

```bash
mongodump --uri="mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0" --out=leetour-backup
```

Check backup size:

```bash
du -sh leetour-backup
```

---

## Step 8: Restore to Local MongoDB

Restore the database:

```bash
mongorestore --uri="mongodb://localhost:27017" --drop leetour-backup/leetour
```

---

## Step 9: Verify Restoration

Connect to MongoDB and check:

```bash
mongosh
```

Inside mongosh:

```javascript
use leetour
show collections
db.tours.countDocuments()
db.bookings.countDocuments()
db.users.countDocuments()
exit
```

---

## Step 10: Update API Configuration

Edit API .env file:

```bash
cd /var/www/leetour/apps/api
```

```bash
nano .env
```

Find the line:
```
MONGODB_URI=mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
```

Change it to:
```
MONGODB_URI=mongodb://localhost:27017/leetour
```

Save and exit:
- Press `Ctrl + O` (to save)
- Press `Enter` (to confirm)
- Press `Ctrl + X` (to exit)

Or use sed to do it automatically:

```bash
sed -i 's|MONGODB_URI=mongodb+srv://.*|MONGODB_URI=mongodb://localhost:27017/leetour|g' /var/www/leetour/apps/api/.env
```

Verify the change:

```bash
grep MONGODB_URI /var/www/leetour/apps/api/.env
```

---

## Step 11: Update Admin Configuration

Edit Admin .env.local file:

```bash
cd /var/www/leetour/apps/admin
```

```bash
nano .env.local
```

Find the line:
```
MONGODB_URI=mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
```

Change it to:
```
MONGODB_URI=mongodb://localhost:27017/leetour
```

Save and exit (same as above).

Or use sed:

```bash
sed -i 's|MONGODB_URI=mongodb+srv://.*|MONGODB_URI=mongodb://localhost:27017/leetour|g' /var/www/leetour/apps/admin/.env.local
```

Verify:

```bash
grep MONGODB_URI /var/www/leetour/apps/admin/.env.local
```

---

## Step 12: Restart Applications

Restart all PM2 applications:

```bash
pm2 restart all
```

Check status:

```bash
pm2 status
```

Check logs for any errors:

```bash
pm2 logs --lines 50
```

---

## Step 13: Test Everything

### Test API directly:

```bash
curl http://localhost:3001/api/tours?limit=1
```

You should see JSON data with tours.

### Test in browser:

1. Admin dashboard: http://admin.goreise.com/dashboard-main
2. API endpoint: http://api.goreise.com/api/tours
3. Frontend: http://tour.goreise.com/tours

---

## Step 14: Clean Up

Remove backup files:

```bash
rm -rf /tmp/leetour-backup
```

---

## Troubleshooting

### If MongoDB won't start:

Check logs:
```bash
sudo tail -100 /var/log/mongodb/mongod.log
```

```bash
sudo journalctl -u mongod -n 50
```

### If permission denied:

```bash
sudo chown -R mongodb:mongodb /var/lib/mongodb
sudo chown -R mongodb:mongodb /var/log/mongodb
sudo systemctl restart mongod
```

### If port 27017 is already in use:

Find what's using it:
```bash
sudo lsof -i :27017
```

Kill the process:
```bash
sudo kill -9 <PID>
```

Then start MongoDB again.

### If mongodump fails:

Check internet connection:
```bash
ping -c 3 cluster0.nz7bupo.mongodb.net
```

Try with verbose output:
```bash
mongodump --uri="mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour" --out=leetour-backup --verbose
```

---

## Quick Reference

### MongoDB Atlas Connection:
```
mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
```

### Local MongoDB Connection:
```
mongodb://localhost:27017/leetour
```

### Useful Commands:

Check MongoDB status:
```bash
sudo systemctl status mongod
```

Start MongoDB:
```bash
sudo systemctl start mongod
```

Stop MongoDB:
```bash
sudo systemctl stop mongod
```

Restart MongoDB:
```bash
sudo systemctl restart mongod
```

View MongoDB logs:
```bash
sudo tail -f /var/log/mongodb/mongod.log
```

Connect to MongoDB shell:
```bash
mongosh
```

Restart PM2 apps:
```bash
pm2 restart all
```

View PM2 logs:
```bash
pm2 logs
```

---

## Notes

- You're currently using MongoDB 8.0.15 on the server
- The migration guide uses MongoDB 7.0, but 8.0 works fine too
- Always keep MongoDB Atlas as a backup option
- Consider setting up automated backups after migration
- Monitor disk space - local MongoDB will use server storage

---

## After Migration

Once everything is working with local MongoDB, you can:

1. Keep MongoDB Atlas for backups only
2. Set up a backup cron job (see MONGODB_MIGRATION.md for script)
3. Monitor MongoDB performance with `mongosh` commands
4. Consider enabling MongoDB authentication for security
