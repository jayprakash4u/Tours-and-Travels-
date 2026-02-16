using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Repositories;

namespace TravelAgency.Application.Services;

public class VehicleBookingService : IVehicleBookingService
{
    private readonly IVehicleBookingRepository _vehicleRepository;
    private readonly IUserRepository _userRepository;

    public VehicleBookingService(IVehicleBookingRepository vehicleRepository, IUserRepository userRepository)
    {
        _vehicleRepository = vehicleRepository;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<VehicleBookingDto>> GetAllBookingsAsync()
    {
        var bookings = await _vehicleRepository.GetAllAsync();
        return bookings.Select(MapToDto);
    }

    public async Task<VehicleBookingDto?> GetBookingByIdAsync(int id)
    {
        var booking = await _vehicleRepository.GetByIdAsync(id);
        return booking == null ? null : MapToDto(booking);
    }

    public async Task<IEnumerable<VehicleBookingDto>> GetBookingsByUserIdAsync(int userId)
    {
        var bookings = await _vehicleRepository.GetByUserIdAsync(userId);
        return bookings.Select(MapToDto);
    }

    public async Task<VehicleBookingDto> CreateBookingAsync(int userId, CreateVehicleBookingDto createDto)
    {
        // Verify user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            throw new InvalidOperationException($"User with ID {userId} not found");

        var booking = new VehicleBooking
        {
            UserId = userId,
            VehicleType = createDto.VehicleType,
            PickupDate = createDto.PickupDate,
            DropDate = createDto.DropDate,
            PickupLocation = createDto.PickupLocation,
            DropLocation = createDto.DropLocation,
            NumberOfPassengers = createDto.NumberOfPassengers,
            Notes = createDto.Notes,
            Status = BookingStatus.Pending,
            CreatedDate = DateTime.UtcNow
        };

        var createdBooking = await _vehicleRepository.AddAsync(booking);
        await _vehicleRepository.SaveChangesAsync();

        return MapToDto(createdBooking);
    }

    public async Task<VehicleBookingDto> UpdateBookingAsync(int id, UpdateVehicleBookingDto updateDto)
    {
        var booking = await _vehicleRepository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException($"Booking with ID {id} not found");

        booking.PickupDate = updateDto.PickupDate;
        booking.DropDate = updateDto.DropDate;
        booking.PickupLocation = updateDto.PickupLocation;
        booking.DropLocation = updateDto.DropLocation;
        booking.NumberOfPassengers = updateDto.NumberOfPassengers;
        booking.Notes = updateDto.Notes;

        var updatedBooking = await _vehicleRepository.UpdateAsync(booking);
        await _vehicleRepository.SaveChangesAsync();

        return MapToDto(updatedBooking);
    }

    public async Task<VehicleBookingDto> UpdateBookingStatusAsync(int id, UpdateVehicleStatusDto updateStatusDto)
    {
        var booking = await _vehicleRepository.GetByIdAsync(id);
        if (booking == null)
            throw new InvalidOperationException($"Booking with ID {id} not found");

        booking.Status = updateStatusDto.Status;
        booking.DriverName = updateStatusDto.DriverName;
        booking.DriverContactNumber = updateStatusDto.DriverContactNumber;
        if (updateStatusDto.TotalCost.HasValue)
            booking.TotalCost = updateStatusDto.TotalCost.Value;

        if (updateStatusDto.Status == BookingStatus.Completed)
            booking.CompletedDate = DateTime.UtcNow;

        var updatedBooking = await _vehicleRepository.UpdateAsync(booking);
        await _vehicleRepository.SaveChangesAsync();

        return MapToDto(updatedBooking);
    }

    public async Task<bool> DeleteBookingAsync(int id)
    {
        var deleted = await _vehicleRepository.DeleteAsync(id);
        if (deleted)
            await _vehicleRepository.SaveChangesAsync();
        return deleted;
    }

    public async Task<IEnumerable<VehicleBookingDto>> GetBookingsByStatusAsync(string status)
    {
        if (!Enum.TryParse<BookingStatus>(status, true, out var bookingStatus))
            throw new InvalidOperationException($"Invalid status: {status}");

        var bookings = await _vehicleRepository.GetByStatusAsync(bookingStatus);
        return bookings.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleBookingDto>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime dropDate)
    {
        var vehicles = await _vehicleRepository.GetAvailableVehiclesAsync(pickupDate, dropDate);
        return vehicles.Select(MapToDto);
    }

    private static VehicleBookingDto MapToDto(VehicleBooking booking)
    {
        return new VehicleBookingDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            VehicleType = booking.VehicleType,
            PickupDate = booking.PickupDate,
            DropDate = booking.DropDate,
            Status = booking.Status,
            PickupLocation = booking.PickupLocation,
            DropLocation = booking.DropLocation,
            NumberOfPassengers = booking.NumberOfPassengers,
            TotalCost = booking.TotalCost,
            DriverName = booking.DriverName,
            DriverContactNumber = booking.DriverContactNumber,
            Notes = booking.Notes,
            CreatedDate = booking.CreatedDate
        };
    }
}
