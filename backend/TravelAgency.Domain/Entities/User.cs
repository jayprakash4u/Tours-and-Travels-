namespace TravelAgency.Domain.Entities;

public class User
{
    public int Id { get; set; }
    
    public required string FullName { get; set; }
    
    public required string Email { get; set; }
    
    public required string PasswordHash { get; set; }
    
    public required string Role { get; set; } = "Customer"; // Admin or Customer
    
    public string? PhoneNumber { get; set; }
    
    public string? Address { get; set; }
    
    public string? ProfilePicture { get; set; } // URL to profile picture
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedDate { get; set; }
    
    // Navigation Properties
    public ICollection<TicketRequest> TicketRequests { get; set; } = new List<TicketRequest>();
    
    public ICollection<VehicleBooking> VehicleBookings { get; set; } = new List<VehicleBooking>();
    
    public ICollection<ImmigrationApplication> ImmigrationApplications { get; set; } = new List<ImmigrationApplication>();
}
