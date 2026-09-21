using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Entities;

public class CourseSection
{
    public int SectionId { get; set; }
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SectionOrder { get; set; }
    public DateTime CreatedAt { get; set; }

    public Course Course { get; set; } = null!;
    public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}
