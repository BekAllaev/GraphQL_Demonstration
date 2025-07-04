using BogusWithInMemoryDb.Model;
using GraphQL.Types;

namespace GraphQL_CQRS.Types
{
    public class ProductCreateGraphType : InputObjectGraphType<Product>
    {
        public ProductCreateGraphType()
        {
            Name = "ProductCreate";

            Field(x => x.Name).Description("Product Name");
            Field(x => x.UnitPrice).Description("Product Price");
            Field(x => x.CategoryId).Description("Category id");
        }
    }
}
