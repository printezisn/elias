using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Elias.Shared.Helpers
{
    /// <summary>
    /// Helper class for password encryption methods
    /// </summary>
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">Stores the salt used for the password hashing</param>
        /// <returns>The hashed password</returns>
        public static string HashPassword(string password, out string salt)
        {
            // Generate the hash, with an automatic 32 byte salt
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32);
            rfc2898DeriveBytes.IterationCount = 10000;
            byte[] hashBytes = rfc2898DeriveBytes.GetBytes(20);
            byte[] saltBytes = rfc2898DeriveBytes.Salt;

            //Return the salt and the hash
            salt = Convert.ToBase64String(saltBytes);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Hashes a password and uses a specific salt
        /// </summary>
        /// <param name="password">The password to hash</param>
        /// <param name="salt">The salt to use for the hashing</param>
        /// <returns>The hashed password</returns>
        public static string HashPasswordWithSalt(string password, string salt)
        {
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), 10000);
            byte[] hashBytes = rfc2898DeriveBytes.GetBytes(20);

            return Convert.ToBase64String(hashBytes);
        }
    }
}
