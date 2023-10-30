using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraCreateBilletBatchErrorResponse
    {
        public int statusCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string field { get; set; }
        public string errorCode { get; set; }
    }
}
