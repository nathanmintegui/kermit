using Kermit.Models;

namespace Kermit.Repositories;

public class EventoRepository: IEventoRepository
{
    public Task AddAsync(Evento evento)
    {
        throw new NotImplementedException();
    }

    public Task<List<Evento>> FindAllAsync()
    {
        throw new NotImplementedException();
    }
}
