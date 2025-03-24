using Dapper;
using HS.Core.Configuration;
using HS.Core.DTOs;
using HS.Core.Interfaces;
using MySqlConnector;

namespace HS.Core.Repositories;

public class HashRepository : IHashRepository
{
    public async Task SaveHashAsync(string hash)
    {
        using var connection = new MySqlConnection(DbConfig.ConnectionString);
        await connection.ExecuteAsync(
            "INSERT INTO hashes (date, sha1) VALUES (@Date, @Sha1)",
            new { Date = DateTime.Now, Sha1 = hash });
    }

    public async Task<IEnumerable<HashCountDto>> GetHashCountsByDayAsync()
    {
        using var connection = new MySqlConnection(DbConfig.ConnectionString);
        return await connection.QueryAsync<HashCountDto>(
            "SELECT DATE(date) AS Date, COUNT(*) AS Count FROM hashes GROUP BY DATE(date)");
    }
}