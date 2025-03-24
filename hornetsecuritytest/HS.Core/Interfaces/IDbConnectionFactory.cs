using System.Data;

namespace HS.Core.Interfaces;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}