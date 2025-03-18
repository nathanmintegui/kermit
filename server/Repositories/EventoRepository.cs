using Dapper;

using Kermit.Database;
using Kermit.Models;

namespace Kermit.Repositories;

public class EventoRepository : IEventoRepository
{
    private readonly DbSession _session;

    public EventoRepository(DbSession session)
    {
        _session = session;
    }

    public async Task AddAsync(Evento evento)
    {
        const string query = """
                             insert into eventos (nome, cor)
                             values (@Nome, @cor)
                             returning id;
                             """;

        int id = await _session.Connection.ExecuteScalarAsync<int>(query, evento, _session.Transaction);

        evento.Id = EventoId.Create(id);
    }

    public async Task AddAsync(List<Evento> eventos)
    {
        const string query = """
                             insert into eventos (nome, cor)
                             values (@Nome, @cor)
                             returning id;
                             """;

        foreach (Evento evento in eventos)
        {
            int id = await _session.Connection.ExecuteScalarAsync<int>(query, evento, _session.Transaction);

            evento.Id = EventoId.Create(id);
        }
    }

    public Task<List<Evento>> FindAllAsync()
    {
        throw new NotImplementedException();
    }
}
