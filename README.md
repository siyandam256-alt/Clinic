# Clinic Booking System - PHCIS

A comprehensive, enterprise-grade Clinic Booking System built for Primary Health Care Information System (PHCIS). This system enables patients to seamlessly book, reschedule, and cancel appointments while providing clinic staff and administrators with robust management tools.

## 🎯 Project Overview

This practical assessment demonstrates a complete implementation of a modern healthcare booking platform using cutting-edge technologies and industry best practices.

**Repository**: [siyandam256-alt/Clinic](https://github.com/siyandam256-alt/Clinic)

---

## 📋 Table of Contents

- [Quick Start](#quick-start)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Documentation](#documentation)
- [Contributing](#contributing)

---

## 🚀 Quick Start

### Prerequisites
- .NET 8.0 SDK or higher
- SQL Server 2019+ or PostgreSQL 12+
- Visual Studio 2022 or VS Code
- Git

### Installation

```bash
# Clone the repository
git clone https://github.com/siyandam256-alt/Clinic.git
cd Clinic

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the API
cd src/ClinicBookingSystem.Api
dotnet run

# Run the Blazor WASM frontend (in another terminal)
cd src/ClinicBookingSystem.Wasm
dotnet run
```

The application will be available at:
- **API**: https://localhost:7000
- **Frontend**: https://localhost:7001
- **Swagger UI**: https://localhost:7000/swagger

---

## ✨ Features

### Patient Portal
- ✅ User registration and authentication
- ✅ Browse available clinics and providers
- ✅ View real-time appointment availability
- ✅ Book appointments with instant confirmation
- ✅ Reschedule or cancel appointments
- ✅ View appointment history
- ✅ Download appointment confirmations

### Clinic Management
- ✅ Register and manage multiple clinics
- ✅ Define operating hours and holidays
- ✅ Manage healthcare providers/specialists
- ✅ Create and manage time slots
- ✅ Block unavailable time slots
- ✅ View appointment statistics

### System Administration
- ✅ User and role management
- ✅ Audit logging and compliance tracking
- ✅ System configuration and settings
- ✅ Reporting and analytics dashboard

---

## 🛠️ Technology Stack

### Frontend
| Technology | Purpose |
|-----------|---------|
| **Blazor WebAssembly** | SPA framework with C# |
| **MudBlazor** | UI component library |
| **FluentValidation** | Form validation |
| **HttpClient** | API communication |

### Backend
| Technology | Purpose |
|-----------|---------|
| **ASP.NET Core 8.0** | REST API framework |
| **Entity Framework Core** | ORM and data access |
| **AutoMapper** | DTO mapping |
| **FluentValidation** | Business rule validation |
| **Serilog** | Structured logging |
| **JWT** | Authentication |

### Database
| Option | Purpose |
|--------|---------|
| **SQL Server** | Primary production database |
| **PostgreSQL** | Alternative relational database |

### Testing
| Framework | Purpose |
|-----------|---------|
| **xUnit** | Unit testing framework |
| **Moq** | Mocking library |
| **bUnit** | Blazor component testing |

---

## 🏗️ Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────┐
│         Blazor WebAssembly (WASM)           │
│      - Patient Portal                       │
│      - Admin Dashboard                      │
│      - Staff Interface                      │
└─────────────────┬───────────────────────────┘
                  │ API Calls (HTTPS)
                  ▼
┌─────────────────────────────────────────────┐
│      ASP.NET Core Web API (REST)            │
│      - Controllers & Routes                 │
│      - JWT Authentication                   │
│      - Input Validation                     │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────────────────────────────────┐
│      Business Logic & Services              │
│      - AppointmentService                   │
│      - TimeSlotService                      │
│      - ValidationService                    │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────────────────────────────────┐
│      Data Access Layer (Repository)         │
│      - Entity Framework Core                │
│      - Generic Repository Pattern           │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────────────────────────────────┐
│    Database (SQL Server / PostgreSQL)       │
│    - Patients                               │
│    - Clinics                                │
│    - Providers                              │
│    - Appointments                           │
│    - TimeSlots                              │
└─────────────────────────────────────────────┘
```

### Design Patterns

- **Repository Pattern**: Abstraction for data access
- **Service Layer Pattern**: Business logic encapsulation
- **Dependency Injection**: Loose coupling and testability
- **DTO Pattern**: Data transfer between layers
- **Factory Pattern**: Object creation
- **Observer Pattern**: Event notifications

---

## 📦 Core Components

### Data Models

#### Patient
- PatientId, FirstName, LastName, Email, PhoneNumber
- DateOfBirth, Gender, Address
- CreatedAt, UpdatedAt

#### Clinic
- ClinicId, Name, Address, City, State, ZipCode
- PhoneNumber, Email
- WorkingHoursStart, WorkingHoursEnd
- Latitude, Longitude

#### Provider
- ProviderId, FirstName, LastName, ClinicId
- Specialization, LicenseNumber
- Email, PhoneNumber

#### TimeSlot
- TimeSlotId, ProviderId
- StartTime, EndTime, Duration
- IsAvailable, IsBlocked

#### Appointment
- AppointmentId, PatientId, ClinicId, ProviderId, TimeSlotId
- Status (Pending, Confirmed, Cancelled, Completed, NoShow)
- BookedAt, ConfirmedAt, CancelledAt

### API Endpoints

#### Appointments
```
GET    /api/appointments/{id}
GET    /api/appointments/patient/{patientId}
POST   /api/appointments
PUT    /api/appointments/{id}
DELETE /api/appointments/{id}
```

#### Time Slots
```
GET  /api/timeslots/provider/{providerId}
GET  /api/timeslots/clinic/{clinicId}/date/{date}
POST /api/timeslots
PUT  /api/timeslots/{id}
```

#### Clinics
```
GET  /api/clinics
GET  /api/clinics/{id}
POST /api/clinics
PUT  /api/clinics/{id}
```

#### Patients
```
GET  /api/patients/{id}
POST /api/patients
PUT  /api/patients/{id}
```

---

## 📁 Project Structure

```
Clinic/
├── src/
│   ├── ClinicBookingSystem.Api/
│   │   ├── Controllers/
│   │   ├── Services/
│   │   ├── Repositories/
│   │   ├── Data/
│   │   ├── DTOs/
│   │   ├── Validators/
│   │   ├── Middleware/
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ClinicBookingSystem.Wasm/
│   │   ├── Components/
│   │   ├── Pages/
│   │   ├── Services/
│   │   ├── wwwroot/
│   │   └── Program.cs
│   │
│   └── ClinicBookingSystem.Shared/
│       ├── DTOs/
│       └── Models/
│
├── tests/
│   ├── ClinicBookingSystem.Tests/
│   └── ClinicBookingSystem.Integration.Tests/
│
├── docs/
│   ├── PRACTICAL_ASSESSMENT.md
│   ├── API.md
│   ├── ARCHITECTURE.md
│   └── DATABASE.md
│
└── README.md
```

---

## 🔐 Security Features

- **JWT Authentication**: Token-based secure API access
- **Role-Based Access Control (RBAC)**: Patient, Staff, Admin roles
- **Password Hashing**: ASP.NET Identity password management
- **HTTPS/TLS**: Encrypted data transmission
- **Input Validation**: FluentValidation on all inputs
- **SQL Injection Prevention**: Parameterized queries via EF Core
- **CORS Policy**: Configured for secure cross-origin requests
- **Audit Logging**: Track all critical operations

---

## 📊 Code Quality Standards

- **SOLID Principles**: Single Responsibility, Open/Closed, Liskov Substitution, Interface Segregation, Dependency Inversion
- **Clean Code**: Meaningful naming, minimal comments, DRY principle
- **Unit Testing**: >80% code coverage target
- **Integration Testing**: End-to-end workflow validation
- **Code Reviews**: PR-based review process
- **Documentation**: Comprehensive inline and external docs

---

## 🗄️ Database Schema

### ER Diagram
See `PRACTICAL_ASSESSMENT.md` for detailed ERD and relationships.

### Key Relationships
- Patient → Appointments (1:Many)
- Clinic → Providers (1:Many)
- Provider → TimeSlots (1:Many)
- TimeSlot → Appointment (1:1)
- Appointment → AppointmentNotes (1:Many)

---

## 🧪 Testing

### Running Tests

```bash
# Unit tests
dotnet test tests/ClinicBookingSystem.Tests

# Integration tests
dotnet test tests/ClinicBookingSystem.Integration.Tests

# With coverage
dotnet test /p:CollectCoverageMetrics=true
```

### Test Categories
- **Unit Tests**: Individual service and component testing
- **Integration Tests**: API endpoint and database integration
- **Component Tests**: Blazor UI component testing with bUnit

---

## 📚 Documentation

| Document | Purpose |
|----------|---------|
| [PRACTICAL_ASSESSMENT.md](./PRACTICAL_ASSESSMENT.md) | Complete assessment requirements and specifications |
| API.md | REST API documentation |
| ARCHITECTURE.md | System architecture and design patterns |
| DATABASE.md | Database schema and migrations |

---

## 🚀 Deployment

### Docker Deployment

```bash
# Build and run with Docker Compose
docker-compose up -d

# Access the application
# Frontend: http://localhost:80
# API: http://localhost:5000
```

### Cloud Deployment
- **Azure App Service**: Web App + SQL Database
- **AWS**: EC2 + RDS
- **Docker**: Containerized deployment
- **Kubernetes**: Orchestration support

---

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding Standards
- Follow C# naming conventions (PascalCase for classes/methods)
- Use meaningful variable names (camelCase for locals)
- Write XML documentation for public members
- Implement unit tests for all services
- Follow SOLID principles

---

## 📋 Checklist & Success Criteria

- ✅ Requirements analysis with stakeholders and use cases
- ✅ System design with architecture diagrams and ERD
- ✅ Blazor WASM implementation with validation
- ✅ Double booking prevention
- ✅ Time slot management
- ✅ Confirmation messaging
- ✅ SOLID principles implementation
- ✅ Unit tests (>80% coverage)
- ✅ Security best practices
- ✅ API documentation with Swagger
- ✅ Clean code and meaningful naming
- ✅ Comprehensive inline documentation

---

## 📞 Support

For issues, feature requests, or questions:
- Open an issue on GitHub
- Check documentation in `/docs` folder
- Review `PRACTICAL_ASSESSMENT.md` for detailed specifications

---

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## 👥 Authors

- **Developer**: siyandam256-alt
- **Project**: Clinic Booking System - PHCIS
- **Last Updated**: May 28, 2026

---

## 🎓 Learning Resources

- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core)
- [Entity Framework Core](https://docs.microsoft.com/ef/core)
- [Clean Code Principles](https://en.wikipedia.org/wiki/SOLID)

---

**Status**: ✅ Active Development

For the complete practical assessment details, see [PRACTICAL_ASSESSMENT.md](./PRACTICAL_ASSESSMENT.md)
