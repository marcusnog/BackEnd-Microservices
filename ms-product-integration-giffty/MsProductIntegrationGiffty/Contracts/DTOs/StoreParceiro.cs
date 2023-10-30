using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationGiffty.Contracts.DTOs
{
    public class StoreParceiro
    {
        public int IdFabricante { get; set; }
        public string NomeFabricante { get; set; }
        public int IdFornecedor { get; set; }
        public string NomeFornecedor { get; set; }
    }
}
