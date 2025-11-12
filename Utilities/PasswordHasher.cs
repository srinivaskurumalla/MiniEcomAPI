using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MiniEcom.Api.Utilities
{
    public static class PasswordHasher
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var rng = RandomNumberGenerator.Create();
            passwordSalt = new byte[128 / 8];
            rng.GetBytes(passwordSalt);

            passwordHash = KeyDerivation.Pbkdf2(
                password: password,
                salt: passwordSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8);
        }

        public static bool Verify(string password, byte[] storedHash, byte[] storedSalt)
        {
            var hash = KeyDerivation.Pbkdf2(
                password: password,
                salt: storedSalt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100_000,
                numBytesRequested: 256 / 8);

            return CryptographicOperations.FixedTimeEquals(hash, storedHash);
        }
    }
}
