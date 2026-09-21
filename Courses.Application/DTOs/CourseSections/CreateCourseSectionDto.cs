namespace Courses.Application.DTOs.CourseSections;

public class CreateCourseSectionDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SectionOrder { get; set; }
}