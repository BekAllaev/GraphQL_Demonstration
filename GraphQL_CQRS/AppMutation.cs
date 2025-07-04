using BogusWithInMemoryDb.Data;
using BogusWithInMemoryDb.Model;
using BogusWithInMemoryDb.Types;
using GraphQL;
using GraphQL.Types;
using GraphQL_CQRS.Types;
using Microsoft.EntityFrameworkCore;

namespace GraphQL_CQRS
{
    public class AppMutation : ObjectGraphType
    {
        public AppMutation(AppDbContext dbContext)
        {
            Field<ProductReadGraphType>("AddProduct")
                .Argument<NonNullGraphType<ProductCreateGraphType>>("product")
                .ResolveAsync(async context =>
                {
                    var product = context.GetArgument<Product>("product");
                    var productEntity = dbContext.Products.Add(product);
                    await dbContext.SaveChangesAsync();

                    return productEntity.Entity;
                });

            Field<ProductReadGraphType>("UpdateProduct")
                .Argument<NonNullGraphType<ProductUpdateGraphType>>("product")
                .ResolveAsync(async context =>
                {
                    var product = context.GetArgument<Product>("product");

                    var updateProduct = dbContext.Products.Update(product);

                    await dbContext.SaveChangesAsync();

                    return updateProduct.Entity;
                });

            Field<ProductReadGraphType>("DeleteProduct")
                .Argument<IntGraphType>("id")
                .ResolveAsync(async context =>
                {
                    var id = context.GetArgument<int>("id");

                    var product = await dbContext.Products.FindAsync(id);

                    if (product is not null)
                    {
                        var deleteProduct = dbContext.Products.Remove(product);
                    }
                    else
                    {
                        throw new ArgumentNullException(nameof(product));
                    }

                    await dbContext.SaveChangesAsync();

                    return product;
                });
        }
    }
}
