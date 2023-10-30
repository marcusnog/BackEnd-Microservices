using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class CategoryDB
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<CategoryDB>? Children { get; set; }
    }
}
