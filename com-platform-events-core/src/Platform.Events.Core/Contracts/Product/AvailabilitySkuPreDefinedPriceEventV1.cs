using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Events.Core.Contracts.Product
{
    public class AvailabilitySkuPreDefinedPriceEventV1 : EventBase
    {
        public string ApiVersion => "AvailabilitySkuPreDefinedPrice/v1";
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public bool Availability { get; set; }
    }
}
