namespace MsProductIntegration.Contracts.UseCases
{
    public interface IProcessProductQueueMessageUseCase
    {
        Task Process(string message);
    }
}
