using Kermit.Models;

namespace Kermit.Repositories;

public interface IDiasCalendarioRepository
{
    Task AddAsync(CalendarioId calendarioId, EventoId eventoId, List<DateOnly> datas);
}
