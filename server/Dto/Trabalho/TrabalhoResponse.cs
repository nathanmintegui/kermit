namespace Kermit.Dto.Trabalho;

public record TrabalhoResponse(Guid Id, string Trabalho, List<GrupoTrabalho> Grupos);

public record GrupoTrabalho(int Id, string Nome, List<Alunos> Alunos);

public record Alunos(string Nome, string Cargo, string Abreviacao);
