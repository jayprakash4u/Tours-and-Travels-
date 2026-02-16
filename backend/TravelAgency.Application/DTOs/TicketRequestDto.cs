using TravelAgency.Domain.Enums;

namespace TravelAgency.Application.DTOs;

public class TicketRequestDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string FromLocation { get; set; }
    public required string ToLocation { get; set; }
    public DateTime TravelDate { get; set; }
    public TicketType TicketType { get; set; }
    public BookingStatus Status { get; set; }
    public int? NumberOfPassengers { get; set; }
    public decimal? EstimatedPrice { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateTicketRequestDto
{
    public required string FromLocation { get; set; }
    public required string ToLocation { get; set; }
    public DateTime TravelDate { get; set; }
    public TicketType TicketType { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? Notes { get; set; }
}

public class UpdateTicketRequestDto
{
    public required string FromLocation { get; set; }
    public required string ToLocation { get; set; }
    public DateTime TravelDate { get; set; }
    public TicketType TicketType { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? Notes { get; set; }
}

public class UpdateTicketStatusDto
{
    public BookingStatus Status { get; set; }
    public decimal? Price { get; set; }
}
