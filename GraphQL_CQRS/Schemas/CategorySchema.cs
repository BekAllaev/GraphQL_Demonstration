using BogusWithInMemoryDb.Queries;
using GraphQL.Types;

namespace BogusWithInMemoryDb.Schemas
{
    public class CategorySchema : Schema
    {
        public CategorySchema(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CategoryQuery>();
        }
    }
}
