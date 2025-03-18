using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class DiasCalendarioRepository : IDiasCalendarioRepository
{
    private readonly DbSession _session;

    public DiasCalendarioRepository(DbSession session)
    {
        _session = session;
    }

    public async Task AddAsync(CalendarioId calendarioId, EventoId eventoId, List<DateOnly> datas)
    {
        const string query = """
                             insert into dias_calendario (mes_calendario_id, id_evento, dia)
                             values (
                                 (select id
                                  from meses_calendario mc
                                  where mc.calendario_id = @CalendarioId and mc.ano = @Ano and mc.mes = @Mes),
                                 @IdEvento,
                                 @Dia)
                             returning id;
                             """;

        foreach (DateOnly date in datas)
        {
            await _session.Connection.ExecuteAsync(query,
                new
                {
                    CalendarioId = calendarioId.Valor,
                    Ano = date.Year,
                    Mes = date.Month,
                    IdEvento = eventoId.Valor,
                    Dia = date.Day
                },
                _session.Transaction);
        }
    }
}
