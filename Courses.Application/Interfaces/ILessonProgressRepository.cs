using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface ILessonProgressRepository
{
    Task<List<LessonProgress>> GetAllAsync();

    Task<LessonProgress?> GetByIdAsync(int id);

    Task<LessonProgress?> GetByUserAndLessonAsync(
        int userId,
        int lessonId);

    Task<LessonProgress> AddAsync(
        LessonProgress progress);

    Task UpdateAsync(
        LessonProgress progress);

    Task DeleteAsync(
        LessonProgress progress);
}