using Kermit.Models;

namespace Kermit.Repositories;

public interface IEventoRepository
{
    Task AddAsync(Evento evento);
    Task AddAsync(List<Evento> eventos);
    Task<List<Evento>> FindAllAsync();
}
