using Courses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Enrollments;

public class CreateEnrollmentDto
{
    public int UserId { get; set; }
    public int CourseId { get; set; }
}