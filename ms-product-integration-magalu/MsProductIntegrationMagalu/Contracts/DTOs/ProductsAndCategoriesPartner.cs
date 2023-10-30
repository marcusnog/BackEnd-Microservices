using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class ProductsAndCategoriesPartner
    {
        public List<ProductPartner> ProductsPartner { get; set; }
        public List<CategoryPartner> CategoriesPartner { get; set; }
    }
}
