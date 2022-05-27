namespace ConsoleApp1.Infrastructure;

using System.Data;
using System.Data.SqlClient;

class SqlConnectionProvider : IConnectionProvider
{
    const string connectionstring = "server=(localdb)\\mssqllocaldb;Database=eventstore;integrated security=true";
    public IDbConnection GetConnection()
    {
        var connection = new SqlConnection(connectionstring);
        connection.Open();
        return connection;
    }
}
