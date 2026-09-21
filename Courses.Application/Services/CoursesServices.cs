using Courses.Application.Interfaces;
using Courses.Application.DTOs.Courses;
using Courses.Domain.Entities;

namespace Courses.Application.Services;

public class CoursesService : ICoursesService
{
    private readonly ICourseRepository _coursesRepository;

    public CoursesService(ICourseRepository coursesRepository)
    {
        _coursesRepository = coursesRepository;
    }

    public async Task<IEnumerable<CourseDto>> GetAllAsync()
    {
        var course = await _coursesRepository.GetAllAsync();

        return course.Select(MapToDto);
    }

    public async Task<CourseDto> GetByIdAsync(int id)
    {
        var course = await _coursesRepository.GetByIdAsync(id);
        if (course is null)
        {
            throw new KeyNotFoundException(
                $"No se encontró el curso con ID {id}.");
        }
        return MapToDto(course);
    }

    public async Task<CourseDto> CreateAsync(CreateCourseDto createCourseDto)
    {
        var course = new Course
        {
            Title = createCourseDto.Title,
            Description = createCourseDto.Description,
            CategoryId = createCourseDto.CategoryId,
            CreatedByUserId = createCourseDto.CreatedByUserId,
            ThumbnailUrl = createCourseDto.ThumbnailUrl,
            Level = createCourseDto.Level,
            Price = createCourseDto.Price,
            IsPublished = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        var CreatedCourse = await _coursesRepository.AddAsync(course);
        return MapToDto(CreatedCourse);
    }

    public async Task<bool> UpdateAsync(int id, UpdateCourseDto updateCourseDto)
    {
        var course = await _coursesRepository.GetByIdAsync(id);
        if (course is null)
        {
            return false;
        }
        course.Title = updateCourseDto.Title;
        course.Description = updateCourseDto.Description;
        course.CategoryId = updateCourseDto.CategoryId;
        course.ThumbnailUrl = updateCourseDto.ThumbnailUrl;
        course.Level = updateCourseDto.Level;
        course.Price = updateCourseDto.Price;
        course.IsPublished = updateCourseDto.IsPublished;
        course.IsActive = updateCourseDto.IsActive;
        course.UpdatedAt = DateTime.UtcNow;

        await _coursesRepository.UpdateAsync(course);

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var course = await _coursesRepository.GetByIdAsync(id);
        if (course is null)
        {
            return false;
        }
        await _coursesRepository.DeleteAsync(course);
        return true;
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto
        {
            CourseId = course.CourseId,
            Title = course.Title,
            Description = course.Description,
            CategoryId = course.CategoryId,
            CreatedByUserId = course.CreatedByUserId,
            ThumbnailUrl = course.ThumbnailUrl,
            Level = course.Level,
            Price = course.Price,
            IsPublished = course.IsPublished,
            IsActive = course.IsActive,
            CreatedAt = course.CreatedAt,
            UpdatedAt = course.UpdatedAt
        };
    }
}