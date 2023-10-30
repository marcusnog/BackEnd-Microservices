using System.Text;

namespace Ms.Api.Utilities.Extensions
{
    public static class ExceptionExtensions
    {
        public static string? GetFullException(this Exception ex)
        {
            if(ex == null) return null;

            StringBuilder stringBuilder = new ();
            Exception? exception = ex;
            do
            {
                stringBuilder.AppendLine(exception.Message);
                exception = exception?.InnerException;
            }
            while(exception != null);

            return stringBuilder.ToString();

        }
    }
}
