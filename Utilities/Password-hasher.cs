using System;
using System.ComponentModel;
using System.Security.Cryptography;

namespace Server.Utilities
{
    public class passwordHasher
    {
        private readonly static int iterations = 10000;
        private readonly static int saltSize = 16;
        private readonly static int hashSize = 20;

        public static string hashPassword(string password)
        {
            byte[] salt = GenerateSaltNewInstance(saltSize);
            var ppk = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = ppk.GetBytes(hashSize);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);
            
            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        private static byte[] GenerateSaltNewInstance(int size)
        {
            using (var generator = RandomNumberGenerator.Create())
            {
                var salt = new byte[size];
                generator.GetBytes(salt);
                return salt;
            }
        }

        public static bool verifyPassword(string savedPassword, string rawPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(savedPassword);
            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            var pass = new Rfc2898DeriveBytes(rawPassword, salt, iterations);
            byte[] hash =  pass.GetBytes(hashSize);
            for (int i=0; i < hashSize; i++)  {
                if (hashBytes[i+16] != hash[i]) {
                    return false;
                }
            }
            return true;
        }
    }
}