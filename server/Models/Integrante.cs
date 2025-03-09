namespace Kermit.Models;

public class Integrante
{
#pragma warning disable CS8618, CS9264
    private Integrante() { }
#pragma warning restore CS8618, CS9264

    public Integrante(int alunoId, int cargoId, int grupoId)
    {
        AlunoId = alunoId;
        CargoId = cargoId;
        GrupoId = grupoId;
    }

    public int AlunoId { get; set; }
    public int CargoId { get; set; }
    public int GrupoId { get; set; }
    public Grupo Grupo { get; set; } = null!;
}
