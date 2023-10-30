using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Utils
{
    internal class Utils
    {
        public static string GetVoltagm(int? VoltagemId)
        {
            switch (VoltagemId)
            {
                case 0: return "Bivolt";
                case 1: return "110V";
                case 2: return "220V";
                case 3: return "Não utiliza energia elétrica";
                case 4: return "380V";
                case 5: return "Bateria Recarregável";
                case 6: return "Pilha";
                case 7: return "Energia Solar";
                default: return VoltagemId.ToString();
            }
        }
    }
}
