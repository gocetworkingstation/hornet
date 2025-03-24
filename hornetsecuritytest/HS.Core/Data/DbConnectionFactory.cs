using System.Data;
using HS.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace HS.Core.Data;

public class DbConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public DbConnectionFactory(IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration), "Configuration is null");
        }
        _connectionString = configuration.GetConnectionString("MariaDb")
                            ?? throw new ArgumentNullException(nameof(configuration), "MariaDb connection string not found");
    }

    public IDbConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }
}