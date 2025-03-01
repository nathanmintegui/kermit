namespace Kermit.Models;

public sealed class Trabalho
{
    public Guid Id { get; set; }
    public required string Nome { get; set; }
    public int TrilhaEdicaoId { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    public DateTime? FinalizadoEm { get; set; } = DateTime.Now;
}
