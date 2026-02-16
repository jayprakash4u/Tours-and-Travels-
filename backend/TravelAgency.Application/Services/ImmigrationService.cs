using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Repositories;

namespace TravelAgency.Application.Services;

public class ImmigrationService : IImmigrationService
{
    private readonly IImmigrationRepository _immigrationRepository;
    private readonly IUserRepository _userRepository;

    public ImmigrationService(IImmigrationRepository immigrationRepository, IUserRepository userRepository)
    {
        _immigrationRepository = immigrationRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<ImmigrationApplicationDto>> GetAllApplicationsAsync()
    {
        var applications = await _immigrationRepository.GetAllAsync();
        return applications.Select(MapToDto);
    }

    public async Task<ImmigrationApplicationDto?> GetApplicationByIdAsync(int id)
    {
        var application = await _immigrationRepository.GetByIdAsync(id);
        return application == null ? null : MapToDto(application);
    }

    public async Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByUserIdAsync(int userId)
    {
        var applications = await _immigrationRepository.GetByUserIdAsync(userId);
        return applications.Select(MapToDto);
    }

    public async Task<ImmigrationApplicationDto> CreateApplicationAsync(int userId, CreateImmigrationApplicationDto createDto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var application = new ImmigrationApplication
        {
            UserId = userId,
            FullName = createDto.FullName,
            PassportNumber = createDto.PassportNumber,
            TargetCountry = createDto.TargetCountry,
            PassportExpiryDate = createDto.PassportExpiryDate,
            VisaType = createDto.VisaType,
            Status = ApplicationStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        var createdApplication = await _immigrationRepository.AddAsync(application);
        await _immigrationRepository.SaveChangesAsync();

        return MapToDto(createdApplication);
    }

    public async Task<ImmigrationApplicationDto> UpdateApplicationAsync(int id, UpdateImmigrationApplicationDto updateDto)
    {
        var application = await _immigrationRepository.GetByIdAsync(id);
        if (application == null)
            throw new InvalidOperationException($"Application with ID {id} not found");

        application.FullName = updateDto.FullName;
        application.PassportNumber = updateDto.PassportNumber;
        application.TargetCountry = updateDto.TargetCountry;
        application.PassportExpiryDate = updateDto.PassportExpiryDate;
        application.VisaType = updateDto.VisaType;
        application.UpdatedDate = DateTime.UtcNow;

        var updatedApplication = await _immigrationRepository.UpdateAsync(application);
        await _immigrationRepository.SaveChangesAsync();

        return MapToDto(updatedApplication);
    }

    public async Task<ImmigrationApplicationDto> UpdateApplicationStatusAsync(int id, UpdateApplicationStatusDto updateStatusDto)
    {
        var application = await _immigrationRepository.GetByIdAsync(id);
        if (application == null)
            throw new InvalidOperationException($"Application with ID {id} not found");

        application.Status = updateStatusDto.Status;
        application.RejectionReason = updateStatusDto.RejectionReason;
        application.AdminNotes = updateStatusDto.AdminNotes;
        application.ExectedProcessingDate = updateStatusDto.ExectedProcessingDate;
        application.UpdatedDate = DateTime.UtcNow;

        if (updateStatusDto.Status == ApplicationStatus.Approved || updateStatusDto.Status == ApplicationStatus.Rejected)
            application.CompletedDate = DateTime.UtcNow;

        var updatedApplication = await _immigrationRepository.UpdateAsync(application);
        await _immigrationRepository.SaveChangesAsync();

        return MapToDto(updatedApplication);
    }

    public async Task<bool> DeleteApplicationAsync(int id)
    {
        var deleted = await _immigrationRepository.DeleteAsync(id);
        if (deleted)
            await _immigrationRepository.SaveChangesAsync();
        return deleted;
    }

    public async Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByStatusAsync(string status)
    {
        if (!Enum.TryParse<ApplicationStatus>(status, true, out var applicationStatus))
            throw new InvalidOperationException($"Invalid status: {status}");

        var applications = await _immigrationRepository.GetByStatusAsync(applicationStatus);
        return applications.Select(MapToDto);
    }

    public async Task<IEnumerable<ImmigrationApplicationDto>> GetApplicationsByCountryAsync(string country)
    {
        var applications = await _immigrationRepository.GetByCountryAsync(country);
        return applications.Select(MapToDto);
    }

    public async Task<bool> UploadDocumentsAsync(int applicationId, string documentPath)
    {
        var application = await _immigrationRepository.GetByIdAsync(applicationId);
        if (application == null)
            return false;

        application.DocumentsPath = documentPath;
        application.UpdatedDate = DateTime.UtcNow;

        await _immigrationRepository.UpdateAsync(application);
        await _immigrationRepository.SaveChangesAsync();

        return true;
    }

    private static ImmigrationApplicationDto MapToDto(ImmigrationApplication application)
    {
        return new ImmigrationApplicationDto
        {
            Id = application.Id,
            UserId = application.UserId,
            FullName = application.FullName,
            PassportNumber = application.PassportNumber,
            TargetCountry = application.TargetCountry,
            PassportExpiryDate = application.PassportExpiryDate,
            Status = application.Status,
            DocumentsPath = application.DocumentsPath,
            VisaType = application.VisaType,
            ExectedProcessingDate = application.ExectedProcessingDate,
            RejectionReason = application.RejectionReason,
            CreatedDate = application.CreatedDate
        };
    }
}
