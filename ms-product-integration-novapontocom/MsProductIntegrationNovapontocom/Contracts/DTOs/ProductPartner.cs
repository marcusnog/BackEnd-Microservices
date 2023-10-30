using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class ProductPartner
    {
        public int Codigo { get; set; }
        public string? DisplayName { get; set; }
        public string? DescricaoLonga { get; set; }
        public int Categoria { get; set; }
        public int CodigoFabricante { get; set; }
        public string? Fabricante { get; set; }
        public string? FotoPequena { get; set; }
        public string? FotoMedia { get; set; }
        public string? FotoGrande { get; set; }
        public string? PalavraChave { get; set; }
        public int MaisVendidos { get; set; }
        public List<SkuPartner> Skus { get; set; } = new List<SkuPartner>();
        public List<GroupsPartner> Grupos { get; set; } = new List<GroupsPartner>();

    }
}
