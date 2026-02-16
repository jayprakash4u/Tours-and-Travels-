using TravelAgency.Domain.Enums;

namespace TravelAgency.Domain.Entities;

public class VehicleBooking
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public required string VehicleType { get; set; } // Car, SUV, Van, Bus
    
    public DateTime PickupDate { get; set; }
    
    public DateTime DropDate { get; set; }
    
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    
    public string? PickupLocation { get; set; }
    
    public string? DropLocation { get; set; }
    
    public int? NumberOfPassengers { get; set; }
    
    public decimal? TotalCost { get; set; }
    
    public string? DriverName { get; set; }
    
    public string? DriverContactNumber { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedDate { get; set; }
    
    // Navigation Property
    public User? User { get; set; }
}
