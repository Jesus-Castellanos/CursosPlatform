using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class CourseSectionRepository : ICourseSectionRepository
{
    private readonly CoursesDbContext _context;

    public CourseSectionRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<CourseSection>> GetAllAsync()
    {
        return await _context.CoursesSections
            .Include(x => x.Course)
            .AsNoTracking()
            .OrderBy(x => x.CourseId)
            .ThenBy(x => x.SectionOrder)
            .ToListAsync();
    }

    public async Task<CourseSection?> GetByIdAsync(int id)
    {
        return await _context.CoursesSections
            .Include(x => x.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.SectionId == id);
    }

    public async Task<CourseSection?> GetByCourseAndOrderAsync(
        int courseId,
        int sectionOrder)
    {
        return await _context.CoursesSections
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.CourseId == courseId &&
                x.SectionOrder == sectionOrder);
    }

    public async Task<CourseSection> AddAsync(
        CourseSection section)
    {
        _context.CoursesSections.Add(section);

        await _context.SaveChangesAsync();

        return section;
    }

    public async Task<CourseSection> UpdateAsync(
        CourseSection section)
    {
        _context.CoursesSections.Update(section);

        await _context.SaveChangesAsync();

        return section;
    }

    public async Task DeleteAsync(
        CourseSection section)
    {
        _context.CoursesSections.Remove(section);

        await _context.SaveChangesAsync();
    }
}