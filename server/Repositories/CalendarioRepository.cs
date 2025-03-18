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

    public async Task<List<string>> FindAllCompetenciasCalendarioGeralAsync()
    {
        const string query = """
                             select
                             	mc.mes || '/' || mc.ano as competencias
                             from
                                meses_calendario mc
                             where
                                mc.calendario_id = (
                                    select
                                        c.id
                                    from 
                                        calendarios c
                                    join trilhas_edicoes te on te.id = c.trilha_edicao_id
                                    join edicoes e on e.id = te.edicao_id
                                    join trilhas t on t.id = te.trilha_id
                                    where
                                        e.em_andamento = true and
                                        t.id = 1
                                )
                             order by
                                mc.ano, mc.mes;
                             """;

        List<string> competencias = (await _session.Connection.QueryAsync<string>(query))?.ToList() ?? [];

        return competencias;
    }

    public async Task<List<string>> FindAllCompetenciasByCalendarioIdAsync(Guid id)
    {
        const string query = """
                             select
                             	mc.mes || '/' || mc.ano as competencias
                             from meses_calendario mc
                             where mc.calendario_id = @Id
                             order by mc.ano, mc.mes;
                             """;

        List<string> competencias =
            (await _session.Connection.QueryAsync<string>(query, new { id }))?.ToList() ?? [];

        return competencias;
    }

    public Task<List<ConteudoProgramatico>> FindAllConteudoProgramaticoCalendarioGeralAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<ConteudoProgramatico>> FindAllConteudoProgramaticoByCalendarioIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
