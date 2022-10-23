using FooDash.Application.Common.Interfaces.Security;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;

namespace FooDash.Application.Security.Services

{
    public class EncryptionOptions
    {
        public string Key { get; set; }
    }

    public class EncryptionService : IEncryptionService
    {
        private readonly EncryptionOptions _encryptionOptions;

        public EncryptionService(IOptions<EncryptionOptions> encryptionOptions)
        {
            _encryptionOptions = encryptionOptions.Value;
        }

        public string Decrypt(string cipherText)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            var key = Encoding.UTF8.GetBytes(_encryptionOptions.Key);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                using (var decryptor = aesAlg.CreateDecryptor())
                {
                    using (var msDecrypt = new MemoryStream(cipher))
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    using (var srDecrypt = new StreamReader(csDecrypt, Encoding.Unicode))
                    {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }

        public string Encrypt(string input)
        {
            var key = Encoding.UTF8.GetBytes(_encryptionOptions.Key);

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                using (var encryptor = aesAlg.CreateEncryptor())
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (var swEncrypt = new StreamWriter(csEncrypt, Encoding.Unicode))
                    {
                        swEncrypt.Write(input);
                    }

                    var iv = aesAlg.IV;

                    var decryptedContent = msEncrypt.ToArray();

                    var result = new byte[iv.Length + decryptedContent.Length];

                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                    return Convert.ToBase64String(result);
                }
            }
        }
    }
}