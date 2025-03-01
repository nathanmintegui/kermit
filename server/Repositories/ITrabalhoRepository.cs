using Kermit.Dto.Grupo;
using Kermit.Models;

namespace Kermit.Repositories;

public interface ITrabalhoRepository
{
    Task<Trabalho?> FindByIdAsync(Guid trabalhoId);
    Task<List<GrupoDto>> FindAllGruposByTrabalhoIdAsync(Guid trabalhoId);
}
