using Hsc.Repository;

namespace Hsc.ModelRepository
{
    public class Repository : IRepository
    {
        public Repository(IEntityTypeRepository entityTypeRepository,
                          IEntityRepository entityRepository)
        {
            EntityTypes = entityTypeRepository;
            Entities = entityRepository;
        }

        #region IRepository Members

        public IEntityTypeRepository EntityTypes { get; set; }

        public IEntityRepository Entities { get; set; }

        #endregion
    }
}