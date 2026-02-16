using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for managing vehicle bookings and rentals.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleBookingService _vehicleService;
    private readonly ILogger<VehiclesController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehiclesController"/> class.
    /// </summary>
    /// <param name="vehicleService">The vehicle booking service for managing vehicle operations.</param>
    /// <param name="logger">The logger for recording vehicle events.</param>
    public VehiclesController(IVehicleBookingService vehicleService, ILogger<VehiclesController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    /// <summary>
    /// Get all vehicle bookings (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleBookingDto>>>> GetAllBookings()
    {
        try
        {
            var bookings = await _vehicleService.GetAllBookingsAsync();
            return Ok(new ApiResponse<IEnumerable<VehicleBookingDto>>
            {
                Success = true,
                Message = "Bookings retrieved successfully",
                Data = bookings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get booking by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<VehicleBookingDto>>> GetBookingById(int id)
    {
        try
        {
            var booking = await _vehicleService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new ApiResponse { Success = false, Message = "Booking not found" });

            return Ok(new ApiResponse<VehicleBookingDto>
            {
                Success = true,
                Message = "Booking retrieved successfully",
                Data = booking
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get user's vehicle bookings
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleBookingDto>>>> GetUserBookings(int userId)
    {
        try
        {
            var bookings = await _vehicleService.GetBookingsByUserIdAsync(userId);
            return Ok(new ApiResponse<IEnumerable<VehicleBookingDto>>
            {
                Success = true,
                Message = "User bookings retrieved successfully",
                Data = bookings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user bookings");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Create a new vehicle booking
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<VehicleBookingDto>>> CreateBooking([FromBody] CreateVehicleBookingDto createDto, [FromQuery] int userId)
    {
        try
        {
            var booking = await _vehicleService.CreateBookingAsync(userId, createDto);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, new ApiResponse<VehicleBookingDto>
            {
                Success = true,
                Message = "Booking created successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Booking creation failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update a vehicle booking
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<VehicleBookingDto>>> UpdateBooking(int id, [FromBody] UpdateVehicleBookingDto updateDto)
    {
        try
        {
            var booking = await _vehicleService.UpdateBookingAsync(id, updateDto);
            return Ok(new ApiResponse<VehicleBookingDto>
            {
                Success = true,
                Message = "Booking updated successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Booking update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update booking status (Admin only)
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<VehicleBookingDto>>> UpdateBookingStatus(int id, [FromBody] UpdateVehicleStatusDto updateStatusDto)
    {
        try
        {
            var booking = await _vehicleService.UpdateBookingStatusAsync(id, updateStatusDto);
            return Ok(new ApiResponse<VehicleBookingDto>
            {
                Success = true,
                Message = "Booking status updated successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Status update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating booking status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Delete a vehicle booking
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteBooking(int id)
    {
        try
        {
            var deleted = await _vehicleService.DeleteBookingAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse { Success = false, Message = "Booking not found" });

            return Ok(new ApiResponse { Success = true, Message = "Booking deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get available vehicles for date range
    /// </summary>
    [HttpGet("available")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleBookingDto>>>> GetAvailableVehicles(
        [FromQuery] DateTime pickupDate, 
        [FromQuery] DateTime dropDate)
    {
        try
        {
            var vehicles = await _vehicleService.GetAvailableVehiclesAsync(pickupDate, dropDate);
            return Ok(new ApiResponse<IEnumerable<VehicleBookingDto>>
            {
                Success = true,
                Message = "Available vehicles retrieved successfully",
                Data = vehicles
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available vehicles");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get bookings by status
    /// </summary>
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<VehicleBookingDto>>>> GetBookingsByStatus(string status)
    {
        try
        {
            var bookings = await _vehicleService.GetBookingsByStatusAsync(status);
            return Ok(new ApiResponse<IEnumerable<VehicleBookingDto>>
            {
                Success = true,
                Message = "Bookings retrieved successfully",
                Data = bookings
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving bookings by status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }
}
