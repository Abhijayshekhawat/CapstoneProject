using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CapstoneProject.Models.Utilities
{
    public class EncryptionHelper
    {
        private readonly string _encryptionKey;
        private readonly string _saltValue;

        public EncryptionHelper(IConfiguration configuration)
        {
            _encryptionKey = configuration["EncryptionKey"];
            _saltValue = configuration["SaltValue"];
        }
        // Encrypt a string using AES with random IV
        public string Encrypt(string plainText)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(_saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aes = Aes.Create())
            {
                using (var keyDerivation = new Rfc2898DeriveBytes(_encryptionKey, saltBytes, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = keyDerivation.GetBytes(32); // 256-bit key
                    aes.GenerateIV(); // Generate a random IV
                }

                using (var ms = new MemoryStream())
                {
                    // Write the IV to the MemoryStream first
                    ms.Write(aes.IV, 0, aes.IV.Length);

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cs.FlushFinalBlock();
                    }

                    // Return the encrypted data along with the IV (prepended)
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }


        // Decrypt a string using AES with the IV prepended
        public string Decrypt(string encryptedText)
        {
            byte[] saltBytes = Encoding.UTF8.GetBytes(_saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            using (Aes aes = Aes.Create())
            {
                using (var keyDerivation = new Rfc2898DeriveBytes(_encryptionKey, saltBytes, 1000, HashAlgorithmName.SHA256))
                {
                    aes.Key = keyDerivation.GetBytes(32); // 256-bit key
                }

                using (var ms = new MemoryStream(cipherTextBytes))
                {
                    // Read the IV from the encrypted data
                    byte[] iv = new byte[aes.BlockSize / 8];
                    ms.Read(iv, 0, iv.Length);
                    aes.IV = iv;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }


        // Compute a SHA-256 hash of the input (useful for password hashing)
        public static string ComputeHash(string input)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Convert hash bytes to a hex string
                StringBuilder builder = new StringBuilder();
                foreach (var b in hashBytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
