using System.Collections.Generic;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;

namespace Hsc.Repository
{
    public interface IEntityRepository
    {
        int Create(Entity entity);
        Entity Read(EntityType entityType, int id);
        List<Entity> ReadAll(EntityType entityType);
        void Update(Entity entity);
        void Delete(EntityType entityType, int id);
    }
}