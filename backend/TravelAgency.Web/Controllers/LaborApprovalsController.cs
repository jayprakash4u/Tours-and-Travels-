using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;

namespace TravelAgency.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LaborApprovalsController : ControllerBase
{
    private readonly ILaborApprovalService _laborApprovalService;
    private readonly ILogger<LaborApprovalsController> _logger;

    public LaborApprovalsController(ILaborApprovalService laborApprovalService, ILogger<LaborApprovalsController> logger)
    {
        _laborApprovalService = laborApprovalService;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<LaborApprovalDto>>> GetAllApplications()
    {
        var applications = await _laborApprovalService.GetAllApplicationsAsync();
        return Ok(applications);
    }

    [HttpGet("my-applications")]
    public async Task<ActionResult<IEnumerable<LaborApprovalDto>>> GetMyApplications()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var applications = await _laborApprovalService.GetApplicationsByUserIdAsync(userId.Value);
        return Ok(applications);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LaborApprovalDto>> GetApplication(int id)
    {
        var application = await _laborApprovalService.GetApplicationByIdAsync(id);
        if (application == null)
            return NotFound();

        // Check if user owns this application or is admin
        var userId = GetCurrentUserId();
        var isAdmin = User.IsInRole("Admin");
        
        if (!isAdmin && application.UserId != userId)
            return Forbid();

        return Ok(application);
    }

    [HttpPost]
    public async Task<ActionResult<LaborApprovalDto>> CreateApplication([FromBody] CreateLaborApprovalDto createDto)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        try
        {
            var application = await _laborApprovalService.CreateApplicationAsync(userId.Value, createDto);
            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating labor approval application");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<LaborApprovalDto>> UpdateStatus(int id, [FromBody] UpdateLaborApprovalStatusDto statusDto)
    {
        try
        {
            var application = await _laborApprovalService.UpdateApplicationStatusAsync(id, statusDto);
            return Ok(application);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/payment")]
    public async Task<ActionResult<LaborApprovalDto>> ProcessPayment(int id, [FromBody] LaborApprovalPaymentDto paymentDto)
    {
        try
        {
            var application = await _laborApprovalService.ProcessPaymentAsync(id, paymentDto);
            return Ok(application);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("calculate-fee")]
    public async Task<ActionResult<LaborApprovalFeeDto>> CalculateFee([FromQuery] string country, [FromQuery] string jobCategory, [FromQuery] decimal salary)
    {
        var fee = await _laborApprovalService.CalculateFeeAsync(country, jobCategory, salary);
        return Ok(fee);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteApplication(int id)
    {
        var result = await _laborApprovalService.DeleteApplicationAsync(id);
        if (!result)
            return NotFound();

        return NoContent();
    }

    private int? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(userIdClaim, out var userId))
            return userId;
        return null;
    }
}
