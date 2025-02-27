using System.Diagnostics;

namespace Kermit.Models;

public class TrilhaCompetencia
{
#pragma warning disable CS8618, CS9264
    private TrilhaCompetencia() { }
#pragma warning restore CS8618, CS9264

    private TrilhaCompetencia(TrilhaComptenciaId id, AnoMes anoMes, Trilha trilha, Calendario calendario)
    {
        Id = id;
        AnoMes = anoMes;
        Trilha = trilha;
        Calendario = calendario;
    }

    public static TrilhaCompetencia Create(AnoMes anoMes, Trilha trilha, Calendario calendario)
    {
        Debug.Assert(trilha is not null, "Parâmetro trilha não pode ser nulo.");
        Debug.Assert(calendario is not null, "Parâmetro calendario não pode ser nulo.");

        TrilhaCompetencia trilhaCompetencia = new(TrilhaComptenciaId.Empty, anoMes, trilha, calendario);

        return trilhaCompetencia;
    }

    public TrilhaComptenciaId Id { get; internal set; }
    public AnoMes AnoMes { get; private set; }
    public Trilha Trilha { get; private set; }
    public Calendario Calendario { get; private set; }
}

public record struct TrilhaComptenciaId(int Valor)
{
    public static TrilhaComptenciaId Empty => new(0);

    public static TrilhaComptenciaId Create(int valor)
    {
        return new TrilhaComptenciaId(valor);
    }
}

public sealed record AnoMes
{
    public AnoMes(int ano, int mes)
    {
        if (ano < 1)
        {
            throw new ArgumentException("Ano deve ser maior que zero.");
        }

        if (mes < 1)
        {
            throw new ArgumentException("Mês deve ser maior que zero.");
        }

        if (!isValorValido(ano, mes))
        {
            throw new ArgumentException("Ano ou mês inválidos.");
        }

        Value = CalcularAnoMes(ano, mes);
    }

    /*
     * Valida se o mês extraido do valor final esta entre 1 e 12 e
     * se o ano é igual ou superior ao ano atual.
     */
    private bool isValorValido(int ano, int mes)
    {
        if (mes is < 1 or > 12)
        {
            return false;
        }

        if (ano < DateTime.Now.Year)
        {
            return false;
        }

        return CalcularAnoMes(ano, mes) % 100 >= 1 &&
               CalcularAnoMes(ano, mes) % 100 <= 12;
    }

    /*
     * Multiplica o ano por 100 e adiciona o valor do mês no final.
     *
     *      ano          mes
     * Ex.: 2025 * 100 + 06 = 202506
     */
    private int CalcularAnoMes(int ano, int mes)
    {
        return (ano * 100) + mes;
    }

    /*
     * Retorna o valor do mês.
     */
    public int GetValorMes()
    {
        return Value % 100;
    }

    /*
     * Retorna o valor do ano.
     */
    public int GetValorAno()
    {
        return Value / 100;
    }

    public int Value { get; private set; }
}
