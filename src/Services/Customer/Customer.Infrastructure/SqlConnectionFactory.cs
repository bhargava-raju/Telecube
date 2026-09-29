using System.Data;
using Microsoft.Data.SqlClient;

namespace Customer.Infrastructure;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}

public sealed class SqlConnectionFactory(string connectionString) : ISqlConnectionFactory
{
    public IDbConnection CreateConnection() => new SqlConnection(connectionString);
}
