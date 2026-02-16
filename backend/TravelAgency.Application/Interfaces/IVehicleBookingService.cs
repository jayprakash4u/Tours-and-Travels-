using TravelAgency.Application.DTOs;

namespace TravelAgency.Application.Interfaces;

public interface IVehicleBookingService
{
    Task<IEnumerable<VehicleBookingDto>> GetAllBookingsAsync();
    Task<VehicleBookingDto?> GetBookingByIdAsync(int id);
    Task<IEnumerable<VehicleBookingDto>> GetBookingsByUserIdAsync(int userId);
    Task<VehicleBookingDto> CreateBookingAsync(int userId, CreateVehicleBookingDto createDto);
    Task<VehicleBookingDto> UpdateBookingAsync(int id, UpdateVehicleBookingDto updateDto);
    Task<VehicleBookingDto> UpdateBookingStatusAsync(int id, UpdateVehicleStatusDto updateStatusDto);
    Task<bool> DeleteBookingAsync(int id);
    Task<IEnumerable<VehicleBookingDto>> GetBookingsByStatusAsync(string status);
    Task<IEnumerable<VehicleBookingDto>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime dropDate);
}
