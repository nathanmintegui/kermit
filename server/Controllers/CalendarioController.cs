using Kermit.Models;

using Microsoft.AspNetCore.Mvc;

using static System.Convert;

namespace Kermit.Controllers;

[ApiController]
[Route("v1/calendarios")]
public class CalendarioController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        string[] competenciasCalendarioGeral = ["02/2025", "03/2025", "04/2025"];

        List<Competencia> competencias = new(competenciasCalendarioGeral.Length);
        foreach (string compentecia in competenciasCalendarioGeral)
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

        for (DateTime date = new(year, month, 1); date.Month == month; date = date.AddDays(1))
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
