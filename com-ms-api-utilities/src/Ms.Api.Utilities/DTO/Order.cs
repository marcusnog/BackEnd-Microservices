using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Api.Utilities.Contracts.DTOs
{
    public class OrderFilter
    {
        public string CPFCNPJ { get; set; }
    }

    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string OrderCode { get; set; }
        public string CampaignId { get; set; }
        public decimal ConversationFactor { get; set; }
        public bool Active { get; set; }
        public double DateCreation { get; set; }
        public double DateDisabled { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalOrderAmountPoints { get; set; }

        public decimal TotalOrderAmountCurrency { get; set; }
        public string PaymentMethodId { get; set; }

        public string Status { get; set; }



        //public double StartDate { get; set; }
        //public double EndDate { get; set; }
        //public string WebBanner { get; set; }
        //public string MobileBanner { get; set; }
        //public double ModificationDate { get; set; }
        //public double? DeletionDate { get; set; }
        
        public IEnumerable<OrderStore> Shops { get; set; }
        //public IEnumerable<Campaign> Campaigns { get; set; }

        public OrderRecipient Recipient { get; set; }

        public OrderDeliveryAddress DeliveryAddress { get; set; }
    }

    //public class OrderProduct
    //{
    //    [BsonRepresentation(BsonType.ObjectId)]
    //    public string SkuId { get; set; }

    //    public bool Active { get; set; }

    //    public decimal ValueOf { get; set; }
    //    public decimal ValueFor { get; set; }
    //}

    //public class Campaign
    //{
    //    public string CampaignId { get; set; }

    //    public bool Active { get; set; }
    //}

    public class OrderStore
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderStoreId { get; set; }
        public string OrderCode { get; set; }
        public decimal ShippingValue { get; set; }
        public bool Confirmed { get; set; }
        public string StoreId { get; set; }
        public decimal TotalValueProducts { get; set; }
        public double DateDeliveryForecast { get; set; }

        public IEnumerable<OrderProduct> Products { get; set; }
    }

    public class OrderProduct
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderProductId { get; set; }
        public string CodeSku { get; set; }
        public string CodeProduct { get; set; }

        public int Quantity { get; set; }
        public double DateDeliveryForecast { get; set; }


        public decimal ValueUnitary { get; set; }
        public decimal ValueUnitaryPoints { get; set; }

        public decimal TotalOrderAmountCurrency { get; set; }

        public string StatusDelivery { get; set; }

        public double DateBilling { get; set; }
    }

    public class OrderRecipient
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderRecipientId { get; set; }
        public string CPFCNPJ { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string StateRegistration { get; set; }
        public bool Active { get; set; }
        public double DateCreation { get; set; }
        public double DateDisabled { get; set; }

    }

    public class OrderDeliveryAddress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string OrderDeliveryAddressId { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Complement { get; set; }
        public string State { get; set; }
        public string PublicPlace { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public string Telephone { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public bool Active { get; set; }
        public double DateCreation { get; set; }
        public double DateDisabled { get; set; }

    }

    //[InformacaoComplementoCartaoId] [int] NULL,
}
