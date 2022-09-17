using Customer.API.Repositories;
using Customer.API.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Customer.API.Services;

public class CustomerService : ICustomerService
{
    private readonly CustomerDbContext _context;

    public CustomerService(CustomerDbContext context)
    {
        _context = context;
    }

    public async ValueTask<int?> Remove(Guid customerId, CancellationToken cancellationToken)
    {
        var customer = await GetCustomer(customerId, cancellationToken);
        if (customer == null) return null;
        _context.Customers.Remove(customer);
        var executed = await _context.SaveChangesAsync(cancellationToken);
        return executed;
    }

    public Task<Domain.Customer?> GetCustomer(Guid customerId, CancellationToken cancellationToken)
    {
        return _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(customer => customer.Id == customerId, cancellationToken);
    }
    
    public Task<List<Domain.Customer>> GetCustomers(CancellationToken cancellationToken)
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