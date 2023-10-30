using System.ComponentModel.DataAnnotations;

namespace Ms.Api.Utilities.DTO.Request
{
    public class BarcodeValidationRequest
    {
        public string Barcode { get; set; }
    }

    public class BilletFilterRequest
    {
        public string? BilletId { get; set; }
    }

    public class BilletRequest
    {
        public string? BilletId { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string CampaignId { get; set; }
        public decimal BilletValue { get; set; }
        public decimal BilletFeeValue { get; set; }
        public decimal BilletPointsValue { get; set; }
        public BilletDetailsRequest BilletDetails { get; set; }
        public BilletPaymentRequest BilletPaymentInfos { get; set; }
        public string token { get; set; }
        public string environment { get; set; }
        public string campaign { get; set; }
    }

    public class BilletDetailsRequest
    {
        public string? BankCode { get; set; }
        public string BankName { get; set; }
        public string Barcode { get; set; }
        public string DigitableLine { get; set; }
        public string? DueDate { get; set; }
        public decimal? Value { get; set; }
        public string Type { get; set; }
    }

    public class BilletPaymentRequest
    {
        public string? RecipientDocument { get; set; }
        public string? RecipientName { get; set; }
        public string? PayerDocument { get; set; }
        public string? PayerName { get; set; }
        public string? DueDate { get; set; }
        public string? LimitDate { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public decimal? FineValue { get; set; }
        public decimal? InterestValue { get; set; }
        public decimal OriginalValue { get; set; }
        public decimal TotalUpdatedValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalAdditionalValue { get; set; }
    }

    //public class CampaignUserRequest
    //{
    //    public string token { get; set; }
    //    public string environment { get; set; }
    //    public string campaign { get; set; }
    //    public decimal? points { get; set; }
    //    public string? releaseCode { get; set; }
    //    public string? orderNumber { get; set; }
    //    public string? requestNumber { get; set; }
    //}

    public class TransfeeraAuthRequest
    {
        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string grant_type { get; set; }
    }

    public class TransfeeraCreateBilletBatchRequest
    {
        public string name { get; set; }
        public List<TransfeeraCreateBilletRequest> billets { get; set; }
        public string type { get; set; }
    }

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
