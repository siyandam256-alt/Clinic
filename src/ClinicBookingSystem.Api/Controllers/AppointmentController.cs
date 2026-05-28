using ClinicBookingSystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IValidator<CreateAppointmentDto> _createValidator;
    private readonly IValidator<UpdateAppointmentDto> _updateValidator;
    private readonly ILogger<AppointmentController> _logger;

    public AppointmentController(
        IAppointmentService appointmentService,
        IValidator<CreateAppointmentDto> createValidator,
        IValidator<UpdateAppointmentDto> updateValidator,
        ILogger<AppointmentController> logger)
    {
        _appointmentService = appointmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get appointment by ID
    /// </summary>
    [HttpGet("{id}")]
    [ProduceResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAppointment(int id)
    {
        try
        {
            var appointment = await _appointmentService.GetAppointmentAsync(id);
            return Ok(ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment retrieved successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Appointment not found: {ex.Message}");
            return NotFound(ApiResponse<AppointmentDto>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Get all appointments for a patient
    /// </summary>
    [HttpGet("patient/{patientId}")]
    [ProduceResponseType(typeof(ApiResponse<List<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPatientAppointments(int patientId)
    {
        try
        {
            var appointments = await _appointmentService.GetPatientAppointmentsAsync(patientId);
            return Ok(ApiResponse<List<AppointmentDto>>.SuccessResponse(appointments, "Patient appointments retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving patient appointments: {ex.Message}");
            return StatusCode(500, ApiResponse<List<AppointmentDto>>.ErrorResponse("An error occurred while retrieving appointments"));
        }
    }

    /// <summary>
    /// Get all appointments for a provider on a specific date
    /// </summary>
    [HttpGet("provider/{providerId}/date/{date}")]
    [ProduceResponseType(typeof(ApiResponse<List<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviderAppointments(int providerId, DateTime date)
    {
        try
        {
            var appointments = await _appointmentService.GetProviderAppointmentsAsync(providerId, date);
            return Ok(ApiResponse<List<AppointmentDto>>.SuccessResponse(appointments, "Provider appointments retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving provider appointments: {ex.Message}");
            return StatusCode(500, ApiResponse<List<AppointmentDto>>.ErrorResponse("An error occurred while retrieving appointments"));
        }
    }

    /// <summary>
    /// Get all appointments for a clinic on a specific date
    /// </summary>
    [HttpGet("clinic/{clinicId}/date/{date}")]
    [ProduceResponseType(typeof(ApiResponse<List<AppointmentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetClinicAppointments(int clinicId, DateTime date)
    {
        try
        {
            var appointments = await _appointmentService.GetClinicAppointmentsAsync(clinicId, date);
            return Ok(ApiResponse<List<AppointmentDto>>.SuccessResponse(appointments, "Clinic appointments retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving clinic appointments: {ex.Message}");
            return StatusCode(500, ApiResponse<List<AppointmentDto>>.ErrorResponse("An error occurred while retrieving appointments"));
        }
    }

    /// <summary>
    /// Book a new appointment
    /// </summary>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<AppointmentDto>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BookAppointment([FromBody] CreateAppointmentDto request)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse("Validation failed", errors));
            }

            var appointment = await _appointmentService.BookAppointmentAsync(request);
            return CreatedAtAction(nameof(GetAppointment), new { id = appointment.AppointmentId }, 
                ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment booked successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Invalid booking request: {ex.Message}");
            return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error booking appointment: {ex.Message}");
            return StatusCode(500, ApiResponse<AppointmentDto>.ErrorResponse("An error occurred while booking the appointment"));
        }
    }

    /// <summary>
    /// Cancel an appointment
    /// </summary>
    [HttpDelete("{id}")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CancelAppointment(int id)
    {
        try
        {
            var result = await _appointmentService.CancelAppointmentAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Appointment cancelled successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Cannot cancel appointment: {ex.Message}");
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error cancelling appointment: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while cancelling the appointment"));
        }
    }

    /// <summary>
    /// Reschedule an appointment
    /// </summary>
    [HttpPut("{id}/reschedule")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RescheduleAppointment(int id, [FromBody] int newTimeSlotId)
    {
        try
        {
            var result = await _appointmentService.RescheduleAppointmentAsync(id, newTimeSlotId);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Appointment rescheduled successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Cannot reschedule appointment: {ex.Message}");
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error rescheduling appointment: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while rescheduling the appointment"));
        }
    }

    /// <summary>
    /// Confirm an appointment
    /// </summary>
    [HttpPut("{id}/confirm")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConfirmAppointment(int id)
    {
        try
        {
            var result = await _appointmentService.ConfirmAppointmentAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Appointment confirmed successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Cannot confirm appointment: {ex.Message}");
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error confirming appointment: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while confirming the appointment"));
        }
    }
}
