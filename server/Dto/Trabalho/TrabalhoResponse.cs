namespace Kermit.Dto.Trabalho;

public record TrabalhoResponse(Guid Id, string Trabalho, List<Grupo> Grupos);

public record Grupo(int Id, string Nome, List<Alunos> Alunos);

public record Alunos(string Nome, string Cargo, string Abreviacao);
