using Bogus;
using BogusWithInMemoryDb.Data;
using BogusWithInMemoryDb.Model;
using GraphQL;
using GraphQL.Types;
using GraphQL_CQRS;
using GraphQL_CQRS.Model;
using Microsoft.EntityFrameworkCore;

namespace BogusWithInMemoryDb
{
    public class Program
    {
        private const string InMemoryDbConnectionStringName = "InMemory";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString(InMemoryDbConnectionStringName)));

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

            var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            // Make sure the database is created and then seed it.
            context.Database.Migrate();
            context.Database.EnsureCreated();

            SeedData(context);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.UseGraphQL<AppSchema>();
            app.UseGraphQLGraphiQL(options: new GraphQL.Server.Ui.GraphiQL.GraphiQLOptions());

            app.Run();
        }

        static void SeedData(AppDbContext context)
        {
            // ===== CATEGORIES =====
            var categoryId = 1;
            var categories = new Faker<Category>("en")
                .RuleFor(c => c.Id, _ => categoryId++)
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .Generate(10);

            // ===== PRODUCTS =====
            var productId = 1;
            var products = new Faker<Product>("en")
                .RuleFor(p => p.Id, _ => productId++)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.UnitPrice, f => double.Parse(f.Commerce.Price(5, 300)))
                .RuleFor(p => p.CategoryId, f => f.PickRandom(categories).Id)
                .Generate(50);

            // ===== EMPLOYEES =====
            var employeeId = 1;
            var employees = new Faker<Employee>("en")
                .RuleFor(e => e.EmployeeId, _ => employeeId++)
                .RuleFor(e => e.FirstName, f => f.Name.FirstName())
                .RuleFor(e => e.LastName, f => f.Name.LastName())
                .Generate(20);

            // ===== CUSTOMERS =====
            var customerId = 1;
            var customers = new Faker<Customer>("en")
                .RuleFor(c => c.CustomerId, _ => customerId++)
                .RuleFor(c => c.Country, f => f.Address.Country())
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .Generate(100);

            // ===== ORDERS + ORDERDETAILS =====
            var orderId = 1;
            var orders = new List<Order>();
            var orderDetails = new List<OrderDetail>();

            var orderFaker = new Faker<Order>("en")
                .RuleFor(o => o.OrderId, _ => orderId++)
                .RuleFor(o => o.OrderDate, f => f.Date.Recent(90))
                .RuleFor(o => o.CustomerId, f => f.PickRandom(customers).CustomerId)
                .RuleFor(o => o.EmployeeId, f => f.PickRandom(employees).EmployeeId)
                .FinishWith((f, o) =>
                {
                    var lines = f.Random.Int(1, 5);
                    foreach (var prod in f.PickRandom(products, lines))
                    {
                        orderDetails.Add(new OrderDetail
                        {
                            OrderId = o.OrderId,
                            ProductId = prod.Id,
                            Quantity = f.Random.Short(1, 10),
                            UnitPrice = prod.UnitPrice
                        });
                    }
                });

            orders = orderFaker.Generate(400);

            // ===== SAVE =====
            context.AddRange(categories);
            context.AddRange(products);
            context.AddRange(employees);
            context.AddRange(customers);
            context.AddRange(orders);
            context.AddRange(orderDetails);

            context.SaveChanges();
        }
    }
}
