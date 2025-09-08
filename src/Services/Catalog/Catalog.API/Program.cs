using Catalog.API.Products.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter(null, cfg =>
{
    cfg.WithModules(typeof(CreateProductEndpoint));
});

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.Run();
