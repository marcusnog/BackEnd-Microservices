using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Models;

namespace PlatformConfiguration.Api.Contracts.DTOs
{
    public class Partner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool AcceptCardPayment { get; set; }
        public double CreationDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreationUserId { get; set; }
        public double? DeletionDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeletionUserId { get; set; }
        public bool Active { get; set; }

    }
}
