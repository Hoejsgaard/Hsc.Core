using System;
using System.Collections.Generic;
using System.Linq;
using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.ModelRepository
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private EntityTypeCollection EntityTypes = new EntityTypeCollection();

        public void Create(EntityType entityType)
        {
            EntityTypes.Add(entityType);
        }

        public void Udpate(EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public EntityType Get(string name)
        {
            return EntityTypes[name];
        }

        public List<EntityType> GetAll()
        {
            return EntityTypes.ToList<EntityType>();
        }

        public void Delete(string name)
        {
            EntityTypes.Remove(name);
        }
    }
}
