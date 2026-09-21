using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Roles;

public class CreateRoleDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
