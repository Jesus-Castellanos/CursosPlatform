namespace Courses.Application.DTOs.CourseSections;

public class CourseSectionDto
{
    public int SectionId { get; set; }
    public int CourseId { get; set; }
    public string? CourseTitle { get; set; }

    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SectionOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}