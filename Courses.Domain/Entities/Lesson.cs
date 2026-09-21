using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Entities;

public class Lesson
{
    public int LessonId { get; set; }
    public int SectionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ContentType { get; set; } = string.Empty;
    public string ContentUrl { get; set; } = string.Empty;
    public int LessonOrder { get; set; }
    public int? DurationMinutes { get; set; }
    public bool? IsPreview { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public CourseSection Section { get; set; } = null!;
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
}
