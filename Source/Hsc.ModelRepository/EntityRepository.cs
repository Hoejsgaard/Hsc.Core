using System.Collections.Generic;
using System.Linq;
using Hsc.Model.Knowledge;
using Hsc.Model.Operation;
using Hsc.Repository;

namespace Hsc.ModelRepository
{
    public class EntityRepository : IEntityRepository
    {
        private Dictionary<EntityType, List<Entity>> _entitites = new Dictionary<EntityType, List<Entity>>();

        private void CreateCollectionIfNotExist(EntityType entityType)
        {
            if (!_entitites.Keys.Contains(entityType))
            {
                _entitites.Add(entityType, new List<Entity>());
            }
        }

        public int Add(Entity entity)
        {
            CreateCollectionIfNotExist(entity.EntityType);
            _entitites[entity.EntityType].Add(entity);
            return 0; //Broken
        }

        public Entity Get(EntityType entityType, int id)
        {
            CreateCollectionIfNotExist(entityType);

            if (_entitites[entityType].Any(e => e.Id == id))
            {
                return _entitites[entityType][id];
            }
            return null;
        }

        public List<Entity> GetAll(EntityType entityType)
        {
            CreateCollectionIfNotExist(entityType);

            return _entitites[entityType];
        }

        public void Delete(EntityType entityType, int id)
        {
            CreateCollectionIfNotExist(entityType);

            if (_entitites[entityType].Any(e => e.Id == id))
            {
                Entity target = Get(entityType, id);
                _entitites[entityType].Remove(target);
            }
        }
    }
}
