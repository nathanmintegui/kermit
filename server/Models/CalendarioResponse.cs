namespace Kermit.Models;

public sealed class CalendarioResponse
{
    public required List<Competencia> Competencias { get; init; }
    public required Legenda Legenda { get; init; }
}

public sealed class Competencia
{
    public required string Mes { get; init; }
    public required HashSet<DiaCalendario> Dias { get; init; }
}

public sealed class Legenda
{
    public required List<ItemLegenda> ItemsLegenda { get; set; }
}

public sealed class ItemLegenda
{
    public required string Cor { get; set; }
    public required string Modulo { get; set; }
    public required string Dias { get; set; }
}

public class DiaCalendario
{
    public string? Data { get; set; }
}
