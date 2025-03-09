using Kermit.Models;

namespace Kermit.Repositories;

public interface IIntegranteRepository
{
   Task SaveAsync(List<Integrante> integrantes); 
}
