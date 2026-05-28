# Clinic Practical Assessment: Clinic Booking System

## Scenario Overview

You are tasked with designing and implementing a simplified Clinic Booking System for PHCIS (Primary Health Care Information System). The system should allow patients to book appointments at clinics, view available time slots, and receive confirmation.

---

## Assessment Tasks

### 1. Requirements Analysis

#### Key Stakeholders
- **Patients**: End-users who need to book appointments
- **Clinic Staff**: Manage appointments, time slots, and patient records
- **System Administrator**: Manage clinics, staff, and system configurations
- **Healthcare Providers**: Maintain schedules and patient information

#### Functional Requirements
- **Patient Registration & Authentication**
  - User registration and login
  - Profile management
  - Password reset functionality

- **Clinic Management**
  - Register multiple clinics
  - Define clinic operating hours
  - Manage clinic staff and specialists

- **Appointment Booking**
  - View available time slots
  - Book appointments
  - Receive booking confirmation
  - Cancel appointments
  - Reschedule appointments

- **Time Slot Management**
  - Define working hours per clinic
  - Set appointment duration (e.g., 30 minutes)
  - Block unavailable time slots
  - Prevent double bookings

- **Appointment History**
  - View past and upcoming appointments
  - Download appointment confirmations
  - Track appointment status

#### Non-Functional Requirements
- **Security**
  - Password encryption
  - JWT token-based authentication
  - Role-based access control (RBAC)
  - HTTPS/TLS for data transmission
  - HIPAA compliance for healthcare data

- **Performance**
  - API response time < 200ms
  - Support concurrent users (up to 1000)
  - Database query optimization

- **Scalability**
  - Horizontal scaling capability
  - Database replication support
  - Microservices-ready architecture

- **Availability & Reliability**
  - 99.5% uptime SLA
  - Automated backups
  - Disaster recovery plan

- **Usability**
  - Mobile-responsive interface
  - Intuitive UI/UX
  - Accessible (WCAG 2.1 AA compliance)

---

### 2. System Design

#### Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                    Presentation Layer                       │
│                  (Blazor WebAssembly)                       │
│  - Patient Portal                                           │
│  - Admin Dashboard                                          │
│  - Staff Interface                                          │
└─────────────────────────────────────────────────────────────┘
                            │
                   (API Calls via HttpClient)
                            │
┌─────────────────────────────────────────────────────────────┐
│                    API Layer                                │
│              (ASP.NET Core Web API)                         │
│  - Authentication Controller                               │
│  - Appointment Controller                                  │
│  - Clinic Controller                                       │
│  - Time Slot Controller                                    │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                  Business Logic Layer                       │
│              (Service/Repository Pattern)                  │
│  - AppointmentService                                      │
│  - ClinicService                                           │
│  - TimeSlotService                                         │
│  - ValidationService                                       │
│  - NotificationService                                     │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                  Data Access Layer                          │
│                 (Entity Framework Core)                     │
│  - Repositories                                            │
│  - DbContext                                               │
│  - Migrations                                              │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                  Data Layer                                 │
│              (SQL Server/PostgreSQL)                        │
└─────────────────────────────────────────────────────────────┘
```

#### Entity Relationship Diagram (ERD)

```
┌──────────────────┐
│     Patient      │
├──────────────────┤
│ PatientId (PK)   │
│ FirstName        │
│ LastName         │
│ Email            │
│ PhoneNumber      │
│ DateOfBirth      │
│ Gender           │
│ Address          │
│ CreatedAt        │
│ UpdatedAt        │
└────────┬─────────┘
         │ 1
         │
         │ *
         ├──────────────────────────┐
         │                          │
         ▼ *                        ▼ *
┌──────────────────┐       ┌──────────────────┐
│   Appointment    │       │ AppointmentNote  │
├──────────────────┤       ├──────────────────┤
│ AppointmentId    │       │ NoteId (PK)      │
│ (PK)             │       │ AppointmentId    │
│ PatientId (FK)   │───────│ (FK)             │
│ ClinicId (FK)    │       │ Notes            │
│ ProviderId (FK)  │       │ CreatedAt        │
│ TimeSlotId (FK)  │       └──────────────────┘
│ Status           │
│ BookedAt         │
│ ConfirmedAt      │
│ CancelledAt      │
│ UpdatedAt        │
└────────┬─────────┘
         │
         │ *
         ▼
┌──────────────────────┐
│     TimeSlot         │
├──────────────────────┤
│ TimeSlotId (PK)      │
│ ProviderId (FK)      │
│ StartTime            │
│ EndTime              │
│ Duration (minutes)   │
│ IsAvailable          │
│ IsBlocked            │
│ CreatedAt            │
│ UpdatedAt            │
└──────────┬───────────┘
           │
           │ *
           ▼
┌──────────────────────┐
│     Provider         │
├──────────────────────┤
│ ProviderId (PK)      │
│ ClinicId (FK)        │
│ FirstName            │
│ LastName             │
│ Specialization       │
│ LicenseNumber        │
│ Email                │
│ PhoneNumber          │
│ CreatedAt            │
│ UpdatedAt            │
└──────────┬───────────┘
           │
           │ *
           ▼
┌──────────────────────┐
│      Clinic          │
├──────────────────────┤
│ ClinicId (PK)        │
│ Name                 │
│ Address              │
│ City                 │
│ State                │
│ ZipCode              │
│ PhoneNumber          │
│ Email                │
│ WorkingHoursStart    │
│ WorkingHoursEnd      │
│ Latitude             │
│ Longitude            │
│ CreatedAt            │
│ UpdatedAt            │
└──────────────────────┘
```

#### Data Model Classes

**Patient.cs**
```csharp
public class Patient
{
    public int PatientId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Address { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public ICollection<Appointment> Appointments { get; set; }
}
```

**Clinic.cs**
```csharp
public class Clinic
{
    public int ClinicId { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public TimeSpan WorkingHoursStart { get; set; }
    public TimeSpan WorkingHoursEnd { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public ICollection<Provider> Providers { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}
```

**Provider.cs**
```csharp
public class Provider
{
    public int ProviderId { get; set; }
    public int ClinicId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Specialization { get; set; }
    public string LicenseNumber { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public Clinic Clinic { get; set; }
    public ICollection<TimeSlot> TimeSlots { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}
```

**TimeSlot.cs**
```csharp
public class TimeSlot
{
    public int TimeSlotId { get; set; }
    public int ProviderId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Duration { get; set; } // in minutes
    public bool IsAvailable { get; set; }
    public bool IsBlocked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public Provider Provider { get; set; }
    public Appointment Appointment { get; set; }
}
```

**Appointment.cs**
```csharp
public class Appointment
{
    public int AppointmentId { get; set; }
    public int PatientId { get; set; }
    public int ClinicId { get; set; }
    public int ProviderId { get; set; }
    public int TimeSlotId { get; set; }
    public AppointmentStatus Status { get; set; }
    public DateTime BookedAt { get; set; }
    public DateTime? ConfirmedAt { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation Properties
    public Patient Patient { get; set; }
    public Clinic Clinic { get; set; }
    public Provider Provider { get; set; }
    public TimeSlot TimeSlot { get; set; }
    public ICollection<AppointmentNote> Notes { get; set; }
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Completed,
    NoShow
}
```

**AppointmentNote.cs**
```csharp
public class AppointmentNote
{
    public int NoteId { get; set; }
    public int AppointmentId { get; set; }
    public string Notes { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation Properties
    public Appointment Appointment { get; set; }
}
```

#### Key Workflows & Sequence Diagrams

**Appointment Booking Flow**
```
Patient                API                Database              Email Service
   │                   │                      │                     │
   ├─ View Clinics ────>│                      │                     │
   │                   ├─ Query Clinics ─────>│                     │
   │                   │<─ Return Clinics ────│                     │
   │<─ Display Clinics ─│                      │                     │
   │                   │                      │                     │
   ├─ Select Provider ─>│                      │                     │
   │                   ├─ Query Available ──→ │                     │
   │                   │<─ Time Slots ────────│                     │
   │<─ Display Slots ───│                      │                     │
   │                   │                      │                     │
   ├─ Book Slot ──────>│                      │                     │
   │                   ├─ Validate ───────────│                     │
   │                   │<─ Validation OK ─────│                     │
   │                   ├─ Create Appointment ─│                     │
   │                   │<─ ID: 123 ───────────│                     │
   │                   ├─ Mark Slot Booked ──>│                     │
   │                   │                      ├─ Send Confirmation ─>│
   │                   │                      │<─ Sent ──────────────│
   │<─ Confirmation ────│                      │                     │
```

---

### 3. Implementation

#### Technology Stack

**Frontend:**
- Blazor WebAssembly
- MudBlazor for UI components
- FluentValidation for form validation
- HttpClient for API communication

**Backend:**
- ASP.NET Core 8.0 Web API
- Entity Framework Core for ORM
- AutoMapper for DTO mapping
- FluentValidation for business rules
- Serilog for logging

**Database:**
- SQL Server or PostgreSQL
- EF Core Migrations for schema management

**Authentication:**
- ASP.NET Identity with JWT tokens
- Role-based access control (RBAC)

#### Core Implementation Components

##### 1. API Controllers

**AppointmentController.cs** - Manage appointment operations
- `GET /api/appointments/{id}` - Get appointment details
- `GET /api/appointments/patient/{patientId}` - Get patient's appointments
- `POST /api/appointments` - Create new appointment
- `PUT /api/appointments/{id}` - Update appointment
- `DELETE /api/appointments/{id}` - Cancel appointment

**TimeSlotController.cs** - Manage available time slots
- `GET /api/timeslots/provider/{providerId}` - Get provider's available slots
- `GET /api/timeslots/clinic/{clinicId}/date/{date}` - Get slots for date
- `POST /api/timeslots` - Create time slots
- `PUT /api/timeslots/{id}` - Update slot availability

**ClinicController.cs** - Manage clinics
- `GET /api/clinics` - List all clinics
- `GET /api/clinics/{id}` - Get clinic details
- `POST /api/clinics` - Create clinic
- `PUT /api/clinics/{id}` - Update clinic

**PatientController.cs** - Manage patients
- `GET /api/patients/{id}` - Get patient profile
- `POST /api/patients` - Register patient
- `PUT /api/patients/{id}` - Update patient profile

##### 2. Service Layer (Business Logic)

**IAppointmentService.cs**
```csharp
public interface IAppointmentService
{
    Task<AppointmentDto> BookAppointmentAsync(CreateAppointmentDto request);
    Task<AppointmentDto> GetAppointmentAsync(int appointmentId);
    Task<List<AppointmentDto>> GetPatientAppointmentsAsync(int patientId);
    Task<bool> CancelAppointmentAsync(int appointmentId);
    Task<bool> RescheduleAppointmentAsync(int appointmentId, int newTimeSlotId);
    Task<List<AppointmentDto>> GetProviderAppointmentsAsync(int providerId, DateTime date);
}
```

**ITimeSlotService.cs**
```csharp
public interface ITimeSlotService
{
    Task<List<TimeSlotDto>> GetAvailableSlotsAsync(int providerId, DateTime date);
    Task<List<TimeSlotDto>> GetAvailableSlotsAsync(int clinicId, DateTime date);
    Task<bool> IsSlotAvailableAsync(int timeSlotId);
    Task<bool> CreateSlotsAsync(int providerId, DateTime startDate, DateTime endDate);
    Task<bool> BlockSlotAsync(int timeSlotId);
}
```

**IValidationService.cs**
```csharp
public interface IValidationService
{
    Task<ValidationResult> ValidateBookingAsync(CreateAppointmentDto request);
    bool IsDoubleBooking(int patientId, int timeSlotId);
    bool IsProviderAvailable(int providerId, DateTime dateTime);
}
```

##### 3. Data Access Layer

**IRepository<T>.cs** - Generic repository pattern
```csharp
public interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
    Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<int> SaveChangesAsync();
}
```

##### 4. DTOs (Data Transfer Objects)

**CreateAppointmentDto.cs**
```csharp
public class CreateAppointmentDto
{
    public int PatientId { get; set; }
    public int ClinicId { get; set; }
    public int ProviderId { get; set; }
    public int TimeSlotId { get; set; }
    public string Notes { get; set; }
}
```

**AppointmentDto.cs**
```csharp
public class AppointmentDto
{
    public int AppointmentId { get; set; }
    public PatientDto Patient { get; set; }
    public ClinicDto Clinic { get; set; }
    public ProviderDto Provider { get; set; }
    public DateTime AppointmentDateTime { get; set; }
    public string Status { get; set; }
    public DateTime BookedAt { get; set; }
}
```

##### 5. Blazor Components

**AppointmentBooking.razor** - Main booking component
```razor
@page "/appointment/book"
@using ClinicBookingSystem.Shared.DTOs
@using MudBlazor
@inject IAppointmentService AppointmentService
@inject ITimeSlotService TimeSlotService
@inject NavigationManager NavigationManager

<MudContainer MaxWidth="MaxWidth.Medium" Class="py-8">
    <MudCard>
        <MudCardHeader>
            <CardHeaderContent>
                <MudText Typo="Typo.h5">Book an Appointment</MudText>
            </CardHeaderContent>
        </MudCardHeader>
        <MudCardContent>
            <EditForm Model="@appointmentModel" OnValidSubmit="@HandleSubmit">
                <DataAnnotationsValidator/>
                <MudTextField @bind-Value="appointmentModel.PatientId" 
                             Label="Patient ID" 
                             Variant="Variant.Outlined"/>
                <MudSelect @bind-Value="appointmentModel.ClinicId" 
                          Label="Select Clinic" 
                          Variant="Variant.Outlined">
                    @foreach(var clinic in clinics)
                    {
                        <MudSelectItem Value="clinic.ClinicId">@clinic.Name</MudSelectItem>
                    }
                </MudSelect>
                <MudButton ButtonType="ButtonType.Submit" 
                          Variant="Variant.Filled" 
                          Color="Color.Primary"
                          FullWidth="true">
                    Book Appointment
                </MudButton>
            </EditForm>
        </MudCardContent>
    </MudCard>
</MudContainer>

@code {
    private CreateAppointmentDto appointmentModel = new();
    private List<ClinicDto> clinics = new();

    protected override async Task OnInitializedAsync()
    {
        // Load clinics
    }

    private async Task HandleSubmit()
    {
        var result = await AppointmentService.BookAppointmentAsync(appointmentModel);
        if(result != null)
        {
            NavigationManager.NavigateTo("/appointment-confirmation");
        }
    }
}
```

---

### 4. Code Quality

#### SOLID Principles Implementation

1. **Single Responsibility Principle (SRP)**
   - Each service handles one domain concern
   - Controllers delegate to services
   - Repositories manage data access only

2. **Open/Closed Principle (OCP)**
   - Use interfaces for extensibility
   - Services accept dependencies via DI

3. **Liskov Substitution Principle (LSP)**
   - All repositories implement IRepository<T>
   - Services implement specific interfaces

4. **Interface Segregation Principle (ISP)**
   - Specific service interfaces (IAppointmentService, ITimeSlotService)
   - Generic repository interface with specific implementations

5. **Dependency Inversion Principle (DIP)**
   - Depend on abstractions (interfaces)
   - Inject dependencies in constructors

#### Naming Conventions

```csharp
// Classes: PascalCase
public class AppointmentService { }

// Methods: PascalCase
public async Task<AppointmentDto> BookAppointmentAsync() { }

// Properties: PascalCase
public int AppointmentId { get; set; }

// Local variables: camelCase
var appointmentId = 1;

// Constants: UPPER_SNAKE_CASE or PascalCase
private const int MAX_APPOINTMENT_DURATION = 60;

// Interfaces: IPascalCase
public interface IAppointmentService { }

// Async methods: AsyncSuffix
public async Task GetAppointmentAsync() { }
```

#### Documentation & Comments

```csharp
/// <summary>
/// Books a new appointment for a patient.
/// </summary>
/// <param name="request">Appointment booking request details</param>
/// <returns>Created appointment DTO with confirmation ID</returns>
/// <exception cref="ValidationException">Thrown when booking validation fails</exception>
/// <remarks>
/// This method performs:
/// - Double booking validation
/// - Slot availability check
/// - Appointment creation and confirmation
/// </remarks>
public async Task<AppointmentDto> BookAppointmentAsync(CreateAppointmentDto request)
{
    // Validate booking request
    var validationResult = await _validationService.ValidateBookingAsync(request);
    if (!validationResult.IsValid)
        throw new ValidationException(validationResult.Errors);

    // Check for double bookings
    if (_validationService.IsDoubleBooking(request.PatientId, request.TimeSlotId))
        throw new InvalidOperationException("Patient already has an appointment at this time");

    // Create appointment
    var appointment = new Appointment
    {
        PatientId = request.PatientId,
        ClinicId = request.ClinicId,
        ProviderId = request.ProviderId,
        TimeSlotId = request.TimeSlotId,
        Status = AppointmentStatus.Pending,
        BookedAt = DateTime.UtcNow
    };

    await _appointmentRepository.AddAsync(appointment);
    await _appointmentRepository.SaveChangesAsync();

    return _mapper.Map<AppointmentDto>(appointment);
}
```

#### Unit Tests

**AppointmentServiceTests.cs**
```csharp
[TestFixture]
public class AppointmentServiceTests
{
    private IAppointmentService _appointmentService;
    private Mock<IRepository<Appointment>> _mockAppointmentRepo;
    private Mock<ITimeSlotService> _mockTimeSlotService;
    private Mock<IValidationService> _mockValidationService;

    [SetUp]
    public void Setup()
    {
        _mockAppointmentRepo = new Mock<IRepository<Appointment>>();
        _mockTimeSlotService = new Mock<ITimeSlotService>();
        _mockValidationService = new Mock<IValidationService>();

        _appointmentService = new AppointmentService(
            _mockAppointmentRepo.Object,
            _mockTimeSlotService.Object,
            _mockValidationService.Object
        );
    }

    [Test]
    public async Task BookAppointmentAsync_WithValidRequest_ReturnsAppointmentDto()
    {
        // Arrange
        var request = new CreateAppointmentDto
        {
            PatientId = 1,
            ClinicId = 1,
            ProviderId = 1,
            TimeSlotId = 1
        };

        _mockValidationService
            .Setup(x => x.ValidateBookingAsync(request))
            .ReturnsAsync(new ValidationResult());

        _mockValidationService
            .Setup(x => x.IsDoubleBooking(1, 1))
            .Returns(false);

        // Act
        var result = await _appointmentService.BookAppointmentAsync(request);

        // Assert
        Assert.IsNotNull(result);
        _mockAppointmentRepo.Verify(x => x.AddAsync(It.IsAny<Appointment>()), Times.Once);
    }

    [Test]
    public async Task BookAppointmentAsync_WithDoubleBooking_ThrowsException()
    {
        // Arrange
        var request = new CreateAppointmentDto { PatientId = 1, TimeSlotId = 1 };

        _mockValidationService
            .Setup(x => x.IsDoubleBooking(1, 1))
            .Returns(true);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _appointmentService.BookAppointmentAsync(request)
        );
    }

    [Test]
    public async Task CancelAppointmentAsync_WithValidId_ReturnTrue()
    {
        // Arrange
        var appointmentId = 1;
        var appointment = new Appointment { AppointmentId = appointmentId };

        _mockAppointmentRepo
            .Setup(x => x.GetByIdAsync(appointmentId))
            .ReturnsAsync(appointment);

        // Act
        var result = await _appointmentService.CancelAppointmentAsync(appointmentId);

        // Assert
        Assert.IsTrue(result);
        _mockAppointmentRepo.Verify(x => x.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
    }
}
```

---

## Optional Enhancements

| Feature | Technology | Priority | Use Case |
|---------|-----------|----------|----------|
| Appointment Reminders | Twilio SMS / SendGrid Email | High | Send SMS/email reminders 24hrs before appointment |
| Calendar Sync | Outlook API / Google Calendar | Medium | Sync appointments to patient's calendar |
| Real-time Updates | SignalR | Medium | Live availability updates, notifications |
| Analytics Dashboard | Chart.js / Plotly | Medium | Show booking trends, provider utilization |
| Video Consultations | Twilio Video / Zoom API | High | Telehealth support |
| Insurance Integration | Third-party APIs | Low | Validate patient insurance coverage |
| GraphQL API | Hot Chocolate | Low | Efficient data queries for advanced clients |
| Multi-language Support | Localization (i18n) | Medium | Support multiple languages |
| SMS Notifications | Twilio | High | Booking confirmations via SMS |
| Reporting | SSRS / Power BI | Low | Generate clinical and administrative reports |

---

## Project Structure

```
ClinicBookingSystem/
├── src/
│   ├── ClinicBookingSystem.Api/
│   │   ├── Controllers/
│   │   │   ├── AppointmentController.cs
│   │   │   ├── TimeSlotController.cs
│   │   │   ├── ClinicController.cs
│   │   │   └── PatientController.cs
│   │   ├── Services/
│   │   │   ├── IAppointmentService.cs
│   │   │   ├── AppointmentService.cs
│   │   │   ├── ITimeSlotService.cs
│   │   │   ├── TimeSlotService.cs
│   │   │   └── ValidationService.cs
│   │   ├── Repositories/
│   │   │   ├── IRepository.cs
│   │   │   ├── Repository.cs
│   │   │   └── SpecializedRepositories/
│   │   ├── Data/
│   │   │   ├── ClinicDbContext.cs
│   │   │   └── Migrations/
│   │   ├── DTOs/
│   │   │   ├── AppointmentDto.cs
│   │   │   ├── TimeSlotDto.cs
│   │   │   ├── ClinicDto.cs
│   │   │   └── PatientDto.cs
│   │   ├── Mapping/
│   │   │   └── MappingProfile.cs
│   │   ├── Validators/
│   │   │   ├── CreateAppointmentValidator.cs
│   │   │   └── PatientValidator.cs
│   │   ├── Middleware/
│   │   │   ├── ErrorHandlingMiddleware.cs
│   │   │   └── JwtMiddleware.cs
│   │   ├── Program.cs
│   │   └── appsettings.json
│   │
│   ├── ClinicBookingSystem.Wasm/
│   │   ├── Components/
│   │   │   ├── Appointment/
│   │   │   │   ├── AppointmentBooking.razor
│   │   │   │   ├── AppointmentList.razor
│   │   │   │   └── AppointmentDetails.razor
│   │   │   ├── Admin/
│   │   │   │   ├── ClinicManagement.razor
│   │   │   │   └── TimeSlotManagement.razor
│   │   │   └── Shared/
│   │   │       ├── MainLayout.razor
│   │   │       └── NavMenu.razor
│   │   ├── Services/
│   │   │   ├── IAppointmentApiService.cs
│   │   │   ├── AppointmentApiService.cs
│   │   │   └── AuthService.cs
│   │   ├── Pages/
│   │   │   ├── Index.razor
│   │   │   ├── Login.razor
│   │   │   └── Dashboard.razor
│   │   ├── wwwroot/
│   │   │   └── index.html
│   │   └── Program.cs
│   │
│   └── ClinicBookingSystem.Shared/
│       ├── DTOs/
│       │   └── (Shared DTOs)
│       └── Models/
│           └── (Shared domain models)
│
├── tests/
│   ├── ClinicBookingSystem.Tests/
│   │   ├── Services/
│   │   │   ├── AppointmentServiceTests.cs
│   │   │   └── TimeSlotServiceTests.cs
│   │   ├── Validators/
│   │   │   └── CreateAppointmentValidatorTests.cs
│   │   └── Controllers/
│   │       └── AppointmentControllerTests.cs
│   │
│   └── ClinicBookingSystem.Integration.Tests/
│       └── AppointmentBookingIntegrationTests.cs
│
├── docs/
│   ├── API.md
│   ├── ARCHITECTURE.md
│   └── DATABASE.md
│
├── docker-compose.yml
├── Dockerfile
└── README.md
```

---

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- SQL Server or PostgreSQL
- Visual Studio 2022 or VS Code
- Node.js (optional, for frontend tooling)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/siyandam256-alt/Clinic.git
   cd Clinic
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Set up database**
   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

4. **Run the application**
   ```bash
   dotnet run
   ```

---

## Success Criteria

✅ System allows patients to book, reschedule, and cancel appointments
✅ Prevents double bookings with validation logic
✅ Displays available time slots with real-time updates
✅ Sends booking confirmations to patients
✅ Implements SOLID principles and clean code practices
✅ Includes comprehensive unit tests (>80% coverage)
✅ Follows security best practices (JWT, RBAC, HTTPS)
✅ Provides API documentation (Swagger/OpenAPI)
✅ Supports horizontal scaling and microservices architecture
✅ Implements proper error handling and logging

---

## Support & Documentation

- **API Documentation**: Swagger UI at `/swagger`
- **Architecture Guide**: See `docs/ARCHITECTURE.md`
- **Database Schema**: See `docs/DATABASE.md`
- **Contributing**: See `CONTRIBUTING.md`

---

**Last Updated**: May 28, 2026
