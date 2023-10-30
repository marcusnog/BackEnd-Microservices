using MongoDB.Bson.Serialization.Attributes;

namespace MsProductsSearch.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuBill : Sku
    {
        public string BarCode { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime DueDate { get; set; }
        public string Document { get; set; } // CpfCnpj
    }
}
