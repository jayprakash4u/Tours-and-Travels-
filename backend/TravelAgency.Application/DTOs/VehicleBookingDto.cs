using TravelAgency.Domain.Enums;

namespace TravelAgency.Application.DTOs;

public class VehicleBookingDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string VehicleType { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime DropDate { get; set; }
    public BookingStatus Status { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropLocation { get; set; }
    public int? NumberOfPassengers { get; set; }
    public decimal? TotalCost { get; set; }
    public string? DriverName { get; set; }
    public string? DriverContactNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateVehicleBookingDto
{
    public required string VehicleType { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime DropDate { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropLocation { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? Notes { get; set; }
}

public class UpdateVehicleBookingDto
{
    public DateTime PickupDate { get; set; }
    public DateTime DropDate { get; set; }
    public string? PickupLocation { get; set; }
    public string? DropLocation { get; set; }
    public int? NumberOfPassengers { get; set; }
    public string? Notes { get; set; }
}

public class UpdateVehicleStatusDto
{
    public BookingStatus Status { get; set; }
    public string? DriverName { get; set; }
    public string? DriverContactNumber { get; set; }
    public decimal? TotalCost { get; set; }
}
