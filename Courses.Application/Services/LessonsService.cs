using Courses.Application.DTOs.Lessons;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class LessonsService : ILessonsService
{
    private readonly ILessonRepository _lessonRepository;
    private readonly ICourseSectionRepository _sectionRepository;

    public LessonsService(
        ILessonRepository lessonRepository,
        ICourseSectionRepository sectionRepository)
    {
        _lessonRepository = lessonRepository;
        _sectionRepository = sectionRepository;
    }

    public async Task<IEnumerable<LessonDto>> GetAllAsync()
    {
        var lessons =
            await _lessonRepository.GetAllAsync();

        return lessons.Select(MapToDto);
    }

    public async Task<LessonDto?> GetByIdAsync(int id)
    {
        var lesson =
            await _lessonRepository.GetByIdAsync(id);

        return lesson is null
            ? null
            : MapToDto(lesson);
    }

    public async Task<LessonDto> AllAsync(
        CreateLessonDto dto)
    {
        if (dto.SectionId <= 0)
        {
            throw new ArgumentException(
                "El SectionId debe ser mayor que cero.");
        }

        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException(
                "El título de la lección es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.ContentType))
        {
            throw new ArgumentException(
                "El tipo de contenido es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.ContentUrl))
        {
            throw new ArgumentException(
                "La URL del contenido es obligatoria.");
        }

        if (dto.LessonOrder <= 0)
        {
            throw new ArgumentException(
                "El orden de la lección debe ser mayor que cero.");
        }

        if (dto.Duration.HasValue &&
            dto.Duration.Value < 0)
        {
            throw new ArgumentException(
                "La duración no puede ser negativa.");
        }

        var section =
            await _sectionRepository.GetByIdAsync(dto.SectionId);

        if (section is null)
        {
            throw new KeyNotFoundException(
                $"No existe la sección con ID {dto.SectionId}.");
        }

        var existing =
            await _lessonRepository.GetBySectionAndOrderAsync(
                dto.SectionId,
                dto.LessonOrder);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "Ya existe una lección con ese orden dentro de la sección.");
        }

        var lesson = new Lesson
        {
            SectionId = dto.SectionId,
            Title = dto.Title.Trim(),

            Description = string.IsNullOrWhiteSpace(
                dto.Description)
                ? null
                : dto.Description.Trim(),

            ContentType = dto.ContentType.Trim(),
            ContentUrl = dto.ContentUrl.Trim(),

            LessonOrder = dto.LessonOrder,
            DurationMinutes = dto.Duration,
            IsPreview = dto.IsPreview,

            CreatedAt = DateTime.UtcNow
        };

        var created =
            await _lessonRepository.AddAsync(lesson);

        created.Section = section;

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateLessonDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
        {
            throw new ArgumentException(
                "El título de la lección es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.ContentType))
        {
            throw new ArgumentException(
                "El tipo de contenido es obligatorio.");
        }

        if (string.IsNullOrWhiteSpace(dto.ContentUrl))
        {
            throw new ArgumentException(
                "La URL del contenido es obligatoria.");
        }

        if (dto.LessonOrder <= 0)
        {
            throw new ArgumentException(
                "El orden de la lección debe ser mayor que cero.");
        }

        if (dto.DurationMinutes.HasValue &&
            dto.DurationMinutes.Value < 0)
        {
            throw new ArgumentException(
                "La duración no puede ser negativa.");
        }

        var lesson =
            await _lessonRepository.GetByIdAsync(id);

        if (lesson is null)
        {
            return false;
        }

        var existing =
            await _lessonRepository.GetBySectionAndOrderAsync(
                lesson.SectionId,
                dto.LessonOrder);

        if (existing is not null &&
            existing.LessonId != id)
        {
            throw new InvalidOperationException(
                "Ya existe otra lección con ese orden dentro de la sección.");
        }

        lesson.Title = dto.Title.Trim();

        lesson.Description =
            string.IsNullOrWhiteSpace(dto.Description)
                ? null
                : dto.Description.Trim();

        lesson.ContentType = dto.ContentType.Trim();
        lesson.ContentUrl = dto.ContentUrl.Trim();
        lesson.LessonOrder = dto.LessonOrder;
        lesson.DurationMinutes = dto.DurationMinutes;
        lesson.IsPreview = dto.IsPreview;
        lesson.UpdatedAt = DateTime.UtcNow;

        await _lessonRepository.UpdateAsync(lesson);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var lesson =
            await _lessonRepository.GetByIdAsync(id);

        if (lesson is null)
        {
            return false;
        }

        await _lessonRepository.DeleteAsync(lesson);

        return true;
    }

    private static LessonDto MapToDto(Lesson lesson)
    {
        return new LessonDto
        {
            LessonId = lesson.LessonId,

            SectionId = lesson.SectionId,
            SectionTitle = lesson.Section?.Title,

            Title = lesson.Title,
            Description = lesson.Description,

            ContentType = lesson.ContentType,
            ContentUrl = lesson.ContentUrl,

            LessonOrder = lesson.LessonOrder,
            DurationMinutes = lesson.DurationMinutes,
            IsPreview = lesson.IsPreview,

            CreatedAt = lesson.CreatedAt,
            UpdatedAt = lesson.UpdatedAt
        };
    }
}