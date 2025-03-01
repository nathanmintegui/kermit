using Kermit.Dto.Grupo;
using Kermit.Dto.Trabalho;
using Kermit.Models;
using Kermit.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace Kermit.Controllers;

[ApiController]
[Route("v1/trabalhos")]
public class TrabalhoController : ControllerBase
{
    [HttpGet]
    [Route("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromServices] ITrabalhoRepository trabalhoRepository)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("ID do trabalho inválido.");
        }

        Trabalho? trabalho = await trabalhoRepository.FindByIdAsync(id);
        if (trabalho is null)
        {
            return BadRequest($"Trabalho com ID {id} não encontrado.");
        }

        List<GrupoDto> grupos = await trabalhoRepository.FindAllGruposByTrabalhoIdAsync(id);

        List<Grupo> gruposDtos = [];
        if (grupos.Count != 0)
        {
            gruposDtos = grupos
                .GroupBy(g => g.Id)
                .Select(grupo => new Grupo(
                    grupo.Key,
                    grupo.First().Grupo,
                    grupo.Select(g => new Alunos(g.Aluno, g.Cargo, g.Abreviacao)).ToList()
                ))
                .ToList();
        }

        TrabalhoResponse response = new(trabalho.Id, trabalho.Nome, gruposDtos);

        return Ok(response);
    }
}
