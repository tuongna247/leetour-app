# LeeTour Frontend

User-facing website for browsing and booking tours.

## Overview

This is the public-facing Next.js application where users can:
- Browse available tours
- Search and filter tours
- View tour details
- Book tours
- Leave reviews

## Getting Started

### Prerequisites

- Node.js 18+ installed
- API server running on port 3001

### Installation

1. Install dependencies:
```bash
npm install
```

2. Create a `.env` file based on `.env.example`:
```bash
cp .env.example .env
```

3. Update the environment variables in `.env`:
```
NEXT_PUBLIC_API_URL=http://localhost:3001
```

### Running the Application

Development mode (runs on port 3002):
```bash
npm run dev
```

Production build:
```bash
npm run build
npm start
```

## Features

### Tour Browsing
- **Grid Layout**: Tours displayed in a responsive grid
- **Search**: Search tours by title, description, or location
- **Filters**: Filter by difficulty level
- **Sorting**: Sort by name, price, rating, or duration
- **Tour Cards**: Beautiful cards showing key tour information

### Tour Information Display
- Tour title and description
- Price per person
- Duration in days
- Maximum group size
- Difficulty level (Easy, Medium, Difficult)
- Rating and review count
- Location
- Next available tour date
- Cover image

## Project Structure

```
apps/frontend/
├── src/
│   ├── app/
│   │   ├── layout.js       # Root layout with theme
│   │   └── page.js         # Tours listing page
│   ├── components/
│   │   ├── Header.js       # Navigation header
│   │   └── TourCard.js     # Tour card component
│   └── lib/
│       ├── api.js          # API utilities
│       └── theme.js        # Material-UI theme
├── .env                    # Environment variables
├── .env.example            # Environment template
├── jsconfig.json           # JavaScript configuration
├── next.config.mjs         # Next.js configuration
├── package.json            # Dependencies
└── README.md               # This file
```

## Components

### Header
Navigation bar with:
- LeeTour logo
- Navigation links (Tours, About, Contact)
- Login/Sign Up buttons

### TourCard
Displays individual tour information:
- Cover image with difficulty badge
- Title and summary
- Location, duration, group size icons
- Rating display
- Price
- "View Details" button
- Hover effects for better UX

### ToursPage (Main Page)
Main tour listing page with:
- Hero section
- Search bar
- Difficulty filter
- Sort options
- Results count
- Tour grid
- Loading and error states
- Empty state

## API Integration

The frontend connects to the API server to fetch tour data:

```javascript
// Fetch all tours
GET http://localhost:3001/api/tours

// Fetch single tour
GET http://localhost:3001/api/tours/:id
```

## Styling

Uses Material-UI for consistent, modern design:
- Custom theme with primary/secondary colors
- Responsive layout with Grid system
- Typography hierarchy
- Interactive elements with hover states
- Loading and error states

## Development Notes

- The frontend runs on port 3002 by default
- Requires the API server to be running on port 3001
- Uses React 19 and Next.js 15
- Server components and client components are separated
- All data fetching is done client-side with React hooks

## Related Apps

- [API Server](../api/README.md) - Backend API (port 3001)
- [Admin App](../admin/README.md) - Admin dashboard (port 3000)

## Next Steps

To expand the frontend:
1. Add tour detail page
2. Add booking functionality
3. Add user authentication
4. Add review submission
5. Add user profile page
6. Add booking history
7. Add payment integration
