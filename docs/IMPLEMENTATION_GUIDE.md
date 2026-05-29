# Clinic Booking System - Complete Implementation Guide

## 📋 Overview

This document provides step-by-step instructions to set up, build, and run the complete Clinic Booking System with Blazor WebAssembly frontend and ASP.NET Core API backend.

---

## 🔧 System Architecture

```
┌─────────────────────────────────────────────┐
│     Blazor WebAssembly (WASM) Frontend      │
│  - MudBlazor Components                     │
│  - Real-time UI Updates                     │
│  - Client-side Validation                   │
└──────────────────┬──────────────────────────┘
                   │ HTTPS API Calls
                   ▼
┌─────────────────────────────────────────────┐
│   ASP.NET Core 8.0 REST API Backend         │
│  - RESTful Endpoints                        │
│  - JWT Authentication                       │
│  - Swagger Documentation                    │
└──────────────────┬──────────────────────────┘
                   │ EF Core ORM
                   ▼
┌─────────────────────────────────────────────┐
│     SQL Server Database                     │
│  - Entity Framework Core                    │
│  - Migrations                               │
│  - Data Persistence                         │
└─────────────────────────────────────────────┘
```

---

## 📦 Project Structure

```
Clinic/
├── src/
│   ├── ClinicBookingSystem.Api/
│   │   ├── Controllers/              # API Endpoints
│   │   ├── Services/                 # Business Logic
│   │   ├── Repositories/             # Data Access
│   │   ├── Validators/               # FluentValidation Rules
│   │   ├── Data/                     # DbContext & Migrations
│   │   ├── Mapping/                  # AutoMapper Profiles
│   │   ├── Program.cs                # Startup Configuration
│   │   └── appsettings.json          # Configuration
│   │
│   ├── ClinicBookingSystem.Wasm/
│   │   ├── Components/               # Blazor Components
│   │   ├── Pages/                    # Page Components
│   │   ├── Services/                 # API Service Layer
│   │   ├── Shared/                   # Shared UI Components
│   │   ├── Program.cs                # WASM Startup
│   │   └── wwwroot/                  # Static Assets
│   │
│   └── ClinicBookingSystem.Shared/
│       ├── Models/                   # Domain Models
│       ├── DTOs/                     # Data Transfer Objects
│       └── GlobalUsings.cs           # Global Usings
│
└── docs/
    └── IMPLEMENTATION_GUIDE.md       # This file
```

---

## 🚀 Prerequisites

Before you begin, ensure you have the following installed:

- **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** (or VS Code with C# extension)
- **SQL Server 2019+** or **SQL Server Express LocalDB**
- **Git** - for version control
- **Node.js** (optional, for frontend tooling)

### Verify Installation

```bash
dotnet --version
sqlserver --version  # If using SQL Server
```

---

## 📥 Installation & Setup

### Step 1: Clone the Repository

```bash
git clone https://github.com/siyandam256-alt/Clinic.git
cd Clinic
```

### Step 2: Check out Development Branch

```bash
git checkout dev
git pull origin dev
```

### Step 3: Restore NuGet Packages

```bash
dotnet restore
```

### Step 4: Update Database Connection String

Edit `src/ClinicBookingSystem.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ClinicBookingSystemDb;Trusted_Connection=true;"
  }
}
```

**Options:**
- **LocalDB**: `Server=(localdb)\\mssqllocaldb;Database=ClinicBookingSystemDb;Trusted_Connection=true;`
- **SQL Server Express**: `Server=.\\SQLEXPRESS;Database=ClinicBookingSystemDb;Trusted_Connection=true;`
- **Remote Server**: `Server=your-server-address;Database=ClinicBookingSystemDb;User Id=sa;Password=your-password;`

### Step 5: Create & Apply Database Migrations

```bash
cd src/ClinicBookingSystem.Api

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migrations to database
dotnet ef database update
```

**Verify Database Creation:**
- Open SQL Server Management Studio (SSMS)
- Connect to your server
- Look for `ClinicBookingSystemDb` database
- Verify all tables are created

---

## ▶️ Running the Application

### Option 1: Run Both Projects Simultaneously (Recommended)

#### Terminal 1 - Start API Backend

```bash
cd src/ClinicBookingSystem.Api
dotnet run
```

Expected output:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7000
      Now listening on: http://localhost:5000
```

#### Terminal 2 - Start Blazor WASM Frontend

```bash
cd src/ClinicBookingSystem.Wasm
dotnet run
```

Expected output:
```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:7001
      Now listening on: http://localhost:5001
```

### Option 2: Run from Visual Studio

1. Set as startup projects:
   - **Solution** → **Set Startup Projects**
   - Select **Multiple startup projects**
   - Set both `ClinicBookingSystem.Api` and `ClinicBookingSystem.Wasm` to **Start**
   - Click **OK**

2. Press **F5** to run

### Access the Application

- **Frontend**: https://localhost:7001
- **API**: https://localhost:7000
- **Swagger UI**: https://localhost:7000/swagger

---

## 🔌 API Endpoints

### Authentication

```
POST   /api/auth/login
POST   /api/auth/register
POST   /api/auth/logout
```

### Patients

```
GET    /api/patient                    # Get all patients
GET    /api/patient/{id}               # Get patient by ID
POST   /api/patient                    # Create new patient
PUT    /api/patient/{id}               # Update patient
DELETE /api/patient/{id}               # Delete patient
```

### Clinics

```
GET    /api/clinic                     # Get all clinics
GET    /api/clinic/{id}                # Get clinic by ID
POST   /api/clinic                     # Create clinic
PUT    /api/clinic/{id}                # Update clinic
```

### Providers

```
GET    /api/provider                   # Get all providers
GET    /api/provider/{id}              # Get provider by ID
POST   /api/provider                   # Create provider
PUT    /api/provider/{id}              # Update provider
```

### Appointments

```
GET    /api/appointment/{id}                           # Get appointment
GET    /api/appointment/patient/{patientId}            # Get patient appointments
GET    /api/appointment/provider/{providerId}/date/{date}  # Get provider appointments
GET    /api/appointment/clinic/{clinicId}/date/{date}      # Get clinic appointments
POST   /api/appointment                                # Book appointment
PUT    /api/appointment/{id}/confirm                   # Confirm appointment
PUT    /api/appointment/{id}/reschedule                # Reschedule appointment
DELETE /api/appointment/{id}                           # Cancel appointment
```

### Time Slots

```
GET    /api/timeslot/provider/{providerId}/date/{date}     # Get available slots
GET    /api/timeslot/provider/{providerId}                  # Get provider slots
POST   /api/timeslot                                        # Create slot
POST   /api/timeslot/bulk                                   # Create bulk slots
PUT    /api/timeslot/{id}/block                             # Block slot
PUT    /api/timeslot/{id}/unblock                           # Unblock slot
```

---

## 🧪 Testing the API

### Using Swagger UI

1. Navigate to: https://localhost:7000/swagger
2. Expand any endpoint
3. Click **Try it out**
4. Enter parameters
5. Click **Execute**

### Using cURL

```bash
# Get all patients
curl -X GET "https://localhost:7000/api/patient" \
  -H "accept: application/json" \
  --insecure

# Create patient
curl -X POST "https://localhost:7000/api/patient" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{"firstName":"John","lastName":"Doe","email":"john@example.com","phoneNumber":"1234567890","dateOfBirth":"1990-01-01","gender":"M","address":"123 Main St"}' \
  --insecure
```

---

## 📝 Database Schema

### Tables Created

1. **Patients**
   - PatientId (PK)
   - FirstName, LastName
   - Email (Unique Index)
   - PhoneNumber, DateOfBirth, Gender
   - Address
   - CreatedAt, UpdatedAt

2. **Clinics**
   - ClinicId (PK)
   - Name, Address, City, State, ZipCode
   - PhoneNumber, Email
   - WorkingHoursStart, WorkingHoursEnd
   - Latitude, Longitude
   - CreatedAt, UpdatedAt

3. **Providers**
   - ProviderId (PK)
   - ClinicId (FK)
   - FirstName, LastName, Specialization
   - LicenseNumber (Unique Index)
   - Email, PhoneNumber
   - CreatedAt, UpdatedAt

4. **TimeSlots**
   - TimeSlotId (PK)
   - ProviderId (FK)
   - StartTime, EndTime, Duration
   - IsAvailable, IsBlocked
   - CreatedAt, UpdatedAt
   - Index: (ProviderId, StartTime)

5. **Appointments**
   - AppointmentId (PK)
   - PatientId (FK), ClinicId (FK), ProviderId (FK), TimeSlotId (FK)
   - Status (Pending, Confirmed, Cancelled, Completed, NoShow)
   - BookedAt, ConfirmedAt, CancelledAt
   - UpdatedAt
   - Indexes: (PatientId, BookedAt), Status

6. **AppointmentNotes**
   - NoteId (PK)
   - AppointmentId (FK)
   - Notes
   - CreatedAt

---

## 🧑‍💼 Key Features Implemented

### ✅ Patient Management
- Register new patients
- View/update profile information
- View appointment history
- Download appointment confirmations

### ✅ Appointment Booking
- Browse available clinics and providers
- View real-time time slots
- Book appointments with validation
- Prevent double bookings
- Receive confirmation messages
- Reschedule appointments
- Cancel appointments

### ✅ Time Slot Management
- Create individual time slots
- Create bulk time slots for date ranges
- Block/unblock time slots for breaks
- Check availability in real-time
- Automatic time slot marking when booked

### ✅ Data Validation
- FluentValidation on all inputs
- Server-side business rule validation
- Email uniqueness validation
- Date/time validation
- Phone number format validation

### ✅ Error Handling
- Comprehensive exception handling
- Meaningful error messages
- API response wrapping
- Logging with Serilog

### ✅ API Documentation
- Swagger/OpenAPI integration
- XML documentation on endpoints
- Request/response models documented
- Interactive API testing

---

## 🔐 Security Features Implemented

1. **Input Validation**
   - Client-side validation in Blazor
   - Server-side validation with FluentValidation
   - SQL injection prevention (EF Core parameterization)

2. **CORS Configuration**
   - Restricted to localhost origins
   - Can be configured for production

3. **HTTPS/TLS**
   - All communication encrypted
   - Self-signed certificates for development

4. **Logging**
   - Serilog configured for structured logging
   - Logs written to console and file
   - Log rolling by day

---

## 📊 Database Migrations

### View Migration History

```bash
cd src/ClinicBookingSystem.Api
dotnet ef migrations list
```

### Create New Migration (After Model Changes)

```bash
dotnet ef migrations add DescriptionOfChanges
```

### Revert Migration

```bash
dotnet ef database update <PreviousMigrationName>
```

### Drop Database

```bash
dotnet ef database drop
```

---

## 🐛 Troubleshooting

### Issue: "Cannot connect to database"

**Solution:**
```bash
# Check connection string in appsettings.json
# Verify SQL Server is running
# Try LocalDB:
sqlcmd -S (localdb)\mssqllocaldb
```

### Issue: "Migrations pending"

**Solution:**
```bash
cd src/ClinicBookingSystem.Api
dotnet ef database update
```

### Issue: "Port already in use"

**Solution:**
```bash
# Change port in launchSettings.json or use:
dotnet run --urls "https://localhost:7002"
```

### Issue: "CORS error when calling API"

**Solution:**
- Verify API is running on https://localhost:7000
- Check CORS policy in Program.cs
- Ensure frontend URL is in allowed origins

---

## 📚 Development Workflow

### Adding New Features

1. **Create Domain Model** in `Shared/Models/`
2. **Create DTOs** in `Shared/DTOs/`
3. **Add Database Migration**:
   ```bash
   dotnet ef migrations add FeatureName
   ```
4. **Create Repository Interface** in `Api/Repositories/`
5. **Implement Repository** in `Api/Repositories/`
6. **Create Service Interface** in `Api/Services/`
7. **Implement Service** in `Api/Services/`
8. **Add Validator** in `Api/Validators/`
9. **Create Controller** in `Api/Controllers/`
10. **Create Blazor Component** in `Wasm/Components/`
11. **Add Integration Tests**

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test tests/ClinicBookingSystem.Tests

# With code coverage
dotnet test /p:CollectCoverageMetrics=true
```

---

## 🚢 Deployment

### Docker Deployment

```bash
# Build Docker image
docker build -t clinic-booking-system .

# Run container
docker run -p 7000:7000 -p 7001:7001 clinic-booking-system
```

### Azure Deployment

```bash
# Publish to Azure App Service
dotnet publish -c Release -o ./publish

# Using Azure CLI
az webapp deployment source config-zip \
  --resource-group myResourceGroup \
  --name myAppService \
  --src-path ./publish.zip
```

### Local IIS Deployment

```bash
# Publish for IIS
dotnet publish -c Release -o ./iis-publish

# Copy to IIS folder
# Configure IIS application pool and website
```

---

## 📚 Useful Commands

```bash
# Build solution
dotnet build

# Run tests with coverage
dotnet test /p:CollectCoverageMetrics=true

# Clean build
dotnet clean && dotnet build

# Update packages
dotnet package update

# Check for vulnerabilities
dotnet list package --vulnerable

# Generate code analysis
dotnet build /p:EnableNETAnalyzers=true
```

---

## 📞 Support & Documentation

- **API Documentation**: https://localhost:7000/swagger
- **Practical Assessment**: See `PRACTICAL_ASSESSMENT.md`
- **Architecture Guide**: See `docs/ARCHITECTURE.md`
- **Database Schema**: See `docs/DATABASE.md`

---

## ✅ Success Checklist

- [ ] .NET 8.0 SDK installed
- [ ] SQL Server installed and running
- [ ] Repository cloned and dev branch checked out
- [ ] NuGet packages restored
- [ ] Database connection string configured
- [ ] Database migrations applied
- [ ] API running on https://localhost:7000
- [ ] Blazor frontend running on https://localhost:7001
- [ ] Swagger UI accessible
- [ ] Can create/read/update/delete records
- [ ] API and frontend can communicate

---

## 🎯 Next Steps

1. **Add Authentication**: Implement JWT-based authentication
2. **Add Email Notifications**: Integrate SendGrid for appointment confirmations
3. **Add Calendar Integration**: Sync with Outlook/Google Calendar
4. **Add Real-time Updates**: Implement SignalR for live notifications
5. **Add Analytics**: Dashboard with booking statistics
6. **Add Video Consultations**: Integrate Twilio Video API
7. **Performance Optimization**: Implement caching and query optimization
8. **Mobile App**: Create React Native or Flutter mobile app

---

**Last Updated**: May 29, 2026

**Version**: 1.0.0

**Status**: ✅ Ready for Development
