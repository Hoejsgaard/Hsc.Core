using System.Data.SqlClient;

namespace Hsc.SqlRepository
{
    public interface IConnectionProvider
    {
        SqlConnection GetConnection();
    }
}