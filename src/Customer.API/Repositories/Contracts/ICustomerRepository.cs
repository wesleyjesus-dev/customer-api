namespace Customer.API.Repositories.Contracts;

public interface ICustomerRepository
{
    ValueTask<int?> RemoveAsync(Guid customerId, CancellationToken cancellationToken);
    Task<Domain.Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken);
    Task<List<Domain.Customer>> GetCustomersAsync(CancellationToken cancellationToken);
    ValueTask<Domain.Customer?> CreateAsync(Domain.Customer customer, CancellationToken cancellationToken);
}