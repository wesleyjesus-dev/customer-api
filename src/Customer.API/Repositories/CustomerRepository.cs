using Customer.API.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }
    
    public async ValueTask<int?> RemoveAsync(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await GetCustomerAsync(customerId, cancellationToken);
        if (customer == null) return null;
        _context.Customers.Remove(customer);
        var executed = await _context.SaveChangesAsync(cancellationToken);
        return executed;
    }

    public Task<Domain.Customer?> GetCustomerAsync(Guid customerId, CancellationToken cancellationToken)
    {
        return _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == customerId, cancellationToken);
    }
    
    public Task<List<Domain.Customer>> GetCustomersAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) throw new OperationCanceledException();
        
        return _context.Customers.ToListAsync(cancellationToken: cancellationToken);
    }

    public async ValueTask<Domain.Customer?> CreateAsync(Domain.Customer customer, CancellationToken cancellationToken)
    {
        try
        {
            customer.Id = Guid.NewGuid();
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync(cancellationToken);
            return customer;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }
}