using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductIntegration.Contracts.DTOs
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public Category[]? Children  { get; set; }
    }
}
