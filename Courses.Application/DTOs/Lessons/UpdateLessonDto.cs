namespace Courses.Application.DTOs.Lessons;

public class UpdateLessonDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    public string ContentType { get; set; } = string.Empty;
    public string ContentUrl { get; set; } = string.Empty;

    public int LessonOrder { get; set; }
    public int? DurationMinutes { get; set; }
    public bool? IsPreview { get; set; }
}