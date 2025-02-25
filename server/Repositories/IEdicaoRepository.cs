using Kermit.Models;

namespace Kermit.Repositories;

public interface IEdicaoRepository
{
    Task<List<Edicao>> FindAllAsync();
}
