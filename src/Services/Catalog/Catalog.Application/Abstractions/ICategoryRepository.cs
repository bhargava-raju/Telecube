using Catalog.Domain;

namespace Catalog.Application.Abstractions;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken);
    Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Category category, CancellationToken cancellationToken);
    Task UpdateAsync(Category category, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
