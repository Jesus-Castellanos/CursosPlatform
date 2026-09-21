using Courses.Application.Interfaces;
using Argon2id.PasswordHasher;

namespace Courses.Application.Services;

public class Argon2PasswordHasherServices : IPasswordHasherServices
{
    private readonly Argon2idPasswordHasher _hasher;

    public Argon2PasswordHasherServices()
    {
        _hasher = new Argon2idPasswordHasher();
    }

    public string Hash(string password)
    {
        return _hasher.HashPassword(password);
    }

    public bool Verify(string password, string passwordHash)
    {
        return _hasher.VerifyPassword(password, passwordHash);
    }
}
