namespace Kermit.Dto.Grupo;

public sealed class GrupoDto
{
#pragma warning disable CS8618, CS9264
    public GrupoDto()
#pragma warning restore CS8618, CS9264
    {
    }

    public GrupoDto(int id, string grupo, string aluno, string cargo, string abreviacao)
    {
        Id = id;
        Grupo = grupo;
        Aluno = aluno;
        Cargo = cargo;
        Abreviacao = abreviacao;
    }

    public int Id { get; set; }
    public string Grupo { get; set; }
    public string Aluno { get; set; }
    public string Cargo { get; set; }
    public string Abreviacao { get; set; }
}
