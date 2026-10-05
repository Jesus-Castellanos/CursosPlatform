using System.ComponentModel.DataAnnotations;

namespace Courses.Application.DTOs.Users;

public class CreateUserDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "El apellido debe tener entre 2 y 100 caracteres.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El correo electrónico no tiene un formato válido.")]
    [StringLength(
        255,
        ErrorMessage = "El correo electrónico no puede superar los 255 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [StringLength(
        100,
        MinimumLength = 4,
        ErrorMessage = "La contraseña debe tener entre 8 y 100 caracteres.")]
    public string Password { get; set; } = string.Empty;
}