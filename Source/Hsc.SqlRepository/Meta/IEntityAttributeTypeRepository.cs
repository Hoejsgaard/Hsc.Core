using Hsc.Model.Knowledge;
using Hsc.Repository;

namespace Hsc.SqlRepository.Meta
{
    public interface IEntityAttributeTypeRepository
    {
        void Create(AttributeTypeCollection attributeTypeCollection, EntityType onEntity);
        AttributeTypeCollection Read(EntityType entityType, IEntityTypeRepository entityTypeRepository);
        void Create(EntityAttributeType entityAttributeType, EntityType onEntity);
        void CreateTable();
    }
}