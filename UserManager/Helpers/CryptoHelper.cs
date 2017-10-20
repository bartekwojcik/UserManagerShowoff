using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UserManager.Helpers
{
    internal static class CryptoHelper
    {
        // resources: https://medium.com/@mehanix/lets-talk-security-salted-password-hashing-in-c-5460be5c3aae
        private static byte[] CreateSalt()
        {
            byte[] salt = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(salt);
            return salt;
        }

        private static string CreateHash(string password, byte[] salt)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 21370);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16); //todo concatgghghh
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        internal static string HashPassword(string password)
        {
            var salt = CreateSalt();
            var hashedPassword = CreateHash(password, salt);
            return hashedPassword;
        }

        internal static bool ComparePasswords(string password, string hashedPasswords)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPasswords);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16); //todo concat
            var asdf2 = new Rfc2898DeriveBytes(password, salt, 21370);
            byte[] hash = asdf2.GetBytes(20);
            bool areTheSame = true;
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    areTheSame = false;
                }
            }

            return areTheSame;
        }

        internal static string GenerateToken(DateTime expirationTime)
        {
            //resources: https://stackoverflow.com/questions/14643735/how-to-generate-a-unique-token-which-expires-after-24-hours
            byte[] time = BitConverter.GetBytes(expirationTime.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return token;
        }
    }
}
