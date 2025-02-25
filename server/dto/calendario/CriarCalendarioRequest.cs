namespace Kermit.dto.calendario;

public record CriarCalendarioRequest(string edicao, List<TrilhaCompetenciaRequest> trilhas);

public record TrilhaCompetenciaRequest(string valor, List<CompetenciaRequest> competencias);

public record CompetenciaRequest(int mes, int ano);
