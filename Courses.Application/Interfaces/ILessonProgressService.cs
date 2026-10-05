using Courses.Application.DTOs.LessonProgress;

namespace Courses.Application.Interfaces;

public interface ILessonProgressService
{
    Task<IEnumerable<LessonProgressDto>> GetAllAsync();

    Task<LessonProgressDto?> GetByIdAsync(int id);

    Task<LessonProgressDto> CreateAsync(
        CreateLessonProgressDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateLessonProgressDto dto);

    Task<bool> DeleteAsync(int id);
}