using System.Security.Cryptography;
using System.Text;
using KonsciousArgon2id = Konscious.Security.Cryptography.Argon2id;

namespace Courses.Application.Security;

public class Argon2PasswordHasher : IPasswordHasher
{
    public async Task<bool> VerifyAsync(
        string password,
        string encodedHash)
    {
        if (string.IsNullOrWhiteSpace(password) ||
            string.IsNullOrWhiteSpace(encodedHash))
        {
            return false;
        }

        try
        {
            var parts = encodedHash.Split('$');

            // Formato:
            // $argon2id$v=19$m=65536,t=3,p=1$salt$hash
            if (parts.Length != 6 ||
                parts[1] != "argon2id")
            {
                return false;
            }

            var parameters = ParseParameters(parts[3]);

            var salt = DecodeBase64WithoutPadding(parts[4]);
            var expectedHash = DecodeBase64WithoutPadding(parts[5]);

            var argon2 = new KonsciousArgon2id(
                Encoding.UTF8.GetBytes(password))
            {
                Salt = salt,
                MemorySize = parameters.Memory,
                Iterations = parameters.Iterations,
                DegreeOfParallelism = parameters.Parallelism
            };

            var computedHash =
                await argon2.GetBytesAsync(expectedHash.Length);

            return CryptographicOperations.FixedTimeEquals(
                computedHash,
                expectedHash);
        }
        catch (FormatException)
        {
            return false;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }

    private static byte[] DecodeBase64WithoutPadding(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new FormatException(
                "Valor Base64 vacío.");
        }

        var padding = (4 - value.Length % 4) % 4;

        if (padding > 0)
        {
            value += new string('=', padding);
        }

        return Convert.FromBase64String(value);
    }

    private static (
        int Memory,
        int Iterations,
        int Parallelism) ParseParameters(
        string parameterString)
    {
        var parameters = parameterString.Split(',');

        int memory = 0;
        int iterations = 0;
        int parallelism = 0;

        foreach (var parameter in parameters)
        {
            var pair = parameter.Split('=');

            if (pair.Length != 2)
                continue;

            switch (pair[0])
            {
                case "m":
                memory = int.Parse(pair[1]);
                break;

                case "t":
                iterations = int.Parse(pair[1]);
                break;

                case "p":
                parallelism = int.Parse(pair[1]);
                break;
            }
        }

        if (memory <= 0 ||
            iterations <= 0 ||
            parallelism <= 0)
        {
            throw new ArgumentException(
                "Los parámetros del hash Argon2id no son válidos.");
        }

        return (
            memory,
            iterations,
            parallelism);
    }
}