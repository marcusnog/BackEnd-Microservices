using MsProductIntegrationNetShoes.Contracts.DTOs;
using NetshoesService;

namespace MsProductIntegrationNetShoes.Contracts.Service
{
    public interface IIntegrationNetshoesService
    {
        Task<IEnumerable<CategoryPartner>> GetCategories();
        Task<IEnumerable<ProdutoRplEN>> GetProducts();
        Task<IEnumerable<string>> GetAvailabilities();
        Task<IEnumerable<PrecoRplEN>> GetProductsPrice();
        void ConfirmReceiptPrices(ConfirmarRecebimentoPrecoRequest request);
        void ConfirmReceiptProducts(ConfirmarRecebimentoProdutoRequest request);
    }
}
