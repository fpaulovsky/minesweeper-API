using System;
using System.Linq;
using System.Security.Cryptography;

namespace MinesweeperAPI
{
    public sealed class PasswordHasher
    {
        private const int DefaultSaltSize = 16; // 128 bit 
        private const int DefaultKeySize = 32; // 256 bit
        private const int DefaultIterations = 10000;

        public string Hash(string password)
        {
            using (var algorithm = new Rfc2898DeriveBytes(password, DefaultSaltSize, DefaultIterations, HashAlgorithmName.SHA512))
            {
                var key = Convert.ToBase64String(algorithm.GetBytes(DefaultKeySize));
                var salt = Convert.ToBase64String(algorithm.Salt);
                return $"{DefaultIterations}.{salt}.{key}";
            }
        }

        public (bool Verified, bool NeedsUpgrade) Check(string hash, string password)
        {
            var parts = hash.Split('.', 3);

            if (parts.Length != 3)
            {
                throw new FormatException("Unexpected hash format. Should be formatted as `{iterations}.{salt}.{hash}`");
            }

            var iterations = Convert.ToInt32(parts[0]);
            var salt = Convert.FromBase64String(parts[1]);
            var key = Convert.FromBase64String(parts[2]);

            var needsUpgrade = iterations != DefaultIterations;

            using (var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA512))
            {
                var keyToCheck = algorithm.GetBytes(DefaultKeySize);
                var verified = keyToCheck.SequenceEqual(key);
                return (verified, needsUpgrade);
            }
        }
    }
}
