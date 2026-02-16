using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Common;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Repositories;

public class TicketRepository : Repository<TicketRequest>, ITicketRepository
{
    private readonly ApplicationDbContext _context;

    public TicketRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TicketRequest>> GetByUserIdAsync(int userId)
    {
        return await _context.TicketRequests
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketRequest>> GetByStatusAsync(BookingStatus status)
    {
        return await _context.TicketRequests
            .Where(t => t.Status == status)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketRequest>> GetByTicketTypeAsync(TicketType ticketType)
    {
        return await _context.TicketRequests
            .Where(t => t.TicketType == ticketType)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<TicketRequest>> GetPendingTicketsAsync()
    {
        return await _context.TicketRequests
            .Where(t => t.Status == BookingStatus.Pending)
            .OrderByDescending(t => t.CreatedDate)
            .ToListAsync();
    }
}

public interface ITicketRepository : IRepository<TicketRequest>
{
    Task<IEnumerable<TicketRequest>> GetByUserIdAsync(int userId);
    Task<IEnumerable<TicketRequest>> GetByStatusAsync(BookingStatus status);
    Task<IEnumerable<TicketRequest>> GetByTicketTypeAsync(TicketType ticketType);
    Task<IEnumerable<TicketRequest>> GetPendingTicketsAsync();
}
