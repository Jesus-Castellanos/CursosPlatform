using Courses.Application.DTOs.UserRoles;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class UserRolesService : IUserRolesService
{
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;

    public UserRolesService(
        IUserRoleRepository userRoleRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository)
    {
        _userRoleRepository = userRoleRepository;
        _userRepository = userRepository;
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<UserRolesDto>> GetAllAsync()
    {
        var userRoles =
            await _userRoleRepository.GetAllAsync();

        return userRoles.Select(MapToDto);
    }

    public async Task<UserRolesDto> AssignRoleAsync(
        CreateUserRoleDto dto)
    {
        var user =
            await _userRepository.GetByIdAsync(dto.UserId);

        if (user is null)
        {
            throw new KeyNotFoundException(
                $"No existe el usuario con ID {dto.UserId}.");
        }

        var role =
            await _roleRepository.GetByIdAsync(dto.RoleId);

        if (role is null)
        {
            throw new KeyNotFoundException(
                $"No existe el rol con ID {dto.RoleId}.");
        }

        var existing =
            await _userRoleRepository.GetAsync(
                dto.UserId,
                dto.RoleId);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "El usuario ya tiene asignado ese rol.");
        }

        var userRole = new UserRole
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId,
            AssignedAt = DateTime.UtcNow
        };

        var created =
            await _userRoleRepository.AddAsync(userRole);

        created.User = user;
        created.Role = role;

        return MapToDto(created);
    }

    public async Task<bool> RemoveRoleAsync(
        int userId,
        int roleId)
    {
        var userRole =
            await _userRoleRepository.GetAsync(
                userId,
                roleId);

        if (userRole is null)
        {
            return false;
        }

        await _userRoleRepository.DeleteAsync(userRole);

        return true;
    }

    private static UserRolesDto MapToDto(UserRole userRole)
    {
        return new UserRolesDto
        {
            UserId = userRole.UserId,
            UserName =
                $"{userRole.User.FirstName} {userRole.User.LastName}",
            RoleId = userRole.RoleId,
            RoleName = userRole.Role.Name,
            AssignedAt = userRole.AssignedAt
        };
    }
}