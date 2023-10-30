


using Ms.Api.Utilities.Contracts.DTOs;
using MsOrderApi.Contracts.DTOs.Request;

namespace MsOrderApi.Contracts.Repository
{
    public interface IOrderRepository
    {
        //Task<List<StoreOrder>>  InsertItemOrder(OrderRequest request);
        //Task<List<StoreBasket>> GetOrder(OrderRequest request);
        //Task<List<StoreOrder>> DeleteItemOrder(OrderRequest request);
        //Task DeleteOrder(OrderRequest request);

        //Task<List<OrderResponse>> InsertItemOrder(OrderRequest request);

        Task<List<StoreBasket>> GetBasket(BasketRequest request);

        Task Create(Order oOrder);

        Task<List<Order>> List(OrderFilter oPromotion);

        Task<bool> Update(Order obj);

    }
}
