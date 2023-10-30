using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MsPointsPurchaseApi.Contracts.DTOs
{
    public class Points
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MaxPointsValue { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MinPointsValue { get; set; }
        public double Fee { get; set; }
        public double FeeValue { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
    }
}
