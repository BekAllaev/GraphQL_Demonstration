using GraphQL_Demonstration.Model;
using GraphQL.Types;

namespace GraphQL_Demonstration.Types
{
    public class ProductUpdateGraphType : InputObjectGraphType<Product>
    {
        public ProductUpdateGraphType()
        {
            Name = "ProductUpdate";

            Field(x => x.Id, type: typeof(IntGraphType)).Description("Product Id");
            Field("Name", typeof(StringGraphType)).Description("Product Name");
            Field("UnitPrice", typeof(FloatGraphType)).Description("Product Price");
            Field("CategoryId", typeof(IntGraphType)).Description("Category id");                
        }
    }
}
