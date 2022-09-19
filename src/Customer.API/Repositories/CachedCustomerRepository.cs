using Customer.API.Repositories.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Customer.API.Repositories;

public sealed class CachedCustomerRepository : ICustomerRepository
{
    private readonly IMemoryCache _cache;
    private readonly CustomerRepository _repository;

    public CachedCustomerRepository(CustomerRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }
    
    public ValueTask<int?> RemoveAsync(Guid customerId, CancellationToken cancellationToken)
        => _repository.RemoveAsync(customerId, cancellationToken);
    
    public Task<Domain.Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return _cache.GetOrCreateAsync(customerId.ToString(),
            async cacheOptions =>
            {

                var customer = await _repository.GetCustomerAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.Zero;
                    return customer;
                }

                cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return customer;
            });
    }

    public Task<List<Domain.Customer>> GetCustomersAsync(CancellationToken cancellationToken)
    {
        var cacheKey = "customers";
        return _cache.GetOrCreateAsync(cacheKey,
            async cacheOptions =>
            {
                List<Domain.Customer> customers = await _repository.GetCustomersAsync(cancellationToken);
                if (!customers.Any())
                {
                    cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromMilliseconds(1);
                    return customers;
                }

                cacheOptions.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                return customers;
            })!;
    }

    public ValueTask<Domain.Customer?> CreateAsync(Domain.Customer customer, CancellationToken cancellationToken)
        => _repository.CreateAsync(customer, cancellationToken);
}