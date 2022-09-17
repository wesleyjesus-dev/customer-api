using Microsoft.EntityFrameworkCore;

namespace Customer.API.Repositories;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options)
    :base(options)
    {
        
    }
    public DbSet<Domain.Customer> Customers => Set<Domain.Customer>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Customer>()
            .HasKey(customer => customer.Id);
        
        base.OnModelCreating(modelBuilder);
    }
}