using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User.Api.Contracts.DTOs.Response
{
    public class UserParticipantResponse
    {
        public string? Id { get; set; }
        public string? Nickname { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? Cpf { get; set; }
        public List<Address> Addresses { get; set; }
    }


    [BsonIgnoreExtraElements]
    public class Address
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string Street { get; set; }
        public string Complement { get; set; }
        public string Number { get; set; }
        public string Neighborhood { get; set; }
        public string ZipCode { get; set; }
        public bool Active { get; set; }
        public bool? Default { get; set; }
    }

    public class AddressFilter
    {
        public string? Id { get; set; }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
    }
}
