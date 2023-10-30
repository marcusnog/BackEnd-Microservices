namespace MsProductDetailsApi.Extensions
{
    public class ConverInPoints
    {
        public static decimal ConvertInPonts(decimal value, decimal fatorConversao)
        {
            return Math.Ceiling(value / fatorConversao * 100) / 100;
        }
    }
}
