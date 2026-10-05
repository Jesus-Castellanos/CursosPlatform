using System;
using System.Collections.Generic;
using System.Text;
using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface IEnrollmentRepository
{
    Task<List<Enrollment>> GetAllAsync();

    Task<Enrollment?> GetByIdAsync(int id);

    Task<Enrollment?> GetByUserAndCourseAsync(
        int userId,
        int courseId);

    Task<Enrollment> AddAsync(
        Enrollment enrollment);
    
    Task UpdateAsync(
        Enrollment enrollment);

    Task DeleteAsync(
        Enrollment enrollment);
}