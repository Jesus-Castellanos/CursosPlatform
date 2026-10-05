using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Enrollments;

public class UpdateEnrollmentDto
{
    public decimal CompletionPercentage { get; set; }
    public DateTime? CompletedAt { get; set; }
}