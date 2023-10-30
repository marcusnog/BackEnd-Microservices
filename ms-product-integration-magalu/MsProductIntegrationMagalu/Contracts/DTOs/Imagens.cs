using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class Imagens
    {
        public string ImagemMenor { get; set; }
        public string ImagemMaior { get; set; }
        public string ImagemZoom { get; set; }
        public int Ordem { get; set; }
        public int SKUId { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataDesativado { get; set; }
    }
}
