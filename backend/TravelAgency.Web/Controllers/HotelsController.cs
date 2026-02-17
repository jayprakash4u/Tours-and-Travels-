using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for managing hotel bookings and reservations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class HotelsController : ControllerBase
{
    private readonly IHotelBookingService _hotelService;
    private readonly ILogger<HotelsController> _logger;

    /// <summary>
    /// Initializes a new instance of the HotelsController class.
    /// </summary>
    public HotelsController(IHotelBookingService hotelService, ILogger<HotelsController> logger)
    {
        _hotelService = hotelService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all hotel bookings (Admin only).
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelBookingDto>>>> GetAllBookings()
    {
        try
        {
            var bookings = await _hotelService.GetAllBookingsAsync();
            return Ok(new ApiResponse<IEnumerable<HotelBookingDto>>
            {
                Success = true,
                Message = "Hotel bookings retrieved successfully",
                Data = bookings
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotel bookings");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Gets a hotel booking by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<HotelBookingDto>>> GetBookingById(int id)
    {
        try
        {
            var booking = await _hotelService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(new ApiResponse { Success = false, Message = "Booking not found" });

            return Ok(new ApiResponse<HotelBookingDto>
            {
                Success = true,
                Message = "Hotel booking retrieved successfully",
                Data = booking
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving hotel booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Gets user's hotel bookings.
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelBookingDto>>>> GetUserBookings(int userId)
    {
        try
        {
            var bookings = await _hotelService.GetBookingsByUserIdAsync(userId);
            return Ok(new ApiResponse<IEnumerable<HotelBookingDto>>
            {
                Success = true,
                Message = "User hotel bookings retrieved successfully",
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
    /// Searches for available hotels.
    /// </summary>
    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<IEnumerable<HotelBookingDto>>>> SearchHotels(
        [FromQuery] string location,
        [FromQuery] DateTime checkIn,
        [FromQuery] DateTime checkOut,
        [FromQuery] int guests = 1)
    {
        try
        {
            var hotels = await _hotelService.SearchHotelsAsync(location, checkIn, checkOut, guests);
            return Ok(new ApiResponse<IEnumerable<HotelBookingDto>>
            {
                Success = true,
                Message = "Hotels found",
                Data = hotels
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching hotels");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Creates a new hotel booking.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<HotelBookingDto>>> CreateBooking([FromBody] CreateHotelBookingDto createDto, [FromQuery] int userId)
    {
        try
        {
            var booking = await _hotelService.CreateBookingAsync(userId, createDto);
            return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, new ApiResponse<HotelBookingDto>
            {
                Success = true,
                Message = "Hotel booking created successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Hotel booking creation failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating hotel booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Updates a hotel booking.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<HotelBookingDto>>> UpdateBooking(int id, [FromBody] UpdateHotelBookingDto updateDto)
    {
        try
        {
            var booking = await _hotelService.UpdateBookingAsync(id, updateDto);
            return Ok(new ApiResponse<HotelBookingDto>
            {
                Success = true,
                Message = "Hotel booking updated successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Hotel booking update failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating hotel booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Updates a hotel booking status.
    /// </summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<HotelBookingDto>>> UpdateBookingStatus(int id, [FromBody] UpdateHotelBookingStatusDto statusDto)
    {
        try
        {
            var booking = await _hotelService.UpdateBookingStatusAsync(id, statusDto);
            return Ok(new ApiResponse<HotelBookingDto>
            {
                Success = true,
                Message = "Hotel booking status updated successfully",
                Data = booking
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Hotel booking status update failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating hotel booking status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Deletes a hotel booking.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteBooking(int id)
    {
        try
        {
            var result = await _hotelService.DeleteBookingAsync(id);
            if (!result)
                return NotFound(new ApiResponse { Success = false, Message = "Booking not found" });

            return Ok(new ApiResponse { Success = true, Message = "Hotel booking deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting hotel booking");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }
}
