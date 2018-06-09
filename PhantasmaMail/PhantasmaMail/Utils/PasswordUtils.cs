using System;
using System.Security.Cryptography;
using System.Text;

namespace PhantasmaMail.Utils
{
    public static class PasswordUtils
    {
        public static byte[] DeriveKey(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username is required");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password is required");
            }

            if (password.ToLowerInvariant().Contains(username.ToLowerInvariant()))
            {
                throw new ArgumentException("Password cannot be similar to username");
            }

            bool hasSpecial = false;
            foreach  (var c in password)
            {
                if (!char.IsLetter(c))
                {
                    hasSpecial = true;
                    break;
                }
            }

            if (hasSpecial)
            {
                throw new ArgumentException("Password must contain at least a number or other special character");
            }

            var buffer = new byte[username.Length + password.Length];
            int i = 0;
            int j = 0;
            int k = 0;

            // reshuffling of cypher content
            while (k < buffer.Length)
            {
                byte a = i < username.Length ? (byte)username[i] : (byte)0;
                byte b = j < password.Length ? (byte)password[j] : (byte)0;

                bool takeFirst = ((a > b) == (k % 2 == 0));

                if (takeFirst && i >= username.Length)
                {
                    takeFirst = false;
                }
                else
                if (!takeFirst && j >= password.Length)
                {
                    takeFirst = true;
                }

                byte c;
                int n;

                if (takeFirst)
                {
                    c = a;
                    n = i;
                    i++;
                }
                else
                {
                    c = b;
                    n = j;
                    j++;
                }

                if (char.IsLetter((char)c) && ((c + n) % 2 == k % 2))
                {
                    c = (byte)(char.ToUpper((char)c));
                }

                //Console.Write((char)c);
                buffer[k] = c;
                k++;
            }

            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(buffer);
            }
        }
    }
}
