using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class DetalheSKU
    {
        public int SkuId { get; set; }
        public decimal PrecoDe { get; set; }
        public decimal PrecoPor { get; set; }
        public bool Disponivel { get; set; }
        public string CodigoCampanhaFornecedor { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataDesativado { get; set; }
        public decimal Frete { get; set; }
    }
}
