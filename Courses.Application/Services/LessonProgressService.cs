using Courses.Application.DTOs.LessonProgress;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class LessonProgressService : ILessonProgressService
{
    private readonly ILessonProgressRepository _progressRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILessonRepository _lessonRepository;

    public LessonProgressService(
        ILessonProgressRepository progressRepository,
        IUserRepository userRepository,
        ILessonRepository lessonRepository)
    {
        _progressRepository = progressRepository;
        _userRepository = userRepository;
        _lessonRepository = lessonRepository;
    }

    public async Task<IEnumerable<LessonProgressDto>> GetAllAsync()
    {
        var progress =
            await _progressRepository.GetAllAsync();

        return progress.Select(MapToDto);
    }

    public async Task<LessonProgressDto?> GetByIdAsync(int id)
    {
        var progress =
            await _progressRepository.GetByIdAsync(id);

        return progress is null
            ? null
            : MapToDto(progress);
    }

    public async Task<LessonProgressDto> CreateAsync(
        CreateLessonProgressDto dto)
    {
        if (dto.UserId <= 0)
        {
            throw new ArgumentException(
                "El UserId debe ser mayor que cero.");
        }

        if (dto.LessonId <= 0)
        {
            throw new ArgumentException(
                "El LessonId debe ser mayor que cero.");
        }

        ValidatePercentage(dto.ProgressPercentage);

        var user =
            await _userRepository.GetByIdAsync(dto.UserId);

        if (user is null)
        {
            throw new KeyNotFoundException(
                $"No existe el usuario con ID {dto.UserId}.");
        }

        var lesson =
            await _lessonRepository.GetByIdAsync(dto.LessonId);

        if (lesson is null)
        {
            throw new KeyNotFoundException(
                $"No existe la lección con ID {dto.LessonId}.");
        }

        var existing =
            await _progressRepository.GetByUserAndLessonAsync(
                dto.UserId,
                dto.LessonId);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "Ya existe progreso para este usuario y esta lección.");
        }

        bool completed = dto.ProgressPercentage >= 100;

        var progress = new LessonProgress
        {
            UserId = dto.UserId,
            LessonId = dto.LessonId,
            ProgressPercentage = dto.ProgressPercentage,
            IsCompleted = completed,
            LastAccessedAt = DateTime.UtcNow,
            CompletedAt = completed
                ? DateTime.UtcNow
                : null
        };

        var created =
            await _progressRepository.AddAsync(progress);

        created.User = user;
        created.Lesson = lesson;

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateLessonProgressDto dto)
    {
        ValidatePercentage(dto.ProgressPercentage);

        var progress =
            await _progressRepository.GetByIdAsync(id);

        if (progress is null)
        {
            return false;
        }

        progress.ProgressPercentage =
            dto.ProgressPercentage;

        progress.LastAccessedAt =
            DateTime.UtcNow;

        if (dto.ProgressPercentage >= 100)
        {
            progress.ProgressPercentage = 100;
            progress.IsCompleted = true;

            progress.CompletedAt ??=
                DateTime.UtcNow;
        }
        else
        {
            progress.IsCompleted = false;
            progress.CompletedAt = null;
        }

        await _progressRepository.UpdateAsync(progress);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var progress =
            await _progressRepository.GetByIdAsync(id);

        if (progress is null)
        {
            return false;
        }

        await _progressRepository.DeleteAsync(progress);

        return true;
    }

    private static void ValidatePercentage(
        decimal percentage)
    {
        if (percentage < 0 || percentage > 100)
        {
            throw new ArgumentException(
                "El porcentaje de progreso debe estar entre 0 y 100.");
        }
    }

    private static LessonProgressDto MapToDto(
        LessonProgress progress)
    {
        return new LessonProgressDto
        {
            LessonProgressId = progress.LessonProgressId,

            UserId = progress.UserId,
            UserName = progress.User is null
                ? null
                : $"{progress.User.FirstName} {progress.User.LastName}",

            LessonId = progress.LessonId,
            LessonTitle = progress.Lesson?.Title,

            ProgressPercentage =
                progress.ProgressPercentage,

            IsCompleted =
                progress.IsCompleted,

            LastAccessedAt =
                progress.LastAccessedAt,

            CompletedAt =
                progress.CompletedAt
        };
    }
}