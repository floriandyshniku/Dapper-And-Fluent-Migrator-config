using Microsoft.Data.SqlClient;

namespace MinimalApiTest.Service
{
    public class SqlConnectionFactory
    {

        private readonly string _connection;

        public SqlConnectionFactory(string _connection)
        {
            this._connection = _connection;
        }

        public SqlConnection Create()
        {
            return new SqlConnection(_connection);
        }
    }
}
