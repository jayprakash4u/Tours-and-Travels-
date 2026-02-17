using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TravelAgency.Domain.Enums;

namespace TravelAgency.Domain.Entities;

/// <summary>
/// Represents a hotel booking entity for travel accommodations.
/// </summary>
[Table("HotelBookings")]
public class HotelBooking
{
    /// <summary>
    /// Gets or sets the unique identifier for the hotel booking.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the user ID associated with the booking.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the hotel.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string HotelName { get; set; } = default!;

    /// <summary>
    /// Gets or sets the location of the hotel.
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Location { get; set; } = default!;

    /// <summary>
    /// Gets or sets the check-in date.
    /// </summary>
    public DateTime CheckInDate { get; set; }

    /// <summary>
    /// Gets or sets the check-out date.
    /// </summary>
    public DateTime CheckOutDate { get; set; }

    /// <summary>
    /// Gets or sets the number of guests.
    /// </summary>
    public int NumberOfGuests { get; set; }

    /// <summary>
    /// Gets or sets the guest's full name.
    /// </summary>
    [StringLength(100)]
    public string? GuestName { get; set; }

    /// <summary>
    /// Gets or sets the guest's email address.
    /// </summary>
    [StringLength(100)]
    public string? GuestEmail { get; set; }

    /// <summary>
    /// Gets or sets the guest's phone number.
    /// </summary>
    [StringLength(20)]
    public string? GuestPhone { get; set; }

    /// <summary>
    /// Gets or sets the room type (Standard, Deluxe, Suite, etc.).
    /// </summary>
    [StringLength(50)]
    public string? RoomType { get; set; }

    /// <summary>
    /// Gets or sets any special requests from the guest.
    /// </summary>
    [StringLength(500)]
    public string? SpecialRequests { get; set; }

    /// <summary>
    /// Gets or sets the booking status.
    /// </summary>
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// Gets or sets the total price for the booking.
    /// </summary>
    [Column(TypeName = "decimal(10,2)")]
    public decimal? TotalPrice { get; set; }

    /// <summary>
    /// Gets or sets any admin notes about the booking.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the booking was created.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the date and time when the booking was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}
