namespace Kermit.Repositories;

public interface IAlunoRepository
{
    Task<List<int>> GetAllIdsByTrilhaIdAsync(int trilhaId);
}
