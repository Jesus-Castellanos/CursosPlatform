using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface ILessonRepository
{
    Task<List<Lesson>> GetAllAsync();

    Task<Lesson?> GetByIdAsync(int id);

    Task<Lesson?> GetBySectionAndOrderAsync(
        int sectionId,
        int lessonOrder);

    Task<Lesson> AddAsync(Lesson lesson);

    Task<Lesson> UpdateAsync(Lesson lesson);

    Task DeleteAsync(Lesson lesson);
}