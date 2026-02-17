using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;
using TravelAgency.Infrastructure.Common;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Repositories;

/// <summary>
/// Repository for managing hotel bookings.
/// </summary>
public class HotelBookingRepository : Repository<HotelBooking>, IHotelBookingRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the HotelBookingRepository class.
    /// </summary>
    public HotelBookingRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all hotel bookings for a specific user.
    /// </summary>
    public async Task<IEnumerable<HotelBooking>> GetByUserIdAsync(int userId)
    {
        return await _context.HotelBookings
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets hotel bookings by location.
    /// </summary>
    public async Task<IEnumerable<HotelBooking>> GetByLocationAsync(string location)
    {
        return await _context.HotelBookings
            .Where(b => b.Location.Contains(location))
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }

    /// <summary>
    /// Gets available hotels for specific dates.
    /// </summary>
    public async Task<IEnumerable<HotelBooking>> GetAvailableHotelsAsync(DateTime checkInDate, DateTime checkOutDate)
    {
        return await _context.HotelBookings
            .Where(b => b.CheckOutDate <= checkInDate || b.CheckInDate >= checkOutDate)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
    }
}

/// <summary>
/// Interface for hotel booking repository operations.
/// </summary>
public interface IHotelBookingRepository : IRepository<HotelBooking>
{
    /// <summary>
    /// Gets all hotel bookings for a specific user.
    /// </summary>
    Task<IEnumerable<HotelBooking>> GetByUserIdAsync(int userId);

    /// <summary>
    /// Gets hotel bookings by location.
    /// </summary>
    Task<IEnumerable<HotelBooking>> GetByLocationAsync(string location);

    /// <summary>
    /// Gets available hotels for specific dates.
    /// </summary>
    Task<IEnumerable<HotelBooking>> GetAvailableHotelsAsync(DateTime checkInDate, DateTime checkOutDate);
}
