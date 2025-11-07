# MongoDB Migration Guide
## Backup from MongoDB Atlas and Restore to Ubuntu Server

This guide explains how to backup your MongoDB Atlas database and restore it to a local MongoDB instance on your Ubuntu server.

## Prerequisites
- MongoDB Atlas account with database access
- Ubuntu server with MongoDB installed
- SSH access to the server

## Connection Details

### MongoDB Atlas (Source)
```
Connection String: mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
Database Name: leetour
Username: leetour
Password: RN1vmYdHHjnTwEqM
Cluster: cluster0.nz7bupo.mongodb.net
```

### Local MongoDB (Destination)
```
Connection String: mongodb://localhost:27017/leetour
Database Name: leetour
```

## Step 1: Install MongoDB Tools on Server

SSH into your server:
```bash
ssh deployer@157.173.124.250
```

Install MongoDB database tools:
```bash
sudo apt update
sudo apt install -y mongodb-database-tools
```

Verify installation:
```bash
mongodump --version
mongorestore --version
```

## Step 2: Backup from MongoDB Atlas

### Option A: Backup directly on the server

```bash
cd /tmp
mongodump --uri="mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0" --out=leetour-backup
```

### Option B: Backup on local machine, then transfer

On your local machine:
```bash
mongodump --uri="mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0" --out=leetour-backup
```

Compress the backup:
```bash
tar -czf leetour-backup.tar.gz leetour-backup/
```

Transfer to server:
```bash
scp leetour-backup.tar.gz deployer@157.173.124.250:/tmp/
```

Extract on server:
```bash
ssh deployer@157.173.124.250
cd /tmp
tar -xzf leetour-backup.tar.gz
```

## Step 3: Install MongoDB on Ubuntu Server

### Install MongoDB 7.0 (Stable)

Import MongoDB GPG key:
```bash
curl -fsSL https://www.mongodb.org/static/pgp/server-7.0.asc | sudo gpg --dearmor -o /usr/share/keyrings/mongodb-server-7.0.gpg
```

Add MongoDB repository:
```bash
echo "deb [ arch=amd64,arm64 signed-by=/usr/share/keyrings/mongodb-server-7.0.gpg ] https://repo.mongodb.org/apt/ubuntu jammy/mongodb-org/7.0 multiverse" | sudo tee /etc/apt/sources.list.d/mongodb-org-7.0.list
```

Update package list:
```bash
sudo apt update
```

Install MongoDB:
```bash
sudo apt install -y mongodb-org
```

Start MongoDB:
```bash
sudo systemctl start mongod
```

Enable MongoDB to start on boot:
```bash
sudo systemctl enable mongod
```

Verify MongoDB is running:
```bash
sudo systemctl status mongod
```

Check MongoDB is listening:
```bash
sudo netstat -tlnp | grep 27017
```

## Step 4: Restore Database to Local MongoDB

Restore the backup:
```bash
mongorestore --uri="mongodb://localhost:27017" --db=leetour /tmp/leetour-backup/leetour
```

Or if the backup has a different structure:
```bash
mongorestore --uri="mongodb://localhost:27017" /tmp/leetour-backup/
```

Verify restoration:
```bash
mongosh
use leetour
show collections
db.tours.countDocuments()
db.bookings.countDocuments()
exit
```

## Step 5: Update Application Configuration

### Update API .env file
```bash
nano /var/www/leetour/apps/api/.env
```

Change from:
```
MONGODB_URI=mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
```

To:
```
MONGODB_URI=mongodb://localhost:27017/leetour
```

### Update Admin .env.local file
```bash
nano /var/www/leetour/apps/admin/.env.local
```

Change from:
```
MONGODB_URI=mongodb+srv://leetour:RN1vmYdHHjnTwEqM@cluster0.nz7bupo.mongodb.net/leetour?retryWrites=true&w=majority&appName=Cluster0
```

To:
```
MONGODB_URI=mongodb://localhost:27017/leetour
```

## Step 6: Restart Applications

Restart all PM2 applications:
```bash
pm2 restart all
```

Or restart individually:
```bash
pm2 restart leetour-admin
pm2 restart leetour-api
pm2 restart leetour-frontend
```

Check logs for errors:
```bash
pm2 logs
```

## Step 7: Verify Everything Works

### Check MongoDB connection
```bash
pm2 logs leetour-api --lines 50 | grep -i mongo
```

### Test API endpoint
```bash
curl http://localhost:3001/api/tours?limit=1
```

### Test in browser
- Admin: http://admin.goreise.com/dashboard-main
- API: http://api.goreise.com/api/tours
- Frontend: http://tour.goreise.com/tours

## Backup Script (Optional)

Create automated backup script:
```bash
nano /home/deployer/backup-mongodb.sh
```

Add:
```bash
#!/bin/bash
BACKUP_DIR="/home/deployer/mongodb-backups"
DATE=$(date +%Y%m%d_%H%M%S)
mkdir -p $BACKUP_DIR

mongodump --uri="mongodb://localhost:27017/leetour" --out="$BACKUP_DIR/leetour-$DATE"
tar -czf "$BACKUP_DIR/leetour-$DATE.tar.gz" -C "$BACKUP_DIR" "leetour-$DATE"
rm -rf "$BACKUP_DIR/leetour-$DATE"

# Keep only last 7 backups
cd "$BACKUP_DIR" && ls -t leetour-*.tar.gz | tail -n +8 | xargs -r rm

echo "Backup completed: $BACKUP_DIR/leetour-$DATE.tar.gz"
```

Make executable:
```bash
chmod +x /home/deployer/backup-mongodb.sh
```

Run backup:
```bash
/home/deployer/backup-mongodb.sh
```

## Troubleshooting

### MongoDB won't start
```bash
sudo journalctl -u mongod -n 50
sudo cat /var/log/mongodb/mongod.log | tail -50
```

### Permission issues
```bash
sudo chown -R mongodb:mongodb /var/lib/mongodb
sudo chown mongodb:mongodb /tmp/mongodb-27017.sock
sudo systemctl restart mongod
```

### Connection refused
Check if MongoDB is listening:
```bash
sudo lsof -i :27017
```

Check firewall (if enabled):
```bash
sudo ufw status
sudo ufw allow 27017/tcp  # Only if needed for remote access
```

## Security Considerations

### Create MongoDB admin user (Recommended for production)
```bash
mongosh
use admin
db.createUser({
  user: "admin",
  pwd: "your-strong-password",
  roles: [ { role: "userAdminAnyDatabase", db: "admin" }, "readWriteAnyDatabase" ]
})
exit
```

Enable authentication:
```bash
sudo nano /etc/mongod.conf
```

Add:
```yaml
security:
  authorization: enabled
```

Restart MongoDB:
```bash
sudo systemctl restart mongod
```

Update connection strings to include credentials:
```
MONGODB_URI=mongodb://admin:your-strong-password@localhost:27017/leetour?authSource=admin
```

## Notes

- Always test on a staging environment first
- Keep regular backups of your database
- Monitor disk space on the server
- MongoDB Atlas can still be kept as a backup
- Local MongoDB should be faster than Atlas for production use
