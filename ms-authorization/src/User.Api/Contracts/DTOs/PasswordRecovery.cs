using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    public class PasswordRecovery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }

        public double ExpiresIn { get; set; }
        public string Code { get; set; }
        public string Ip { get; set; }
        public bool Active { get;set; }
    }
}
