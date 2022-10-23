using FooDash.Application.Common.Interfaces.Security;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace FooDash.Application.Security.Services
{
    public class HashingService : IHashingService
    {
        public byte[] GetSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return salt;
        }

        public string Hash(string input, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA1, 1000, 256 / 8));
        }
    }
}