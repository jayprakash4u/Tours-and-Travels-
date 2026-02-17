using TravelAgency.Domain.Entities;

namespace TravelAgency.Application.DTOs;

/// <summary>
/// DTO for labor approval application.
/// </summary>
public class LaborApprovalDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public DateTime PassportExpiryDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string PermanentAddress { get; set; } = string.Empty;
    public string CurrentAddress { get; set; } = string.Empty;
    public string DestinationCountry { get; set; } = string.Empty;
    public string RecruitingAgency { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string JobCategory { get; set; } = string.Empty;
    public string VisaType { get; set; } = string.Empty;
    public decimal OfferedSalary { get; set; }
    public string? ContractDuration { get; set; }
    public string? DocumentsPath { get; set; }
    public bool HasPoliceClearance { get; set; }
    public bool HasMedicalCertificate { get; set; }
    public bool HasTrainingCertificate { get; set; }
    public decimal ApplicationFee { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsFeePaid { get; set; }
    public string? PaymentReference { get; set; }
    public DateTime? PaymentDate { get; set; }
    public LaborApprovalStatus Status { get; set; }
    public string? AdminNotes { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
}

/// <summary>
/// DTO for creating a new labor approval application.
/// </summary>
public class CreateLaborApprovalDto
{
    public string FullName { get; set; } = string.Empty;
    public string PassportNumber { get; set; } = string.Empty;
    public DateTime PassportExpiryDate { get; set; }
    public string Gender { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string PermanentAddress { get; set; } = string.Empty;
    public string CurrentAddress { get; set; } = string.Empty;
    public string DestinationCountry { get; set; } = string.Empty;
    public string RecruitingAgency { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string JobCategory { get; set; } = string.Empty;
    public string VisaType { get; set; } = string.Empty;
    public decimal OfferedSalary { get; set; }
    public string? ContractDuration { get; set; }
    public bool HasPoliceClearance { get; set; }
    public bool HasMedicalCertificate { get; set; }
    public bool HasTrainingCertificate { get; set; }
}

/// <summary>
/// DTO for updating labor approval status.
/// </summary>
public class UpdateLaborApprovalStatusDto
{
    public LaborApprovalStatus Status { get; set; }
    public string? AdminNotes { get; set; }
    public string? RejectionReason { get; set; }
}

/// <summary>
/// DTO for fee payment.
/// </summary>
public class LaborApprovalPaymentDto
{
    public string PaymentReference { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

/// <summary>
/// Fee calculation result.
/// </summary>
public class LaborApprovalFeeDto
{
    public decimal ApplicationFee { get; set; }
    public decimal ServiceCharge { get; set; }
    public decimal TotalFee { get; set; }
    public string Country { get; set; } = string.Empty;
    public string JobCategory { get; set; } = string.Empty;
}
