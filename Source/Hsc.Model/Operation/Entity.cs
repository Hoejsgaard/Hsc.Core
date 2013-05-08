using Hsc.Model.Knowledge;
using ServiceStack.DataAnnotations;

namespace Hsc.Model.Operation
{
    public class Entity
    {
        public Entity(EntityType entityType)
        {
            if (entityType == null)
            {
                throw new OperationException();
            }

            EntityType = entityType;
            Attributes = new AttributeCollection(entityType.Attributes);
        }

        public Entity()
        {
            // for serialization
        }

        [Alias("EntityId")]
        [AutoIncrement]
        public int Id { get; set; }

        public EntityType EntityType { get; private set; }

        public AttributeCollection Attributes { get; private set; }
    }
}