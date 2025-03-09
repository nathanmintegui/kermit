using Dapper;

using Kermit.Database;

namespace Kermit.Repositories;

public class AlunoRepository : IAlunoRepository
{
    private readonly DbSession _session;

    public AlunoRepository(DbSession session)
    {
        _session = session;
    }

    public async Task<List<int>> GetAllIdsByTrilhaIdAsync(int trilhaId)
    {
        const string query = """
                             select
                                a.id
                                 from alunos a
                                 join trilhas_edicoes te on (te.id = a.trilha_edicao_id)
                                 join edicoes e on(e.id = te.edicao_id)
                             where
                             	e.em_andamento = true and
                             	te.trilha_id = @TrilhaId;
                             """;

        IEnumerable<int> ids = await _session.Connection.QueryAsync<int>(query, new { TrilhaId = trilhaId });

        return ids.ToList();
    }
}
