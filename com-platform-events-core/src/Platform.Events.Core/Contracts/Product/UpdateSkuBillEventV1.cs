namespace Platform.Events.Core.Contracts.Product
{
    public class UpdateSkuBillEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "UpdateSkuBill/v1";
        public string BarCode { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DueDate { get; set; }
        public string Document { get; set; }
    }
}
