using Kermit.Database;
using Kermit.Dto.Grupo;
using Kermit.Dto.Trabalho;
using Kermit.Models;
using Kermit.Repositories;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

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

    [HttpPost]
    [Route("trilhas/{trilhaId}")]
    public async Task<IActionResult> Post(
        [FromRoute] int trilhaId,
        [FromBody] CriarTrabalhoRequest request,
        [FromServices] IUnitOfWork unitOfWork,
        [FromServices] ITrabalhoRepository trabalhoRepository,
        [FromServices] ITrilhaRepository trilhaRepository,
        [FromServices] IEdicaoRepository edicaoRepository)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            return BadRequest("Campo nome deve ser preenchido.");
        }

        if (trilhaId < 1)
        {
            return BadRequest($"Trilha com ID {trilhaId} inválido.");
        }

        Trilha? trilha = await trilhaRepository.FindByIdAsync(trilhaId);
        if (trilha is null)
        {
            return NotFound($"Trilha com ID {trilhaId} não encontrada");
        }

        Trabalho? trabalho = await trabalhoRepository.FindByNomeAndTrilhaIdAsync(request.Nome, trilhaId);
        if (trabalho is not null)
        {
            return Conflict($"Já existe um trabalho cadastrado com o nome ${request.Nome}.");
        }

        if (request.ShufflePadrao)
        {
            /* count quantos alunos existem antes de prosseguir */

            /*
             * TODO: Modelar classe para realizar o algoritmo de shuffle.
             */
        }

        TrilhaEdicao? trilhaEdicao = await trilhaRepository.FindTrilhaEdicaoByTrilhaIdAsync(trilhaId);

        trabalho = Trabalho.Create(request.Nome, trilhaEdicao!.Id);

        unitOfWork.BeginTransaction();
        try
        {
            await trabalhoRepository.SaveAsync(trabalho);

            if (request.ShufflePadrao)
            {
                /*
                 * Persistir os dados nas tabelas de grupos e integrantes.
                 */
            }

            unitOfWork.Commit();
        }
        catch (NpgsqlException)
        {
            unitOfWork.Rollback();
        }
        finally
        {
            unitOfWork.Dispose();
        }

        return NoContent();
    }
}
