using System;
using System.Collections.Generic;
using System.Text;
using Courses.Application.DTOs.Courses;

namespace Courses.Application.Interfaces;

public interface ICoursesService
{
    Task<IEnumerable<CourseDto>> GetAllAsync();
    Task<CourseDto> GetByIdAsync(int id);
    Task<CourseDto> CreateAsync(CreateCourseDto createCourseDto);
    Task<bool> UpdateAsync(int id, UpdateCourseDto updateCourseDto);
    Task<bool> DeleteAsync(int id);
}
