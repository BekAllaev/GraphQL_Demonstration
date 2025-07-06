using GraphQL.Types;

namespace GraphQL_Demonstration
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
