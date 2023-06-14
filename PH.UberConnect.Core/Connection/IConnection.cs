namespace PH.UberConnect.Core.Connection
{
    public interface IConnection
    {
        string ConnectionString { get; }

        /// <summary>
        /// Executes a query and returns a list of a specific object
        /// </summary>
        List<T> RetrieveList<T>(string query, object _params);
        List<T> RetrieveList<T>(string query);

        /// <summary>
        /// Executes a query and returns the count of rows affected
        /// </summary>
        int ExecuteQuery(string query);
        int ExecuteQuery(string query, object _params);
    }
}
