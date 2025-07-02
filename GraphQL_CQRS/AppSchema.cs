using GraphQL.Types;

namespace GraphQL_CQRS
{
    public class AppSchema : Schema
    {
        public AppSchema(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
            RegisterTypeMapping(typeof(int), typeof(IntGraphType));

            Query = serviceProvider.GetRequiredService<AppQuery>();
            Mutation = serviceProvider.GetRequiredService<AppMutation>();
        }
    }
}
