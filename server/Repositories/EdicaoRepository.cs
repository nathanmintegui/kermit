using Dapper;

using Kermit.Database;
using Kermit.Models;

using Npgsql;

namespace Kermit.Repositories;

public class EdicaoRepository : IEdicaoRepository
{
    private readonly DbSession _session;
    private readonly ILogger<EdicaoRepository> _logger;

    public EdicaoRepository(ILogger<EdicaoRepository> logger, DbSession session)
    {
        _session = session;
        _logger = logger;
    }

    public async Task<List<Edicao>> FindAllAsync()
    {
        try
        {
            string query = @"select id, nome, em_andamento from edicoes;";

            IEnumerable<Edicao> edicoes =
                await _session.Connection.QueryAsync<Edicao>(query, null, _session.Transaction);

            return edicoes.ToList();
        }
        catch (NpgsqlException e)
        {
            _logger.LogError("{Date} | An Exception occured - {Error}", DateTime.UtcNow, e);
        }

        return [];
    }

    public async Task InsertAsync(Edicao edicao)
    {
        const string query = @"insert into edicoes(nome, em_andamento) values (@Nome, @EmAndamento) returning id;";

        int id = await _session.Connection.ExecuteScalarAsync<int>(query, edicao, _session.Transaction);

        edicao.Id = EdicaoId.Create(id);
    }
}
