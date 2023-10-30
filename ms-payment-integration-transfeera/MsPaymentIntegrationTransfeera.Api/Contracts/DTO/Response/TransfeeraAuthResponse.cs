using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraAuthResponse
    {
        public string access_token { get; set; }
        public int? expires_in { get; set; }
    }

    public class TransfeeraAuthErrorResponse
    {
        public int? statusCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }
}
