# Simple CRUD with GraphQL

Step by step guide how to make CRUD with GraphQL

1. Create `Category` and `Product` models
```
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<Product> Products { get; set; }
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public double UnitPrice { get; set; }
    [JsonIgnore]
    public Category? Category { get; set; }
    public virtual ICollection<OrderDetail> OrderDetails { get; set; }
}
```

2. Create `CategoryGraphType`(this is `GraphQl` wrapper), I don't mutate `Category` so it is okay to have one type, I will use it for read operation.
```
public class CategoryGraphType : ObjectGraphType<Category>
{
    public CategoryGraphType() 
    { 
        Name = "Category";
        Field(x => x.Id, type: typeof(IntGraphType)).Description("Category Id");
        Field(x => x.Name).Description("Category's Name");
        Field(x => x.Description).Description("Category's Description");
        Field<ListGraphType<ProductReadGraphType>>("products")
            .Description("Products in this category")
            .Resolve(x => x.Source.Products);
    }
}
```

Wrapper for `Product` entity. 
> Even though I don't do read operations with `Product`, `Product` is used in `Category` entity (Category has list of product). There we need to use GraphQl type which is `ProductReadGraphType`
```
public class ProductReadGraphType : ObjectGraphType<Product>
{
    public ProductReadGraphType()
    {
        Name = "Product";
        Field(x => x.Id, type: typeof(IntGraphType)).Description("Product Id");
        Field(x => x.Name).Description("Product Name");
        Field(x => x.UnitPrice).Description("Product Price");
    }
}
```

One wrapper for write operation
```
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
```

One wrapper for update operation
```
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
```
> The difference between `ProductUpdateGraphType` and `ProdcutCreateGraphType` is that `ProductUpdateGraphType` allow me to leave some of the field empty when I update product. This is possible because I don't use lambda functions. When you use lambda function, field will behave the same way as property of the entity. That means if some field is nullable, in GraphQl wrapper field also will be nullable. 

3. Then we create only one class that will keep all read(or queries) operations - `AppQuery`
```
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
```
> Here we create one "endpoint" that returns list of category and another "endpoint" return one category by ID

4. We create only one class for write(mutations) operations - `AppMutation`
```
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
```

6. `AppSchema` for registering `AppQuery` and `AppMutation`
```
public class AppSchema : Schema
{
    public AppSchema(IServiceProvider serviceProvider)
        :base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<AppQuery>();
        Mutation = serviceProvider.GetRequiredService<AppMutation>();
    }
}
```

7. In `Program` we register types like this:
```
using GraphQL;

namespace GraphQL_Demonstration
{
    public class Program
    {
        private const string InMemoryDbConnectionStringName = "InMemory";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ...

            builder.Services.AddTransient<AppQuery>();
            builder.Services.AddTransient<AppMutation>();
            builder.Services.AddTransient<AppSchema>();
            builder.Services.AddGraphQL(x => x.AddGraphTypes()
                .AddGraphTypes(typeof(AppSchema).Assembly)
                .AddSystemTextJson()
                .AddErrorInfoProvider(opt =>
                {
                    opt.ExposeExceptionDetails = true;
                }));

            var app = builder.Build();

            ...

            app.UseGraphQL<AppSchema>();

            // This line adds GraphQl UI client for testing
            app.UseGraphQLGraphiQL(options: new GraphQL.Server.Ui.GraphiQL.GraphiQLOptions());

            ...
        }
    }
}
```