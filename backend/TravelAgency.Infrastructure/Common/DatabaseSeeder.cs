using System.Security.Cryptography;
using System.Text;
using TravelAgency.Domain.Entities;
using TravelAgency.Domain.Enums;
using TravelAgency.Infrastructure.Data;

namespace TravelAgency.Infrastructure.Common;

public class DatabaseSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext context)
    {
        // Check if data already exists
        if (context.Users.Any())
            return;

        // Create test users
        var testCustomer = new User
        {
            FullName = "John Doe",
            Email = "customer@alfatravels.com",
            PasswordHash = HashPassword("Customer@123"),
            Role = "Customer",
            PhoneNumber = "9841234567",
            Address = "Kathmandu, Nepal",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var testAdmin = new User
        {
            FullName = "Admin User",
            Email = "admin@alfatravels.com",
            PasswordHash = HashPassword("Admin@123"),
            Role = "Admin",
            PhoneNumber = "9840000000",
            Address = "Kathmandu, Nepal",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var testCustomer2 = new User
        {
            FullName = "Jane Smith",
            Email = "jane@alfatravels.com",
            PasswordHash = HashPassword("Jane@123"),
            Role = "Customer",
            PhoneNumber = "9849876543",
            Address = "Pokhara, Nepal",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        context.Users.AddRange(testCustomer, testAdmin, testCustomer2);
        await context.SaveChangesAsync();

        // Create sample ticket requests
        var ticketRequests = new List<TicketRequest>
        {
            new TicketRequest
            {
                UserId = testCustomer.Id,
                FromLocation = "Kathmandu (KTM)",
                ToLocation = "Delhi (DEL)",
                TravelDate = DateTime.UtcNow.AddDays(30),
                TicketType = TicketType.Flight,
                Status = BookingStatus.Pending,
                NumberOfPassengers = 2,
                Notes = "Prefer morning flight",
                CreatedDate = DateTime.UtcNow
            },
            new TicketRequest
            {
                UserId = testCustomer2.Id,
                FromLocation = "Kathmandu",
                ToLocation = "Pokhara",
                TravelDate = DateTime.UtcNow.AddDays(15),
                TicketType = TicketType.Bus,
                Status = BookingStatus.Confirmed,
                NumberOfPassengers = 1,
                EstimatedPrice = 600,
                Notes = "Direct express bus preferred",
                CreatedDate = DateTime.UtcNow
            },
            new TicketRequest
            {
                UserId = testCustomer.Id,
                FromLocation = "Delhi (DEL)",
                ToLocation = "Mumbai (BOM)",
                TravelDate = DateTime.UtcNow.AddDays(45),
                TicketType = TicketType.Flight,
                Status = BookingStatus.Pending,
                NumberOfPassengers = 3,
                Notes = "Traveling with family",
                CreatedDate = DateTime.UtcNow
            }
        };

        context.TicketRequests.AddRange(ticketRequests);
        await context.SaveChangesAsync();

        // Create sample vehicle bookings
        var vehicleBookings = new List<VehicleBooking>
        {
            new VehicleBooking
            {
                UserId = testCustomer.Id,
                VehicleType = "SUV",
                PickupDate = DateTime.UtcNow.AddDays(20),
                DropDate = DateTime.UtcNow.AddDays(25),
                Status = BookingStatus.Confirmed,
                PickupLocation = "Kathmandu Airport",
                DropLocation = "Pokhara",
                NumberOfPassengers = 4,
                TotalCost = 8000,
                DriverName = "Ram Kumar",
                DriverContactNumber = "9841111111",
                Notes = "Need child seat",
                CreatedDate = DateTime.UtcNow
            },
            new VehicleBooking
            {
                UserId = testCustomer2.Id,
                VehicleType = "Car",
                PickupDate = DateTime.UtcNow.AddDays(10),
                DropDate = DateTime.UtcNow.AddDays(12),
                Status = BookingStatus.Pending,
                PickupLocation = "Pokhara City",
                DropLocation = "Lakeside Area",
                NumberOfPassengers = 2,
                Notes = "Self-drive preferred",
                CreatedDate = DateTime.UtcNow
            },
            new VehicleBooking
            {
                UserId = testCustomer.Id,
                VehicleType = "Van",
                PickupDate = DateTime.UtcNow.AddDays(50),
                DropDate = DateTime.UtcNow.AddDays(55),
                Status = BookingStatus.Pending,
                PickupLocation = "Delhi Airport",
                DropLocation = "Jaipur",
                NumberOfPassengers = 8,
                Notes = "Group tour vehicle",
                CreatedDate = DateTime.UtcNow
            }
        };

        context.VehicleBookings.AddRange(vehicleBookings);
        await context.SaveChangesAsync();

        // Create sample immigration applications
        var immigrationApplications = new List<ImmigrationApplication>
        {
            new ImmigrationApplication
            {
                UserId = testCustomer.Id,
                FullName = "John Doe",
                PassportNumber = "NA0001234",
                TargetCountry = "Canada",
                PassportExpiryDate = DateTime.UtcNow.AddYears(5),
                Status = ApplicationStatus.Pending,
                VisaType = "Work",
                AdminNotes = "Skilled worker application",
                CreatedDate = DateTime.UtcNow
            },
            new ImmigrationApplication
            {
                UserId = testCustomer2.Id,
                FullName = "Jane Smith",
                PassportNumber = "NA0005678",
                TargetCountry = "Australia",
                PassportExpiryDate = DateTime.UtcNow.AddYears(4),
                Status = ApplicationStatus.UnderReview,
                VisaType = "Study",
                AdminNotes = "Currently under document verification",
                CreatedDate = DateTime.UtcNow.AddDays(-10),
                UpdatedDate = DateTime.UtcNow
            },
            new ImmigrationApplication
            {
                UserId = testCustomer.Id,
                FullName = "John Doe",
                PassportNumber = "NA0009876",
                TargetCountry = "Germany",
                PassportExpiryDate = DateTime.UtcNow.AddYears(6),
                Status = ApplicationStatus.Approved,
                VisaType = "Work",
                AdminNotes = "Approved for long-term residence",
                CreatedDate = DateTime.UtcNow.AddDays(-30),
                CompletedDate = DateTime.UtcNow.AddDays(-5)
            }
        };

        context.ImmigrationApplications.AddRange(immigrationApplications);
        await context.SaveChangesAsync();
    }

    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }
}
