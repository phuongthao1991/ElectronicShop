﻿using System.Security.Cryptography;
using System.Text;

namespace ElectronicShop.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (var b in bytes)
                    builder.Append(b.ToString("x2")); // dạng hexa

                return builder.ToString();
            }
        }
    }
}
