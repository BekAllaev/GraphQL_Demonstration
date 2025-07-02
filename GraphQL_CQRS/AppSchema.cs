using GraphQL.Types;

namespace GraphQL_CQRS
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<AppQuery>();
            Mutation = serviceProvider.GetRequiredService<AppMutation>();
        }
    }
}
