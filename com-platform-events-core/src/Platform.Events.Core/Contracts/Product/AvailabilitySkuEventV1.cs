using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.Events.Core.Contracts.Product
{
    public class AvailabilitySkuEventV1 : EventBase
    {
        public string ApiVersion => "AvailabilitySku/v1";
        public string SkuId { get; set; }
        public string ProductId { get; set; }
        public bool Availability { get; set; }
    }
}
