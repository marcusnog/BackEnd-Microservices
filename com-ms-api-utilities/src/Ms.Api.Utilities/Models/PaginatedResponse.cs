using System.Text.Json.Serialization;

namespace Ms.Api.Utilities.Models
{
    public class PaginatedResponse<T> : DefaultResponse<T>
    {
        [JsonConstructor]
        public PaginatedResponse()
        {
        }
        public PaginatedResponse(bool success = true) : base(success)
        {

        }
        public PaginatedResponse(T data, bool success = true) : base(data, success)
        {

        }
        public PaginatedResponse(Exception ex, string? messageCode = null) : base(ex, messageCode)
        {

        }
        public PaginatedResponse(string? erroValidacao, string? messageCode = null) : base(erroValidacao, messageCode)
        {

        }
        public Metadata? Metadata { get; set; }
    }
    public class Metadata
    {
        public Metadata() { }

        public long Page { get; set; }
        public int PageSize { get; set; }
        public long TotalItems { get; set; }
        public  { get; set; }
    }
}
