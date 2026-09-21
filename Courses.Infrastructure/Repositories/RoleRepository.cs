using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly CoursesDbContext _context;

    public RoleRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Role>> GetAllAsync()
    {
        return await _context.Roles
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.RoleId == id);
    }

    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _context.Roles
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Name.ToLower() == name.ToLower());
    }

    public async Task<Role> AddAsync(Role role)
    {
        _context.Roles.Add(role);

        await _context.SaveChangesAsync();

        return role;
    }

    public async Task<Role> UpdateAsync(Role role)
    {
        _context.Roles.Update(role);

        await _context.SaveChangesAsync();

        return role;
    }

    public async Task DeleteAsync(Role role)
    {
        _context.Roles.Remove(role);

        await _context.SaveChangesAsync();
    }
}