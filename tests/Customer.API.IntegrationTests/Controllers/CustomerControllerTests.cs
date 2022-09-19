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
        Assert.Equal(3,customers?.Count);
    }

    [Fact(DisplayName = "Should return a customer")]
    public async Task Get_GetCustomer_ShouldRetunrOnlyCustomer()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var response = await _customerApiApplication.HttpClient.GetAsync($"/customer/{id}");
        var customer = JsonSerializer.Deserialize<Domain.Customer>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("Batman", customer?.Name);
        Assert.Equal(29, customer?.Age);
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

    [Fact(DisplayName = "Delete a customer")]
    public async Task Delete_DeleteCustomer()
    {
        var id = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var response = await _customerApiApplication.HttpClient.DeleteAsync($"/customer/{id}");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}