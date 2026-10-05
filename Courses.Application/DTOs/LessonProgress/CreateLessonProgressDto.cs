namespace Courses.Application.DTOs.LessonProgress;

public class CreateLessonProgressDto
{
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public decimal ProgressPercentage { get; set; }
}