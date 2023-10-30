namespace Ms.Api.Utilities.Extensions
{
    public static class NumberExtension
    {
        public static string ToHex(this int i, int length = 24)
        {
            return i.ToString($"X{length}");
        }
    }
}