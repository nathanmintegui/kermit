using Kermit.Models;

namespace Kermit.Repositories;

public interface ITrilhaRepository
{
    /// <summary>
    /// Recupera todas as trilhas cadastradas no banco de dados.
    /// </summary>
    /// <returns>Uma lista de objetos <see cref="Trilha"/>. Retorna uma lista vazia caso nenhuma trilha seja encontrada.</returns>
    Task<List<Trilha>> FindAllAsync();
    
}
