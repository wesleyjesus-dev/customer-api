using System.Net;
using System.Text;
using System.Text.Json;

namespace Customer.API.IntegrationTests.Controllers;

public sealed class CustomerControllerTests : IClassFixture<CustomerApiApplication>
{
    private readonly CustomerApiApplication _customerApiApplication;
    
    public CustomerControllerTests()
    {
        _customerApiApplication = new();
    }
    
    [Fact(DisplayName = "Should return a customer list")]
    public async Task Get_GetCustomer_ReturnAllCustomer()
    {
        var response = await _customerApiApplication.HttpClient.GetAsync("/customer");
        var customers = JsonSerializer.Deserialize<List<Domain.Customer>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(4,customers?.Count);
    }
    
    [Fact(DisplayName = "Should create a new customer")]
    public async Task Post_Create_CreateCustomer()
    {
        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new Domain.Customer()
        {
            Id = id,
            Age = 30,
            Name = "Superman"
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _customerApiApplication.HttpClient.PostAsync("/customer", content);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
    
    [Theory]
    [InlineData("5a23fb2d-3f7d-4df6-b096-42574f7b8be6")]
    [InlineData("517c202d-3461-444b-b8a8-7991a90495a7")]
    [InlineData("425285b4-dc98-4670-a747-e8d86d6d2267")]
    public async Task Get_GetCustomer_ShouldRetunrOnlyCustomer(Guid id)
    {
        var response = await _customerApiApplication.HttpClient.GetAsync($"/customer/{id}");
        var customer = JsonSerializer.Deserialize<Domain.Customer>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }


    [Fact(DisplayName = "Delete a customer")]
    public async Task Delete_DeleteCustomer()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var response = await _customerApiApplication.HttpClient.DeleteAsync($"/customer/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact(DisplayName = "Should update the name customer")]
    public async Task Update_ShouldUpdateNameCustomer()
    {
        var id = Guid.Parse("517c202d-3461-444b-b8a8-7991a90495a7");
        var json = JsonSerializer.Serialize(new Domain.Customer()
        {
            Id = id,
            Age = 30,
            Name = "Superman"
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _customerApiApplication.HttpClient.PatchAsync("/customer", content);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}