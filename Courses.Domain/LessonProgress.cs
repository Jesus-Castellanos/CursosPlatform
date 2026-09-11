using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Entities;

public class LessonProgress
{
    public int LessonProgressId { get; set; }
    public int UserId { get; set; }
    public int LessonId { get; set; }
    public bool IsCompleted { get; set; }
    public decimal ProgressPercentage { get; set; }
    public DateTime? LastAccessedAt { get; set; }
    public DateTime? CompletedAt { get; set; }


    public User User { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}
