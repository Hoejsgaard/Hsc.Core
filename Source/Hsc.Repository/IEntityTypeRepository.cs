using System.Collections.Generic;
using Hsc.Model.Knowledge;

namespace Hsc.Repository
{
    public interface IEntityTypeRepository
    {
        void Create(EntityType entityType);
        EntityType Read(string name);
        List<EntityType> ReadAll();
        void Update(EntityType entityType);
        void Delete(string name);
    }
}