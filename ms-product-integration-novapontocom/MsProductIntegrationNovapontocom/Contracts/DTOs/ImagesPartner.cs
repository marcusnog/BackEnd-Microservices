using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class ImagesPartner
    {
        public string? UrlImagemMenor { get; set; }
        public string? UrlImagemMaior { get; set; }
        public string? UrlImagemZoom { get; set; }
        public int Ordem { get; set; }
    }
}
