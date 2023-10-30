namespace Ms.Api.Utilities.Models
{
    public class QueryPage<T>
    {
        public long Page { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public T Items { get; set; }
    }
}
