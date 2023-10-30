using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraCreateBilletBatchRequest
    {
        public string name { get; set; }
        public List<TransfeeraCreateBilletRequest> billets { get; set; }
        public string type { get; set; }
    }
}
