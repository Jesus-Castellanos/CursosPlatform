using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly CoursesDbContext _context;

    public UserRoleRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<UserRole>> GetAllAsync()
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<UserRole?> GetAsync(
        int userId,
        int roleId)
    {
        return await _context.UserRoles
            .Include(x => x.User)
            .Include(x => x.Role)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.RoleId == roleId);
    }

    public async Task<UserRole> AddAsync(UserRole userRole)
    {
        _context.UserRoles.Add(userRole);

        await _context.SaveChangesAsync();

        return userRole;
    }

    public async Task DeleteAsync(UserRole userRole)
    {
        _context.UserRoles.Remove(userRole);

        await _context.SaveChangesAsync();
    }
}