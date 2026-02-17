using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Repositories;

namespace TravelAgency.Application.Services;

/// <summary>
/// Service for managing hotel bookings.
/// </summary>
public class HotelBookingService : IHotelBookingService
{
    private readonly IHotelBookingRepository _hotelRepository;

    /// <summary>
    /// Initializes a new instance of the HotelBookingService class.
    /// </summary>
    public HotelBookingService(IHotelBookingRepository hotelRepository)
    {
        _hotelRepository = hotelRepository;
    }

    /// <summary>
    /// Gets all hotel bookings.
    /// </summary>
    public async Task<IEnumerable<HotelBookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _hotelRepository.GetAllAsync();
        return bookings.Select(MapToDto);
    }

    /// <summary>
    /// Gets a hotel booking by ID.
    /// </summary>
    public async Task<HotelBookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _hotelRepository.GetByIdAsync(id);
        return booking == null ? null : MapToDto(booking);
    }

    /// <summary>
    /// Gets all bookings for a specific user.
    /// </summary>
    public async Task<IEnumerable<HotelBookingDto>> GetBookingsByUserIdAsync(int userId)
    {
        var bookings = await _hotelRepository.GetByUserIdAsync(userId);
        return bookings.Select(MapToDto);
    }

    /// <summary>
    /// Creates a new hotel booking.
    /// </summary>
    public async Task<HotelBookingDto> CreateBookingAsync(int userId, CreateHotelBookingDto createDto)
    {
        if (string.IsNullOrWhiteSpace(createDto.HotelName))
            throw new InvalidOperationException("Hotel name is required");

        if (string.IsNullOrWhiteSpace(createDto.Location))
            throw new InvalidOperationException("Location is required");

        if (createDto.CheckInDate >= createDto.CheckOutDate)
            throw new InvalidOperationException("Check-in date must be before check-out date");

        var booking = new HotelBooking
        {
            UserId = userId,
            HotelName = createDto.HotelName,
            Location = createDto.Location,
            CheckInDate = createDto.CheckInDate,
            CheckOutDate = createDto.CheckOutDate,
            NumberOfGuests = createDto.NumberOfGuests,
            GuestName = createDto.GuestName,
            GuestEmail = createDto.GuestEmail,
            GuestPhone = createDto.GuestPhone,
            RoomType = createDto.RoomType ?? "Standard",
            SpecialRequests = createDto.SpecialRequests,
            Status = BookingStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        await _hotelRepository.AddAsync(booking);
        await _hotelRepository.SaveChangesAsync();

        return MapToDto(booking);
    }

    /// <summary>
    /// Updates a hotel booking.
    /// </summary>
    public async Task<HotelBookingDto> UpdateBookingAsync(int id, UpdateHotelBookingDto updateDto)
    {
        var booking = await _hotelRepository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException("Booking not found");

        if (updateDto.CheckInDate >= updateDto.CheckOutDate)
            throw new InvalidOperationException("Check-in date must be before check-out date");

        booking.HotelName = updateDto.HotelName;
        booking.Location = updateDto.Location;
        booking.CheckInDate = updateDto.CheckInDate;
        booking.CheckOutDate = updateDto.CheckOutDate;
        booking.NumberOfGuests = updateDto.NumberOfGuests;
        booking.GuestName = updateDto.GuestName;
        booking.GuestEmail = updateDto.GuestEmail;
        booking.GuestPhone = updateDto.GuestPhone;
        booking.RoomType = updateDto.RoomType;
        booking.SpecialRequests = updateDto.SpecialRequests;
        booking.ModifiedDate = DateTime.UtcNow;

        await _hotelRepository.UpdateAsync(booking);
        await _hotelRepository.SaveChangesAsync();

        return MapToDto(booking);
    }

    /// <summary>
    /// Updates the status of a hotel booking.
    /// </summary>
    public async Task<HotelBookingDto> UpdateBookingStatusAsync(int id, UpdateHotelBookingStatusDto statusDto)
    {
        var booking = await _hotelRepository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException("Booking not found");

        booking.Status = statusDto.Status;
        booking.TotalPrice = statusDto.TotalPrice;
        booking.Notes = statusDto.Notes;
        booking.ModifiedDate = DateTime.UtcNow;

        await _hotelRepository.UpdateAsync(booking);
        await _hotelRepository.SaveChangesAsync();

        return MapToDto(booking);
    }

    /// <summary>
    /// Deletes a hotel booking.
    /// </summary>
    public async Task<bool> DeleteBookingAsync(int id)
    {
        var booking = await _hotelRepository.GetByIdAsync(id);
        if (booking == null)
            return false;

        await _hotelRepository.DeleteAsync(id);
        await _hotelRepository.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Searches for hotels (currently returns all bookings for demo purposes).
    /// </summary>
    public async Task<IEnumerable<HotelBookingDto>> SearchHotelsAsync(string location, DateTime checkInDate, DateTime checkOutDate, int numberOfGuests)
    {
        var allBookings = await _hotelRepository.GetAllAsync();
        var filtered = allBookings.Where(b =>
            b.Location.Contains(location, StringComparison.OrdinalIgnoreCase) &&
            b.CheckInDate >= checkInDate &&
            b.CheckOutDate <= checkOutDate &&
            b.NumberOfGuests >= numberOfGuests);

        return filtered.Select(MapToDto);
    }

    /// <summary>
    /// Maps a HotelBooking entity to a HotelBookingDto.
    /// </summary>
    private static HotelBookingDto MapToDto(HotelBooking booking)
    {
        return new HotelBookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            HotelName = booking.HotelName,
            Location = booking.Location,
            CheckInDate = booking.CheckInDate,
            CheckOutDate = booking.CheckOutDate,
            NumberOfGuests = booking.NumberOfGuests,
            GuestName = booking.GuestName,
            GuestEmail = booking.GuestEmail,
            GuestPhone = booking.GuestPhone,
            RoomType = booking.RoomType,
            SpecialRequests = booking.SpecialRequests,
            Status = booking.Status,
            TotalPrice = booking.TotalPrice,
            CreatedDate = booking.CreatedDate
        };
    }
}
