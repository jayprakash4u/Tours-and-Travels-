using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Web.Models;

namespace TravelAgency.Web.Controllers;

/// <summary>
/// Controller for managing immigration applications and visa processes.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImmigrationController : ControllerBase
{
    private readonly IImmigrationService _immigrationService;
    private readonly ILogger<ImmigrationController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ImmigrationController"/> class.
    /// </summary>
    /// <param name="immigrationService">The immigration service for managing immigration operations.</param>
    /// <param name="logger">The logger for recording immigration events.</param>
    public ImmigrationController(IImmigrationService immigrationService, ILogger<ImmigrationController> logger)
    {
        _immigrationService = immigrationService;
        _logger = logger;
    }

    /// <summary>
    /// Get all immigration applications (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ImmigrationApplicationDto>>>> GetAllApplications()
    {
        try
        {
            var applications = await _immigrationService.GetAllApplicationsAsync();
            return Ok(new ApiResponse<IEnumerable<ImmigrationApplicationDto>>
            {
                Success = true,
                Message = "Applications retrieved successfully",
                Data = applications
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get application by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ImmigrationApplicationDto>>> GetApplicationById(int id)
    {
        try
        {
            var application = await _immigrationService.GetApplicationByIdAsync(id);
            if (application == null)
                return NotFound(new ApiResponse { Success = false, Message = "Application not found" });

            return Ok(new ApiResponse<ImmigrationApplicationDto>
            {
                Success = true,
                Message = "Application retrieved successfully",
                Data = application
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving application");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get user's immigration applications
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ImmigrationApplicationDto>>>> GetUserApplications(int userId)
    {
        try
        {
            var applications = await _immigrationService.GetApplicationsByUserIdAsync(userId);
            return Ok(new ApiResponse<IEnumerable<ImmigrationApplicationDto>>
            {
                Success = true,
                Message = "User applications retrieved successfully",
                Data = applications
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user applications");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Create a new immigration application
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ImmigrationApplicationDto>>> CreateApplication(
        [FromBody] CreateImmigrationApplicationDto createDto, 
        [FromQuery] int userId)
    {
        try
        {
            var application = await _immigrationService.CreateApplicationAsync(userId, createDto);
            return CreatedAtAction(nameof(GetApplicationById), new { id = application.Id }, new ApiResponse<ImmigrationApplicationDto>
            {
                Success = true,
                Message = "Application created successfully",
                Data = application
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Application creation failed: {Message}", ex.Message);
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating application");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update an immigration application
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ImmigrationApplicationDto>>> UpdateApplication(
        int id, 
        [FromBody] UpdateImmigrationApplicationDto updateDto)
    {
        try
        {
            var application = await _immigrationService.UpdateApplicationAsync(id, updateDto);
            return Ok(new ApiResponse<ImmigrationApplicationDto>
            {
                Success = true,
                Message = "Application updated successfully",
                Data = application
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Application update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Update application status (Admin only)
    /// </summary>
    [HttpPatch("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<ImmigrationApplicationDto>>> UpdateApplicationStatus(
        int id, 
        [FromBody] UpdateApplicationStatusDto updateStatusDto)
    {
        try
        {
            var application = await _immigrationService.UpdateApplicationStatusAsync(id, updateStatusDto);
            return Ok(new ApiResponse<ImmigrationApplicationDto>
            {
                Success = true,
                Message = "Application status updated successfully",
                Data = application
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Status update failed: {Message}", ex.Message);
            return NotFound(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating application status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Delete an immigration application
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse>> DeleteApplication(int id)
    {
        try
        {
            var deleted = await _immigrationService.DeleteApplicationAsync(id);
            if (!deleted)
                return NotFound(new ApiResponse { Success = false, Message = "Application not found" });

            return Ok(new ApiResponse { Success = true, Message = "Application deleted successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting application");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get applications by status
    /// </summary>
    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ImmigrationApplicationDto>>>> GetApplicationsByStatus(string status)
    {
        try
        {
            var applications = await _immigrationService.GetApplicationsByStatusAsync(status);
            return Ok(new ApiResponse<IEnumerable<ImmigrationApplicationDto>>
            {
                Success = true,
                Message = "Applications retrieved successfully",
                Data = applications
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new ApiResponse { Success = false, Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications by status");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Get applications by target country
    /// </summary>
    [HttpGet("country/{country}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ImmigrationApplicationDto>>>> GetApplicationsByCountry(string country)
    {
        try
        {
            var applications = await _immigrationService.GetApplicationsByCountryAsync(country);
            return Ok(new ApiResponse<IEnumerable<ImmigrationApplicationDto>>
            {
                Success = true,
                Message = "Applications retrieved successfully",
                Data = applications
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving applications by country");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred" });
        }
    }

    /// <summary>
    /// Upload application documents
    /// </summary>
    [HttpPost("{id}/upload-documents")]
    public async Task<ActionResult<ApiResponse>> UploadDocuments(int id, [FromForm] IFormFile file)
    {
        try
        {
            if (file == null || file.Length == 0)
                return BadRequest(new ApiResponse { Success = false, Message = "No file provided" });

            var uploadsFolder = Path.Combine("wwwroot", "uploads", "documents");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{id}_{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var documentPath = Path.Combine("uploads", "documents", fileName);
            var success = await _immigrationService.UploadDocumentsAsync(id, documentPath);

            if (!success)
                return NotFound(new ApiResponse { Success = false, Message = "Application not found" });

            return Ok(new ApiResponse { Success = true, Message = "Documents uploaded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading documents");
            return StatusCode(500, new ApiResponse { Success = false, Message = "An error occurred while uploading documents" });
        }
    }
}
