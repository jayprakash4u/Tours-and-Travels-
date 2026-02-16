using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for handling authentication operations including registration, login, and password management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthController"/> class.
    /// </summary>
    /// <param name="userService">The user service for authentication operations.</param>
    /// <param name="logger">The logger for recording authentication events.</param>
    public AuthController(IUserService userService, ILogger<AuthController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(createUserDto);
            return Ok(new ApiResponse<UserDto>
            {
                Success = true,
                Message = "User registered successfully",
                Data = user
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registration failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration error");
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "An error occurred during registration"
            });
        }
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            var result = await _userService.LoginAsync(loginDto);
            if (result == null)
            {
                return Unauthorized(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            return Ok(new ApiResponse<LoginResponseDto>
            {
                Success = true,
                Message = "Login successful",
                Data = result
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error");
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "An error occurred during login"
            });
        }
    }

    /// <summary>
    /// Change user password
    /// </summary>
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value ?? "0");
            if (userId <= 0)
                return Unauthorized();

            var (success, message) = await _userService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword);
            
            return success 
                ? Ok(new ApiResponse { Success = true, Message = message })
                : BadRequest(new ApiResponse { Success = false, Message = message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Change password error");
            return StatusCode(500, new ApiResponse
            {
                Success = false,
                Message = "An error occurred while changing password"
            });
        }
    }
}

/// <summary>
/// Request model for changing user password.
/// </summary>
public class ChangePasswordRequest
{
    /// <summary>
    /// Gets or sets the current password.
    /// </summary>
    public required string CurrentPassword { get; set; }
    
    /// <summary>
    /// Gets or sets the new password.
    /// </summary>
    public required string NewPassword { get; set; }
}
