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
    public DbSet<HotelBooking> HotelBookings { get; set; } = null!;
    public DbSet<LaborApproval> LaborApprovals { get; set; } = null!;

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

        // HotelBooking configuration
        modelBuilder.Entity<HotelBooking>()
            .HasKey(h => h.Id);

        modelBuilder.Entity<HotelBooking>()
            .Property(h => h.TotalPrice)
            .HasPrecision(10, 2);

        // Add relationship for HotelBookings
        modelBuilder.Entity<User>()
            .HasMany<HotelBooking>()
            .WithOne()
            .HasForeignKey("UserId")
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

        // LaborApproval configuration
        modelBuilder.Entity<LaborApproval>()
            .HasKey(l => l.Id);

        modelBuilder.Entity<LaborApproval>()
            .Property(l => l.ApplicationFee)
            .HasPrecision(10, 2);

        modelBuilder.Entity<LaborApproval>()
            .Property(l => l.ServiceCharge)
            .HasPrecision(10, 2);

        modelBuilder.Entity<LaborApproval>()
            .Property(l => l.TotalFee)
            .HasPrecision(10, 2);

        modelBuilder.Entity<LaborApproval>()
            .Property(l => l.OfferedSalary)
            .HasPrecision(10, 2);

        modelBuilder.Entity<LaborApproval>()
            .HasIndex(l => l.PassportNumber);

        // LaborApproval relationship - use the navigation property
        modelBuilder.Entity<LaborApproval>()
            .HasOne(l => l.User)
            .WithMany()
            .HasForeignKey(l => l.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
