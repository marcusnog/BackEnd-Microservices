using Platform.Events.Core.Contracts;

namespace MsProductIntegration.Contracts.DTOs
{
    public class EventBaseDTO : EventBase
    {
        public string ApiVersion {get; set;}
    }
}
