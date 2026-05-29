using ClinicBookingSystem.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ClinicBookingSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TimeSlotController : ControllerBase
{
    private readonly ITimeSlotService _timeSlotService;
    private readonly ILogger<TimeSlotController> _logger;

    public TimeSlotController(
        ITimeSlotService timeSlotService,
        ILogger<TimeSlotController> logger)
    {
        _timeSlotService = timeSlotService;
        _logger = logger;
    }

    /// <summary>
    /// Get available time slots for a provider on a specific date
    /// </summary>
    [HttpGet("provider/{providerId}/date/{date}")]
    [ProduceResponseType(typeof(ApiResponse<List<TimeSlotDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAvailableSlots(int providerId, DateTime date)
    {
        try
        {
            var slots = await _timeSlotService.GetAvailableSlotsAsync(providerId, date);
            return Ok(ApiResponse<List<TimeSlotDto>>.SuccessResponse(slots, "Available slots retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving available slots: {ex.Message}");
            return StatusCode(500, ApiResponse<List<TimeSlotDto>>.ErrorResponse("An error occurred while retrieving slots"));
        }
    }

    /// <summary>
    /// Get all time slots for a provider within a date range
    /// </summary>
    [HttpGet("provider/{providerId}")]
    [ProduceResponseType(typeof(ApiResponse<List<TimeSlotDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProviderSlots(int providerId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
    {
        try
        {
            var slots = await _timeSlotService.GetProviderSlotsAsync(providerId, startDate, endDate);
            return Ok(ApiResponse<List<TimeSlotDto>>.SuccessResponse(slots, "Provider slots retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving provider slots: {ex.Message}");
            return StatusCode(500, ApiResponse<List<TimeSlotDto>>.ErrorResponse("An error occurred while retrieving slots"));
        }
    }

    /// <summary>
    /// Create a new time slot
    /// </summary>
    [HttpPost]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTimeSlot([FromBody] CreateTimeSlotDto request)
    {
        try
        {
            var result = await _timeSlotService.CreateTimeSlotAsync(request);
            return CreatedAtAction(nameof(GetAvailableSlots), new { providerId = request.ProviderId, date = request.StartTime.Date },
                ApiResponse<bool>.SuccessResponse(result, "Time slot created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Invalid time slot request: {ex.Message}");
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating time slot: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while creating the time slot"));
        }
    }

    /// <summary>
    /// Create bulk time slots
    /// </summary>
    [HttpPost("bulk")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status201Created)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBulkTimeSlots([FromBody] CreateBulkTimeSlotsDto request)
    {
        try
        {
            var result = await _timeSlotService.CreateBulkTimeSlotsAsync(request);
            return CreatedAtAction(nameof(GetProviderSlots), new { providerId = request.ProviderId },
                ApiResponse<bool>.SuccessResponse(result, "Time slots created successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Invalid bulk time slot request: {ex.Message}");
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error creating bulk time slots: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while creating the time slots"));
        }
    }

    /// <summary>
    /// Block a time slot
    /// </summary>
    [HttpPut("{id}/block")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BlockTimeSlot(int id)
    {
        try
        {
            var result = await _timeSlotService.BlockTimeSlotAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Time slot blocked successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Cannot block time slot: {ex.Message}");
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error blocking time slot: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while blocking the time slot"));
        }
    }

    /// <summary>
    /// Unblock a time slot
    /// </summary>
    [HttpPut("{id}/unblock")]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProduceResponseType(typeof(ApiResponse<bool>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UnblockTimeSlot(int id)
    {
        try
        {
            var result = await _timeSlotService.UnblockTimeSlotAsync(id);
            return Ok(ApiResponse<bool>.SuccessResponse(result, "Time slot unblocked successfully"));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning($"Cannot unblock time slot: {ex.Message}");
            return NotFound(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error unblocking time slot: {ex.Message}");
            return StatusCode(500, ApiResponse<bool>.ErrorResponse("An error occurred while unblocking the time slot"));
        }
    }
}
