namespace MsProductsSearch.Contracts.DTOs
{
    public class GetStoresProduct
    {
        public IEnumerable<GetProductShowCase> lstProducts { get; set; }
        public IEnumerable<StoresProducts> lstStores { get; set; }
    }
}
