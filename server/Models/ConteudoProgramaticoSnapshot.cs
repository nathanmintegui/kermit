namespace Kermit.Models;

public sealed class ConteudoProgramaticoSnapshot
{
    public int Id { get; init; }
    public string Nome { get; private set; } = String.Empty;
    public string Cor { get; private set; } = String.Empty;
    public DateTime Data { get; init; }
    public List<DateTime> Datas { get; } = [];
}

