using Courses.Application.DTOs.UserRoles;

namespace Courses.Application.Interfaces;

public interface IUserRolesService
{
    Task<IEnumerable<UserRolesDto>> GetAllAsync();

    Task<UserRolesDto> AssignRoleAsync(
        CreateUserRoleDto dto);

    Task<bool> RemoveRoleAsync(
        int userId,
        int roleId);
}