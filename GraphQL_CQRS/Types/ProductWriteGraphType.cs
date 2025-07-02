using BogusWithInMemoryDb.Model;
using GraphQL.Types;

namespace GraphQL_CQRS.Types
{
    public class ProductWriteGraphType : InputObjectGraphType<Product>
    {
        public ProductWriteGraphType()
        {
            Name = "ProductInput";

            Field(x => x.Id, type: typeof(IntGraphType)).Description("Product Id");
            Field(x => x.Name).Description("Product Name");
            Field(x => x.UnitPrice).Description("Product Price");
            Field(x => x.CategoryId).Description("Category id");
        }
    }
}
