using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace NtlSystem.Models
{
    public class SecurityHelper
    {
        private static string GenerateSalt(int nSalt)
        {
            var saltBytes = new byte[nSalt];

            var provider = new Random(420);
            provider.NextBytes(saltBytes);

            return Convert.ToBase64String(saltBytes);
        }

        private static string HashPassword(string password, string salt, int nIterations, int nHash)
        {
            var saltBytes = Convert.FromBase64String(salt);
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, saltBytes, nIterations);
            return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(nHash));
        }

        public static string HashPasswordFull(string password)
        {
            password = (password == null) ? "" : password;
            string salt = GenerateSalt(128);
            return HashPassword(password, salt, 12000, 64);
        }
    }
}