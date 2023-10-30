namespace Platform.Events.Core.Contracts.Product
{
    public class CreateSkuBillEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "CreateSkuBill/v1";
        public string BarCode { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DueDate { get; set; }
        public string Document { get; set; }
    }
}
