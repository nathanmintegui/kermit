using System.Diagnostics;

using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class GrupoRepository : IGrupoRepository
{
    private readonly DbSession _session;

    public GrupoRepository(DbSession session)
    {
        _session = session;
    }

    public async Task SaveAsync(List<Grupo> grupos)
    {
        Debug.Assert(grupos.Count > 0);

        const string query = """
                             insert into grupos (nome, trabalho_id)
                             values (@Nome, @TrabalhoId)
                             returning id;
                             """;
        foreach (Grupo grupo in grupos)
        {
            int id = await _session.Connection.ExecuteScalarAsync<int>(new CommandDefinition(query, grupo,
                _session.Transaction));

            grupo.Id = id;
        }
    }
}
