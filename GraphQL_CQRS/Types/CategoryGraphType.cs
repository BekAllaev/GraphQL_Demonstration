using BogusWithInMemoryDb.Model;
using GraphQL.Types;

namespace BogusWithInMemoryDb.Types
{
    public class CategoryGraphType : ObjectGraphType<Category>
    {
        public CategoryGraphType() 
        { 
            Name = "Category";
            Field(x => x.Id, type: typeof(IdGraphType)).Description("Category Id");
            Field(x => x.Name).Description("Category's Name");
            Field(x => x.Description).Description("Category's Description");

            Field<ListGraphType<ProductGraphType>>("products")
                .Description("Products in this category")
                .Resolve(x => x.Source.Products);
        }
    }
}
