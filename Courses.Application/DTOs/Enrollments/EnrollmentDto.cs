namespace Courses.Application.DTOs.Enrollments;

public class EnrollmentDto
{
    public int EnrollmentId { get; set; }

    public int UserId { get; set; }
    public string? UserName { get; set; }

    public int CourseId { get; set; }
    public string? CourseTitle { get; set; }

    public DateTime EnrollmentDate { get; set; }

    public decimal CompletionPercentage { get; set; }

    public DateTime? CompletedAt { get; set; }
}