using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.ValidateCIP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class ValidateCIPResponse
    {
        public bool isSuccess => barcode_details != null;
        public string status { get; set; }
        public string message { get; set; }
        public BarcodeDetails barcode_details { get; set; }
        public PaymentInfo payment_info { get; set; }
    }
}
