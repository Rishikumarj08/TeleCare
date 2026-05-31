using System.Security.Cryptography;
using System.Text;

namespace TeleCare.Helpers;

public static class PasswordHasher
{
    private const string Salt = "TeleCareStaticSalt2026!";

    public static string Hash(string input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input));

        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input + Salt);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }

    public static bool Verify(string input, string expectedHash)
    {
        if (input == null || expectedHash == null)
            return false;

        return Hash(input) == expectedHash;
    }
}
