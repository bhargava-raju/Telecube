using Catalog.Application.Abstractions;
using Catalog.Domain;
using Dapper;

namespace Catalog.Infrastructure.Repositories;

public sealed class CategoryRepository(ISqlConnectionFactory connectionFactory) : ICategoryRepository
{
    private const string SelectColumns = "SELECT Id, Name, Description FROM Category";

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, System.Data.DbType.Guid);

        return await connection.QuerySingleOrDefaultAsync<Category>(
            new CommandDefinition($"{SelectColumns} WHERE Id = @Id", parameters, cancellationToken: cancellationToken));
    }

    public async Task<Category?> GetByNameAsync(string name, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Name", name, System.Data.DbType.String);

        return await connection.QuerySingleOrDefaultAsync<Category>(
            new CommandDefinition($"{SelectColumns} WHERE Name = @Name", parameters, cancellationToken: cancellationToken));
    }

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var categories = await connection.QueryAsync<Category>(
            new CommandDefinition($"{SelectColumns} ORDER BY Name", cancellationToken: cancellationToken));

        return categories.ToList();
    }

    public async Task<bool> HasProductsAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, System.Data.DbType.Guid);

        // T-SQL only allows EXISTS inside a condition, so turn it into a 1/0 bit that Dapper reads as bool.
        return await connection.ExecuteScalarAsync<bool>(new CommandDefinition(
            "SELECT CASE WHEN EXISTS (SELECT 1 FROM Product WHERE CategoryId = @Id) THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END",
            parameters, cancellationToken: cancellationToken));
    }

    public async Task AddAsync(Category category, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", category.Id, System.Data.DbType.Guid);
        parameters.Add("Name", category.Name, System.Data.DbType.String);
        parameters.Add("Description", category.Description, System.Data.DbType.String);

        await connection.ExecuteAsync(new CommandDefinition(
            "INSERT INTO Category (Id, Name, Description) VALUES (@Id, @Name, @Description)",
            parameters, cancellationToken: cancellationToken));
    }

    public async Task UpdateAsync(Category category, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", category.Id, System.Data.DbType.Guid);
        parameters.Add("Name", category.Name, System.Data.DbType.String);
        parameters.Add("Description", category.Description, System.Data.DbType.String);

        await connection.ExecuteAsync(new CommandDefinition(
            "UPDATE Category SET Name = @Name, Description = @Description WHERE Id = @Id",
            parameters, cancellationToken: cancellationToken));
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("Id", id, System.Data.DbType.Guid);

        await connection.ExecuteAsync(new CommandDefinition(
            "DELETE FROM Category WHERE Id = @Id", parameters, cancellationToken: cancellationToken));
    }
}
