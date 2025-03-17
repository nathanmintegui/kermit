namespace Kermit.Dto.Trabalho;

public record CriarTrabalhoRequest(string Nome, int? QuantidadeGrupos = null, int? IntegrantesPorGrupo = null);
