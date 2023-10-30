using Catalog.Api.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Contracts.DTOs
{
    public class MainCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public Category[]? Children { get; set; }
    }
}
