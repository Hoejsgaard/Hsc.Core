using Hsc.Repository;

namespace Hsc.SqlRepository
{
    public interface ISqlRepository : IRepository
    {
        void InitializeDatabase();
        void PopulateWithMockTypes();
        void PopulateWithMockData();
    }
}