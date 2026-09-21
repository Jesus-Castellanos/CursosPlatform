using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface ICourseSectionRepository
{
    Task<List<CourseSection>> GetAllAsync();

    Task<CourseSection?> GetByIdAsync(int id);

    Task<CourseSection?> GetByCourseAndOrderAsync(
        int courseId,
        int sectionOrder);

    Task<CourseSection> AddAsync(
        CourseSection section);

    Task<CourseSection> UpdateAsync(
        CourseSection section);

    Task DeleteAsync(
        CourseSection section);
}