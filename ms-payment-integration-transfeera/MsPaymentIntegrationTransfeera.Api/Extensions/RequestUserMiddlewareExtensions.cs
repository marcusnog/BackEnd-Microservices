using MsPaymentIntegrationTransfeera.Middlewares;

namespace MsPaymentIntegrationTransfeera.Extensions
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
