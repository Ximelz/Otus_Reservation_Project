# Hotel Admin System - Demo Guide

## Quick Start

### Prerequisites
- .NET 9.0 SDK
- Node.js 18+
- PostgreSQL 16 (via Docker or local)
- RabbitMQ 3.x (via Docker or local)

### 1. Start Infrastructure
```bash
docker compose up -d postgres rabbitmq
```

This starts:
- PostgreSQL on port **5432** (user: postgres, password: 12345)
- RabbitMQ on port **5672** (management UI: http://localhost:15672, guest/guest)

### 2. Start Hotels Service
```bash
cd Services/Hotels/Hotels.WebApi
dotnet run
```
- Runs on **http://localhost:12254**
- Automatically applies migrations and seeds demo data on first run
- Swagger: http://localhost:12254/swagger

### 3. Start Admin API
```bash
cd Services/Admin/Admin.WebApi
dotnet run
```
- Runs on **http://localhost:5000**
- Swagger: http://localhost:5000/swagger

### 4. Start Frontend
```bash
cd Services/Admin/admin-ui
npm install
npm run dev
```
- Opens on **http://localhost:5173**
- Proxies API calls to Admin.WebApi

---

## Demo Credentials

| User | Password | Role |
|------|----------|------|
| admin | admin | Admin |

These are configured in `Admin.WebApi/appsettings.Development.json` under `Admin:DevAuth`.

---

## Demo Script (Video Recording)

### Scene 1: Login
1. Open http://localhost:5173
2. Login with admin / admin
3. Show the dashboard loads with live data

### Scene 2: Dashboard Overview
1. Point out KPI cards: Hotels, Total Rooms, Available, Occupied, Dirty, Out of Service
2. Show the occupancy rate bar
3. Show the hotel comparison bar chart
4. Show hotel cards with occupancy percentages
5. Scroll to recent activity feed

### Scene 3: Hotels Management
1. Click "Hotels" in sidebar
2. Show 3 demo hotels with star ratings, cities, room counts
3. Use search to filter by name
4. Click on "Grand Aurora Hotel"

### Scene 4: Hotel Details
1. Show hotel info: address, phone, email, check-in/out times
2. Show amenities badges (WiFi, Parking, Breakfast, etc.)
3. Show hotel policy (cancellation, early check-in, late check-out)
4. Show room types list with capacity and area info
5. Click "Manage Rooms"

### Scene 5: Rooms Management
1. Show rooms table with inline status dropdowns
2. Change a room status from Available to Occupied
3. Change housekeeping from Dirty to Clean
4. Show the table updates in real-time

### Scene 6: Housekeeping Board
1. Click "Housekeeping" in sidebar
2. Select a hotel from the dropdown
3. Show floor-grouped room grid with color-coded statuses
4. Show summary counters (Clean/Dirty/Inspected/Out of Service)
5. Click a room to cycle its housekeeping status
6. Show the visual update

### Scene 7: Rate Plans
1. Navigate to hotel detail > Rate Plans
2. Show rate plans table with pricing, cancellation policies
3. Point out Default, Breakfast Included, Non-Refundable badges

### Scene 8: Activity Feed
1. Click "Activity" in sidebar
2. Show timeline of all system events
3. Point out different activity types with icons and colors

---

## Demo Data

### Hotels
1. **Grand Aurora Hotel** - 5 stars, Moscow, Tverskaya st.
2. **Riverside Business Hotel** - 4 stars, St. Petersburg, Nevsky pr.
3. **Old Town Boutique Suites** - 3 stars, Kazan, Bauman st.

### Per Hotel
- 5 room types (Standard, Superior, Deluxe, Family Suite, Executive Suite)
- 16 rooms across 4 floors with varied statuses
- 15 rate plans (Standard, With Breakfast, Non-Refundable per type)
- 7 amenities
- Hotel policy with check-in/out rules
- Activity logs

---

## Architecture Notes

- **Hotels.WebApi**: Source of truth for hotel data, runs MassTransit with RabbitMQ
- **Admin.WebApi**: BFF for the React frontend, directly accesses Hotels database for admin operations
- **admin-ui**: React+Vite SPA with dark luxury theme
- **Shared.Contracts**: Integration events and shared enums
- **Gateway (Ocelot)**: Routes API calls to appropriate services

## Ports
| Service | Port |
|---------|------|
| PostgreSQL | 5432 |
| RabbitMQ | 5672 |
| RabbitMQ Management | 15672 |
| Hotels.WebApi | 12254 |
| Admin.WebApi | 5000 |
| Frontend (dev) | 5173 |
| Gateway | 12321 |
