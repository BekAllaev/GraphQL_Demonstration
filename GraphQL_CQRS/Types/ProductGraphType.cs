using BogusWithInMemoryDb.Model;
using GraphQL.Types;

namespace BogusWithInMemoryDb.Types
{
    public class ProductGraphType : ObjectGraphType<Product>
    {
        public ProductGraphType()
        {
            Name = "Product";

            Field(x => x.Id, type: typeof(IdGraphType)).Description("Product Id");
            Field(x => x.Name).Description("Product Name");
            Field(x => x.UnitPrice).Description("Product Price");
        }
    }
}
