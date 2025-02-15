using System.Data;

using Npgsql;

namespace Kermit.Database;

public class NpgsqlDbConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        NpgsqlConnection connection = new(connectionString);

        await connection.OpenAsync(token);

        return connection;
    }
}

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}
