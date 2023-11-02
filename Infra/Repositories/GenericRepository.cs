using Infra.Data;

namespace Infra.Repositories
{
    public class GenericRepository<T> where T : class
    {
        private Context _context;
        private string _tableName;

        public GenericRepository(string tableName, string connectionString)
        {
            if (_context == null)
                _context = new Context(connectionString);

            _tableName = tableName;
        }

        public IEnumerable<T> GetDb<T>(string[] columnsNames, string[] whereListValues = null)
        {
            var columns = string.Join(",", columnsNames);
            var query = $"SELECT {columns} FROM {_tableName} ";

            if (whereListValues != null)
            {
                var whereList = string.Join("AND ", whereListValues);
                query += $"WHERE {whereList}";
            }

            return _context.Query<T>(query);
        }

        protected void AddDb(string[] values, string[] columnsNames, string[] whereListValues = null)
        {
            var addValues = string.Join(",", values);
            var columns = string.Join(",", columnsNames);

            var insertQuery = $"INSERT INTO {_tableName} ({columns}) " +
                $"VALUES ({addValues}) ";

            if (whereListValues != null)
            {
                var whereList = string.Join("and", whereListValues);
                insertQuery += $"WHERE {whereList}";
            }

            _context.ExecuteQuery(insertQuery);
        }
    }
}