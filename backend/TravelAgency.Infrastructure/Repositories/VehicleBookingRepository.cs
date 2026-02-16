using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Common;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Repositories;

public class VehicleBookingRepository : Repository<VehicleBooking>, IVehicleBookingRepository
{
    private readonly ApplicationDbContext _context;

    public VehicleBookingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VehicleBooking>> GetByUserIdAsync(int userId)
    {
        return await _context.VehicleBookings
            .Where(v => v.UserId == userId)
            .OrderByDescending(v => v.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleBooking>> GetByStatusAsync(BookingStatus status)
    {
        return await _context.VehicleBookings
            .Where(v => v.Status == status)
            .OrderByDescending(v => v.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleBooking>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime dropDate)
    {
        return await _context.VehicleBookings
            .Where(v => v.Status == BookingStatus.Confirmed &&
                   (v.DropDate < pickupDate || v.PickupDate > dropDate))
            .OrderBy(v => v.PickupDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<VehicleBooking>> GetByVehicleTypeAsync(string vehicleType)
    {
        return await _context.VehicleBookings
            .Where(v => v.VehicleType == vehicleType)
            .OrderByDescending(v => v.CreatedDate)
            .ToListAsync();
    }
}

public interface IVehicleBookingRepository : IRepository<VehicleBooking>
{
    Task<IEnumerable<VehicleBooking>> GetByUserIdAsync(int userId);
    Task<IEnumerable<VehicleBooking>> GetByStatusAsync(BookingStatus status);
    Task<IEnumerable<VehicleBooking>> GetAvailableVehiclesAsync(DateTime pickupDate, DateTime dropDate);
    Task<IEnumerable<VehicleBooking>> GetByVehicleTypeAsync(string vehicleType);
}
