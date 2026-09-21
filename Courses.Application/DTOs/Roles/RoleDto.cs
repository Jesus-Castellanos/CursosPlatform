using System;
using System.Collections.Generic;
using System.Text;

namespace Courses.Application.DTOs.Roles;

public class RoleDto
{
    public int RoleId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}
