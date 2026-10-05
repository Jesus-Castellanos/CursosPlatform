using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class LessonRepository : ILessonRepository
{
    private readonly CoursesDbContext _context;

    public LessonRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Lesson>> GetAllAsync()
    {
        return await _context.Lessons
            .Include(x => x.Section)
            .AsNoTracking()
            .OrderBy(x => x.SectionId)
            .ThenBy(x => x.LessonOrder)
            .ToListAsync();
    }

    public async Task<Lesson?> GetByIdAsync(int id)
    {
        return await _context.Lessons
            .Include(x => x.Section)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.LessonId == id);
    }

    public async Task<Lesson?> GetBySectionAndOrderAsync(
        int sectionId,
        int lessonOrder)
    {
        return await _context.Lessons
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.SectionId == sectionId &&
                x.LessonOrder == lessonOrder);
    }

    public async Task<Lesson> AddAsync(Lesson lesson)
    {
        _context.Lessons.Add(lesson);

        await _context.SaveChangesAsync();

        return lesson;
    }

    public async Task<Lesson> UpdateAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);

        await _context.SaveChangesAsync();

        return lesson;
    }

    public async Task DeleteAsync(Lesson lesson)
    {
        _context.Lessons.Remove(lesson);

        await _context.SaveChangesAsync();
    }
}