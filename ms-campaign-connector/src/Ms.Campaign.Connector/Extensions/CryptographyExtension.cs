using System.Security.Cryptography;
using System.Text;

namespace Ms.Campaign.Connector.Extensions
{
    public class CryptographyExtension
    {
        public sealed class Cryptography
        {
            readonly IConfiguration _configuration;
            readonly string _key;
            private Cryptography(IConfiguration? configuration = null)
            {
                _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
                _key = _configuration.GetValue<string>("ENCRYPTKEY");
            }
            private static Cryptography _instance;

            public static Cryptography GetInstance(IConfiguration? configuration = null)
            {
                if (_instance == null)
                {
                    _instance = new Cryptography(configuration);
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
                if (cipherText == null) return null;
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
}
