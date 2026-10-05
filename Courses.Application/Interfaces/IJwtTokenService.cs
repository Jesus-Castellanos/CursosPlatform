using Courses.Domain.Entities;

namespace Courses.Application.Interfaces;

public interface IJwtTokenService
{
    string CreateToken(User user, IReadOnlyCollection<string> roles);
}