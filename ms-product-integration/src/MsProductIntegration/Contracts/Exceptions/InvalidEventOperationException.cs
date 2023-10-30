using System.Runtime.Serialization;

namespace MsProductIntegration.Contracts.Exceptions
{
    public class InvalidEventOperationException : Exception
    {
        public InvalidEventOperationException()
        {
        }

        public InvalidEventOperationException(string? message) : base(message)
        {
        }

        public InvalidEventOperationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidEventOperationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
