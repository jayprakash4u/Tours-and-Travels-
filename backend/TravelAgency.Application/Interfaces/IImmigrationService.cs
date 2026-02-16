using TravelAgency.Application.DTOs;

namespace TravelAgency.Application.Interfaces;

public interface IImmigrationService
{
    Task<IEnumerable<ImmigrationApplicationDto>> GetAllApplicationsAsync();
    Task<ImmigrationApplicationDto?> GetApplicationByIdAsync(int id);
    Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByUserIdAsync(int userId);
    Task<ImmigrationApplicationDto> CreateApplicationAsync(int userId, CreateImmigrationApplicationDto createDto);
    Task<ImmigrationApplicationDto> UpdateApplicationAsync(int id, UpdateImmigrationApplicationDto updateDto);
    Task<ImmigrationApplicationDto> UpdateApplicationStatusAsync(int id, UpdateApplicationStatusDto updateStatusDto);
    Task<bool> DeleteApplicationAsync(int id);
    Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByStatusAsync(string status);
    Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByCountryAsync(string country);
    Task<bool> UploadDocumentsAsync(int applicationId, string documentPath);
}
