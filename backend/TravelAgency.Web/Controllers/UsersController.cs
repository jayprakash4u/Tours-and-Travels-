using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for managing user profiles and information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UsersController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="UsersController"/> class.
    /// </summary>
    /// <param name="userService">The user service for managing user operations.</param>
    /// <param name="logger">The logger for recording user events.</param>
    public UsersController(IUserService userService, ILogger<UsersController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(new ApiResponse<IEnumerable<UserDto>>
            {
                Success = true,
                Message = "Users retrieved successfully",
                Data = users
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get user by ID
    /// </summary>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserById(int id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });

            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = user
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get user by email
    /// </summary>
    [HttpGet("email/{email}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetUserByEmail(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });

            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User retrieved successfully",
                Data = user
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user by email");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update user profile
    /// </summary>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var user = await _userService.UpdateUserAsync(id, updateUserDto);
            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User updated successfully",
                Data = user
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("User update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Delete user account (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse>> DeleteUser(int id)
    {
        try
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse { Success = false, Message = "User not found" });

            return Ok(new ApiResponse { Success = true, Message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Check if email is available
    /// </summary>
    [HttpGet("check-email/{email}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<bool>>> CheckEmailAvailability(string email)
    {
        try
        {
            var user = await _userService.GetUserByEmailAsync(email);
            var isAvailable = user == null;

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Message = isAvailable ? "Email is available" : "Email is already in use",
                Data = isAvailable
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email availability");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }
}
