using TravelAgency.Application.DTOs;

namespace TravelAgency.Application.Interfaces;

/// <summary>
/// Interface for hotel booking service operations.
/// </summary>
public interface IHotelBookingService
{
    /// <summary>
    /// Gets all hotel bookings (Admin only).
    /// </summary>
    Task<IEnumerable<HotelBookingDto>> GetAllBookingsAsync();

    /// <summary>
    /// Gets a hotel booking by ID.
    /// </summary>
    Task<HotelBookingDto?> GetBookingByIdAsync(int id);

    /// <summary>
    /// Gets all hotel bookings for a specific user.
    /// </summary>
    Task<IEnumerable<HotelBookingDto>> GetBookingsByUserIdAsync(int userId);

    /// <summary>
    /// Creates a new hotel booking.
    /// </summary>
    Task<HotelBookingDto> CreateBookingAsync(int userId, CreateHotelBookingDto createDto);

    /// <summary>
    /// Updates a hotel booking.
    /// </summary>
    Task<HotelBookingDto> UpdateBookingAsync(int id, UpdateHotelBookingDto updateDto);

    /// <summary>
    /// Updates the status of a hotel booking.
    /// </summary>
    Task<HotelBookingDto> UpdateBookingStatusAsync(int id, UpdateHotelBookingStatusDto statusDto);

    /// <summary>
    /// Deletes a hotel booking.
    /// </summary>
    Task<bool> DeleteBookingAsync(int id);

    /// <summary>
    /// Searches for hotels by location, dates and number of guests.
    /// </summary>
    Task<IEnumerable<HotelBookingDto>> SearchHotelsAsync(string location, DateTime checkInDate, DateTime checkOutDate, int numberOfGuests);
}
