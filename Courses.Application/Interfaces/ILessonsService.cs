using Courses.Application.DTOs.Lessons;

namespace Courses.Application.Interfaces;

public interface ILessonsService
{
    Task<IEnumerable<LessonDto>> GetAllAsync();

    Task<LessonDto?> GetByIdAsync(int id);

    Task<LessonDto> AllAsync(
        CreateLessonDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateLessonDto dto);

    Task<bool> DeleteAsync(int id);
}