using MongoDB.Bson;
using MongoDB.Driver;

namespace Catalog.Api.Extensions
{
    public class MongoExtension
    {
        public static FilterDefinition<T> GenerateFilter<T>(object obj)
        {
            FilterDefinition<T> filter = null;
            List<FilterDefinition<T>> filters = new();
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.Name.EndsWith("Id"))
                    filters.Add(Builders<T>.Filter.Eq(property.Name, ObjectId.Parse(property.GetValue(obj).ToString())));
                else
                    filters.Add(Builders<T>.Filter.Eq(property.Name, property.GetValue(obj)));
            }
            foreach (var f in filters)
                filter = (filter == null) ? f : filter & f;

            return filter;
        }
    }
}
