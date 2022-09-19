using Customer.API.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Customer.API.IntegrationTests;

public sealed class ApiApplication : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        var root = new InMemoryDatabaseRoot();

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<CustomerDbContext>));
            services.AddDbContext<CustomerDbContext>(options =>
            {
                options.UseInMemoryDatabase("CustomerDB", root);
            });
        });
        return base.CreateHost(builder);
    }
}