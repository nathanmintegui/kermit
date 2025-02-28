namespace Kermit.Models;

public sealed class Evento
{
#pragma warning disable CS8618, CS9264
    private Evento() { }
#pragma warning restore CS8618, CS9264

    private Evento(EventoId id, NomeEvento nome, Cor cor)
    {
        Id = id;
        Nome = nome;
        Cor = cor;
    }

    public static Evento Create(NonEmptyString nome, Cor cor)
    {
        Evento evento = new(EventoId.Empty, new NomeEvento(nome), cor);

        return evento;
    }

    public EventoId Id { get; protected set; }
    public NomeEvento Nome { get; private set; }
    public Cor Cor { get; private set; }
}

public record struct EventoId(int Valor)
{
    public static EventoId Empty => new(0);

    public static EventoId Create(int valor)
    {
        return new EventoId(valor);
    }
}

public sealed record NomeEvento
{
    public NomeEvento(NonEmptyString valor)
    {
        const int tamanhoMaximo = 64;

        if (valor.Value.Length > tamanhoMaximo)
        {
            throw new ArgumentException("Nome deve ter no máximo 64 caracteres");
        }

        Value = valor;
    }

    public string Value { get; }
}

public sealed record Cor
{
    public Cor(NonEmptyString valor)
    {
        const int tamanhoMaximo = 64;

        if (valor.Value.Length > tamanhoMaximo)
        {
            throw new ArgumentException("Nome deve ter no máximo 64 caracteres");
        }

        Value = valor;
    }

    public string Value { get; }
}
