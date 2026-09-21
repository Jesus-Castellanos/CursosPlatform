using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Courses;

public class CourseDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public int CreatedByUserId { get; set; }
    public string? ThumbnailUrl { get; set; }
    public string? Level { get; set; }
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}