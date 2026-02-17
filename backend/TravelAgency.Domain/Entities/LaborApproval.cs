using System.ComponentModel.DataAnnotations;

namespace TravelAgency.Domain.Entities;

/// <summary>
/// Represents a labor approval (Shram Swikriti) application for Nepali foreign employment.
/// </summary>
public class LaborApproval
{
    [Key]
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string FullName { get; set; }
    
    [Required]
    [MaxLength(20)]
    public required string PassportNumber { get; set; }
    
    public DateTime PassportExpiryDate { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string Gender { get; set; }
    
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [MaxLength(200)]
    public required string PermanentAddress { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string CurrentAddress { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string DestinationCountry { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string RecruitingAgency { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string CompanyName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string JobCategory { get; set; }
    
    [Required]
    [MaxLength(50)]
    public required string VisaType { get; set; }
    
    public decimal OfferedSalary { get; set; }
    
    [MaxLength(20)]
    public string? ContractDuration { get; set; }
    
    [MaxLength(500)]
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
    
    public LaborApprovalStatus Status { get; set; } = LaborApprovalStatus.Pending;
    
    [MaxLength(500)]
    public string? AdminNotes { get; set; }
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    public DateTime? ApprovedDate { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedDate { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}

/// <summary>
/// Status of labor approval application.
/// </summary>
public enum LaborApprovalStatus
{
    Pending = 0,
    UnderReview = 1,
    DocumentsVerified = 2,
    FeePending = 3,
    FeePaid = 4,
    Approved = 5,
    Rejected = 6,
    Completed = 7
}
