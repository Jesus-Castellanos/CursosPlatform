using Courses.Application.Interfaces;
using Courses.Application.DTOs.Roles;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class RolesService : IRoleServices
{
    private readonly IRoleRepository _roleRepository;

    public RolesService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles.Select(MapToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);

        return role is null
            ? null
            : MapToDto(role);
    }

    public async Task<RoleDto> CreateAsync(
        CreateRoleDto createRoleDto)
    {
        if (string.IsNullOrWhiteSpace(createRoleDto.Name))
        {
            throw new ArgumentException(
                "El nombre del rol es obligatorio.");
        }

        string roleName = createRoleDto.Name.Trim();

        var existingRole =
            await _roleRepository.GetByNameAsync(roleName);

        if (existingRole is not null)
        {
            throw new InvalidOperationException(
                "Ya existe un rol con ese nombre.");
        }

        var role = new Role
        {
            Name = roleName,
            Description = string.IsNullOrWhiteSpace(
                createRoleDto.Description)
                ? null
                : createRoleDto.Description.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        var createdRole =
            await _roleRepository.AddAsync(role);

        return MapToDto(createdRole);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateRoleDto updateRoleDto)
    {
        var role = await _roleRepository.GetByIdAsync(id);

        if (role is null)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(updateRoleDto.Name))
        {
            throw new ArgumentException(
                "El nombre del rol es obligatorio.");
        }

        string roleName = updateRoleDto.Name.Trim();

        var existingRole =
            await _roleRepository.GetByNameAsync(roleName);

        if (existingRole is not null &&
            existingRole.RoleId != id)
        {
            throw new InvalidOperationException(
                "Ya existe otro rol con ese nombre.");
        }

        role.Name = roleName;
        role.Description =
            string.IsNullOrWhiteSpace(updateRoleDto.Description)
                ? null
                : updateRoleDto.Description.Trim();

        await _roleRepository.UpdateAsync(role);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var role = await _roleRepository.GetByIdAsync(id);

        if (role is null)
        {
            return false;
        }

        await _roleRepository.DeleteAsync(role);

        return true;
    }

    private static RoleDto MapToDto(Role role)
    {
        return new RoleDto
        {
            RoleId = role.RoleId,
            Name = role.Name,
            Description = role.Description,
            CreatedAt = role.CreatedAt
        };
    }
}