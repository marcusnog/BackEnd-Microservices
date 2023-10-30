using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Campaign.Connector.Contracts.DTO
{
    public class CampaignConnector
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Environment { get; set; }
        public string Campaign { get; set; }
        public string ServiceBusUrl { get; set; }
        public string UserInfoEndpoint { get; set; }
        public string UserGetPointsEndpoint { get; set; }
        public string UserBookPointsEndpoint { get; set; }
        public string UserDebitPointsEndpoint { get; set; }
        public string UserReleasePointsEndpoint { get; set; }
        public string UserReversePointsEndpoint { get; set; }

    }
}
