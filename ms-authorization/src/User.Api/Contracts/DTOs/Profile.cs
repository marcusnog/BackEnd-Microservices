using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    public class Profile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("IdSystem")]
        public string IdSystem { get; set; }
        public ProfileAbility[] Abilities { get; set; }
    }
    public class ProfileAbility
    {
        public string action { get; set; }
        public string subject { get; set; }
    }
    public class ProfileFilter
    {
        public string Id { get; set; }
    }
}
