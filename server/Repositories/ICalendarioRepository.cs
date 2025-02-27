using Kermit.Models;

namespace Kermit.Repositories;

public interface ICalendarioRepository
{
    Task SaveAsync(Calendario calendario);
    Task SalvarTrilhasCompetenciasAsync(List<TrilhaCompetencia> trilhasCompetencia);
}
