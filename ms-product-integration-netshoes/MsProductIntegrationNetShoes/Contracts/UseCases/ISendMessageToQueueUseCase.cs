namespace MsProductIntegrationNetShoes.Contracts.UseCases
{
    public interface ISendMessageToQueueUseCase
    {
        Task Queue(object obj);
    }
}
