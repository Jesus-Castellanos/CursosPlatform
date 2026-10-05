using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Courses.Application.Interfaces;
using Courses.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Courses.Infrastructure.Security;

public class JwtTokenService : IJwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(
        User user,
        IReadOnlyCollection<string> roles)
    {
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var issuer = _configuration["Jwt:Issuer"];

        var audience = _configuration["Jwt:Audience"];

        var privateKey = _configuration["Jwt:PrivateKey"];

        var keyId = _configuration["Jwt:KeyId"];

        var expirationMinutesText =
            _configuration["Jwt:AccessTokenMinutes"];

        if (string.IsNullOrWhiteSpace(issuer))
            throw new InvalidOperationException(
                "No está configurado Jwt:Issuer.");

        if (string.IsNullOrWhiteSpace(audience))
            throw new InvalidOperationException(
                "No está configurado Jwt:Audience.");

        if (string.IsNullOrWhiteSpace(privateKey))
            throw new InvalidOperationException(
                "No está configurado Jwt:PrivateKey.");

        if (string.IsNullOrWhiteSpace(keyId))
            throw new InvalidOperationException(
                "No está configurado Jwt:KeyId.");

        if (!int.TryParse(
                expirationMinutesText,
                out var expirationMinutes) ||
            expirationMinutes <= 0)
        {
            throw new InvalidOperationException(
                "Jwt:AccessTokenMinutes no es válido.");
        }

        using var rsa = RSA.Create();

        rsa.ImportFromPem(privateKey);

        var securityKey = new RsaSecurityKey(rsa)
        {
            KeyId = keyId
        };

        var signingCredentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.RsaSsaPssSha512);

        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(
                JwtRegisteredClaimNames.Sub,
                user.Id.ToString()),

            new(
                JwtRegisteredClaimNames.Email,
                user.Email),

            new(
                ClaimTypes.NameIdentifier,
                user.Id.ToString()),

            new(
                ClaimTypes.Name,
                user.Email),

            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString("N")),

            new(
                JwtRegisteredClaimNames.Iat,
                new DateTimeOffset(now)
                    .ToUnixTimeSeconds()
                    .ToString(),
                ClaimValueTypes.Integer64)
        };

        foreach (var role in roles
                     .Where(x => !string.IsNullOrWhiteSpace(x))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            claims.Add(
                new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}