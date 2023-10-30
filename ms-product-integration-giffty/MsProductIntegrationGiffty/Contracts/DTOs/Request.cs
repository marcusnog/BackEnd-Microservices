using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationGiffty.Contracts.DTOs
{
    public class Request
    {
        public ItemRequest Produtos { get; set; }
    }

    public class ItemRequest
    {
        public string Projeto { get; set; }
        public int? fabricanteId { get; set; }
        public int? fornecedorId { get; set; }
    }

    public class RequestAvailability
    {
        public ItemAvailabilityRequest Availability { get; set; }
    }

    public class ItemAvailabilityRequest
    {
        public string Projeto { get; set; }
        public string chave { get; set; }
        public string Produto { get; set; }
    }

    public class CodigosFornecedoresGiffty
    {
        public string[] lista = { "1028", "K63", "K64", "K65", "L85", "L87", "L88", "O07", "O08", "P29", "P32", "P39", "P43", "P73", "P77", "Q06", "Q55", "R16", "R45", "R46", "R47", "R62", "R63", "T22", "T23", "T91", "U00", "U48", "V14", "V75", "V76", "Y94", "Z30", "Z34", "Z35", "Z36", "Z37", "ZBD" };
    }
}
