using Microsoft.EntityFrameworkCore;
using TravelAgency.Domain.Entities;

namespace TravelAgency.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<TicketRequest> TicketRequests { get; set; } = null!;
    public DbSet<VehicleBooking> VehicleBookings { get; set; } = null!;
    public DbSet<ImmigrationApplication> ImmigrationApplications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(u => u.TicketRequests)
            .WithOne(t => t.User)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.VehicleBookings)
            .WithOne(v => v.User)
            .HasForeignKey(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.ImmigrationApplications)
            .WithOne(i => i.User)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // TicketRequest configuration
        modelBuilder.Entity<TicketRequest>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<TicketRequest>()
            .Property(t => t.EstimatedPrice)
            .HasPrecision(10, 2);

        // VehicleBooking configuration
        modelBuilder.Entity<VehicleBooking>()
            .HasKey(v => v.Id);

        modelBuilder.Entity<VehicleBooking>()
            .Property(v => v.TotalCost)
            .HasPrecision(10, 2);

        // ImmigrationApplication configuration
        modelBuilder.Entity<ImmigrationApplication>()
            .HasKey(i => i.Id);

        modelBuilder.Entity<ImmigrationApplication>()
            .HasIndex(i => i.PassportNumber)
            .IsUnique();
    }
}
