using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Xemio.ImageUploader.Shared
{
    /// <summary>
    /// Contains methods to create a unique hash based on a password and an username.
    /// </summary>
    public class Hash
    {
        #region Implementation of IPasswordEncryptionService
        /// <summary>
        /// Creates a hash of the given <paramref name="password"/> using the given <paramref name="username"/>.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="username">The username.</param>
        public static byte[] CreatePasswordHash(string password, string username)
        {
            SHA256 hash = SHA256.Create();

            byte[] usernameHash = hash.ComputeHash(Encoding.Default.GetBytes(username));
            List<byte> passwordBytes = Encoding.Default.GetBytes(password).ToList();

            passwordBytes.AddRange(usernameHash);

            return hash.ComputeHash(passwordBytes.ToArray());
        }
        /// <summary>
        /// Creates a hash of the given <paramref name="content"/> and creates a password-hash with the given <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="password">The password.</param>
        /// <param name="username">The username.</param>
        public static string CreateContentHash(string content, string password, string username)
        {
            byte[] passwordHash = Hash.CreatePasswordHash(password, username);
            return Hash.CreateContentHash(content, passwordHash);
        }
        /// <summary>
        /// Creates a hash of the given <paramref name="content" /> with the given <paramref name="passwordHash" />
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="passwordHash">The password hash.</param>
        public static string CreateContentHash(string content, byte[] passwordHash)
        {
            HMACSHA256 hash = new HMACSHA256(passwordHash);

            byte[] hashedContent = hash.ComputeHash(Encoding.Default.GetBytes(content));

            return Convert.ToBase64String(hashedContent);
        }
        #endregion
    }
}
