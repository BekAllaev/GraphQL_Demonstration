using BogusWithInMemoryDb.Queries;
using GraphQL.Types;

namespace BogusWithInMemoryDb.DemoSchema
{
    public class DemoSchema : Schema
    {
        public DemoSchema(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CategoryQuery>();
        }
    }
}
