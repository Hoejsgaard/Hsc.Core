using Hsc.Repository;

namespace Hsc.SqlRepository.Meta
{
    public interface ISqlEntityTypeRepository : IEntityTypeRepository
    {
        void CreateTable();
    }
}