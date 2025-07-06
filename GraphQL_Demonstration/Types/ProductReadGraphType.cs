using GraphQL_Demonstration.Model;
using GraphQL.Types;

namespace GraphQL_Demonstration.Types
{
    public class ProductReadGraphType : ObjectGraphType<Product>
    {
        public ProductReadGraphType()
        {
            Name = "Product";

            Field(x => x.Id, type: typeof(IntGraphType)).Description("Product Id");
            Field(x => x.Name).Description("Product Name");
            Field(x => x.UnitPrice).Description("Product Price");
        }
    }
}
