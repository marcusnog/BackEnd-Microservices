using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class ProductsDB
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
        public Dictionary<string, ProductSkuDB> Skus { get; set; }
    }

    public class ProductSkuDB
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
    }

}

