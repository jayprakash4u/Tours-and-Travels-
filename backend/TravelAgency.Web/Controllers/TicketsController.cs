using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for managing travel ticket requests and bookings.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketsController : ControllerBase
{
    private readonly ITicketService _ticketService;
    private readonly ILogger<TicketsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TicketsController"/> class.
    /// </summary>
    /// <param name="ticketService">The ticket service for managing ticket operations.</param>
    /// <param name="logger">The logger for recording ticket events.</param>
    public TicketsController(ITicketService ticketService, ILogger<TicketsController> logger)
    {
        _ticketService = ticketService;
        _logger = logger;
    }

    /// <summary>
    /// Get all ticket requests (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketRequestDto>>>> GetAllTickets()
    {
        try
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return Ok(new ApiResponse<IEnumerable<TicketRequestDto>>
            {
                Success = true,
                Message = "Tickets retrieved successfully",
                Data = tickets
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tickets");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get ticket by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<TicketRequestDto>>> GetTicketById(int id)
    {
        try
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound(new ApiResponse { Success = false, Message = "Ticket not found" });

            return Ok(new ApiResponse<TicketRequestDto>
            {
                Success = true,
                Message = "Ticket retrieved successfully",
                Data = ticket
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving ticket");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get user's ticket requests
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketRequestDto>>>> GetUserTickets(int userId)
    {
        try
        {
            var tickets = await _ticketService.GetTicketsByUserIdAsync(userId);
            return Ok(new ApiResponse<IEnumerable<TicketRequestDto>>
            {
                Success = true,
                Message = "User tickets retrieved successfully",
                Data = tickets
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user tickets");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Create a new ticket request
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<TicketRequestDto>>> CreateTicket([FromBody] CreateTicketRequestDto createDto, [FromQuery] int userId)
    {
        try
        {
            var ticket = await _ticketService.CreateTicketAsync(userId, createDto);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, new ApiResponse<TicketRequestDto>
            {
                Success = true,
                Message = "Ticket created successfully",
                Data = ticket
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Ticket creation failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ticket");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update a ticket request
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<TicketRequestDto>>> UpdateTicket(int id, [FromBody] UpdateTicketRequestDto updateDto)
    {
        try
        {
            var ticket = await _ticketService.UpdateTicketAsync(id, updateDto);
            return Ok(new ApiResponse<TicketRequestDto>
            {
                Success = true,
                Message = "Ticket updated successfully",
                Data = ticket
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Ticket update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update ticket status (Admin only)
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<TicketRequestDto>>> UpdateTicketStatus(int id, [FromBody] UpdateTicketStatusDto updateStatusDto)
    {
        try
        {
            var ticket = await _ticketService.UpdateTicketStatusAsync(id, updateStatusDto);
            return Ok(new ApiResponse<TicketRequestDto>
            {
                Success = true,
                Message = "Ticket status updated successfully",
                Data = ticket
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Status update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating ticket status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Delete a ticket request
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteTicket(int id)
    {
        try
        {
            var deleted = await _ticketService.DeleteTicketAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse { Success = false, Message = "Ticket not found" });

            return Ok(new ApiResponse { Success = true, Message = "Ticket deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting ticket");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get tickets by status
    /// </summary>
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<TicketRequestDto>>>> GetTicketsByStatus(string status)
    {
        try
        {
            var tickets = await _ticketService.GetTicketsByStatusAsync(status);
            return Ok(new ApiResponse<IEnumerable<TicketRequestDto>>
            {
                Success = true,
                Message = "Tickets retrieved successfully",
                Data = tickets
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tickets by status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }
}
