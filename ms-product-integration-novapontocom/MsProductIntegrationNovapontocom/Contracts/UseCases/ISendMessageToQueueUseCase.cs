namespace MsProductIntegrationNovapontocom.Contracts.UseCases
{
    public interface ISendMessageToQueueUseCase
    {
        Task Queue(object obj);
    }
}
