using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;
using TravelAgency.Infrastructure.Common;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<IEnumerable<User>> GetAdminsAsync()
    {
        return await _context.Users.Where(u => u.Role == "Admin").ToListAsync();
    }

    public async Task<IEnumerable<User>> GetActiveUsersAsync()
    {
        return await _context.Users.Where(u => u.IsActive).ToListAsync();
    }
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<IEnumerable<User>> GetAdminsAsync();
    Task<IEnumerable<User>> GetActiveUsersAsync();
}
