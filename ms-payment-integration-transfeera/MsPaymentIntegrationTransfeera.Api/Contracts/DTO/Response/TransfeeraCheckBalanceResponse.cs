using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraCheckBalanceResponse
    {
        public decimal value { get; set; }
        public decimal waiting_value { get; set; }
    }
}
