using System;
using System.Collections.Generic;
using System.Linq;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;
using Hsc.Repository;

namespace Hsc.ModelRepository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly Dictionary<EntityType, List<Entity>> _entitites = new Dictionary<EntityType, List<Entity>>();

        public int Create(Entity entity)
        {
            CreateCollectionIfNotExist(entity.EntityType);
            _entitites[entity.EntityType].Add(entity);
            return 0; //Broken
        }

        public Entity Read(EntityType entityType, int id)
        {
            CreateCollectionIfNotExist(entityType);

            if (_entitites[entityType].Any(e => e.Id == id))
            {
                return _entitites[entityType][id];
            }
            return null;
        }

        public void Update(Entity entity)
        {
            throw new NotImplementedException();
        }

        public List<Entity> ReadAll(EntityType entityType)
        {
            CreateCollectionIfNotExist(entityType);

            return _entitites[entityType];
        }

        public void Delete(EntityType entityType, int id)
        {
            CreateCollectionIfNotExist(entityType);

            if (_entitites[entityType].Any(e => e.Id == id))
            {
                Entity target = Read(entityType, id);
                _entitites[entityType].Remove(target);
            }
        }

        private void CreateCollectionIfNotExist(EntityType entityType)
        {
            if (!_entitites.Keys.Contains(entityType))
            {
                _entitites.Add(entityType, new List<Entity>());
            }
        }
    }
}