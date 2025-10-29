using BugStore.Data;
using BugStore.Repositories.Implementations;
using BugStore.Repositories.Interfaces;
using BugStore.Services.Implementations;
using BugStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(x => x.UseSqlite(connectionString));


// CONCEITO: Registrar dependências no container de DI
// AddScoped = uma instância por requisição HTTP (ideal para banco de dados)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Handlers
builder.Services.AddScoped<BugStore.Handlers.Customers.Handler>();

var app = builder.Build();
app.MapGet("/", () => "Hello World!");
app.Run();
