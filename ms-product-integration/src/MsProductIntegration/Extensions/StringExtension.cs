using System.Security.Cryptography;
using System.Text;

namespace MsProductIntegration.Extensions
{
    public static class StringExtension
    {
        public static string Sha256(this string input)
        {
            using (SHA256 shA256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(input);
                return Convert.ToBase64String(((HashAlgorithm)shA256).ComputeHash(bytes));
            }
        }
    }
}
