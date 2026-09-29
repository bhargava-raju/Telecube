using Customer.Application.Abstractions;
using Dapper;
using CustomerEntity = Customer.Domain.Customer;


namespace Customer.Infrastructure.Repositories;

public sealed class CustomerRepository(ISqlConnectionFactory connectionFactory) : ICustomerRepository
{
    public Task<CustomerEntity?> GetByEntraObjectIdAsync(string entraObjectId, CancellationToken cancellationToken)
    {
        var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();
        parameters.Add("EntraObjectId", entraObjectId, System.Data.DbType.String);
        
        return connection.QuerySingleOrDefaultAsync<CustomerEntity>
            ("SELECT * FROM Customer WHERE EntraObjectId = @EntraObjectId", parameters);
    }

    public Task AddAsync(CustomerEntity customer, CancellationToken cancellationToken)
    {
        var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("EntraObjectId", customer.EntraObjectId, System.Data.DbType.String);
        parameters.Add("Email", customer.Email, System.Data.DbType.String);
        parameters.Add("DisplayName", customer.DisplayName, System.Data.DbType.String);

        return connection.ExecuteAsync("INSERT INTO Customer (EntraObjectId, Email, DisplayName) VALUES (@EntraObjectId, @Email, @DisplayName)", parameters);
    }

    public Task UpdateAsync(CustomerEntity customer, CancellationToken cancellationToken)
    {
        var connection = connectionFactory.CreateConnection();
        var parameters = new DynamicParameters();

        parameters.Add("EntraObjectId", customer.EntraObjectId, System.Data.DbType.String);
        parameters.Add("Email", customer.Email, System.Data.DbType.String);
        parameters.Add("DisplayName", customer.DisplayName, System.Data.DbType.String);

        return connection.ExecuteAsync("UPDATE Customer SET Email = @Email, DisplayName = @DisplayName WHERE EntraObjectId = @EntraObjectId", parameters);
    }
}




