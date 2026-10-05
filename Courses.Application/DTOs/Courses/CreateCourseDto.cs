using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Courses;

public class CreateCourseDto
{
    [Required(ErrorMessage = "El título del curso es obligatorio.")]
    [MinLength(3, ErrorMessage = "El título debe tener al menos 3 caracteres.")]
    [MaxLength(200, ErrorMessage = "El título no puede superar los 200 caracteres.")]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000, ErrorMessage = "La descripción no puede superar los 2000 caracteres.")]
    public string? Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
    public int CategoryId { get; set; }

    public int CreatedByUserId { get; set; }

    [Url(ErrorMessage = "La URL de la miniatura no tiene un formato válido.")]
    [MaxLength(500, ErrorMessage = "La URL de la miniatura no puede superar los 500 caracteres.")]
    public string? ThumbnailUrl { get; set; }

    [MaxLength(50, ErrorMessage = "El nivel no puede superar los 50 caracteres.")]
    public string? Level { get; set; }

    [Range(0, 99999999.99, ErrorMessage = "El precio debe estar entre 0 y 99,999,999.99.")]
    public decimal Price { get; set; }
}