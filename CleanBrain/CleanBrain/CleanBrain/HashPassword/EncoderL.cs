using System;
using System.Security.Cryptography;

namespace Encoder
{
    public static class EncoderL
    {
        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            string passwordHash = HashPassword(password);
            return string.Equals(passwordHash, hashedPassword);
        }
    }
}