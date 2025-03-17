using System.Diagnostics;

namespace Kermit.Models;

public sealed class Trabalho
{
#pragma warning disable CS8618, CS9264
    private Trabalho()
#pragma warning restore CS8618, CS9264
    {
    }

    public Trabalho(Guid id, string nome, int trilhaEdicaoId)
    {
        Id = id;
        Nome = nome;
        TrilhaEdicaoId = trilhaEdicaoId;
    }

    public static Trabalho Create(string nome, int trilhaEdicaoId)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(nome), "Parâmetro nome não pode ser vazio.");
        Debug.Assert(trilhaEdicaoId > 0, "Parâmetro trilhaEdicaoId deve ser um valor positivo.");

        Trabalho trabalho = new(Guid.Empty, nome, trilhaEdicaoId);

        return trabalho;
    }

    public Guid Id { get; set; }
    public string Nome { get; set; }
    public int TrilhaEdicaoId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public DateTime? FinalizadoEm { get; set; } = DateTime.Now;
}
