using Hsc.Repository;

namespace Hsc.SqlRepository
{
    public interface ISqlRepository : IRepository
    {
        void InitializeDatabase();

        /// <remarks>
        ///     Should be moved to separate repository
        /// </remarks>
        void PopulateWithMockTypes();

        /// <remarks>
        ///     Should be moved to separate repository
        /// </remarks>
        void PopulateWithMockData();
    }
}