using System.Security.Cryptography;
using System.Text;

namespace GameTracker.Helpers
{
    public class PasswordHelper
    {
        /// <summary>
        /// Tworzy hash hasła przy użyciu PBKDF2 z losowym salt'em
        /// </summary>
        public static string HashPassword(string password)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] hash = pbkdf2.GetBytes(256 / 8);

                    // Zwracamy salt i hash w formacie: salt:hash (base64)
                    string saltBase64 = Convert.ToBase64String(salt);
                    string hashBase64 = Convert.ToBase64String(hash);
                    return $"{saltBase64}:{hashBase64}";
                }
            }
        }

        /// <summary>
        /// Weryfikuje hasło względem jego hash'a
        /// </summary>
        public static bool VerifyPassword(string password, string hash)
        {
            try
            {
                var hashComponents = hash.Split(':');
                if (hashComponents.Length != 2)
                    return false;

                var salt = Convert.FromBase64String(hashComponents[0]);
                var storedHash = Convert.FromBase64String(hashComponents[1]);

                using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                {
                    byte[] hash_computed = pbkdf2.GetBytes(256 / 8);
                    return hash_computed.SequenceEqual(storedHash);
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
