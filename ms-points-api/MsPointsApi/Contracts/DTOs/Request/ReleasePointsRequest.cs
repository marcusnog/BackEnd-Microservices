using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class ReleasePointsRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReleaseCode { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
