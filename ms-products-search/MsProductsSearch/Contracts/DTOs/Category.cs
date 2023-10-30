using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Extensions;

namespace MsProductsSearch.Contracts.DTOs
{
    public class Category
    {
        public Category() { }
        public Category(string name, bool active = true)
        {
            Id = ObjectId.GenerateNewId().ToString();
            Name = name;
            Active = active;
            CreationDate = DateTime.UtcNow.ToUnixTimestamp();
        }
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
