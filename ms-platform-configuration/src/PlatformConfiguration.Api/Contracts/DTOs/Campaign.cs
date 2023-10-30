using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PlatformConfiguration.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class Campaign
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string ClientId { get; set; }   
        public string Name { get; set; }
        public decimal CoinConversionFactor { get; set; }
        public byte[] CampaignLogo { get; set; }
        public byte[] CampaignBanner { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondaryColor { get; set; }
        public bool AllowCardPayment { get; set; }
        public decimal AllowedCardPaymentPercentage { get; set; }
        public bool AllowPointAmountSelection { get; set; }
        public double CreationDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CreationUserId { get; set; }
        public double? DeletionDate { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeletionUserId { get; set; }
        public bool Active { get; set; }
        public string[]? Stores { get; set; }

    }
}
