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
            const string query = @"select id, nome from trilhas;";

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

    public async Task AddAsync(List<Trilha> trilhas)
    {
        const string query = @"insert into trilhas (nome) values (@nome) returning id;";

        foreach (Trilha trilha in trilhas)
        {
            int id = await _session.Connection.ExecuteScalarAsync<int>(new CommandDefinition(query, trilha,
                _session.Transaction));

            trilha.Id = TrilhaId.Create(id);
        }
    }
}
