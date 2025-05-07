namespace Kermit.Dto.Calendario;

public record CriarCalendarioRequest(string Edicao, List<TrilhaCompetenciaRequest> Trilhas);

public record TrilhaCompetenciaRequest(string Valor, List<CompetenciaRequest> Competencias);

public record CompetenciaRequest(int Mes, int Ano);
