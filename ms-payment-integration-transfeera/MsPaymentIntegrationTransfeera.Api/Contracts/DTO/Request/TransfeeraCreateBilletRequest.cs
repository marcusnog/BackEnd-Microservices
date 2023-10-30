using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class TransfeeraCreateBilletRequest
    {
        /// <summary>
        /// Valor do Billet
        /// </summary>
        public decimal value { get; set; }
        /// <summary>
        /// Data de pagamento(YYYY-MM-DD)
        /// </summary>
        [Required]
        public string payment_date { get; set; }
        /// <summary>
        /// Código de barras ou linha digitável
        /// </summary>
        [Required]
        public string barcode { get; set; }
        /// <summary>
        /// Descrição do Billet
        /// </summary>
        [Required]
        public string description { get; set; }
        /// <summary>
        /// ID de integração
        /// </summary>
        public string integration_id { get; set; }
    }
}
