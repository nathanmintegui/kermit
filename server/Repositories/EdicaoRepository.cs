using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class EdicaoRepository : IEdicaoRepository
{
    private readonly DbSession _session;

    public EdicaoRepository(DbSession session)
    {
        _session = session;
    }

    public async Task<List<Edicao>> FindAllAsync()
    {
        string query = @"select id, nome, em_andamento from edicoes;";

        IEnumerable<Edicao> edicoes =
            await _session.Connection.QueryAsync<Edicao>(query, null, _session.Transaction);

        return edicoes.ToList();
    }

    public async Task InsertAsync(Edicao edicao)
    {
        const string query = @"insert into edicoes(nome, em_andamento) values (@Nome, @EmAndamento) returning id;";

        int id = await _session.Connection.ExecuteScalarAsync<int>(query, edicao, _session.Transaction);

        edicao.Id = EdicaoId.Create(id);
    }
}
