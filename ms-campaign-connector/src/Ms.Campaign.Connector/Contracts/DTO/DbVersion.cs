using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Campaign.Connector.Contracts.DTO
{
    public class DbVersion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public double CreatedAt { get; set; }
    }
}
