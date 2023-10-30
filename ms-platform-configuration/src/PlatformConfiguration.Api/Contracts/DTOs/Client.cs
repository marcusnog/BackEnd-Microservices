using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlatformConfiguration.Api.Contracts.DTOs
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public Document[] Documents { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
    }
}
