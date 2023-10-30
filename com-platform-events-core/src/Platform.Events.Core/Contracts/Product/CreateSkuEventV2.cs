using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Events.Core.Contracts.Product
{
    public class CreateSkuEventV2 : EventBase
    {
        public string ApiVersion => "CreateSku/v2";
        public string ProductId { get; set; }
        public string Code { get; set; }
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
        public string[] Tags { get; set; }
        public bool Availability { get; set; }
    }
}
