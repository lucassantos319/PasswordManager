using Dapper;
using Npgsql;

namespace Infra.Data
{
    public class Context
    {
        private NpgsqlConnection _connection;

        private string _connectionString;

        public Context(string connectionString)
        {
            _connectionString = connectionString;

            if (!string.IsNullOrEmpty(_connectionString))
                _connection = new NpgsqlConnection(connectionString);
        }

        public void ExecuteQuery(string query)
        {
            try
            {
                _connection.Execute(query);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<T> Query<T>(string query)
        {
            try
            {
                return _connection.Query<T>(query);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}