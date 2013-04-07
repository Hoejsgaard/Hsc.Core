using System.Collections.Generic;
using Hsc.Model.Knowledge;

namespace Hsc.Repository
{
    public interface IEntityTypeRepository
    {
        void Create(EntityType entityType);
        void Delete(string name);
        EntityType Get(string name);
        List<EntityType> GetAll();
    }
}