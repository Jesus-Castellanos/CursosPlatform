using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Lessons;

public class CreateLessonDto
{
    public int SectionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string ContentUrl { get; set; } = string.Empty;
    public int LessonOrder { get; set; }
    public int? Duration { get; set; }
    public bool? IsPreview { get; set; }
}