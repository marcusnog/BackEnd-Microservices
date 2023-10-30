using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace User.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class UserParticipant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string? Nickname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string IdProfile { get; set; }
        public string IdSystem { get; set; }
        public string? Cpf { get; set; }

        public Profile? Profile { get; set; }
        public System? System { get; set; }

        public double CreationDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreationUserId { get; set; }
        public double? DeletionDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeletionUserId { get; set; }
        public bool Active { get; set; }
        public int UserType { get; set; }
        public string CampaignId { get; set; }
        [BsonIgnore]
        public decimal? saldo { get; set; }
    }

    public class UpdateUserParticipant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string? Nickname { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
