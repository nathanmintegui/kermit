using Kermit.Contants;
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

        List<GrupoTrabalho> gruposDtos = [];
        if (grupos.Count != 0)
        {
            gruposDtos = grupos
                .GroupBy(g => g.Id)
                .Select(grupo => new GrupoTrabalho(
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
    [Route("trilhas/{trilhaId:int}")]
    public async Task<IActionResult> Post(
        [FromRoute] int trilhaId,
        [FromBody] CriarTrabalhoRequest request,
        [FromServices] IUnitOfWork unitOfWork,
        [FromServices] ITrabalhoRepository trabalhoRepository,
        [FromServices] ITrilhaRepository trilhaRepository,
        [FromServices] IEdicaoRepository edicaoRepository,
        [FromServices] IAlunoRepository alunoRepository,
        [FromServices] IGrupoRepository grupoRepository,
        [FromServices] IIntegranteRepository integranteRepository)
    {
        if (string.IsNullOrWhiteSpace(request.Nome))
        {
            return BadRequest("Campo nome deve ser preenchido.");
        }

        if (trilhaId < 1)
        {
            return BadRequest($"Trilha com ID {trilhaId} inválido.");
        }

        /*
         * Valida se existe algum parâmetro para dividir os grupos, seja por quantidade ou por integrante.
         */
        if (request.QuantidadeGrupos is null && request.IntegrantesPorGrupo is null)
        {
            return BadRequest("Informe a quantidade de grupos ou integrantes por grupo.");
        }

        if (request.QuantidadeGrupos is not null && request.QuantidadeGrupos < 1)
        {
            return BadRequest("Quantidade de grupos deve ser um valor maior que zero.");
        }

        if (request.IntegrantesPorGrupo is not null && request.IntegrantesPorGrupo < 1)
        {
            return BadRequest("Quantidade de integrantes por grupo deve ser um valor maior que zero.");
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

        TrilhaEdicao? trilhaEdicao = await trilhaRepository.FindTrilhaEdicaoByTrilhaIdAsync(trilhaId);

        List<int> listaIdAlunos = await alunoRepository.GetAllIdsByTrilhaIdAsync(trilhaEdicao!.TrilhaId);
        if (listaIdAlunos.Count == 0)
        {
            return Conflict("Não existem alunos cadastrados no sistema para vincular ao trabalho");
        }

        trabalho = Trabalho.Create(request.Nome, trilhaEdicao!.Id);

        unitOfWork.BeginTransaction();
        try
        {
            await trabalhoRepository.SaveAsync(trabalho);

            List<Grupo> grupos = [];

            if (request.IntegrantesPorGrupo is not null)
            {
                /*
                 * TODO: Handle this edge case when quantidade de alunos / integrante por grupos doesnt have an
                 * integer result, for example: 19/7 or 19/6.
                 */
                if (listaIdAlunos.Count % request.IntegrantesPorGrupo != 0)
                {
                    throw new NotImplementedException("Not implemented yet");
                }

                throw new NotImplementedException("Not implemented yet");
            }

            if (request.QuantidadeGrupos is not null)
            {
                /*
                 * TODO: Handle this edge case when quantidade de alunos / quantidade de grupos doesnt have an
                 * integer result, for example: 19/7 or 19/6.
                 */
                if (listaIdAlunos.Count % request.QuantidadeGrupos != 0)
                {
                    throw new NotImplementedException("Not implemented yet");
                }

                int totalPorGrupo = (int)(listaIdAlunos.Count / request.QuantidadeGrupos);

                FisherYatesShuffle shuffler = new();

                shuffler.Shuffle(listaIdAlunos.ToArray());

                for (int i = 1; i <= request.QuantidadeGrupos; i++)
                {
                    Grupo grupo = new($"Grupo {i}", trabalho.Id);

                    for (int idx = 0; idx < totalPorGrupo; idx++)
                    {
                        int randomIdx = Random.Shared.Next(0, listaIdAlunos.Count);
                        int idCargo = idx == totalPorGrupo - 1 ? Constantes.IdCargoTechlead : Constantes.IdCargoDev;
                        Integrante integrante = new(listaIdAlunos[randomIdx], idCargo, grupo.Id);
                        listaIdAlunos.RemoveAt(randomIdx);
                        grupo.AdicionarIntegrante(integrante);
                    }

                    grupos.Add(grupo);
                }
            }

            await grupoRepository.SaveAsync(grupos);

            foreach (Grupo grupo in grupos)
            {
                foreach (Integrante grupoIntegrante in grupo.Integrantes)
                {
                    grupoIntegrante.GrupoId = grupo.Id;
                    grupoIntegrante.Grupo = grupo;
                }
            }

            await integranteRepository.SaveAsync(grupos.SelectMany(g => g.Integrantes).ToList());

            unitOfWork.Commit();
        }
        catch (NpgsqlException)
        {
            unitOfWork.Rollback();
            return StatusCode(500);
        }
        finally
        {
            unitOfWork.Dispose();
        }

        return NoContent();
    }
}
