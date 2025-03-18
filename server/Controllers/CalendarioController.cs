using System.Globalization;

using Kermit.Database;
using Kermit.dto.calendario;
using Kermit.Dto.ConteudoProgramatico;
using Kermit.Dto.Edicao;
using Kermit.Dto.Trillha;
using Kermit.Models;
using Kermit.Repositories;

using Microsoft.AspNetCore.Mvc;

using Npgsql;

using static System.Convert;

namespace Kermit.Controllers;

[ApiController]
[Route("v1/calendarios")]
public class CalendarioController : ControllerBase
{
    [HttpGet]
    [Route("{id:guid?}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid? id,
        [FromServices] ICalendarioRepository calendarioRepository)
    {
        List<string> competenciasCalendario = id is null
            ? await calendarioRepository.FindAllCompetenciasCalendarioGeralAsync()
            : await calendarioRepository.FindAllCompetenciasByCalendarioIdAsync((Guid)id);

        /*
        List<ConteudoProgramatico> conteudosProgramaticos = id is null
            ? await calendarioRepository.FindAllConteudoProgramaticoCalendarioGeralAsync()
            : await calendarioRepository.FindAllConteudoProgramaticoByCalendarioIdAsync((Guid)id);
        */

        List<Competencia> competencias = new(competenciasCalendario.Count);
        foreach (string compentecia in competenciasCalendario)
        {
            int mes = int.Parse(compentecia.Split("/")[0]);
            int ano = int.Parse(compentecia.Split("/")[1]);

            HashSet<DiaCalendario> diasAntes = fillDaysBefore(ano, mes).ToHashSet();
            HashSet<DiaCalendario> dias = GetDates(ano, mes);

            diasAntes.UnionWith(dias);

            int diasRestantesFinalMes = ToInt32(Math.Ceiling(diasAntes.Count / 7.0) * 7) - diasAntes.Count;
            diasAntes.UnionWith(preencherDiasDepois(diasRestantesFinalMes));

            Competencia competenciaMesAtual = new() { Mes = getMes(mes), Dias = diasAntes };

            competencias.Add(competenciaMesAtual);
        }

        /* TODO: fill out with specific content */
        Legenda legenda = new() { ItemsLegenda = [] };

        CalendarioResponse response = new() { Competencias = competencias, Legenda = legenda };

        return Ok(response);
    }

    [HttpGet]
    [Route("info-cadastro")]
    public async Task<IActionResult> GetInfoCadastro(
        [FromServices] ITrilhaRepository trilhaRepository,
        [FromServices] IEdicaoRepository edicaoRepository)
    {
        Task<List<Trilha>> trilhasTask = trilhaRepository.FindAllAsync();
        Task<List<Edicao>> edicoesTask = edicaoRepository.FindAllAsync();

        await Task.WhenAll(trilhasTask, edicoesTask);

        List<Trilha> trilhas = await trilhasTask;
        List<Edicao> edicoes = await edicoesTask;

        var response = new
        {
            Trilhas = trilhas.Select(t => new TrilhaResponse(t.Id.Valor, t.Nome.Value)),
            Edicoes = edicoes.Select(e => new EdicaoResponse(e.Id.Valor, e.Nome.Value))
        };

        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] CriarCalendarioRequest request,
        [FromServices] IUnitOfWork unitOfWork,
        [FromServices] ITrilhaRepository trilhaRepository,
        [FromServices] IEdicaoRepository edicaoRepository,
        [FromServices] ICalendarioRepository calendarioRepository)
    {
        const int numeroMinimoTrilhas = 1;

        if (request.edicao == string.Empty)
        {
            return BadRequest("Número da edição não pode ser vazio.");
        }

        if (request.trilhas.Count < numeroMinimoTrilhas)
        {
            return BadRequest("Lista de trilhas a serem cadastradas não deve ser vazia.");
        }

        Task<List<Trilha>> trilhasTask = trilhaRepository.FindAllAsync();
        Task<List<Edicao>> edicoesTask = edicaoRepository.FindAllAsync();

        await Task.WhenAll(trilhasTask, edicoesTask);

        List<Trilha> trilhas = await trilhasTask;
        List<Edicao> edicoes = await edicoesTask;

        Edicao edicao = Edicao.Create(new NonEmptyString(request.edicao), true, edicoes);

        List<string> listaTrilhasCadastradas = trilhas
            .Select(t => t.Nome.Value.Trim().ToUpper())
            .ToList();

        List<string> nomesTrilhasASeremCadastradas = trilhas.Count > 0
            ? request.trilhas.Select(t => t.valor).AsEnumerable()
                .Where(t => !listaTrilhasCadastradas.Contains(t.Trim().ToUpper()))
                .ToList()
            : request.trilhas.Select(t => t.valor).ToList();

        List<Trilha> trilhasASeremCadastradas = nomesTrilhasASeremCadastradas
            .Select(t => Trilha.Create(new NonEmptyString(t)))
            .ToList();

        /*
         * NOTE: estudar o melhor fluxo para utilizar estes métodos, se o finally é realmente necessário
         */
        unitOfWork.BeginTransaction();
        try
        {
            await edicaoRepository.InsertAsync(edicao);

            if (trilhasASeremCadastradas.Count != 0)
            {
                await trilhaRepository.AddAsync(trilhasASeremCadastradas);
            }

            /*
             * TODO: validar se já existe um calendário com a mesma edição ou se já existe um calendário ativo,
             * nesse caso setar o ativo para false.
             */
            Calendario calendario = Calendario.Create(edicao);
            await calendarioRepository.SaveAsync(calendario);

            trilhas.AddRange(trilhasASeremCadastradas);

            List<TrilhaCompetencia> trilhaCompetencias = [];
            foreach (TrilhaCompetenciaRequest tcr in request.trilhas)
            {
                Trilha trilha = trilhas.Find(t =>
                                    t.Nome.Value.Trim().Equals(tcr.valor.Trim(),
                                        StringComparison.CurrentCultureIgnoreCase)) ??
                                Trilha.Create(new NonEmptyString(tcr.valor));

                tcr.competencias.ForEach(c =>
                {
                    trilhaCompetencias.Add(TrilhaCompetencia.Create(new AnoMes(c.ano, c.mes), trilha, calendario));
                });
            }

            await calendarioRepository.SalvarTrilhasCompetenciasAsync(trilhaCompetencias);

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

    [HttpPost]
    [Route("{id}/conteudo-programatico")]
    public async Task<IActionResult> CriarConteudoProgramatico(
        [FromRoute] Guid id,
        [FromBody] CriarConteudoProgramaticoRequest request,
        [FromServices] IUnitOfWork unitOfWork,
        [FromServices] ICalendarioRepository calendarioRepository,
        [FromServices] IEventoRepository eventoRepository)
    {
        /*
         * Passo 1 - Validar ID calendário
         */
        Calendario? calendario = await calendarioRepository.FindByIdAsync(id);
        if (calendario is null)
        {
            return NotFound($"Calendário com ID {id} não encontrado.");
        }

        if (!calendario.edicao.EmAndamento)
        {
            return Conflict("Calendário pertence à uma edição antiga/finalizada.");
        }

        /*
         * Passo 2 - validar a request
         */
        if (string.IsNullOrWhiteSpace(request.ConteudoProgramatico))
        {
            return BadRequest("Campo conteúdo programático não pode ser vazio.");
        }

        if (string.IsNullOrWhiteSpace(request.Cor))
        {
            return BadRequest("Campo cor não pode ser vazio.");
        }

        foreach (string data in request.Datas)
        {
            if (!DateOnly.TryParse(data, new CultureInfo("pt-BR"), DateTimeStyles.None, out DateOnly _))
            {
                return UnprocessableEntity($"Data {data} inválida e/ou formato inválido.");
            }
        }

        List<Evento> eventos = await eventoRepository.FindAllAsync();
        if (eventos.Find(e =>
                e.Nome.Value.Equals(request.ConteudoProgramatico.Trim(),
                    StringComparison.OrdinalIgnoreCase)) != null)
        {
            return BadRequest($"Já existe um evento com o nome {request.ConteudoProgramatico}.");
        }

        if (eventos.Find(e =>
                e.Cor.Value.Equals(request.Cor.Trim(),
                    StringComparison.OrdinalIgnoreCase)) != null)
        {
            return BadRequest($"Já existe uma cor com o código {request.Cor}.");
        }

        /*
         * Validar se as datas da request refletem os registros da tabela trilhas_competencias
         */

        List<int> listaAnoMes = [];
        foreach (string data in request.Datas)
        {
            (int ano, int mes) = AnoMes.GetValorAnoEMes(data);

            AnoMes anoMes = new(ano, mes);

            listaAnoMes.Add(anoMes.Value);
        }

        List<TrilhaCompetencia> listaTrilhaCompetencia =
            await calendarioRepository.FindAllTrilhaCompetenciaByAnoMesAndCalendarioAsync(listaAnoMes, calendario);

        if (listaTrilhaCompetencia.Count == 0)
        {
            return BadRequest("Não existem competências cadastradas com as data informadas.");
        }

        foreach (TrilhaCompetencia trilhaCompetencia in listaTrilhaCompetencia)
        {
            /*
             * Verifica se a data da request existe na competência.
             */
        }

        /* Passo 3 - Inserir os dados */

        return NoContent();
    }

    private static List<DiaCalendario> fillDaysBefore(int year, int month)
    {
#pragma warning disable S6562
        DateTime date = new(year, month, 1);
#pragma warning restore S6562
        DayOfWeek weekDay = date.DayOfWeek;

        List<DiaCalendario> dates = [];
        switch (weekDay)
        {
            case DayOfWeek.Monday:
                {
                    DiaCalendario obj = new() { Data = date.ToString("dd/MM/yyyy") };

                    dates.Add(obj);

                    return dates;
                }
            case DayOfWeek.Tuesday:
                {
                    List<DiaCalendario> obj =
                    [
                        new() { Data = string.Empty },
                        new() { Data = string.Empty }
                    ];

                    dates.AddRange(obj);

                    return dates;
                }
            case DayOfWeek.Wednesday:
                {
                    List<DiaCalendario> obj =
                    [
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty }
                    ];

                    dates.AddRange(obj);

                    return dates;
                }
            case DayOfWeek.Thursday:
                {
                    List<DiaCalendario> obj =
                    [
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty }
                    ];

                    dates.AddRange(obj);

                    return dates;
                }
            case DayOfWeek.Friday:
                {
                    List<DiaCalendario> obj =
                    [
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty }
                    ];

                    dates.AddRange(obj);

                    return dates;
                }
            case DayOfWeek.Saturday:
                {
                    List<DiaCalendario> obj =
                    [
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty },
                        new() { Data = string.Empty }
                    ];

                    dates.AddRange(obj);

                    return dates;
                }
            case DayOfWeek.Sunday:
            default:
                return dates;
        }
    }

    private static HashSet<DiaCalendario> GetDates(int year, int month)
    {
        HashSet<DiaCalendario> dates = [];

        for (DateTime date = new(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
             date.Month == month;
             date = date.AddDays(1))
        {
            DiaCalendario obj = new() { Data = date.ToString("dd/MM/yyyy") };

            dates.Add(obj);
        }

        return dates;
    }

    private static string getMes(int mes)
    {
        return mes switch
        {
            1 => "Janeiro",
            2 => "Fevereiro",
            3 => "Março",
            4 => "Abril",
            5 => "Maio",
            6 => "Junho",
            7 => "Julho",
            8 => "Agosto",
            9 => "Setembro",
            10 => "Outubro",
            11 => "Novembro",
            12 => "Dezembro",
            _ => ""
        };
    }

    private static List<DiaCalendario> preencherDiasDepois(int diasRestantes)
    {
        List<DiaCalendario> dates = new(diasRestantes);
        for (int i = 0; i < diasRestantes; i++)
        {
            DiaCalendario obj = new() { Data = string.Empty };

            dates.Add(obj);
        }

        return dates;
    }
}
