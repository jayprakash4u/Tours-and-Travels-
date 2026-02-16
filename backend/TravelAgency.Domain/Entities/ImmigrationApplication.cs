using TravelAgency.Domain.Enums;

namespace TravelAgency.Domain.Entities;

public class ImmigrationApplication
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public required string FullName { get; set; }
    
    public required string PassportNumber { get; set; }
    
    public required string TargetCountry { get; set; }
    
    public DateTime PassportExpiryDate { get; set; }
    
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    
    public string? DocumentsPath { get; set; } // Path to stored documents
    
    public string? VisaType { get; set; } // Work, Study, Tourist, etc.
    
    public DateTime? ExectedProcessingDate { get; set; }
    
    public string? RejectionReason { get; set; }
    
    public string? AdminNotes { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedDate { get; set; }
    
    public DateTime? CompletedDate { get; set; }
    
    // Navigation Property
    public User? User { get; set; }
}
