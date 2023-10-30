namespace Platform.Events.Core.Contracts.Product
{
    public class BaseSku
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string SkuId { get; set; }
        public string Code { get; set; }
        public bool Active { get; set; }
        public List<Attribute> Attributes { get; set; }
        public string[] Tags { get; set; }
        public bool Availability { get; set; }
        public string Model { get; set; }
    }

    public class Attribute
    {
        public string Type { get; set; }

        public string Description { get; set; }

        public string Value { get; set; }
    }
}
