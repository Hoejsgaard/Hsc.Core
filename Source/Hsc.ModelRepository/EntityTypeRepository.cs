using System;
using System.Collections.Generic;
using System.Linq;
using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.ModelRepository
{
    public class EntityTypeRepository : IEntityTypeRepository
    {
        private readonly EntityTypeCollection EntityTypes = new EntityTypeCollection();

        public void Create(EntityType entityType)
        {
            EntityTypes.Add(entityType);
        }

        public EntityType Read(string name)
        {
            return EntityTypes[name];
        }

        public void Update(EntityType entityType)
        {
            throw new NotImplementedException();
        }

        public List<EntityType> ReadAll()
        {
            return EntityTypes.ToList();
        }

        public void Delete(string name)
        {
            EntityTypes.Remove(name);
        }

        public void Udpate(EntityType entityType)
        {
            throw new NotImplementedException();
        }
    }
}