using System.Security.Cryptography;
using System.Text;

namespace Ms.Api.Utilities.Extensions
{
    public static class StringExtensions
    {
        public static string Sha256(this string input)
        {
            using SHA256 shA256 = SHA256.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(((HashAlgorithm)shA256).ComputeHash(bytes));
        }
        public static bool IsValidMongoID(this string id)
        {
            return (id?.Length ?? 0) == 24;
        }
    }
}
