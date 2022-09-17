namespace Customer.API.Domain;

public sealed class Customer
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
}