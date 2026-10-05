using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Courses.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Courses.Infrastructure.Repositories;

public class EnrollmentRepository : IEnrollmentRepository
{
    private readonly CoursesDbContext _context;

    public EnrollmentRepository(CoursesDbContext context)
    {
        _context = context;
    }

    public async Task<List<Enrollment>> GetAllAsync()
    {
        return await _context.Enrollments
            .Include(x => x.User)
            .Include(x => x.Course)
            .AsNoTracking()
            .OrderBy(x => x.EnrollmentDate)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetByIdAsync(int id)
    {
        return await _context.Enrollments
            .Include(x => x.User)
            .Include(x => x.Course)
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.EnrollmentId == id);
    }

    public async Task<Enrollment?> GetByUserAndCourseAsync(
        int userId,
        int courseId)
    {
        return await _context.Enrollments
            .Include(x => x.User)
            .Include(x => x.Course)
            .FirstOrDefaultAsync(x =>
                x.UserId == userId &&
                x.CourseId == courseId);
    }

    public async Task<Enrollment> AddAsync(
        Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);

        await _context.SaveChangesAsync();

        return enrollment;
    }

    public async Task UpdateAsync(
        Enrollment enrollment)
    {
        _context.Enrollments.Update(enrollment);

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(
        Enrollment enrollment)
    {
        _context.Enrollments.Remove(enrollment);

        await _context.SaveChangesAsync();
    }
}