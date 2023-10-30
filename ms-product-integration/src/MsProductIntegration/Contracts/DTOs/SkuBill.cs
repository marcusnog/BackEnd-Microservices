using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductIntegration.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuBill : Sku
    {
        public string BarCode { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal TotalPrice { get; set; }

        public DateTime DueDate { get; set; }
        public string Document { get; set; } // CpfCnpj
    }
}
