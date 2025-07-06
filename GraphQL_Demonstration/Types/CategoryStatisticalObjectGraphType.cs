using GraphQL_Demonstration.StatisticalObjects;
using GraphQL.Types;

namespace GraphQL_Demonstration.Types
{
    public class CategoryStatisticalObjectGraphType : ObjectGraphType<CategoryStatisticalObject>
    {
        public CategoryStatisticalObjectGraphType()
        {
            Name = "CategoryStatisticalObject";

            Field(x => x.Id, type: typeof(IdGraphType))
                .Description("Category Id");

            Field(x => x.Name)
                .Description("Category Name");

            Field(x => x.ProductCount)
                .Description("Number of products in the category");

            Field(x => x.CategoryProductsOverallPrice)
                .Description("Sum of product prices in the category");
        }
    }
}
