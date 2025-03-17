using Kermit.Models;

namespace Kermit.Repositories;

public interface IGrupoRepository
{
    Task SaveAsync(List<Grupo> grupos);
}
