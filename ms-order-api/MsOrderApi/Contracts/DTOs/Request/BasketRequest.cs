
namespace MsOrderApi.Contracts.DTOs.Request
{
    public class BasketRequest
    {
        public string IdUser { get; set; }
        public ProductBasket? Product { get; set; }
    }

    //public class StoreBasket
    //{
    //    public string StoreName { get; set; }
    //    public decimal TotalValueProducts 
    //    { 
    //        get 
    //        { 
    //            decimal total = 0;
    //            foreach (var item in Products)
    //                total += item.ValueInReals * item.Quantity;

    //            return total;
    //        } 
    //    }
    //    public List<ProductBasket> Products { get; set; }
    //}

    public class ProductBasket
    {
        public string IdProductBasket { get; set; }
        public string IdProduct { get; set; }
        public string? ImgProduct { get; set; }
        public string NameProduct { get; set; }
        public int Quantity { get; set; }
        public string StoreName { get; set; }
        public decimal ValueInPoints { get; set; }
        public decimal ValueInReals { get; set; }
    }


    public class StoreBasket
    {
        public string StoreName { get; set; }
        public decimal TotalValueProducts
        {
            get
            {
                decimal total = 0;
                foreach (var item in Products)
                    total += item.ValueInReals * item.Quantity;

                return total;
            }
        }
        public List<ProductBasket> Products { get; set; }
    }

}
