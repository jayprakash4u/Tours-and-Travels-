namespace TravelAgency.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
}

public class CreateUserDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfilePicture { get; set; }
}

public class UpdateUserDto
{
    public required string FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public string? ProfilePicture { get; set; }
}

public class LoginDto
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public string? ProfilePicture { get; set; }
    public required string Token { get; set; }
}
