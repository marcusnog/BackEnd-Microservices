using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraCloseBatchResponse
    {
        public string name { get; set; }
        public string agency { get; set; }
        public string account { get; set; }
        public string type { get; set; }
        public string company { get; set; }
        public string cnpj { get; set; }
        public bool is_batch_paid { get; set; }
    }
}
