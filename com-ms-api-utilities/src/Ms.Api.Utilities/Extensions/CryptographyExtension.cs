using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Ms.Api.Utilities.Extensions
{
    public sealed class CryptographyExtension
    {
        readonly IConfiguration _configuration;
        readonly string _key;
        private CryptographyExtension(IConfiguration? configuration = null)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _key = _configuration.GetValue<string>("ENCRYPTKEY");
        }
        private static CryptographyExtension _instance;

        public static CryptographyExtension GetInstance(IConfiguration? configuration = null)
        {
            if (_instance == null)
            {
                _instance = new CryptographyExtension(configuration);
            }
            return _instance;
        }

        public string Encrypt(string text)
        {
            var payload = Encoding.UTF8.GetBytes(text);
            var key = Encoding.UTF8.GetBytes(_key);

            using var aesAlg = Aes.Create();
            var iv = aesAlg.IV;
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            var encryptor = aesAlg.CreateEncryptor(key, iv);

            var encrypted = encryptor.TransformFinalBlock(payload, 0, payload.Length);

            return Convert.ToBase64String(encrypted);
        }

        public string Decrypt(string cipherText)
        {
            var payload = Convert.FromBase64String(cipherText); ;
            var key = Encoding.UTF8.GetBytes(_key);

            using var aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.ECB;
            aesAlg.Padding = PaddingMode.PKCS7;

            var decryptor = aesAlg.CreateDecryptor(key, aesAlg.IV);

            byte[] cipher = decryptor.TransformFinalBlock(payload, 0, payload.Length);
            var encryptedText = Encoding.UTF8.GetString(cipher);
            return encryptedText;
        }
    }
}