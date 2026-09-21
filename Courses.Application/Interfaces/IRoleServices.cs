using Courses.Application.Interfaces;
using Courses.Application.DTOs.Roles;
using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface IRoleServices
{
    Task<IEnumerable<RoleDto>> GetAllAsync();
    Task<RoleDto?> GetByIdAsync(int id);
    Task<RoleDto> CreateAsync(CreateRoleDto createRoleDto);
    Task<bool> UpdateAsync(int id, UpdateRoleDto updateRoleDto);
    Task<bool> DeleteAsync(int id);
}
