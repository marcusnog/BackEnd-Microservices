using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CampaignId { get; set; }
        public string Cpf { get; set; }
        public bool Active { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CreationUserId { get; set; }
        public DateTime? UpdateddAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedUserId { get; set; }
        public DateTime? DeletedAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeletionUserId { get; set; }
        public List<Account_Moviment> AccountMoviment { get; set; }
    }
}
