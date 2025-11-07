╔══════════════════════════════════════════════════════════════════════════╗
║                                                                          ║
║                   LEETOUR DEPLOYMENT DOCUMENTATION                       ║
║                         Complete Package                                 ║
║                                                                          ║
╚══════════════════════════════════════════════════════════════════════════╝

📁 DOCUMENTATION FILES CREATED
═══════════════════════════════════════════════════════════════════════════

1. DEPLOYMENT_README.md
   📖 Main deployment documentation hub
   • Quick overview of all resources
   • Links to all other documentation
   • Quick reference commands
   • Monitoring and maintenance guides

2. QUICK_START.md
   ⚡ For beginners and quick deployments
   • Step-by-step first deployment
   • SSH setup guide
   • Quick command reference
   • Troubleshooting common issues

3. DEPLOYMENT_GUIDE.md
   📚 Complete detailed guide
   • Full server setup instructions
   • Nginx configuration
   • SSL certificate setup
   • Security best practices
   • Detailed troubleshooting

4. ENV_SETUP.md
   🔐 Environment configuration guide
   • Environment variables templates
   • MongoDB setup (local & Atlas)
   • Cloudinary configuration
   • API keys setup
   • Secret generation

5. ARCHITECTURE.md
   🏗️ System architecture documentation
   • Infrastructure diagrams
   • Request flow diagrams
   • Application structure
   • Security layers
   • Data flow explanation

═══════════════════════════════════════════════════════════════════════════

🛠️ HELPER SCRIPTS CREATED
═══════════════════════════════════════════════════════════════════════════

1. connect-server.sh (Mac/Linux)
   Interactive menu for:
   • Connecting to server
   • Deploying updates
   • Viewing PM2 status
   • Viewing logs
   • Restarting applications
   • Copying environment files

2. connect-server.bat (Windows)
   Same features as .sh version for Windows users

3. deploy.sh (Already existed on server)
   Automated deployment script that:
   • Pulls latest code
   • Installs dependencies
   • Builds applications
   • Restarts PM2 processes
   • Performs health checks

═══════════════════════════════════════════════════════════════════════════

📋 QUICK START CHECKLIST
═══════════════════════════════════════════════════════════════════════════

□ Step 1: Read QUICK_START.md
          Get familiar with the deployment process

□ Step 2: Test Server Connection
          ssh leetour

□ Step 3: Setup SSH Key (optional but recommended)
          ssh-keygen -t rsa -b 4096
          ssh-copy-id leetour

□ Step 4: Create Environment Files
          Follow ENV_SETUP.md to create:
          • apps/admin/.env.local
          • apps/api/.env
          • apps/frontend/.env

□ Step 5: Copy Environment Files to Server
          Use connect-server.bat/sh (option 6)
          OR manually with scp

□ Step 6: Configure DNS
          Add A records:
          • admin.goreise.com → 157.173.124.250
          • api.goreise.com → 157.173.124.250
          • tour.goreise.com → 157.173.124.250

□ Step 7: Deploy Application
          Run: ./connect-server.sh (option 2)
          OR: ssh leetour "cd /var/www/leetour && ./deploy.sh"

□ Step 8: Setup SSL (HTTPS)
          ssh leetour
          sudo certbot --nginx -d admin.goreise.com
          sudo certbot --nginx -d api.goreise.com
          sudo certbot --nginx -d tour.goreise.com

□ Step 9: Verify Deployment
          Check: https://admin.goreise.com
          Check: https://api.goreise.com
          Check: https://tour.goreise.com

═══════════════════════════════════════════════════════════════════════════

🌐 SERVER INFORMATION
═══════════════════════════════════════════════════════════════════════════

Server IP:    157.173.124.250
SSH User:     deployer
App Path:     /var/www/leetour

Applications:
• Admin Panel:  admin.goreise.com  → Port 3000 → PM2: leetour-admin
• API Server:   api.goreise.com    → Port 3001 → PM2: leetour-api
• Frontend:     tour.goreise.com   → Port 3002 → PM2: leetour-frontend

Technology Stack:
• Runtime:      Node.js 18+
• Process Mgr:  PM2
• Web Server:   Nginx
• Database:     MongoDB
• SSL:          Let's Encrypt (Certbot)

═══════════════════════════════════════════════════════════════════════════

⚡ QUICK COMMANDS
═══════════════════════════════════════════════════════════════════════════

CONNECT TO SERVER:
ssh leetour
ssh leetour

DEPLOY UPDATES:
ssh leetour "cd /var/www/leetour && ./deploy.sh"

CHECK STATUS:
ssh leetour "pm2 status"

VIEW LOGS:
ssh leetour "pm2 logs"

RESTART ALL APPS:
ssh leetour "pm2 restart all"

RESTART SPECIFIC APP:
ssh leetour "pm2 restart leetour-admin"
ssh leetour "pm2 restart leetour-api"
ssh leetour "pm2 restart leetour-frontend"

═══════════════════════════════════════════════════════════════════════════

🆘 TROUBLESHOOTING
═══════════════════════════════════════════════════════════════════════════

Can't connect to server?
→ Check: ping 157.173.124.250
→ Verify: SSH key or password
→ See: QUICK_START.md "Common Issues" section

Application not starting?
→ Check: pm2 logs leetour-admin --lines 50
→ Restart: pm2 restart leetour-admin
→ See: DEPLOYMENT_GUIDE.md "Troubleshooting" section

502 Bad Gateway?
→ Check: pm2 status (are apps running?)
→ Check: sudo systemctl status nginx
→ View: sudo tail -f /var/log/nginx/error.log

Database connection failed?
→ Check: sudo systemctl status mongod
→ Verify: MONGODB_URI in .env files
→ See: ENV_SETUP.md "MongoDB Setup" section

═══════════════════════════════════════════════════════════════════════════

📞 WHERE TO GET HELP
═══════════════════════════════════════════════════════════════════════════

Start Here:
1. QUICK_START.md - For quick deployment
2. DEPLOYMENT_GUIDE.md - For detailed instructions
3. ENV_SETUP.md - For environment configuration
4. ARCHITECTURE.md - To understand system design

View Logs:
• pm2 logs
• sudo tail -f /var/log/nginx/error.log

External Resources:
• PM2: https://pm2.keymetrics.io/docs/
• Nginx: https://nginx.org/en/docs/
• Next.js: https://nextjs.org/docs/deployment
• MongoDB: https://www.mongodb.com/docs/

═══════════════════════════════════════════════════════════════════════════

💡 PRO TIPS
═══════════════════════════════════════════════════════════════════════════

1. Use Helper Scripts
   • Windows: Double-click connect-server.bat
   • Mac/Linux: ./connect-server.sh

2. Create SSH Alias
   Add to ~/.ssh/config:
   Host leetour
       HostName 157.173.124.250
       User deployer

   Then use: ssh leetour

3. Bookmark Documentation Files
   Keep QUICK_START.md and DEPLOYMENT_GUIDE.md handy

4. Monitor Regularly
   Check pm2 status daily
   Review logs weekly

5. Keep Environment Files Secure
   Never commit .env files to Git
   Keep backups in secure location

═══════════════════════════════════════════════════════════════════════════

✅ DEPLOYMENT VERIFICATION
═══════════════════════════════════════════════════════════════════════════

After deployment, verify:

□ pm2 status shows 3 applications running
□ Admin panel loads: https://admin.goreise.com
□ API responds: https://api.goreise.com/api/health
□ Frontend loads: https://tour.goreise.com
□ SSL certificates working (HTTPS)
□ Can create/edit tours in admin
□ Tours display on frontend
□ No errors in pm2 logs

═══════════════════════════════════════════════════════════════════════════

🎯 NEXT STEPS
═══════════════════════════════════════════════════════════════════════════

1. [ ] Read documentation files
2. [ ] Setup SSH access to server
3. [ ] Create environment files
4. [ ] Deploy application
5. [ ] Setup SSL certificates
6. [ ] Test all features
7. [ ] Setup monitoring
8. [ ] Configure regular backups

═══════════════════════════════════════════════════════════════════════════

📅 CREATED: 2025-11-06
📝 VERSION: 1.0
🚀 STATUS: Ready for Production

═══════════════════════════════════════════════════════════════════════════

                         HAPPY DEPLOYING! 🚀

═══════════════════════════════════════════════════════════════════════════
7FYq4p9f
ssh leetour "echo 'SSH connection successful'"