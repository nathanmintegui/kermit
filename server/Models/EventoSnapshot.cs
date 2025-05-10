using System.Diagnostics;

namespace Kermit.Models;

public sealed class EventoSnapshot
{
    public static EventoSnapshot From(Evento evento)
    {
        throw new NotImplementedException();
    }

    public static Evento ToModel(EventoSnapshot eventoSnapshot)
    {
        Debug.Assert(eventoSnapshot is not null);
        Debug.Assert(eventoSnapshot.Id > 0);
        Debug.Assert(eventoSnapshot.Nome != "");
        Debug.Assert(eventoSnapshot.Cor != "");

        NonEmptyString nome = new NonEmptyString(eventoSnapshot.Nome);
        Cor cor = new Cor(new NonEmptyString(eventoSnapshot.Cor));

        Evento evento = Evento.Create(nome, cor);

        Debug.Assert(evento is not null);

        return evento;
    }

    public int Id { get; set; }
    public string Nome { get; private set; } = String.Empty;
    public string Cor { get; private set; } = String.Empty;
}

