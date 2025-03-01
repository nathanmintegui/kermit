using System.Diagnostics;

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

    public async Task<Trilha?> FindByIdAsync(int id)
    {
        const string query = """
                             select
                                t.*
                             from trilhas_edicoes te
                                 join edicoes e on (e.id = te.edicao_id)
                                 join trilhas t on (t.id = te.trilha_id)
                             where
                                t.id = @Id and
                             	e.em_andamento = true;
                             """;

        Trilha? trilha = await _session.Connection.QueryFirstOrDefaultAsync<Trilha>(query, new { Id = id });

        return trilha;
    }

    public async Task<TrilhaEdicao?> FindTrilhaEdicaoByTrilhaIdAsync(int id)
    {
        Debug.Assert(id > 0, "Trilha id não pode ser negativo.");

        const string query = """
                             select
                                te.*
                             from trilhas_edicoes te
                                join edicoes e on (e.id = te.edicao_id)
                             where
                                te.trilha_id = @TrilhaId and
                                e.em_andamento = true;
                             """;

        TrilhaEdicao? trilhaEdicao =
            await _session.Connection.QueryFirstOrDefaultAsync<TrilhaEdicao>(query, new { TrilhaId = id });

        return trilhaEdicao;
    }
}
