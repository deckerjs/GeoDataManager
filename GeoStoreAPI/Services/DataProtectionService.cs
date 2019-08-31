using System;
using System.Security.Cryptography;
using GeoStoreAPI.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace GeoStoreAPI.Services
{
    public class DataProtectionService : IDataProtectionService
    {
        private const int _iterations = 10000;

        public DataProtectionService() {}

        public string GetPasswordHash(string passwordClear)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = GetHashValue(salt, passwordClear);
            return $"{_iterations}.{Convert.ToBase64String(salt)}.{hashed}";
        }

        public bool PasswordMatchesHash(string passwordClear, string hashed)
        {
            var hashParameters = hashed.Split('.', 3);
            if (hashParameters.Length != 3)
            {
                throw new FormatException("Unexpected hash format");
            }

            var iterations = Convert.ToInt32(hashParameters[0]);
            var salt = Convert.FromBase64String(hashParameters[1]);
            var key = hashParameters[2];
            string hashCheck = GetHashValue(salt, passwordClear);
            return hashCheck.Equals(key);
        }

        private string GetHashValue(byte[] salt, string value)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: value,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: _iterations,
                numBytesRequested: 256 / 8));
        }

    }

}
