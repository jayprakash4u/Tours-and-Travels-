using TravelAgency.Application.DTOs;

namespace TravelAgency.Application.Interfaces;

/// <summary>
/// Service interface for user-related operations.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Retrieves all users from the system.
    /// </summary>
    Task<IEnumerable<UserDto>> GetAllUsersAsync();
    
    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    Task<UserDto?> GetUserByIdAsync(int id);
    
    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    Task<UserDto?> GetUserByEmailAsync(string email);
    
    /// <summary>
    /// Creates a new user.
    /// </summary>
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    
    /// <summary>
    /// Updates an existing user.
    /// </summary>
    Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    
    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    Task<bool> DeleteUserAsync(int id);
    
    /// <summary>
    /// Changes the password for a user.
    /// </summary>
    Task<(bool success, string message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    
    /// <summary>
    /// Authenticates a user and returns a login response with token.
    /// </summary>
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    
    /// <summary>
    /// Verifies if the provided password matches the user's password.
    /// </summary>
    Task<bool> VerifyPasswordAsync(string email, string password);
}
