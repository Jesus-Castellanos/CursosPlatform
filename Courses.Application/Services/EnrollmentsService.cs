using Courses.Application.DTOs.Enrollments;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class EnrollmentsService : IEnrollmentsService
{
    private readonly IEnrollmentRepository _enrollmentRepository;
    private readonly IUserRepository _userRepository;
    private readonly ICourseRepository _courseRepository;

    public EnrollmentsService(
        IEnrollmentRepository enrollmentRepository,
        IUserRepository userRepository,
        ICourseRepository courseRepository)
    {
        _enrollmentRepository = enrollmentRepository;
        _userRepository = userRepository;
        _courseRepository = courseRepository;
    }

    public async Task<IEnumerable<EnrollmentDto>> GetAllAsync()
    {
        var enrollments =
            await _enrollmentRepository.GetAllAsync();

        return enrollments.Select(MapToDto);
    }

    public async Task<EnrollmentDto?> GetByIdAsync(int id)
    {
        var enrollment =
            await _enrollmentRepository.GetByIdAsync(id);

        return enrollment is null
            ? null
            : MapToDto(enrollment);
    }

    public async Task<EnrollmentDto> CreateAsync(
        CreateEnrollmentDto dto)
    {
        if (dto.UserId <= 0)
        {
            throw new ArgumentException(
                "El UserId debe ser mayor que cero.");
        }

        if (dto.CourseId <= 0)
        {
            throw new ArgumentException(
                "El CourseId debe ser mayor que cero.");
        }

        var user =
            await _userRepository.GetByIdAsync(dto.UserId);

        if (user is null)
        {
            throw new KeyNotFoundException(
                $"No existe el usuario con ID {dto.UserId}.");
        }

        var course =
            await _courseRepository.GetByIdAsync(dto.CourseId);

        if (course is null)
        {
            throw new KeyNotFoundException(
                $"No existe el curso con ID {dto.CourseId}.");
        }

        var existing =
            await _enrollmentRepository
                .GetByUserAndCourseAsync(
                    dto.UserId,
                    dto.CourseId);

        if (existing is not null)
        {
            throw new InvalidOperationException(
                "El usuario ya está inscrito en este curso.");
        }

        var enrollment = new Enrollment
        {
            UserId = dto.UserId,
            CourseId = dto.CourseId,
            EnrollmentDate = DateTime.UtcNow,
            CompletionPercentage = 0,
            CompletedAt = null
        };

        var created =
            await _enrollmentRepository.AddAsync(enrollment);

        created.User = user;
        created.Course = course;

        return MapToDto(created);
    }

    public async Task<bool> UpdateAsync(
        int id,
        UpdateEnrollmentDto dto)
    {
        if (dto.CompletionPercentage < 0 ||
            dto.CompletionPercentage > 100)
        {
            throw new ArgumentException(
                "El porcentaje de finalización debe estar entre 0 y 100.");
        }

        var enrollment =
            await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment is null)
        {
            return false;
        }

        enrollment.CompletionPercentage =
            dto.CompletionPercentage;

        if (dto.CompletionPercentage >= 100)
        {
            enrollment.CompletionPercentage = 100;
            enrollment.CompletedAt =
                dto.CompletedAt ?? DateTime.UtcNow;
        }
        else
        {
            enrollment.CompletedAt = null;
        }

        await _enrollmentRepository.UpdateAsync(
            enrollment);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var enrollment =
            await _enrollmentRepository.GetByIdAsync(id);

        if (enrollment is null)
        {
            return false;
        }

        await _enrollmentRepository.DeleteAsync(
            enrollment);

        return true;
    }

    private static EnrollmentDto MapToDto(
        Enrollment enrollment)
    {
        return new EnrollmentDto
        {
            EnrollmentId = enrollment.EnrollmentId,

            UserId = enrollment.UserId,
            UserName =
                enrollment.User is null
                    ? null
                    : $"{enrollment.User.FirstName} {enrollment.User.LastName}",

            CourseId = enrollment.CourseId,
            CourseTitle = enrollment.Course?.Title,

            EnrollmentDate = enrollment.EnrollmentDate,
            CompletionPercentage =
                enrollment.CompletionPercentage,
            CompletedAt = enrollment.CompletedAt
        };
    }
}