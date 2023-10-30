using Ms.Api.Utilities.Contracts.DTOs;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Enum;

namespace MsPointsPurchaseApi.Contracts.DTOs
{
    public class CreatePointsPurchaseRequest
    {
        public string? UserId { get; set; }
        public string OrderCode { get; set; }
        public string? PointsId { get; set; }
        public string? AccountId { get; set; } 
        public decimal PointsValue { get; set; }
        public Request_OrderPaymentData PaymentData { get; set; }
        public string PaymentMethodId { get; set; }
        public OrderRecipient Recipient { get; set; }
        public string Status { get; set; }
        public Enums.TypeRequest Type { get; set; }
    }
}
