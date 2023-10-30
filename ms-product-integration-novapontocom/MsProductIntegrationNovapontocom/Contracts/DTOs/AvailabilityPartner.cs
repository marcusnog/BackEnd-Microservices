using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class AvailabilityPartner
    {
        public int Codigo { get; set; }
        public string PrecoDe { get; set; }
        public string PrecoPor { get; set; }
        public byte Disponibilidade { get; set; }
    }
}
