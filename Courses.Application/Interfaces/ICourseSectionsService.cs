using Courses.Application.DTOs.CourseSections;

namespace Courses.Application.Interfaces;

public interface ICourseSectionsService
{
    Task<IEnumerable<CourseSectionDto>> GetAllAsync();

    Task<CourseSectionDto?> GetByIdAsync(int id);

    Task<CourseSectionDto> CreateAsync(
        CreateCourseSectionDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateCourseSectionDto dto);

    Task<bool> DeleteAsync(int id);
}