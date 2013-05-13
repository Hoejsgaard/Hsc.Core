using Hsc.Repository;

namespace Hsc.SqlRepository.Knowledge
{
    public interface ISqlEntityTypeRepository : IEntityTypeRepository
    {
        void CreateTable();
    }
}