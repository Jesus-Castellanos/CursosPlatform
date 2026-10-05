using Courses.Application.DTOs.Enrollments;

namespace Courses.Application.Interfaces;

public interface IEnrollmentsService
{
    Task<IEnumerable<EnrollmentDto>> GetAllAsync();

    Task<EnrollmentDto?> GetByIdAsync(int id);

    Task<EnrollmentDto> CreateAsync(
        CreateEnrollmentDto dto);

    Task<bool> UpdateAsync(
        int id,
        UpdateEnrollmentDto dto);

    Task<bool> DeleteAsync(int id);
}