using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.Application.Services;

/// <summary>
/// Service for managing labor approval (Shram Swikriti) applications.
/// </summary>
public class LaborApprovalService : ILaborApprovalService
{
    private readonly ApplicationDbContext _context;

    public LaborApprovalService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<LaborApprovalDto>> GetAllApplicationsAsync()
    {
        var applications = await _context.LaborApprovals
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
        
        return applications.Select(MapToDto);
    }

    public async Task<LaborApprovalDto?> GetApplicationByIdAsync(int id)
    {
        var application = await _context.LaborApprovals.FindAsync(id);
        return application == null ? null : MapToDto(application);
    }

    public async Task<IEnumerable<LaborApprovalDto>> GetApplicationsByUserIdAsync(int userId)
    {
        var applications = await _context.LaborApprovals
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.CreatedDate)
            .ToListAsync();
        
        return applications.Select(MapToDto);
    }

    public async Task<LaborApprovalDto> CreateApplicationAsync(int userId, CreateLaborApprovalDto createDto)
    {
        // Calculate fee based on destination country and job category
        var fee = await CalculateFeeAsync(createDto.DestinationCountry, createDto.JobCategory, createDto.OfferedSalary);

        var application = new LaborApproval
        {
            UserId = userId,
            FullName = createDto.FullName,
            PassportNumber = createDto.PassportNumber,
            PassportExpiryDate = createDto.PassportExpiryDate,
            Gender = createDto.Gender,
            DateOfBirth = createDto.DateOfBirth,
            PermanentAddress = createDto.PermanentAddress,
            CurrentAddress = createDto.CurrentAddress,
            DestinationCountry = createDto.DestinationCountry,
            RecruitingAgency = createDto.RecruitingAgency,
            CompanyName = createDto.CompanyName,
            JobCategory = createDto.JobCategory,
            VisaType = createDto.VisaType,
            OfferedSalary = createDto.OfferedSalary,
            ContractDuration = createDto.ContractDuration,
            HasPoliceClearance = createDto.HasPoliceClearance,
            HasMedicalCertificate = createDto.HasMedicalCertificate,
            HasTrainingCertificate = createDto.HasTrainingCertificate,
            ApplicationFee = fee.ApplicationFee,
            ServiceCharge = fee.ServiceCharge,
            TotalFee = fee.TotalFee,
            Status = LaborApprovalStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        _context.LaborApprovals.Add(application);
        await _context.SaveChangesAsync();

        return MapToDto(application);
    }

    public async Task<LaborApprovalDto> UpdateApplicationStatusAsync(int id, UpdateLaborApprovalStatusDto statusDto)
    {
        var application = await _context.LaborApprovals.FindAsync(id);
        if (application == null)
            throw new InvalidOperationException("Application not found");

        application.Status = statusDto.Status;
        application.AdminNotes = statusDto.AdminNotes;
        application.RejectionReason = statusDto.RejectionReason;
        application.UpdatedDate = DateTime.UtcNow;

        if (statusDto.Status == LaborApprovalStatus.Approved)
        {
            application.ApprovedDate = DateTime.UtcNow;
        }
        else if (statusDto.Status == LaborApprovalStatus.Completed)
        {
            application.CompletedDate = DateTime.UtcNow;
        }

        await _context.SaveChangesAsync();
        return MapToDto(application);
    }

    public async Task<LaborApprovalDto> ProcessPaymentAsync(int id, LaborApprovalPaymentDto paymentDto)
    {
        var application = await _context.LaborApprovals.FindAsync(id);
        if (application == null)
            throw new InvalidOperationException("Application not found");

        if (application.IsFeePaid)
            throw new InvalidOperationException("Payment already processed");

        if (paymentDto.Amount < application.TotalFee)
            throw new InvalidOperationException("Insufficient payment amount");

        application.IsFeePaid = true;
        application.PaymentReference = paymentDto.PaymentReference;
        application.PaymentDate = DateTime.UtcNow;
        application.Status = LaborApprovalStatus.FeePaid;
        application.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDto(application);
    }

    public async Task<bool> DeleteApplicationAsync(int id)
    {
        var application = await _context.LaborApprovals.FindAsync(id);
        if (application == null)
            return false;

        _context.LaborApprovals.Remove(application);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<LaborApprovalFeeDto> CalculateFeeAsync(string destinationCountry, string jobCategory, decimal offeredSalary)
    {
        // Fee calculation based on Nepal DOFE guidelines
        decimal applicationFee = 0;
        decimal serviceCharge = 0;

        // Application fee based on destination country
        switch (destinationCountry.ToLower())
        {
            case "malaysia":
                applicationFee = 7500; // NPR
                break;
            case "qatar":
                applicationFee = 10000;
                break;
            case "uae":
            case "united arab emirates":
                applicationFee = 12000;
                break;
            case "saudi arabia":
                applicationFee = 8500;
                break;
            case "kuwait":
                applicationFee = 9000;
                break;
            case "bahrain":
                applicationFee = 8000;
                break;
            case "oman":
                applicationFee = 8500;
                break;
            case "iran":
                applicationFee = 6000;
                break;
            case "iraq":
                applicationFee = 7000;
                break;
            case "libya":
                applicationFee = 7500;
                break;
            case "afghanistan":
                applicationFee = 5000;
                break;
            default:
                applicationFee = 5000; // Default fee
                break;
        }

        // Service charge based on job category
        switch (jobCategory.ToLower())
        {
            case "manager":
            case "engineer":
            case "doctor":
                serviceCharge = 5000;
                break;
            case "technician":
            case "skilled worker":
                serviceCharge = 3000;
                break;
            case "semi-skilled":
                serviceCharge = 2000;
                break;
            case "unskilled":
            case "labor":
                serviceCharge = 1000;
                break;
            default:
                serviceCharge = 2000;
                break;
        }

        // Additional fee for higher salary
        if (offeredSalary > 50000)
            serviceCharge += 2000;
        else if (offeredSalary > 30000)
            serviceCharge += 1000;

        return new LaborApprovalFeeDto
        {
            ApplicationFee = applicationFee,
            ServiceCharge = serviceCharge,
            TotalFee = applicationFee + serviceCharge,
            Country = destinationCountry,
            JobCategory = jobCategory
        };
    }

    private static LaborApprovalDto MapToDto(LaborApproval application)
    {
        return new LaborApprovalDto
        {
            Id = application.Id,
            UserId = application.UserId,
            FullName = application.FullName,
            PassportNumber = application.PassportNumber,
            PassportExpiryDate = application.PassportExpiryDate,
            Gender = application.Gender,
            DateOfBirth = application.DateOfBirth,
            PermanentAddress = application.PermanentAddress,
            CurrentAddress = application.CurrentAddress,
            DestinationCountry = application.DestinationCountry,
            RecruitingAgency = application.RecruitingAgency,
            CompanyName = application.CompanyName,
            JobCategory = application.JobCategory,
            VisaType = application.VisaType,
            OfferedSalary = application.OfferedSalary,
            ContractDuration = application.ContractDuration,
            DocumentsPath = application.DocumentsPath,
            HasPoliceClearance = application.HasPoliceClearance,
            HasMedicalCertificate = application.HasMedicalCertificate,
            HasTrainingCertificate = application.HasTrainingCertificate,
            ApplicationFee = application.ApplicationFee,
            ServiceCharge = application.ServiceCharge,
            TotalFee = application.TotalFee,
            IsFeePaid = application.IsFeePaid,
            PaymentReference = application.PaymentReference,
            PaymentDate = application.PaymentDate,
            Status = application.Status,
            AdminNotes = application.AdminNotes,
            RejectionReason = application.RejectionReason,
            ApprovedDate = application.ApprovedDate,
            CompletedDate = application.CompletedDate,
            CreatedDate = application.CreatedDate,
            UpdatedDate = application.UpdatedDate
        };
    }
}
