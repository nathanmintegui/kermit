using System.Diagnostics;

namespace Kermit.Models;

public sealed class ConteudoProgramatico
{
#pragma warning disable CS8618, CS9264
    private ConteudoProgramatico() { }
#pragma warning restore CS8618, CS9264

    private ConteudoProgramatico(ConteudoProgramaticoId id, TrilhaCompetencia trilhaCompetencia, Evento evento, int dia,
        DateTime criadoEm, DateTime alteradoEm)
    {
        Id = id;
        TrilhaCompetencia = trilhaCompetencia;
        Evento = evento;
        Dia = dia;
        CriadoEm = criadoEm;
        AlteradoEm = alteradoEm;
    }

    public static ConteudoProgramatico Create(TrilhaCompetencia trilhaCompetencia, Evento evento, int dia,
        DateTime criadoEm, DateTime alteradoEm)
    {
        Debug.Assert(trilhaCompetencia is not null, "Propriedade Triha Competencia não pode ser nulo.");
        Debug.Assert(trilhaCompetencia.Id.Valor != 0, "Trilha competência não pode ter ID 0");

        Debug.Assert(evento is not null, "Propriedade Evento não pode ser nulo.");
        Debug.Assert(evento.Id.Valor != 0, "Evento não pode ter ID 0");

        Debug.Assert(dia > 0, "Propriedade dia deve ser maior que zero.");

        ConteudoProgramatico conteudoProgramatico = new(ConteudoProgramaticoId.Empty, trilhaCompetencia, evento, dia,
            DateTime.Now, DateTime.Now);

        return conteudoProgramatico;
    }

    public ConteudoProgramaticoId Id { get; protected set; }
    public TrilhaCompetencia TrilhaCompetencia { get; private set; }
    public Evento Evento { get; private set; }
    public int Dia { get; private set; }
    public DateTime CriadoEm { get; private set; }
    public DateTime AlteradoEm { get; private set; }
}

public record struct ConteudoProgramaticoId(int Valor)
{
    public static ConteudoProgramaticoId Empty => new(0);

    public static ConteudoProgramaticoId Create(int valor)
    {
        return new ConteudoProgramaticoId(valor);
    }
}
