using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Courses;

public class UpdateCourseDto
{
    [Required, StringLength(200, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Range(1, int.MaxValue)]
    public int CategoryId { get; set; }
    public string? ThumbnailUrl { get; set; }

    [StringLength(50)]
    public string? Level { get; set; }

    [Range(typeof(decimal), "0", "1000000")]
    public decimal Price { get; set; }
    public bool IsPublished { get; set; }
    public bool IsActive { get; set; }
}
