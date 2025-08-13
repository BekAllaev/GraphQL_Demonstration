using GraphQL_Demonstration.Data;
using GraphQL_Demonstration.Model;
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
    }
}
