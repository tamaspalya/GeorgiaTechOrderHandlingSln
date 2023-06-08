using System.Reflection;
using Webshop.Data.Persistence;
using Webshop.Order.Application.Contracts.Persistence;
using Webshop.Application;
using Webshop.Order.Persistence;
using MediatR;
using Webshop.Application.Contracts;
using Webshop.Order.Application;
using Webshop.Service.CustomerClient;
using Webshop.Service;
using Webshop.Service.CatalogClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();

var options = new CustomerApiClientOptions();
builder.Configuration.GetSection("CustomerApiClient").Bind(options);
builder.Services.AddSingleton(options);

var catalogOptions = new CatalogApiClientOptions();
builder.Configuration.GetSection("CatalogApiClient").Bind(catalogOptions);
builder.Services.AddSingleton(catalogOptions);


// Register HttpClient service
builder.Services.AddHttpClient<IHttpClientService, HttpClientService>();

// Register API client service
builder.Services.AddScoped<ICustomerApiClient, CustomerApiClient>();
builder.Services.AddScoped<ICatalogApiClient, CatalogApiClient>();


builder.Services.AddScoped<DataContext, DataContext>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<IDispatcher>(sp => new Dispatcher(sp.GetService<IMediator>()));

builder.Services.AddOrderApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
