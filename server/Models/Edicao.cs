using System.Text.RegularExpressions;

using Kermit.Exceptions;

namespace Kermit.Models;

public class Edicao
{
    public EdicaoId Id { get; private set; }
    public NomeEdicao Nome { get; private set; }
    public bool EmAndamento { get; private set; }

    private Edicao() { }

    private Edicao(EdicaoId id, NomeEdicao nome, bool emAndamento)
    {
        Id = id;
        Nome = nome;
        EmAndamento = emAndamento;
    }

    public static Edicao Create(
        NonEmptyString nome,
        bool emAndamento,
        List<Edicao> edicoes)
    {
        if (edicoes.Any(e => e.Nome.Value == nome))
        {
            throw new DomainException($"An edition with the name '{nome}' already exists.");
        }

        /*
         * Caso já exista edição em andamento seta como false para evitar que mais de uma edição
         * funcione ao mesmo tempo.
         */
        if (edicoes.Any(e => e.EmAndamento))
        {
            emAndamento = false;
        }

        Edicao edicao = new(EdicaoId.Empty, new NomeEdicao(nome), emAndamento);

        return edicao;
    }
}

public record struct EdicaoId(int Valor)
{
    public static EdicaoId Empty => new(0);

    public static EdicaoId Create(int valor)
    {
        return new EdicaoId(valor);
    }
}

public sealed record NomeEdicao
{
    public string Value { get; }

    public NomeEdicao(NonEmptyString valor)
    {
        const int tamanhoMaximo = 64;

        if (valor.Value.Length > tamanhoMaximo)
        {
            throw new DomainException("Nome da edição deve ter no máximo 64 caracteres.");
        }

        string numeroEdicaoSemTexto = Regex.Replace(valor.Value, "[^0-9]", "");
        if (numeroEdicaoSemTexto == string.Empty)
        {
            throw new DomainException("Nome deve informar o número da edição.");
        }

        Value = $"VS {numeroEdicaoSemTexto}";
    }
}
