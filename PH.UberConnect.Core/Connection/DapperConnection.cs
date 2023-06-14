using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace PH.UberConnect.Core.Connection
{
    public class DapperConnection : IConnection
    {
        public string ConnectionString { get; }

        public DapperConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<T> RetrieveList<T>(string query, object _params)
        {
            using IDbConnection connection = new SqlConnection(connectionString: ConnectionString);
            return connection.Query<T>(query, _params, commandType: CommandType.Text).AsList();
        }

        public List<T> RetrieveList<T>(string query)
        {
            using IDbConnection connection = new SqlConnection(connectionString: ConnectionString);
            return connection.Query<T>(query, commandType: CommandType.Text).AsList();
        }

        public int ExecuteQuery(string query)
        {
            using IDbConnection connection = new SqlConnection(connectionString: ConnectionString);
            return connection.Execute(query, commandType: CommandType.Text);
        }

        public int ExecuteQuery(string query, object _params)
        {
            using IDbConnection connection = new SqlConnection(connectionString: ConnectionString);
            return connection.Execute(query, _params, commandType: CommandType.Text);
        }
    }
}
