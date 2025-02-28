using Kermit.Models;

namespace Kermit.Repositories;

public interface ICalendarioRepository
{
    Task SaveAsync(Calendario calendario);
    Task SalvarTrilhasCompetenciasAsync(List<TrilhaCompetencia> trilhasCompetencia);
    Task<Calendario?> FindByIdAsync(Guid calendarioId);

    Task<List<TrilhaCompetencia>> FindAllTrilhaCompetenciaByAnoMesAndCalendarioAsync(List<int> anoMes,
        Calendario calendario);
}
