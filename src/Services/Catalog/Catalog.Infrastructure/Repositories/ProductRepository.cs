using Catalog.Application.Abstractions;
using Catalog.Domain;
using Dapper;

namespace Catalog.Infrastructure.Repositories;

public sealed class ProductRepository(ISqlConnectionFactory connectionFactory) : IProductRepository
{
    private const string SelectColumns = "SELECT Id, CategoryId, Sku, Name, Description, Price FROM Product";

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, System.Data.DbType.Guid);

        return await connection.QuerySingleOrDefaultAsync<Product>(
            new CommandDefinition($"{SelectColumns} WHERE Id = @Id", parameters, cancellationToken: cancellationToken));
    }

    public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Sku", sku, System.Data.DbType.String);

        return await connection.QuerySingleOrDefaultAsync<Product>(
            new CommandDefinition($"{SelectColumns} WHERE Sku = @Sku", parameters, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        return await connection.QueryAsync<Product>(
            new CommandDefinition($"{SelectColumns} ORDER BY Name", cancellationToken: cancellationToken));
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", product.Id, System.Data.DbType.Guid);
        parameters.Add("CategoryId", product.CategoryId, System.Data.DbType.Guid);
        parameters.Add("Sku", product.Sku, System.Data.DbType.String);
        parameters.Add("Name", product.Name, System.Data.DbType.String);
        parameters.Add("Description", product.Description, System.Data.DbType.String);
        parameters.Add("Price", product.Price, System.Data.DbType.Decimal);

        await connection.ExecuteAsync(new CommandDefinition(
            "INSERT INTO Product (Id, CategoryId, Sku, Name, Description, Price) VALUES (@Id, @CategoryId, @Sku, @Name, @Description, @Price)",
            parameters, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", product.Id, System.Data.DbType.Guid);
        parameters.Add("CategoryId", product.CategoryId, System.Data.DbType.Guid);
        parameters.Add("Name", product.Name, System.Data.DbType.String);
        parameters.Add("Description", product.Description, System.Data.DbType.String);
        parameters.Add("Price", product.Price, System.Data.DbType.Decimal);

        await connection.ExecuteAsync(new CommandDefinition(
            "UPDATE Product SET CategoryId = @CategoryId, Name = @Name, Description = @Description, Price = @Price WHERE Id = @Id",
            parameters, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, System.Data.DbType.Guid);

        await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM Product WHERE Id = @Id", parameters, cancellationToken: cancellationToken));
    }
}
