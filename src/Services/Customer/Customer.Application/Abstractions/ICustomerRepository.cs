namespace Customer.Application.Abstractions;
public interface ICustomerRepository
{
    Task<Domain.Customer?> GetByEntraObjectIdAsync(string entraObjectId, CancellationToken cancellationToken);
    Task AddAsync(Domain.Customer customer, CancellationToken cancellationToken);
    Task UpdateAsync(Domain.Customer customer, CancellationToken cancellationToken);
}
