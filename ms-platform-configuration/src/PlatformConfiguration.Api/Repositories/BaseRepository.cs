using MongoDB.Driver;
using Ms.Api.Utilities.Models;

namespace PlatformConfiguration.Api.Repositories
{
    public abstract class BaseRepository
    {
        protected static async Task<QueryPage<IEnumerable<T>>> GetPagerResultAsync<T>(int page, int pageSize, IMongoCollection<T> collection, FilterDefinition<T>? filter = null)
        {
            // count facet, aggregation stage of count
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<T, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<T>()
                }));

            // data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<T, T>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<T>(pageSize),
                }));

            filter ??= Builders<T>.Filter.Empty;

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<T>();

            return new QueryPage<IEnumerable<T>>
            {
                TotalItems = count,
                PageSize = pageSize,
                Page = page,
                Items = data
            };
        }
    }
}
