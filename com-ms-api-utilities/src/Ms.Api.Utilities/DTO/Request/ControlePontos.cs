using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.DTO.Request
{
    public class CreateReserveMovimentRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public decimal Value { get; set; }
    }

    public class CreditPointsRequest
    {
        public string AccountId { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }

    public class ReleasePointsRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string ReleaseCode { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }

    public class ReversePointsRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string DebitMovimentCode { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }

    public class EffectDebitRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReleaseCode { get; set; }
        public string OrderNumber { get; set; }
    }






    public class CampaignUserRequest
    {
        public string token { get; set; }
        public string environment { get; set; }
        public string campaign { get; set; }
        public decimal? points { get; set; }
        public string? releaseCode { get; set; }
        public string? orderNumber { get; set; }
        public string? requestNumber { get; set; }
    }
}
