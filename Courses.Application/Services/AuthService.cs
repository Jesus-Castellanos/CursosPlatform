using Courses.Application.DTOs.Auth;
using Courses.Application.Interfaces;
using Courses.Application.Security;

namespace Courses.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthService(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        if (dto is null ||
            string.IsNullOrWhiteSpace(dto.Email) ||
            string.IsNullOrWhiteSpace(dto.Password))
        {
            return null;
        }

        var email = dto.Email.Trim();

        var user = await _userRepository.GetByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        if (user.IsActive == false)
        {
            return null;
        }

        var passwordValid = await _passwordHasher.VerifyAsync(
            dto.Password,
            user.PasswordHash);

        if (!passwordValid)
        {
            return null;
        }

        var roles = await _userRepository.GetRolesByUserIdAsync(user.Id);

        var token = _jwtTokenService.CreateToken(
            user,
            roles);

        return new AuthResponseDto
        {
            Token = token,
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = roles
        };
    }
}