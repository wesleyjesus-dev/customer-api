using Customer.API.Repositories;
using Customer.API.Repositories.Contracts;
using Customer.API.Services;
using Customer.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<ICustomerRepository>(provider =>
    new CachedCustomerRepository(
        provider.GetRequiredService<CustomerRepository>(),
        provider.GetRequiredService<IMemoryCache>()));

builder.Services.AddDbContext<CustomerDbContext>(optionsAction: optionsBuilder =>
{
    optionsBuilder.UseInMemoryDatabase("CustomerDB");
});

builder.Services.AddScoped<ICustomerService, CustomerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program {}