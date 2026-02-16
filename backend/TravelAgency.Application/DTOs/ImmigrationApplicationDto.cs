using TravelAgency.Domain.Enums;

namespace TravelAgency.Application.DTOs;

public class ImmigrationApplicationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string TargetCountry { get; set; }
    public DateTime PassportExpiryDate { get; set; }
    public ApplicationStatus Status { get; set; }
    public string? DocumentsPath { get; set; }
    public string? VisaType { get; set; }
    public DateTime? ExectedProcessingDate { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateImmigrationApplicationDto
{
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string TargetCountry { get; set; }
    public DateTime PassportExpiryDate { get; set; }
    public string? VisaType { get; set; }
}

public class UpdateImmigrationApplicationDto
{
    public required string FullName { get; set; }
    public required string PassportNumber { get; set; }
    public required string TargetCountry { get; set; }
    public DateTime PassportExpiryDate { get; set; }
    public string? VisaType { get; set; }
}

public class UpdateApplicationStatusDto
{
    public ApplicationStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public string? AdminNotes { get; set; }
    public DateTime? ExectedProcessingDate { get; set; }
}
