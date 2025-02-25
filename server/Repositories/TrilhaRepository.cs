using System.Data;

using Dapper;

using Kermit.Database;
using Kermit.Models;

using Npgsql;

namespace Kermit.Repositories;

public class TrilhaRepository : ITrilhaRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<TrilhaRepository> _logger;

    public TrilhaRepository(IDbConnectionFactory dbConnectionFactory, ILogger<TrilhaRepository> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<List<Trilha>> FindAllAsync()
    {
        using IDbConnection connection = await _dbConnectionFactory.CreateConnectionAsync();

        try
        {
            string query = @"select id, nome from trilhas;";

            IEnumerable<Trilha> trilhas = await connection.QueryAsync<Trilha>(query);

            return trilhas.ToList();
        }
        catch (NpgsqlException e)
        {
            _logger.LogError("{Date} | An Exception occured - {Error}", DateTime.UtcNow, e);
        }

        return [];
    }
}
