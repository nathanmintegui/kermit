namespace Kermit.Models;

public sealed class DiasCalendario
{
#pragma warning disable CS8618, CS9264
    private DiasCalendario() { }
#pragma warning restore CS8618, CS9264

    private DiasCalendario(DiasCalendarioId id, int mesCalendarioId, int dia, int eventoId)
    {
        Id = id;
        MesCalendarioId = mesCalendarioId;
        Dia = dia;
        EventoId = eventoId;
    }

    public static DiasCalendario Create(int mesCalendarioId, int dia, int eventoId)
    {
        DiasCalendario diasCalendario = new(DiasCalendarioId.Empty, mesCalendarioId, dia, eventoId);

        return diasCalendario;
    }

    public DiasCalendarioId Id { get; internal set; }
    public int MesCalendarioId { get; private set; }
    public int Dia { get; private set; }
    public int EventoId { get; private set; }
}

public record struct DiasCalendarioId(int Valor)
{
    public static DiasCalendarioId Empty => new(0);

    public static DiasCalendarioId Create(int valor)
    {
        return new DiasCalendarioId(valor);
    }
}
