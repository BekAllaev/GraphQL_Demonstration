using BogusWithInMemoryDb.Queries;
using GraphQL.Types;
using GraphQL_CQRS.Mutations;

namespace GraphQL_CQRS.Schemas
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<CategoryQuery>();
            Mutation = serviceProvider.GetRequiredService<ProductMutation>();
        }
    }
}
