using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    public class System
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Url { get; set; }
        public string RecoveryUrl { get; set; }
        public string Logo { get; set; }
        public int RecoveryExpiration { get; set; }
        public string ContactEmail { get; set; }
    }

    public class SystemFilter
    {
        public string Id { get; set; }
    }
}
