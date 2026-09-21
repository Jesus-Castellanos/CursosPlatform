using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Domain.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}
