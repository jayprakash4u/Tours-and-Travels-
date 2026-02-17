using TravelAgency.Application.DTOs;
using TravelAgency.Domain.Entities;

namespace TravelAgency.Application.Interfaces;

/// <summary>
/// Interface for labor approval service operations.
/// </summary>
public interface ILaborApprovalService
{
    /// <summary>
    /// Gets all labor approval applications (Admin only).
    /// </summary>
    Task<IEnumerable<LaborApprovalDto>> GetAllApplicationsAsync();

    /// <summary>
    /// Gets a labor approval application by ID.
    /// </summary>
    Task<LaborApprovalDto?> GetApplicationByIdAsync(int id);

    /// <summary>
    /// Gets all labor approval applications for a specific user.
    /// </summary>
    Task<IEnumerable<LaborApprovalDto>> GetApplicationsByUserIdAsync(int userId);

    /// <summary>
    /// Creates a new labor approval application.
    /// </summary>
    Task<LaborApprovalDto> CreateApplicationAsync(int userId, CreateLaborApprovalDto createDto);

    /// <summary>
    /// Updates the status of a labor approval application.
    /// </summary>
    Task<LaborApprovalDto> UpdateApplicationStatusAsync(int id, UpdateLaborApprovalStatusDto statusDto);

    /// <summary>
    /// Processes payment for a labor approval application.
    /// </summary>
    Task<LaborApprovalDto> ProcessPaymentAsync(int id, LaborApprovalPaymentDto paymentDto);

    /// <summary>
    /// Deletes a labor approval application.
    /// </summary>
    Task<bool> DeleteApplicationAsync(int id);

    /// <summary>
    /// Calculates the fee for a labor approval application.
    /// </summary>
    Task<LaborApprovalFeeDto> CalculateFeeAsync(string destinationCountry, string jobCategory, decimal offeredSalary);
}
