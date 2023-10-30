using System.ComponentModel.DataAnnotations;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request
{
    public class TransfeeraGetBilletsRequest
    {
        /// <summary>
        /// Data inicial (YYYY-MM-DD)
        /// </summary>
        [Required]
        public string initialDate { get; set; }
        /// <summary>
        /// Data final (YYYY-MM-DD)
        /// </summary>
        [Required]
        public string endDate { get; set; }
        /// <summary>
        /// Paginação, começando em 0 (zero)
        /// </summary>
        [Required]
        public int page { get; set; }
        /// <summary>
        /// Código de barras ou linha digitável
        /// </summary>
        public string barcode { get; set; }
        /// <summary>
        /// CRIADA / AGENDADO / PAGO / FALHA / DEVOLVIDO
        /// </summary>
        public string status { get; set; }
    }
}
