using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface IUserRoleRepository
{
    Task<List<UserRole>> GetAllAsync();

    Task<UserRole?> GetAsync(
        int userId,
        int roleId);

    Task<UserRole> AddAsync(UserRole userRole);
    Task DeleteAsync(UserRole userRole);
}
