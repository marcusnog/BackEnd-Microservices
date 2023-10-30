namespace Catalog.Api.Contracts.DTOs.Response
{
    public class GetCategoriesResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GetCategoriesResponse>? Children { get; set; }

        public static GetCategoriesResponse Get(Category category)
        {
            if (category == null) return null;
            return new GetCategoriesResponse()
            {
                Id = category.Id,
                Name = category.Name,
                Children = category?.Children?.Select(c=> Get(c))

            };
        }
    }
}
