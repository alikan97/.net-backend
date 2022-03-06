using System;
using System.Security.Cryptography;
using System.Text;

namespace Server.Utilities
{
    public static class SecureHasher
    {
        private const int saltSize = 16;
        private const int hashSize = 20;
        public static string Hash (string password, int iterations) {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);

            var pbk = new Rfc2898DeriveBytes(password,salt, iterations);
            var hash = pbk.GetBytes(hashSize);

            var hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        public static bool Verify(string password, string hashPassword)
        {
            if (!IsHashSupported(hashPassword))
            {
                throw new NotSupportedException("Hash type not supported");
            }
            var splittedHashString = hashPassword.Replace("$MYHASH$V1$","").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);
            
            var pbk = new Rfc2898DeriveBytes(password,salt,iterations);
            byte[] hash = pbk.GetBytes(hashSize);

            for (var i = 0; i < hashSize; i++){
                if (hashBytes[i+saltSize] != hash[i]){
                    return false;
                }
            }
            return true;
        }
    }
}