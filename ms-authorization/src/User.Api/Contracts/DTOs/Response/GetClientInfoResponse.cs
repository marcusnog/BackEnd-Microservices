using User.Api.Contracts.DTOs.Request;

namespace User.Api.Contracts.DTOs.Response
{
    public class GetClientInfoResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //public Document[] Documents { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
    }
}
