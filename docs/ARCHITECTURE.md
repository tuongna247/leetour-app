# LeeTour Application Architecture

## 🏗️ Infrastructure Overview

```
┌─────────────────────────────────────────────────────────────────────┐
│                         INTERNET                                     │
│                                                                       │
│  Users access via:                                                   │
│  • admin.goreise.com  (Admin Dashboard)                             │
│  • api.goreise.com    (API Server)                                  │
│  • tour.goreise.com   (Public Frontend)                             │
└────────────────────────┬────────────────────────────────────────────┘
                         │
                         │ HTTPS (Port 443) / HTTP (Port 80)
                         │
                         ▼
┌─────────────────────────────────────────────────────────────────────┐
│                    DNS PROVIDER (Domain Registrar)                   │
│                                                                       │
│  A Records:                                                          │
│  • admin.goreise.com  → 157.173.124.250                             │
│  • api.goreise.com    → 157.173.124.250                             │
│  • tour.goreise.com   → 157.173.124.250                             │
└────────────────────────┬────────────────────────────────────────────┘
                         │
                         │
                         ▼
┌─────────────────────────────────────────────────────────────────────┐
│                  SERVER: 157.173.124.250                             │
│                  User: deployer                                      │
│                                                                       │
│  ┌───────────────────────────────────────────────────────────────┐ │
│  │                    NGINX (Reverse Proxy)                       │ │
│  │                                                                 │ │
│  │  Port 80/443 → Routes traffic based on domain:                │ │
│  │                                                                 │ │
│  │  admin.goreise.com  → localhost:3000                          │ │
│  │  api.goreise.com    → localhost:3001                          │ │
│  │  tour.goreise.com   → localhost:3002                          │ │
│  │                                                                 │ │
│  │  • SSL Termination (Let's Encrypt certificates)               │ │
│  │  • Load Balancing                                              │ │
│  │  • Rate Limiting                                               │ │
│  │  • CORS Headers                                                │ │
│  └──────────┬────────────────┬────────────────┬──────────────────┘ │
│             │                │                │                     │
│             │                │                │                     │
│  ┌──────────▼────────┐  ┌───▼──────────┐  ┌──▼──────────────┐    │
│  │                    │  │              │  │                  │    │
│  │  PM2 PROCESS      │  │ PM2 PROCESS  │  │  PM2 PROCESS     │    │
│  │  leetour-admin    │  │ leetour-api  │  │  leetour-frontend│    │
│  │                    │  │              │  │                  │    │
│  │  Port: 3000       │  │ Port: 3001   │  │  Port: 3002      │    │
│  │  Next.js App      │  │ Next.js App  │  │  Next.js App     │    │
│  │                    │  │              │  │                  │    │
│  │  Features:         │  │ Features:    │  │  Features:       │    │
│  │  • User Auth      │  │ • REST API   │  │  • Tour Listing  │    │
│  │  • Tour CRUD      │  │ • Database   │  │  • Search        │    │
│  │  • Image Upload   │  │ • Business   │  │  • Booking       │    │
│  │  • Reviews        │  │   Logic      │  │  • Reviews       │    │
│  │  • Reports        │  │ • Auth       │  │  • User Profile  │    │
│  │                    │  │              │  │                  │    │
│  └──────────┬─────────┘  └───┬──────────┘  └──┬───────────────┘    │
│             │                │                │                     │
│             │                │                │                     │
│             └────────────────┴────────────────┘                     │
│                              │                                       │
│                              ▼                                       │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    MongoDB Database                           │  │
│  │                                                                │  │
│  │  Port: 27017 (localhost) or MongoDB Atlas (cloud)            │  │
│  │                                                                │  │
│  │  Collections:                                                 │  │
│  │  • users (admin accounts)                                     │  │
│  │  • tours (tour information)                                   │  │
│  │  • bookings (reservations)                                    │  │
│  │  • reviews (customer reviews)                                 │  │
│  │  • sessions (NextAuth sessions)                               │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    File System                                 │  │
│  │                                                                │  │
│  │  /var/www/leetour/                                            │  │
│  │  ├── apps/                                                     │  │
│  │  │   ├── admin/      (Admin app code)                         │  │
│  │  │   ├── api/        (API app code)                           │  │
│  │  │   └── frontend/   (Frontend app code)                      │  │
│  │  ├── logs/           (Application logs)                       │  │
│  │  ├── backups/        (Deployment backups)                     │  │
│  │  └── public/         (Static files)                           │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

---

## 🔄 Request Flow

### Admin Dashboard Request

```
User Browser
    │
    │ HTTPS Request to admin.goreise.com
    │
    ▼
Nginx (Port 443)
    │
    │ Checks domain = admin.goreise.com
    │ Proxy pass to localhost:3000
    │
    ▼
PM2: leetour-admin (Port 3000)
    │
    │ Next.js Server-Side Rendering
    │ Authentication check (NextAuth)
    │
    ▼
MongoDB
    │
    │ Fetch user, tours, bookings data
    │
    ▼
Response sent back through chain
```

### API Request

```
Admin or Frontend App
    │
    │ HTTPS Request to api.goreise.com/api/tours
    │
    ▼
Nginx (Port 443)
    │
    │ Checks domain = api.goreise.com
    │ Adds CORS headers
    │ Proxy pass to localhost:3001
    │
    ▼
PM2: leetour-api (Port 3001)
    │
    │ Next.js API Routes
    │ JWT Authentication
    │ Business logic
    │
    ▼
MongoDB
    │
    │ CRUD operations
    │
    ▼
JSON Response sent back through chain
```

### Frontend Request

```
User Browser
    │
    │ HTTPS Request to tour.goreise.com
    │
    ▼
Nginx (Port 443)
    │
    │ Checks domain = tour.goreise.com
    │ Proxy pass to localhost:3002
    │
    ▼
PM2: leetour-frontend (Port 3002)
    │
    │ Next.js SSR/SSG
    │ Fetch data from api.goreise.com
    │
    ▼
Response sent to user browser
```

---

## 📦 Application Structure

```
leetour-app/
│
├── apps/
│   │
│   ├── admin/                    # Admin Dashboard
│   │   ├── src/
│   │   │   ├── app/              # Next.js 14 App Router
│   │   │   │   ├── api/          # API routes (NextAuth, uploads)
│   │   │   │   ├── (DashboardLayout)/
│   │   │   │   │   └── admin/    # Admin pages
│   │   │   │   │       ├── tours/
│   │   │   │   │       ├── bookings/
│   │   │   │   │       └── reviews/
│   │   │   │   └── page.jsx      # Login page
│   │   │   ├── components/       # React components
│   │   │   ├── contexts/         # React contexts (Auth)
│   │   │   ├── models/           # Mongoose models
│   │   │   └── lib/              # Utilities
│   │   ├── public/               # Static assets
│   │   ├── .env.local            # Environment variables
│   │   └── package.json
│   │
│   ├── api/                      # API Server
│   │   ├── src/
│   │   │   ├── app/
│   │   │   │   └── api/          # API endpoints
│   │   │   │       ├── tours/
│   │   │   │       ├── bookings/
│   │   │   │       └── auth/
│   │   │   ├── models/           # Mongoose models
│   │   │   ├── middleware/       # Auth, CORS, etc.
│   │   │   └── lib/              # Utilities
│   │   ├── .env                  # Environment variables
│   │   └── package.json
│   │
│   └── frontend/                 # Public Frontend
│       ├── src/
│       │   ├── app/              # Next.js pages
│       │   │   ├── tours/
│       │   │   ├── booking/
│       │   │   └── profile/
│       │   ├── components/       # React components
│       │   └── lib/              # Utilities
│       ├── public/               # Static assets
│       ├── .env                  # Environment variables
│       └── package.json
│
├── ecosystem.config.js           # PM2 configuration
├── nginx-leetour.conf            # Nginx configuration
├── deploy.sh                     # Deployment script
├── connect-server.sh/.bat        # Connection helpers
│
└── Documentation/
    ├── DEPLOYMENT_README.md
    ├── DEPLOYMENT_GUIDE.md
    ├── QUICK_START.md
    ├── ENV_SETUP.md
    └── ARCHITECTURE.md (this file)
```

---

## 🔐 Security Layers

```
┌─────────────────────────────────────────────────────────────┐
│ Layer 1: Network Security                                   │
│ • Firewall (UFW): Allow only 22, 80, 443                   │
│ • SSH Key Authentication                                     │
│ • Fail2Ban: Block brute force attacks                       │
└─────────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│ Layer 2: Nginx Security                                     │
│ • SSL/TLS Encryption (Let's Encrypt)                        │
│ • Rate Limiting                                              │
│ • DDoS Protection                                            │
│ • Security Headers (HSTS, CSP, etc.)                        │
└─────────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│ Layer 3: Application Security                               │
│ • NextAuth.js (Admin authentication)                        │
│ • JWT Tokens (API authentication)                           │
│ • CSRF Protection                                            │
│ • Input Validation                                           │
│ • XSS Prevention                                             │
└─────────────────────────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────┐
│ Layer 4: Database Security                                  │
│ • MongoDB Authentication                                     │
│ • Network Restriction (localhost or IP whitelist)          │
│ • Encrypted Connections                                      │
│ • Regular Backups                                            │
└─────────────────────────────────────────────────────────────┘
```

---

## 🔄 Deployment Workflow

```
Developer Machine                    GitHub                    Production Server
─────────────────                  ──────────                 ──────────────────

1. Code Changes
   │
   │ git add .
   │ git commit -m "..."
   │ git push origin main
   │
   ▼
                                  2. Repository Updated
                                     │
                                     │
                                     ▼
                                                            3. SSH to Server
                                                               │
                                                               │ ssh deployer@IP
                                                               │
                                                               ▼
                                                            4. Pull Latest Code
                                                               │
                                                               │ git pull
                                                               │
                                                               ▼
                                                            5. Run Deploy Script
                                                               │
                                                               │ ./deploy.sh
                                                               │
                                                               ├─ Create Backup
                                                               ├─ Install Dependencies
                                                               ├─ Build Apps
                                                               ├─ Restart PM2
                                                               └─ Health Check
                                                               │
                                                               ▼
                                                            6. Applications Running
                                                               │
                                                               ├─ leetour-admin (3000)
                                                               ├─ leetour-api (3001)
                                                               └─ leetour-frontend (3002)
```

---

## 📊 Data Flow

### Tour Creation Flow

```
1. Admin creates tour
   Admin Dashboard (admin.goreise.com)
   │
   │ POST /api/admin/tours
   │ with tour data + images
   │
   ▼
2. Images uploaded to Cloudinary
   Cloudinary CDN
   │
   │ Returns image URLs
   │
   ▼
3. Tour saved to database
   MongoDB
   │
   │ tour document created
   │
   ▼
4. Success response
   Admin Dashboard shows confirmation
```

### Booking Flow

```
1. User browses tours
   Frontend (tour.goreise.com)
   │
   │ GET https://api.goreise.com/api/tours
   │
   ▼
2. API fetches tours
   MongoDB
   │
   │ Returns tour list
   │
   ▼
3. User selects tour and books
   │
   │ POST https://api.goreise.com/api/bookings
   │
   ▼
4. Booking saved
   MongoDB
   │
   │ Creates booking document
   │ Updates tour capacity
   │
   ▼
5. Confirmation email sent
   Email Service (optional)
```

---

## 🔧 Monitoring & Logging

```
┌─────────────────────────────────────────────────────────────┐
│                    PM2 Process Manager                       │
│                                                               │
│  • Process monitoring                                        │
│  • Auto-restart on crash                                     │
│  • Log rotation                                              │
│  • CPU/Memory monitoring                                     │
│  • Cluster mode support                                      │
└──────────────────────┬────────────────────────────────────────┘
                       │
                       ├─ Application Logs
                       │  └─ /var/www/leetour/logs/
                       │     ├─ admin-out.log
                       │     ├─ admin-error.log
                       │     ├─ api-out.log
                       │     ├─ api-error.log
                       │     ├─ frontend-out.log
                       │     └─ frontend-error.log
                       │
                       └─ Nginx Logs
                          └─ /var/log/nginx/
                             ├─ leetour-admin-access.log
                             ├─ leetour-admin-error.log
                             ├─ leetour-api-access.log
                             ├─ leetour-api-error.log
                             ├─ leetour-frontend-access.log
                             └─ leetour-frontend-error.log
```

---

## 🌐 External Services Integration

```
LeeTour Application
       │
       ├─ MongoDB Atlas (optional)
       │  └─ Cloud database hosting
       │     • Automatic backups
       │     • Monitoring
       │     • Scaling
       │
       ├─ Cloudinary
       │  └─ Image/Media hosting
       │     • CDN delivery
       │     • Image optimization
       │     • Transformations
       │
       ├─ Google OAuth (optional)
       │  └─ Admin authentication
       │     • Social login
       │
       ├─ Google Maps (optional)
       │  └─ Location services
       │     • Map display
       │     • Geocoding
       │
       └─ Email Service (optional)
          └─ Booking confirmations
              • SendGrid
              • AWS SES
              • Mailgun
```

---

## 🚦 Traffic Distribution

```
Average Daily Traffic Distribution:

Frontend (tour.goreise.com)     70%  ████████████████████
    └─ Public users browsing tours

Admin (admin.goreise.com)       15%  ████████
    └─ Staff managing content

API (api.goreise.com)           15%  ████████
    └─ Direct API calls from mobile apps (future)
```

---

## 💾 Backup Strategy

```
┌─────────────────────────────────────────────────────────────┐
│                    Backup Hierarchy                          │
│                                                               │
│  1. Application Code                                         │
│     • Git repository (primary backup)                        │
│     • Server: /var/www/leetour/backups/                     │
│     • Created on each deployment                             │
│                                                               │
│  2. Database                                                 │
│     • MongoDB dumps                                          │
│     • Daily automated backups (recommended)                  │
│     • Stored off-server                                      │
│                                                               │
│  3. Environment Files                                        │
│     • .env files (NOT in git)                               │
│     • Secure backup location                                 │
│     • Encrypted storage                                      │
│                                                               │
│  4. Uploaded Files                                           │
│     • Cloudinary (automatically backed up)                   │
│     • Or: /var/www/leetour/public/uploads/                  │
└─────────────────────────────────────────────────────────────┘
```

---

## 📈 Scaling Options (Future)

### Horizontal Scaling

```
Current Setup (Single Server)
┌──────────────────────┐
│   Server (All-in-one)│
│  • Admin             │
│  • API               │
│  • Frontend          │
│  • MongoDB           │
└──────────────────────┘

Future: Multiple Servers
┌──────────────────────┐    ┌──────────────────────┐
│  App Server 1        │    │  App Server 2        │
│  • Admin             │    │  • Admin             │
│  • API               │    │  • API               │
│  • Frontend          │    │  • Frontend          │
└──────────┬───────────┘    └──────────┬───────────┘
           │                           │
           └───────────┬───────────────┘
                       │
          ┌────────────▼──────────────┐
          │  Load Balancer            │
          └────────────┬──────────────┘
                       │
          ┌────────────▼──────────────┐
          │  Database Server          │
          │  • MongoDB                │
          └───────────────────────────┘
```

### Vertical Scaling

```
Current: Basic VPS
• 2 CPU cores
• 4 GB RAM
• 80 GB SSD

Upgrade Options:
• 4 CPU cores
• 8 GB RAM
• 160 GB SSD
```

---

## 🎯 Performance Optimization

```
1. Application Level
   ├─ Next.js Static Generation (SSG)
   ├─ API Response Caching
   ├─ Database Indexing
   └─ Code Splitting

2. Server Level
   ├─ PM2 Cluster Mode (multiple instances)
   ├─ Nginx Caching
   ├─ Gzip Compression
   └─ HTTP/2 Support

3. Database Level
   ├─ Query Optimization
   ├─ Proper Indexing
   ├─ Connection Pooling
   └─ MongoDB Replica Sets (future)

4. CDN Level
   ├─ Cloudinary for images
   ├─ Static assets on CDN
   └─ Geographic distribution
```

---

## 📱 Future Enhancements

- [ ] Mobile App (React Native)
- [ ] Redis Caching Layer
- [ ] Elasticsearch for Search
- [ ] WebSocket for Real-time Updates
- [ ] Container Deployment (Docker)
- [ ] CI/CD Pipeline (GitHub Actions)
- [ ] Monitoring Dashboard (PM2 Plus)
- [ ] Analytics Integration
- [ ] Multi-language Support
- [ ] Progressive Web App (PWA)

---

For more details on deployment, see [DEPLOYMENT_README.md](./DEPLOYMENT_README.md)
