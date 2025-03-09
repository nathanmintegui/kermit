using System.Diagnostics;

using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class IntegranteRepository : IIntegranteRepository
{
    private readonly DbSession _session;

    public IntegranteRepository(DbSession session)
    {
        _session = session;
    }

    public async Task SaveAsync(List<Integrante> integrantes)
    {
        Debug.Assert(integrantes.Count > 0);

        const string query = """
                             insert into integrantes (aluno_id, cargo_id, grupo_id)
                             values (@AlunoId, @CargoId, @GrupoId);
                             """;

        foreach (Integrante integrante in integrantes)
        {
            await _session.Connection.ExecuteAsync(new CommandDefinition(query, integrante, _session.Transaction));
        }
    }
}
