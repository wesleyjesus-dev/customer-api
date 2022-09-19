using Customer.API.Repositories.Contracts;
using Customer.API.Services.Contracts;

namespace Customer.API.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    
    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async ValueTask<int?> Remove(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await GetCustomer(customerId, cancellationToken);
        if (customer == null) return null;
        var executed = await _repository.RemoveAsync(customer.Id, cancellationToken);
        return executed;
    }

    public Task<Domain.Customer?> GetCustomer(Guid customerId, CancellationToken cancellationToken)
        => _repository.GetCustomerAsync(customerId, cancellationToken);
    
    public Task<List<Domain.Customer>> GetCustomers(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException();

        return _repository.GetCustomersAsync(cancellationToken);
    }

    public async ValueTask<Domain.Customer?> CreateAsync(Domain.Customer customer, CancellationToken cancellationToken)
    {
        try
        {
            customer.Id = Guid.NewGuid();
            var customerCreated = await _repository.CreateAsync(customer, cancellationToken);
            return customerCreated;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async ValueTask<Domain.Customer?> UpdateAsync(Domain.Customer customer, CancellationToken cancellationToken)
    {
        var customerFound = await GetCustomer(customer.Id, cancellationToken);
        if (customerFound == null) return null;
        customer.Name = customer.Name;
        var updatedClient = await _repository.UpdateAsync(customer, cancellationToken);
        return updatedClient;
    }
}