using Catalog.Application.Abstractions;
using Catalog.Domain;

namespace Catalog.Infrastructure.Repositories;

public sealed class CategoryRepository(ISqlConnectionFactory connectionFactory) : ICategoryRepository
{
    // TODO: Load the category row with the given id, or null if none exists.
    public Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Load the category row with the given name, or null if none exists.
    public Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Load every category row.
    public Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Report whether any product row belongs to the category with the given id.
    public Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Insert a new row for the category.
    public Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Save the category's name and description to its existing row.
    public Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    // TODO: Remove the row with the given id.
    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
