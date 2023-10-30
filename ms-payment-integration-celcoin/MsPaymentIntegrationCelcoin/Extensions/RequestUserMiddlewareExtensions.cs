using MsPaymentIntegrationCelcoin.Middlewares;

namespace MsPaymentIntegrationCelcoin.Extensions
{
    public static class RequestUserMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestUserInfo(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestUserMiddleware>();
        }
    }
}
