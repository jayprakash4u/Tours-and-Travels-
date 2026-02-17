using TravelAgency.Domain.Enums;

namespace TravelAgency.Application.DTOs;

public class HotelBookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string HotelName { get; set; }
    public required string Location { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public string? RoomType { get; set; }
    public string? SpecialRequests { get; set; }
    public BookingStatus Status { get; set; }
    public decimal? TotalPrice { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateHotelBookingDto
{
    public required string HotelName { get; set; }
    public required string Location { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public string? RoomType { get; set; }
    public string? SpecialRequests { get; set; }
}

public class UpdateHotelBookingDto
{
    public required string HotelName { get; set; }
    public required string Location { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
    public int NumberOfGuests { get; set; }
    public string? GuestName { get; set; }
    public string? GuestEmail { get; set; }
    public string? GuestPhone { get; set; }
    public string? RoomType { get; set; }
    public string? SpecialRequests { get; set; }
}

public class UpdateHotelBookingStatusDto
{
    public BookingStatus Status { get; set; }
    public decimal? TotalPrice { get; set; }
    public string? Notes { get; set; }
}
