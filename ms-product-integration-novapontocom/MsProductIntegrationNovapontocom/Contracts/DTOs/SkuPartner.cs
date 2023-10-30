using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class SkuPartner
    {
        public int Codigo { get; set; }
        public decimal Preco { get; set; }
        public DateTime UltimaModificacao { get; set; }
        public bool Habilitado { get; set; }
        public string? Modelo { get; set; }
        public string? EAN { get; set; }
        public decimal? Peso { get; set; }
        public decimal? Altura { get; set; }
        public decimal? Largura { get; set; }
        public decimal? Comprimento { get; set; }
        public int IdLojista { get; set; }
        public List<ImagesPartner> Imagens { get; set; } = new List<ImagesPartner>();
        public List<GroupsPartner> GruposSku { get; set; } = new List<GroupsPartner>();
    }
}
