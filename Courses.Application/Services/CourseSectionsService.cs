using Courses.Application.DTOs.CourseSections;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class CourseSectionsService : ICourseSectionsService
{
    private readonly ICourseSectionRepository _sectionRepository;
    private readonly ICourseRepository _courseRepository;

    public CourseSectionsService(
        ICourseSectionRepository sectionRepository,
        ICourseRepository courseRepository)
    {
        _sectionRepository = sectionRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<CourseSectionDto>> GetAllAsync()
    {
        var sections =
            await _sectionRepository.GetAllAsync();

        return sections.Select(MapToDto);
    }

    public async Task<CourseSectionDto?> GetByIdAsync(int id)
    {
        var section =
            await _sectionRepository.GetByIdAsync(id);

        return section is null
            ? null
            : MapToDto(section);
    }

    public async Task<CourseSectionDto> CreateAsync(
        CreateCourseSectionDto dto)
    {
        if (dto.CourseId <= 0)
        {
            throw new ArgumentException(
                "El CourseId debe ser mayor que cero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException(
                "El título de la sección es obligatorio.");
        }

        if (dto.SectionOrder <= 0)
        {
            throw new ArgumentException(
                "El orden de la sección debe ser mayor que cero.");
        }

        var course =
            await _courseRepository.GetByIdAsync(dto.CourseId);

        if (course is null)
        {
            throw new KeyNotFoundException(
                $"No existe el curso con ID {dto.CourseId}.");
        }

        var existing =
            await _sectionRepository.GetByCourseAndOrderAsync(
                dto.CourseId,
                dto.SectionOrder);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "Ya existe una sección con ese orden dentro del curso.");
        }

        var section = new CourseSection
        {
            CourseId = dto.CourseId,
            Title = dto.Title.Trim(),
            Description = string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim(),
            SectionOrder = dto.SectionOrder,
            CreatedAt = DateTime.UtcNow
        };

        var created =
            await _sectionRepository.AddAsync(section);

        created.Course = course;

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateCourseSectionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException(
                "El título de la sección es obligatorio.");
        }

        if (dto.SectionOrder <= 0)
        {
            throw new ArgumentException(
                "El orden de la sección debe ser mayor que cero.");
        }

        var section =
            await _sectionRepository.GetByIdAsync(id);

        if (section is null)
        {
            return false;
        }

        var existing =
            await _sectionRepository.GetByCourseAndOrderAsync(
                section.CourseId,
                dto.SectionOrder);

        if (existing is not null &&
            existing.SectionId != id)
        {
            throw new InvalidOperationException(
                "Ya existe otra sección con ese orden dentro del curso.");
        }

        section.Title = dto.Title.Trim();

        section.Description =
            string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim();

        section.SectionOrder = dto.SectionOrder;

        await _sectionRepository.UpdateAsync(section);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var section =
            await _sectionRepository.GetByIdAsync(id);

        if (section is null)
        {
            return false;
        }

        await _sectionRepository.DeleteAsync(section);

        return true;
    }

    private static CourseSectionDto MapToDto(
        CourseSection section)
    {
        return new CourseSectionDto
        {
            SectionId = section.SectionId,
            CourseId = section.CourseId,
            CourseTitle = section.Course?.Title,
            Title = section.Title,
            Description = section.Description,
            SectionOrder = section.SectionOrder,
            CreatedAt = section.CreatedAt
        };
    }
}