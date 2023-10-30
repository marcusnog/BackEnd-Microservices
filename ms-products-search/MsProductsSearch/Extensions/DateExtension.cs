namespace MsProductsSearch.Extensions
{
    public static class DateExtension
    {
        public static double ToUnixTimestamp(this DateTime date)
        {
            return (date.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds * 1000;
        }
    }
}
