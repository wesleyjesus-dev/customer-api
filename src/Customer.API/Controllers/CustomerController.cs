using Customer.API.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;
    
    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async ValueTask<IActionResult> Post([FromBody] Domain.Customer customer, CancellationToken cancellationToken)
    {
        var customerCreated = await _customerService.CreateAsync(customer, cancellationToken);
        if (customerCreated == null) return BadRequest("Error when try save new customer");
        return Created($"/customer/{customer.Id}", customer);

    }

    [HttpDelete("{customerId}")]
    public async ValueTask<IActionResult> Delete([FromRoute] Guid customerId, CancellationToken cancellationToken)
    {
        var execution = await _customerService.Remove(customerId, cancellationToken);
        if (execution == null) return NotFound($"Customer with id {customerId.ToString()} not found");
        return Ok($"Customer with id {customerId.ToString()} deleted with success");
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _customerService.GetCustomers(cancellationToken));
    }
    
    [HttpGet("{customerId}")]
    public async ValueTask<IActionResult> GetAll(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomer(customerId, cancellationToken);
        if (customer == null) return NotFound();
        return Ok(customer);
    }
}