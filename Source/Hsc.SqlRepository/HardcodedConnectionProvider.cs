using System.Data.SqlClient;

namespace Hsc.SqlRepository
{
    public class HardcodedConnectionProvider : IConnectionProvider
    {
        #region IConnectionProvider Members

        public SqlConnection GetConnection()
        {
            var connection = new SqlConnection("Server=localhost; Database=database; User Id=demoUser; Password=Password42");
            connection.Open();
            return connection;
        }

        #endregion
    }
}