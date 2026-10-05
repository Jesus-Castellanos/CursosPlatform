using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class LessonProgressRepository : ILessonProgressRepository
{
    private readonly CoursesDbContext _context;

    public LessonProgressRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<LessonProgress>> GetAllAsync()
    {
        return await _context.LessonProgresses
            .Include(x => x.User)
            .Include(x => x.Lesson)
            .AsNoTracking()
            .OrderBy(x => x.UserId)
            .ThenBy(x => x.LessonId)
            .ToListAsync();
    }

    public async Task<LessonProgress?> GetByIdAsync(int id)
    {
        return await _context.LessonProgresses
            .Include(x => x.User)
            .Include(x => x.Lesson)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.LessonProgressId == id);
    }

    public async Task<LessonProgress?> GetByUserAndLessonAsync(
        int userId,
        int lessonId)
    {
        return await _context.LessonProgresses
            .Include(x => x.User)
            .Include(x => x.Lesson)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.LessonId == lessonId);
    }

    public async Task<LessonProgress> AddAsync(
        LessonProgress progress)
    {
        _context.LessonProgresses.Add(progress);

        await _context.SaveChangesAsync();

        return progress;
    }

    public async Task UpdateAsync(
        LessonProgress progress)
    {
        _context.LessonProgresses.Update(progress);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        LessonProgress progress)
    {
        _context.LessonProgresses.Remove(progress);

        await _context.SaveChangesAsync();
    }
}