using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    public class ProfileRole
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("IdProfile")]
        public int IdProfile { get; set; }

        [BsonElement("IdRole")]
        public int IdRole { get; set; }
    }
}
