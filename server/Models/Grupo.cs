using Kermit.Exceptions;

namespace Kermit.Models;

public class Grupo
{
#pragma warning disable CS8618, CS9264
    private Grupo() { }
#pragma warning restore CS8618, CS9264

    public Grupo(string nome, Guid trabalhoId)
    {
        Nome = nome;
        TrabalhoId = trabalhoId;
    }

    public void AdicionarIntegrante(Integrante integrante)
    {
        if (_integrantes.Any(i => i.AlunoId == integrante.AlunoId))
        {
            throw new DomainException("O aluno já está neste grupo!");
        }

        _integrantes.Add(integrante);
    }

    public int Id { get; set; }
    public string Nome { get; private set; }
    public Guid TrabalhoId { get; private set; }
    private readonly List<Integrante> _integrantes = [];
    public IReadOnlyCollection<Integrante> Integrantes => _integrantes.AsReadOnly();
}
