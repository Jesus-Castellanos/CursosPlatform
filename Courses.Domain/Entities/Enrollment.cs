using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Entities;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int UserId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public decimal CompletionPercentage { get; set; }
    public DateTime? CompletedAt { get; set; }

    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
