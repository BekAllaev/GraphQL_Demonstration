using GraphQL_Demonstration.Data;
using GraphQL_Demonstration.Model;
using GraphQL_Demonstration.StatisticalObjects;
using GraphQL_Demonstration.Types;
using GraphQL;
using GraphQL.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_Demonstration
{
    public class AppQuery : ObjectGraphType
    {
        private readonly AppDbContext _context;
        public AppQuery(AppDbContext context)
        {
            _context = context;
            Name = "Query";

            Field<ListGraphType<CategoryGraphType>>("Categories")
                .Description("List of categories")
                .Resolve(_ => _context.Categories.Include(x => x.Products).ToList());

            Field<ListGraphType<CategoryStatisticalObjectGraphType>>("CategoryStatisticalObject")
                .Description("List of category statistical objects")
                .ResolveAsync(async _ => await GetStatisticalObjects(_context));

            Field<CategoryGraphType>("CategoryById")
                .Description("Returns category by id")
                .Argument<IntGraphType>("id")
                .ResolveAsync(async context =>
                {
                    var categoryId = context.GetArgument<int>("id");
                    var category = await _context.Categories.FindAsync(categoryId);
                    return category;
                });
        }

        private async Task<List<CategoryStatisticalObject>> GetStatisticalObjects(AppDbContext context)
        {
            var categoriesWithProducts = await context.Categories.Include(x => x.Products).ToListAsync();

            var result = categoriesWithProducts.Select(x => new CategoryStatisticalObject()
            {
                Id = x.Id,
                Name = x.Name,
                ProductCount = x.Products.Count,
                CategoryProductsOverallPrice = x.Products.Sum(x => x.UnitPrice)
            }).ToList();

            return result;
        }
    }
}
