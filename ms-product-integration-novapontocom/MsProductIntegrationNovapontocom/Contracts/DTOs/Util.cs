using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public static class Util
    {
        public static string Decode(string valor)
        {
            try
            {
                List<char> characters = new List<char>() { '£', ';', '§', '&', '³', '©', 'ª', '¡', '?', '´', 'ã', 'ã', 'á', 'à', 'â', 'ã', 'ó', 'ò', 'õ', 'ô', 'é', 'è', 'ê', 'ç', 'í', 'ì', 'ú', 'ù' };

                if (valor == null)
                    valor = "";

                var _valor = valor?.ToLower();
                if (_valor?.IndexOfAny(characters?.ToArray()) == -1)
                    return valor;
                byte[] bytes = Encoding.GetEncoding("UTF-8").GetBytes(valor);
                return WebUtility.HtmlDecode(Encoding.UTF8.GetString(bytes, 0, bytes.Length));
            }
            catch
            {
                return valor;
            }
        }
    }
}
