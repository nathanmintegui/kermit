using Dapper;

using Kermit.Database;
using Kermit.Models;

using Npgsql;

namespace Kermit.Repositories;

public class TrilhaRepository : ITrilhaRepository
{
    private readonly DbSession _session;
    private readonly ILogger<TrilhaRepository> _logger;

    public TrilhaRepository(DbSession session, ILogger<TrilhaRepository> logger)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<List<Trilha>> FindAllAsync()
    {
        try
        {
            string query = @"select id, nome from trilhas;";

            IEnumerable<Trilha> trilhas =
                await _session.Connection.QueryAsync<Trilha>(query, null, _session.Transaction);

            return trilhas.ToList();
        }
        catch (NpgsqlException e)
        {
            _logger.LogError("{Date} | An Exception occured - {Error}", DateTime.UtcNow, e);
        }

        return [];
    }
}
