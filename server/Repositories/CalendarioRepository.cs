using System.Diagnostics;

using Dapper;

using Kermit.Database;
using Kermit.Dto.Trillha;
using Kermit.Models;

using Npgsql;

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
            new { EdicaoId = calendario.Edicao.Id.Valor, CriadoEm = DateTime.Now, AlteradoEm = DateTime.Now },
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
                                select * from (
                                    select distinct
                                        to_char(cp."data", 'mm/yyyy') as mes_ano
                                    from conteudo_programatico cp
                                    join calendarios c on c.id = cp.calendario_id
                                    join edicoes e on e.id = c.edicao_id
                                    join trilhas t on t.id = c.trilha_id
                                    where e.em_andamento = true and
                                          t.id = 1
                                ) order by mes_ano;
                             """;

        List<string> competencias = (await _session.Connection.QueryAsync<string>(query))?.ToList() ?? [];

        return competencias;
    }

    public async Task<List<string>> FindAllCompetenciasByCalendarioIdAsync(Guid id)
    {
        const string query = """
                                select * from (
                                    select distinct
                                        to_char(cp."data", 'mm/yyyy') as mes_ano
                                    from conteudo_programatico cp
                                    join calendarios c on c.id = cp.calendario_id
                                    join edicoes e on e.id = c.edicao_id
                                    where cp.calendario_id = @Id and
                                          e.em_andamento = true
                                ) order by mes_ano;
                             """;

        List<string> competencias =
            (await _session.Connection.QueryAsync<string>(query, new { id }))?.ToList() ?? [];

        return competencias;
    }

    public async Task<List<ConteudoProgramaticoSnapshot>> FindAllConteudoProgramaticoCalendarioGeralAsync()
    {
        try
        {
            const string query = """
                                 select
                                     e.id,
                                     e.nome,
                                     e.cor,
                                     cp."data"
                                 from conteudo_programatico cp
                                 join eventos e on e.id = cp.id_evento
                                 join calendarios c on c.id = cp.calendario_id
                                 join edicoes e2 on e2.id = c.edicao_id
                                 where e2.em_andamento = true and
                                       c.trilha_id = 1 -- TRILHA GERAL
                                 order by cp."data";
                                 """;

            List<ConteudoProgramaticoSnapshot> eventosSnapshot =
                (await _session.Connection.QueryAsync<ConteudoProgramaticoSnapshot>(query)).ToList();

            if (eventosSnapshot.Count == 0)
            {
                return [];
            }

            Debug.Assert(eventosSnapshot.Count > 0);

            List<ConteudoProgramaticoSnapshot> response = new(eventosSnapshot.Count);

            for (int idx = 0; idx < eventosSnapshot.Count; idx++)
            {
                ConteudoProgramaticoSnapshot? buffer = response.Find(e => e.Id == eventosSnapshot[idx].Id);
                if (buffer is null)
                {
                    response.Add(eventosSnapshot[idx]);
                    response[idx].Datas.Add(response[idx].Data);
                    continue;
                }

                buffer.Datas.Add(eventosSnapshot[idx].Data);
            }

            Debug.Assert(response.Count > 0);

            return response;
        }
        catch (NpgsqlException)
        {
            return [];
        }
    }

    public async Task<List<ConteudoProgramaticoSnapshot>> FindAllConteudoProgramaticoByCalendarioIdAsync(Guid id)
    {
        Debug.Assert(id != Guid.Empty);

        try
        {
            const string query = """
                                 select
                                     e.id,
                                     e.nome,
                                     e.cor,
                                     cp."data"
                                 from conteudo_programatico cp
                                 join eventos e on e.id = cp.id_evento
                                 join calendarios c on c.id = cp.calendario_id
                                 join edicoes e2 on e2.id = c.edicao_id
                                 where e2.em_andamento = true and
                                       c.id = @Id
                                 order by cp."data";
                                 """;

            List<ConteudoProgramaticoSnapshot> eventosSnapshot =
                (await _session.Connection.QueryAsync<ConteudoProgramaticoSnapshot>(query, new { Id = id })).ToList();

            if (eventosSnapshot.Count == 0)
            {
                return [];
            }

            Debug.Assert(eventosSnapshot.Count != 0);

            List<ConteudoProgramaticoSnapshot> response = new(eventosSnapshot.Count);

            for (int idx = 0; idx < eventosSnapshot.Count; idx++)
            {
                ConteudoProgramaticoSnapshot? buffer = response.Find(e => e.Id == eventosSnapshot[idx].Id);
                if (buffer is null)
                {
                    response.Add(eventosSnapshot[idx]);
                    response[idx].Datas.Add(response[idx].Data);
                    continue;
                }

                buffer.Datas.Add(eventosSnapshot[idx].Data);
            }

            Debug.Assert(response.Count > 0);

            return response;
        }
        catch (NpgsqlException)
        {
            return [];
        }
    }

    public async Task<List<TrilhaResponse>> FindAllCalendariosWithTrilhasAsync()
    {
        const string query = """
                             select
                                 c.id as calendario_id,
                                 t.nome as trilha
                             from calendarios c
                             join edicoes e on e.id = c.edicao_id
                             join trilhas t on t.id = c.trilha_id
                             where
                                 e.em_andamento = true;
                             """;

        List<TrilhaResponse> calendarios =
            (await _session.Connection.QueryAsync<TrilhaResponse>(query))?.ToList() ?? [];

        return calendarios;
    }
}
