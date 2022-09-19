namespace Customer.API.Services.Contracts;

public interface ICustomerService
{
    ValueTask<int?> Remove(Guid customerId, CancellationToken cancellationToken);
    Task<Domain.Customer?> GetCustomer(Guid customerId, CancellationToken cancellationToken);
    Task<List<Domain.Customer>> GetCustomers(CancellationToken cancellationToken);
    ValueTask<Domain.Customer?> CreateAsync(Domain.Customer customer, CancellationToken cancellationToken);
    ValueTask<Domain.Customer?> UpdateAsync(Domain.Customer customer, CancellationToken cancellationToken);
}