using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class ReversePointsRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string DebitMovimentCode { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
