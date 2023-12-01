using System;
using System.Security.Cryptography;
using System.Text;

namespace TimeManager.MainLibrary.Helpers
{
    public static class PasswordHashHelper
    {
        // The number of iterations for the bcrypt algorithm (adjust as needed)
        private const int BcryptWorkFactor = 12;

        // Define a constant salt
        private const string SaltConstant = "YouAreTheSaltOfTheEarth";

        public static string GeneratePasswordHash(string password)
        {
            // Convert the constant salt to a byte array
            byte[] saltBytes = Encoding.UTF8.GetBytes(SaltConstant);

            // Create a new instance of the Rfc2898DeriveBytes class with the password and salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, BcryptWorkFactor);

            // Get the derived key
            byte[] hash = pbkdf2.GetBytes(20);

            // Combine the constant salt and password hash for storage
            byte[] hashBytes = new byte[36];
            Array.Copy(saltBytes, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            // Convert the combined byte array to a Base64-encoded string
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }

        public static bool VerifyPassword(string password, string savedPasswordHash)
        {
            // Convert the constant salt to a byte array
            byte[] saltBytes = Encoding.UTF8.GetBytes(SaltConstant);

            // Extract the salt and hash from the saved password hash
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            // Compute the hash of the provided password and constant salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, BcryptWorkFactor);
            byte[] hash = pbkdf2.GetBytes(20);

            // Compare the computed hash with the saved hash
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
  