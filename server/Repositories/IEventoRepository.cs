using Kermit.Models;

namespace Kermit.Repositories;

public interface IEventoRepository
{
    Task AddAsync(Evento evento);
    Task<List<Evento>> FindAllAsync();
}
