using System.Data;

using Dapper;

using Kermit.Database;
using Kermit.Models;

using Npgsql;

namespace Kermit.Repositories;

public class EdicaoRepository : IEdicaoRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<EdicaoRepository> _logger;

    public EdicaoRepository(IDbConnectionFactory dbConnectionFactory, ILogger<EdicaoRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<List<Edicao>> FindAllAsync()
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        try
        {
            string query = @"select id, nome, em_andamento from edicoes;";

            IEnumerable<Edicao> edicoes = await connection.QueryAsync<Edicao>(query);

            return edicoes.ToList();
        }
        catch (NpgsqlException e)
        {
            _logger.LogError("{Date} | An Exception occured - {Error}", DateTime.UtcNow, e);
        }

        return [];
    }
}
