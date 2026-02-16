using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Common;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Repositories;

public class ImmigrationRepository : Repository<ImmigrationApplication>, IImmigrationRepository
{
    private readonly ApplicationDbContext _context;

    public ImmigrationRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ImmigrationApplication>> GetByUserIdAsync(int userId)
    {
        return await _context.ImmigrationApplications
            .Where(i => i.UserId == userId)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ImmigrationApplication>> GetByStatusAsync(ApplicationStatus status)
    {
        return await _context.ImmigrationApplications
            .Where(i => i.Status == status)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ImmigrationApplication>> GetByCountryAsync(string country)
    {
        return await _context.ImmigrationApplications
            .Where(i => i.TargetCountry.ToLower().Contains(country.ToLower()))
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ImmigrationApplication>> GetByVisaTypeAsync(string visaType)
    {
        return await _context.ImmigrationApplications
            .Where(i => i.VisaType == visaType)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ImmigrationApplication>> GetPendingApplicationsAsync()
    {
        return await _context.ImmigrationApplications
            .Where(i => i.Status == ApplicationStatus.Pending)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync();
    }
}

public interface IImmigrationRepository : IRepository<ImmigrationApplication>
{
    Task<IEnumerable<ImmigrationApplication>> GetByUserIdAsync(int userId);
    Task<IEnumerable<ImmigrationApplication>> GetByStatusAsync(ApplicationStatus status);
    Task<IEnumerable<ImmigrationApplication>> GetByCountryAsync(string country);
    Task<IEnumerable<ImmigrationApplication>> GetByVisaTypeAsync(string visaType);
    Task<IEnumerable<ImmigrationApplication>> GetPendingApplicationsAsync();
}
