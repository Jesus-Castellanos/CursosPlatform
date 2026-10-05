using Courses.Application.DTOs.Auth;

namespace Courses.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}