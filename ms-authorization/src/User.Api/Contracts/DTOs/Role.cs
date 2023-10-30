using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    public class Role
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }

    public class RoleFilter
    {
        public string Id { get; set; }
    }
}
