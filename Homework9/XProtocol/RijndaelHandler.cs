using System;
using System.IO;
using System.Security.Cryptography;

namespace XProtocol
{
    public static class RijndaelHandler
    {
        private const int Keysize = 256;
        private const int Blocksize = 128;
        private const int DerivationIterations = 1000;

        public static byte[] Encrypt(byte[] data, string passPhrase)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(Keysize / 8);
            var ivBytes   = RandomNumberGenerator.GetBytes(Blocksize / 8);

            using var password = new Rfc2898DeriveBytes(
                passPhrase,
                saltBytes,
                DerivationIterations,
                HashAlgorithmName.SHA256);

            var keyBytes = password.GetBytes(Keysize / 8);

            using var aes = Aes.Create();
            aes.KeySize = Keysize;
            aes.BlockSize = Blocksize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var encryptor = aes.CreateEncryptor(keyBytes, ivBytes);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(data, 0, data.Length);
                cs.FlushFinalBlock();
            }

            var cipherBytes = ms.ToArray();

            var result = new byte[saltBytes.Length + ivBytes.Length + cipherBytes.Length];
            Buffer.BlockCopy(saltBytes, 0, result, 0, saltBytes.Length);
            Buffer.BlockCopy(ivBytes,   0, result, saltBytes.Length, ivBytes.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, saltBytes.Length + ivBytes.Length, cipherBytes.Length);

            return result;
        }

        public static byte[] Decrypt(byte[] data, string passPhrase)
        {
            var saltSize = Keysize / 8;
            var ivSize   = Blocksize / 8;

            if (data.Length < saltSize + ivSize)
                throw new ArgumentException("Invalid encrypted data");

            var saltBytes = new byte[saltSize];
            var ivBytes   = new byte[ivSize];
            Buffer.BlockCopy(data, 0, saltBytes, 0, saltSize);
            Buffer.BlockCopy(data, saltSize, ivBytes, 0, ivSize);

            var cipherBytes = new byte[data.Length - saltSize - ivSize];
            Buffer.BlockCopy(data, saltSize + ivSize, cipherBytes, 0, cipherBytes.Length);

            using var password = new Rfc2898DeriveBytes(
                passPhrase,
                saltBytes,
                DerivationIterations,
                HashAlgorithmName.SHA256);

            var keyBytes = password.GetBytes(Keysize / 8);

            using var aes = Aes.Create();
            aes.KeySize = Keysize;
            aes.BlockSize = Blocksize;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor(keyBytes, ivBytes);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var resultMs = new MemoryStream();

            cs.CopyTo(resultMs);
            return resultMs.ToArray();
        }
    }
}
