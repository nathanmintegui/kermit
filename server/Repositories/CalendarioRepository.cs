using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class CalendarioRepository : ICalendarioRepository
{
    private readonly DbSession _session;

    public CalendarioRepository(DbSession session)
    {
        _session = session;
    }

    public async Task SaveAsync(Calendario calendario)
    {
        const string query =
            """
            insert into calendarios (edicao_id, criado_em, alterado_em)
                            values (@EdicaoId, @CriadoEm, @AlteradoEm) returning id;
            """;

        Guid id = await _session.Connection.ExecuteScalarAsync<Guid>(query,
            new { EdicaoId = calendario.edicao.Id.Valor, CriadoEm = DateTime.Now, AlteradoEm = DateTime.Now },
            _session.Transaction);

        calendario.Id = CalendarioId.Create(id);
    }

    public async Task SalvarTrilhasCompetenciasAsync(List<TrilhaCompetencia> trilhasCompetencia)
    {
        const string query = """
                             insert into trilhas_competencias (ano_mes, trilha_id, calendario_id)
                                                                 values (@AnoMes, @TrilhaId, @CalendarioId)
                                                                    returning id;
                             """;

        foreach (TrilhaCompetencia trilhaCompetencia in trilhasCompetencia)
        {
            int id = await _session.Connection.ExecuteScalarAsync<int>(new CommandDefinition(query,
                new
                {
                    AnoMes = trilhaCompetencia.AnoMes.Value,
                    TrilhaId = trilhaCompetencia.Trilha.Id.Valor,
                    CalendarioId = trilhaCompetencia.Calendario.Id.Valor
                },
                _session.Transaction));

            trilhaCompetencia.Id = TrilhaComptenciaId.Create(id);
        }
    }

    public Task<Calendario?> FindByIdAsync(Guid calendarioId)
    {
        throw new NotImplementedException();
    }

    public Task<List<TrilhaCompetencia>> FindAllTrilhaCompetenciaByAnoMesAndCalendarioAsync(List<int> anoMes,
        Calendario calendario)
    {
        throw new NotImplementedException();
    }
}
