using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Auth.Api.Contracts.DTOs
{
    public class IdentityClient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public bool AllowAccessTokensViaBrowser { get; set; }
        public string ClientId { get; set; }
        public string[] ClientSecrets { get; set; }
        public int AccessTokenType { get; set; }
        public string[] AllowedGrantTypes { get; set; }
        public string[] AllowedScopes { get; set; }
    }
}
