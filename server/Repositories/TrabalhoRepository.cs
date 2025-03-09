using System.Diagnostics;

using Dapper;

using Kermit.Database;
using Kermit.Dto.Grupo;
using Kermit.Models;

namespace Kermit.Repositories;

public class TrabalhoRepository : ITrabalhoRepository
{
    private readonly DbSession _session;

    public TrabalhoRepository(DbSession session)
    {
        _session = session;
    }

    public async Task<Trabalho?> FindByIdAsync(Guid trabalhoId)
    {
        Debug.Assert(trabalhoId != Guid.Empty, "ID do trabalho não pode ser vazio.");

        const string query = """
                             select t.*
                             from trabalhos t
                                 join trilhas_edicoes td on (td.id = t.trilha_edicao_id)
                                 join edicoes e on (e.id = td.edicao_id)
                             where
                                t.id = @TrabalhoId and
                                e.em_andamento = true;
                             """;

        Trabalho? trabalho =
            await _session.Connection.QuerySingleOrDefaultAsync<Trabalho>(query, new { TrabalhoId = trabalhoId });

        return trabalho;
    }

    public async Task<List<GrupoDto>> FindAllGruposByTrabalhoIdAsync(Guid trabalhoId)
    {
        Debug.Assert(trabalhoId != Guid.Empty, "ID do trabalho não pode ser vazio.");

        const string query = """
                             select
                             	g.id,
                             	g.nome as grupo, 
                             	a.nome as aluno,
                             	c.nome as cargo,
                             	c.abreviacao
                             from integrantes i
                             	join alunos a on (a.id = i.aluno_id)
                             	join cargos c on (c.id = i.cargo_id)
                             	join grupos g on (g.id = i.grupo_id)
                             	join trabalhos t on (t.id = g.trabalho_id)
                             	join trilhas_edicoes te on (te.id = t.trilha_edicao_id)
                             	join edicoes e on (e.id = te.edicao_id )
                             where
                             	t.id = @TrabalhoId and
                             	e.em_andamento = true;
                             """;

        IEnumerable<GrupoDto> grupos =
            await _session.Connection.QueryAsync<GrupoDto>(query, new { TrabalhoId = trabalhoId });

        return grupos.ToList();
    }

    public async Task<Trabalho?> FindByNomeAndTrilhaIdAsync(string nome, int trilhaId)
    {
        Debug.Assert(!string.IsNullOrWhiteSpace(nome), "ID do trabalho não pode ser vazio.");

        const string query = """
                             select t.*
                             from trabalhos t
                                 join trilhas_edicoes td on (td.id = t.trilha_edicao_id)
                                 join edicoes e on (e.id = td.edicao_id)
                             where
                                t.nome = @Nome and
                                td.trilha_id = @TrilhaId and
                                e.em_andamento = true;
                             """;

        Trabalho? trabalho =
            await _session.Connection.QuerySingleOrDefaultAsync<Trabalho>(query,
                new { Nome = nome, TrilhaId = trilhaId });

        return trabalho;
    }

    public async Task SaveAsync(Trabalho trabalho)
    {
        Debug.Assert(trabalho is not null, "Parâmetro trabalho não pode ser nulo.");

        const string query = """
                             INSERT INTO trabalhos (nome, trilha_edicao_id, criado_em, finalizado_em)
                             VALUES(@Nome, @TrilhaEdicaoId, @CriadoEm, @FinalizadoEm)
                             RETURNING id;
                             """;

        Guid id = await _session.Connection.ExecuteScalarAsync<Guid>(query, trabalho, _session.Transaction);
        
        trabalho.Id = id;
    }
}
