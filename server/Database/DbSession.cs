using System.Data;

using Npgsql;

namespace Kermit.Database;

public sealed class DbSession : IDisposable
{
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; set; } = default!;

    public DbSession(string connectionStrring)
    {
        Connection = new NpgsqlConnection(connectionStrring);
        Connection.Open();
    }

    public void Dispose()
    {
        Connection?.Dispose();
    }
}
