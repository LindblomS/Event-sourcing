namespace ConsoleApp1.Infrastructure;

using System.Data;

interface IConnectionProvider
{
    IDbConnection GetConnection();
}
