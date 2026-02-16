using System.Security.Cryptography;
using System.Text;
using TravelAgency.Application.DTOs;
using TravelAgency.Application.Interfaces;
using TravelAgency.Domain.Entities;
using TravelAgency.Infrastructure.Repositories;

namespace TravelAgency.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public UserService(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto createUserDto)
    {
        // Check if email already exists
        var existingUser = await _userRepository.GetByEmailAsync(createUserDto.Email);
        if (existingUser != null)
            throw new InvalidOperationException($"Email {createUserDto.Email} is already registered");

        var user = new User
        {
            FullName = createUserDto.FullName,
            Email = createUserDto.Email,
            PasswordHash = HashPassword(createUserDto.Password),
            PhoneNumber = createUserDto.PhoneNumber,
            Address = createUserDto.Address,
            ProfilePicture = createUserDto.ProfilePicture,
            Role = "Customer",
            IsActive = true,
            CreatedDate = DateTime.UtcNow
        };

        var createdUser = await _userRepository.AddAsync(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(createdUser);
    }

    public async Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new InvalidOperationException($"User with ID {id} not found");

        user.FullName = updateUserDto.FullName;
        user.PhoneNumber = updateUserDto.PhoneNumber;
        user.Address = updateUserDto.Address;
        user.ProfilePicture = updateUserDto.ProfilePicture;
        user.UpdatedDate = DateTime.UtcNow;

        var updatedUser = await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var deleted = await _userRepository.DeleteAsync(id);
        if (deleted)
            await _userRepository.SaveChangesAsync();
        return deleted;
    }

    public async Task<(bool success, string message)> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
            return (false, "User not found");

        if (!VerifyPassword(currentPassword, user.PasswordHash))
            return (false, "Current password is incorrect");

        user.PasswordHash = HashPassword(newPassword);
        user.UpdatedDate = DateTime.UtcNow;

        await _userRepository.UpdateAsync(user);
        await _userRepository.SaveChangesAsync();

        return (true, "Password changed successfully");
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        var user = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
            return null;

        if (!user.IsActive)
            return null;

        var token = _jwtTokenService.GenerateToken(user);

        return new LoginResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            ProfilePicture = user.ProfilePicture,
            Token = token
        };
    }

    public async Task<bool> VerifyPasswordAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null)
            return false;

        return VerifyPassword(password, user.PasswordHash);
    }

    // Helper methods
    private static string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput.Equals(hash);
    }

    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber,
            Address = user.Address,
            ProfilePicture = user.ProfilePicture,
            IsActive = user.IsActive,
            CreatedDate = user.CreatedDate
        };
    }
}

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
