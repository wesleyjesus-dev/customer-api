using Customer.API.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.API.IntegrationTests.Controllers;

public class CustomerApiApplication
{
    public HttpClient HttpClient;
    
    public CustomerApiApplication()
    {
        var apiApplication = new ApiApplication();
        HttpClient = apiApplication.CreateClient();
        CreateDataInDatabase(apiApplication);
    }

    private static void CreateDataInDatabase(ApiApplication application)
    {
        using var scope = application.Services.CreateScope();
        var provider = scope.ServiceProvider;
        var context = provider.GetService<CustomerDbContext>();
        if (context == null) throw new ArgumentException("context is null");
        
        var customers = new List<Domain.Customer>()
        {
            new(){ Id = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6"), Name = "Batman", Age = 29 },
            new(){ Id = Guid.Parse("5a23fb2d-3f7d-4df6-b096-42574f7b8be6"), Name = "Spider-Man", Age = 29 },
            new(){ Id = Guid.Parse("517c202d-3461-444b-b8a8-7991a90495a7"), Name = "Wolverine", Age = 29 },
            new(){ Id = Guid.Parse("425285b4-dc98-4670-a747-e8d86d6d2267"), Name = "Wolverine", Age = 29 },
        };
        
        context.Customers.AddRange(customers);
        context.SaveChanges();
    }
}