using System.Collections.Generic;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;

namespace Hsc.Repository
{
    public interface IEntityRepository
    {
        int Add(Entity entity);
        void Delete(EntityType entityType, int id);
        Entity Get(EntityType entityType, int id);
        List<Entity> GetAll(EntityType entityType);
    }
}