using BogusWithInMemoryDb.Data;
using BogusWithInMemoryDb.Model;
using BogusWithInMemoryDb.Types;
using GraphQL;
using GraphQL.Types;
using GraphQL_CQRS.Types;

namespace GraphQL_CQRS.Mutations
{
    public class ProductMutation : ObjectGraphType
    {
        public ProductMutation(AppDbContext dbContext)
        {
            Field<ProductReadGraphType>("AddProduct")
                .Argument<NonNullGraphType<ProductWriteGraphType>>("product")
                .ResolveAsync(async context =>
                {
                    var product = context.GetArgument<Product>("product");
                    var productEntity = dbContext.Products.Add(product);
                    await dbContext.SaveChangesAsync();

                    return productEntity.Entity;
                });
        }
    }
}
