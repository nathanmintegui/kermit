using System.Diagnostics;

namespace Kermit.Models;

public sealed class Calendario
{
#pragma warning disable CS8618, CS9264
    private Calendario() { }
#pragma warning restore CS8618, CS9264

    private Calendario(CalendarioId id, Edicao edicao, DateTime criadoEm, DateTime alteradoEm)
    {
        Id = id;
        this.edicao = edicao;
        CriadoEm = criadoEm;
        AlteradoEm = alteradoEm;
    }

    public static Calendario Create(Edicao edicao)
    {
        Debug.Assert(edicao is not null, "Edição não pode ser nulo.");

        Calendario calendario = new(CalendarioId.Empty, edicao, DateTime.Now, DateTime.Now);

        return calendario;
    }

    public CalendarioId Id { get; internal set; }
    public Edicao edicao { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime AlteradoEm { get; private set; }
}

public record struct CalendarioId(Guid Valor)
{
    public static CalendarioId Empty => new(Guid.Empty);

    public static CalendarioId Create(Guid valor)
    {
        return new CalendarioId(valor);
    }
}
