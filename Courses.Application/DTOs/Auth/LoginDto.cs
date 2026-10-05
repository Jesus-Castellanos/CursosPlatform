using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Auth;

public class LoginDto
{
    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [MaxLength(150, ErrorMessage = "El correo electrónico no puede superar los 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(4, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
    [MaxLength(200, ErrorMessage = "La contraseña no puede superar los 200 caracteres.")]
    public string Password { get; set; } = string.Empty;
}