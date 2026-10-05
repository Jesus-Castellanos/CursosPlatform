using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Roles;

public class UpdateRoleDto
{
    [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
    [MinLength(3, ErrorMessage = "El nombre del rol debe tener al menos 3 caracteres.")]
    [MaxLength(50, ErrorMessage = "El nombre del rol no puede superar los 50 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255, ErrorMessage = "La descripción no puede superar los 255 caracteres.")]
    public string? Description { get; set; }
}