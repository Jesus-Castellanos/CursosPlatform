using Courses.Application.DTOs.Users;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasherServices _passwordHasher;

    public UserService(
        IUserRepository userRepository,
        IPasswordHasherServices passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(MapToDto);
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        return user is null ? null : MapToDto(user);
    }

    public async Task<UserDto> CreateAsync(
        CreateUserDto createUserDto)
    {
        var existingUser =
            await _userRepository.GetByEmailAsync(
                createUserDto.Email);

        if (existingUser != null)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está en uso.");
        }

        var passwordHash =
            _passwordHasher.Hash(createUserDto.Password);

        var user = new User
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdUser =
            await _userRepository.AddAsync(user);

        return MapToDto(createdUser);
    }

    public async Task<UserDto?> UpdateAsync(
    int id,
    UpdateUserDto dto)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return null;
        }

        var existingUser =
            await _userRepository.GetByEmailAsync(dto.Email);

        if (existingUser is not null &&
            existingUser.Id != id)
        {
            throw new InvalidOperationException(
                "El correo electrónico ya está en uso.");
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.IsActive = dto.IsActive;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser =
            await _userRepository.UpdateAsync(user);

        return MapToDto(updatedUser);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(user);

        return true;
    }

    public static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            IsActive = user.IsActive
        };
    }
}