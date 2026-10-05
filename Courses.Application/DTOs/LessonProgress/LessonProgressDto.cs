namespace Courses.Application.DTOs.LessonProgress;

public class LessonProgressDto
{
    public int LessonProgressId { get; set; }

    public int UserId { get; set; }
    public string? UserName { get; set; }

    public int LessonId { get; set; }
    public string? LessonTitle { get; set; }

    public decimal ProgressPercentage { get; set; }
    public bool IsCompleted { get; set; }

    public DateTime? LastAccessedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}