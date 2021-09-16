using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace WebApplication8
{
    public class Cryptography
    {
        public static class Algorithms
        {
            public static readonly HashAlgorithm MD5 = new MD5CryptoServiceProvider();
            public static readonly HashAlgorithm SHA1 = new SHA1Managed();
            public static readonly HashAlgorithm SHA256 = new SHA256Managed();
            public static readonly HashAlgorithm SHA384 = new SHA384Managed();
            public static readonly HashAlgorithm SHA512 = new SHA512Managed();
            // public static readonly HashAlgorithm RIPEMD160 = new RIPEMD160Managed();
        }

        string _passPhrase = "Pas5pr@se";
        string _saltValue = "s@1tValue";
        readonly string _hashAlgorithm = "SHA1"; // [SHA1, MD5]
        readonly int _passwordIterations = 2;
        readonly string _initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        readonly int _keySize = 256; // [256, 192 128]

        public string PassPhrase { set { this._passPhrase = value; } }

        public string SaltValue { set { this._saltValue = value; } }

        public string EncryptString(string text)
        {
            return string.IsNullOrEmpty(text) ? null : this.Encrypt(text);
        }

        public string DecryptString(string text)
        {
            return string.IsNullOrEmpty(text) ? null : this.Decrypt(text);
        }

        public void EncryptToFile(string file, string text)
        {
            File.WriteAllText(file, this.Encrypt(text));
        }

        public void DecryptToFile(string file, string text)
        {
            File.WriteAllText(file, this.Decrypt(text));
        }

        public void EncryptFile(string file, string text)
        {
            File.WriteAllText(file, this.Encrypt(text));
        }

        public string DecryptFile(string file)
        {
            return this.Decrypt(File.ReadAllText(file));
        }

        private string Encrypt(string plainText)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(this._initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(this._saltValue);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(this._passPhrase, saltValueBytes, this._hashAlgorithm, this._passwordIterations);
            byte[] keyBytes = password.GetBytes(this._keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherTextBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }

        private string Decrypt(string cipherText)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(this._initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(this._saltValue);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            PasswordDeriveBytes password = new PasswordDeriveBytes(this._passPhrase, saltValueBytes, this._hashAlgorithm, this._passwordIterations);
            byte[] keyBytes = password.GetBytes(this._keySize / 8);
            RijndaelManaged symmetricKey = new RijndaelManaged
            {
                Mode = CipherMode.CBC
            };
            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
        }

        public static string GetMD5HashFromFile(string fileName)
        {
            using (MD5 md5 = MD5.Create())
            {
                using (FileStream stream = File.OpenRead(fileName))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream));
                }
            }
        }

        public static string GetHashFromFile(string fileName, HashAlgorithm algorithm)
        {
            using (BufferedStream stream = new BufferedStream(File.OpenRead(fileName), 100000))
            {
                return BitConverter.ToString(algorithm.ComputeHash(stream));
            }
        }

        public static string GetHash(byte[] bytes)
        {
            if (bytes != null)
            {
                using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
                {
                    return Convert.ToBase64String(sha1.ComputeHash(bytes));
                }
            }
            return null;
        }

        public static string GetHash(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Cryptography.GetHash(Encoding.UTF8.GetBytes(text));
            }
            return null;
        }
    }
}
