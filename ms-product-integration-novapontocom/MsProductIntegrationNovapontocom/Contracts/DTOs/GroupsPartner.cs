using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class GroupsPartner
    {
        public int Codigo { get; set; }
        public string Nome { get; set; }
        public List<ItemsPartner> Itens { get; set; } = new List<ItemsPartner>();
    }
}
