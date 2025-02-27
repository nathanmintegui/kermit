namespace Kermit.Models;

public sealed class Trilha
{
#pragma warning disable CS8618, CS9264
    private Trilha() { }
#pragma warning restore CS8618, CS9264

    private Trilha(TrilhaId id, NomeTrilha nome)
    {
        Id = id;
        Nome = nome;
    }

    public static Trilha Create(NonEmptyString nome)
    {
        Trilha trilha = new(TrilhaId.Empty, new NomeTrilha(nome));

        return trilha;
    }

    internal void SetId(int id)
    {
        const int zero = 0;

        if (id <= zero)
        {
            throw new ArgumentException("ID inválido");
        }

        Id = TrilhaId.Create(id);
    }

    public bool IsNomeUnico(List<Trilha> trilhas)
    {
        return trilhas switch
        {
            null => throw new ArgumentException("Lista de trilhas deve ser informada"),
            { Count: 0 } => true,
            _ => trilhas.Exists(t => t.Nome == Nome)
        };
    }

    public TrilhaId Id { get; internal set; }
    public NomeTrilha Nome { get; private set; }
}

public record struct TrilhaId(int Valor)
{
    public static TrilhaId Empty => new(0);

    public static TrilhaId Create(int valor)
    {
        return new TrilhaId(valor);
    }
}

public sealed record NomeTrilha
{
    public NomeTrilha(NonEmptyString valor)
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
