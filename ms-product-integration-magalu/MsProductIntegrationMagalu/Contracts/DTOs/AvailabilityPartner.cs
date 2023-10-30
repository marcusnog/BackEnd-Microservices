using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class AvailabilityPartner
    {
        public string Codigo { get; set; }
        public string CodigoSKU { get; set; }
        public decimal Preco { get; set; }
        public DateTime UltimaModificacao { get; set; }
        public string Modelo { get; set; }
        public string EAN { get; set; }
        public bool Habilitado { get; set; }
        public int ProdutoId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataDesativado { get; set; }
        public int SkuId { get; set; }
        public decimal PrecoDe { get; set; }
        public decimal PrecoPor { get; set; }
        public bool Disponivel { get; set; }
        public string CodigoCampanhaFornecedor { get; set; }
        public decimal Frete { get; set; }
    }
}
