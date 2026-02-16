using TravelAgency.Domain.Enums;

namespace TravelAgency.Domain.Entities;

public class TicketRequest
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public required string FromLocation { get; set; }
    
    public required string ToLocation { get; set; }
    
    public DateTime TravelDate { get; set; }
    
    public TicketType TicketType { get; set; } = TicketType.Flight;
    
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    
    public int? NumberOfPassengers { get; set; }
    
    public decimal? EstimatedPrice { get; set; }
    
    public string? Notes { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? CompletedDate { get; set; }
    
    // Navigation Property
    public User? User { get; set; }
}
