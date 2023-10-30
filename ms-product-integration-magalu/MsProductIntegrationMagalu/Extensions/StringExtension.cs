using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Extensions
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
